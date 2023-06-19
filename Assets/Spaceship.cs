using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Spaceship : MonoBehaviour
{

    public Canvas pickupUI;
    public Canvas spaceshipHUD;
    public CinemachineVirtualCamera cam;

    [HideInInspector] // should be set programmatically
    public GameObject player;

    public Transform ejectionPoint;

    public bool activating = false;
    public bool piloting = false;

    [Tooltip("Time for the spaceship to boot up and controls to activate")]
    public float activationTime;

    [Tooltip("Time in between bullet spawns")]
    public float fireRate;
    float nextFire;
    [Tooltip("Maximum heat value")]
    public float maxHeat;

    [Tooltip("Seconds it takes to cooldown")]
    public float heatCooldownDelay;

    [Tooltip("Seconds it takes to cooldown")]
    public float heatCooldownTime;
    float timeUntilCooling = 0f;
    float heat = 0f;
    public GameObject bulletPrefab;

    public Image heatFillImage;

    public float thrustPower;
    public float lookPower;

    public float liftForce;

    public Transform firingPoint;
    public MeshCollider shipCollider;
    public CapsuleCollider interactionCollider;


    bool eject = false;
    float thrust = 0f;
    float lookHorizontal = 0f;
    float lookVertical = 0f;
    bool fire = false;

    Vector3 bulletDir = new Vector3(0f,0f,0f);
    Rigidbody rb;


    void Start()
    {
        spaceshipHUD.gameObject.SetActive(false); // should activate HUD through Activate()
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        
        if (piloting) {

            GetInput();
            ProcessInput();
            
            
        }

        if (activating) {
            rb.AddForce(Vector3.up*liftForce);
        }

        if (timeUntilCooling > 0) {
            timeUntilCooling -= Time.deltaTime;
        }

        if (heat > 0 && timeUntilCooling <= 0) {
            float dh = (Time.deltaTime / heatCooldownTime) * maxHeat;
            heat -= dh;
        }

        heatFillImage.fillAmount = heat / maxHeat;
        
    }

    void GetInput()
    {
        eject = player.GetComponent<MyPlayerController>().player.GetButtonDown("Eject");
        thrust = player.GetComponent<MyPlayerController>().player.GetAxis("Thrust");
        lookHorizontal = player.GetComponent<MyPlayerController>().player.GetAxis("AimHorizontal");
        lookVertical = player.GetComponent<MyPlayerController>().player.GetAxis("AimVertical");
        
        fire = player.GetComponent<MyPlayerController>().player.GetButton("Fire");
    }

    void ProcessInput() 
    {
        if (eject) {
            Debug.Log("time to eject!");
            Eject();
        }

        if (fire) {
            Fire();
        }

        if (thrust != 0) {
            rb.AddForce(transform.forward * thrust * thrustPower );
        }

        if (lookHorizontal != 0) {
            rb.AddTorque(transform.up * lookHorizontal * lookPower );
        }

        if (lookVertical != 0) {
            rb.AddTorque(-1.0f*transform.right * lookVertical * lookPower );
        }
    }


    public IEnumerator LoadEveryThingUp()
    {
        yield return new WaitForSeconds(activationTime / 2f);
        // turn on HUD (flicker on each element in future)
        spaceshipHUD.gameObject.SetActive(true);
        yield return new WaitForSeconds(activationTime / 2f);
        piloting = true;
        rb.velocity = Vector3.zero;
        activating = false;
    }

    // power up the spaceship
    public void Activate()
    {
        activating = true;
        StartCoroutine(LoadEveryThingUp());

        // camera transition 
        cam.Priority = 1; 
        player.GetComponent<MyPlayerController>().mainCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
    }

    public void Deactivate()
    {
        spaceshipHUD.gameObject.SetActive(false);
        piloting = false;
        cam.Priority = 0;
        player.GetComponent<MyPlayerController>().mainCamera.GetComponent<CinemachineVirtualCamera>().Priority = 1;
    }

    void Eject()
    {
        Deactivate();

        // reposition player and spawn
        player.transform.position = ejectionPoint.position;
        player.transform.rotation = transform.rotation;
        interactionCollider.enabled = false;
        player.GetComponent<MyPlayerController>().disabled = false;
        player.GetComponent<MyPlayerController>().bodyRenderer.enabled = true;
        player.GetComponent<MyPlayerController>().SwitchToControlMap("Default");
        
    }

    void Fire()
    {

        var bulletHeat = bulletPrefab.GetComponent<Bullet>().heat;

        if (Time.time > nextFire && fireRate > 0 && heat + bulletHeat < maxHeat) {
            nextFire = Time.time + fireRate;
            timeUntilCooling = heatCooldownDelay;
            heat = heat + bulletHeat;
            Debug.Log("Firing Bullet");


            // Bit shift the index of the layer (31) to get a bit mask
            int layerMask = 1 << 31;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 500f, layerMask))
            {
                Debug.DrawRay(cam.transform.position, cam.transform.forward * hit.distance, Color.yellow, 10f);
                Vector3 hitLocation = cam.transform.position + cam.transform.forward * hit.distance;
                Debug.Log("Did Hit");
                bulletDir = hitLocation - firingPoint.position;
                bulletDir.Normalize();

                Debug.DrawRay(firingPoint.position, bulletDir, Color.blue, 10f);
            }
            else
            {
                Debug.DrawRay(cam.transform.position, cam.transform.forward * 500f, Color.white, 10f);
                Vector3 fakeHitLocation = cam.transform.position + cam.transform.forward * 500f;
                bulletDir = fakeHitLocation - firingPoint.position;
                bulletDir.Normalize();
                Debug.Log("Did not Hit");
            }


            

         

            var bullet = (GameObject)Instantiate(bulletPrefab, firingPoint.position, transform.rotation);

            var bulletCollider = bullet.GetComponent<CapsuleCollider>();
            var interactionCollider = GetComponent<CapsuleCollider>();
            Physics.IgnoreCollision(shipCollider, bulletCollider);
            Physics.IgnoreCollision(interactionCollider, bulletCollider);
            var bulletSpeed = bulletPrefab.GetComponent<Bullet>().speed;
            
            var projectileRb = bullet.GetComponent<Rigidbody>();
            var expectedVelocity = bulletDir * bulletSpeed;
            projectileRb.AddForce(expectedVelocity - projectileRb.velocity, ForceMode.VelocityChange);
        


            
        }
    }

}
