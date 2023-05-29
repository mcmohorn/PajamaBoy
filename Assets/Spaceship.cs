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
    public GameObject bulletPrefab;

    public float thrustPower;
    public float lookPower;

    public Transform firingPoint;
    public MeshCollider shipCollider;


    bool eject = false;
    float thrust = 0f;
    float lookHorizontal = 0f;
    bool fire = false;

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
            rb.AddForce(Vector3.up*0.25f);
        }
        
    }

    void GetInput()
    {
        eject = player.GetComponent<MyPlayerController>().player.GetButtonDown("Eject");
        thrust = player.GetComponent<MyPlayerController>().player.GetAxis("Thrust");
        lookHorizontal = player.GetComponent<MyPlayerController>().player.GetAxis("AimHorizontal");
        
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
        player.GetComponent<MyPlayerController>().disabled = false;
        player.GetComponent<MyPlayerController>().bodyRenderer.enabled = true;
        player.GetComponent<MyPlayerController>().SwitchToControlMap("Default");
        
    }

    void Fire()
    {

        if (Time.time > nextFire && fireRate > 0) {
            nextFire = Time.time + fireRate;
            Debug.Log("Firing Bullet");
            var bullet = (GameObject)Instantiate(bulletPrefab, firingPoint.position, transform.rotation);
            var bulletCollider = bullet.GetComponent<CapsuleCollider>();
            var interactionCollider = GetComponent<CapsuleCollider>();
            Physics.IgnoreCollision(shipCollider, bulletCollider);
            Physics.IgnoreCollision(interactionCollider, bulletCollider);
            var bulletSpeed = bulletPrefab.GetComponent<Bullet>().speed;
            var projectileRb = bullet.GetComponent<Rigidbody>();
            var expectedVelocity = bullet.transform.forward * bulletSpeed;
            projectileRb.AddForce(expectedVelocity - projectileRb.velocity, ForceMode.VelocityChange);
        }
    }

}
