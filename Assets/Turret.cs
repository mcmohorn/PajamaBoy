using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public bool continuous;
    public float fireRate;

    public GameObject bulletPrefab;
    public Transform firingPoint;
    float timeUntilNextFire;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeUntilNextFire <= 0) {
            TurretFire();
        }
        else {
            timeUntilNextFire -= Time.deltaTime;
        }
    }


    void TurretFire()
    {
        var bulletDir = transform.forward;
        Debug.Log("Turret firing bullet");
        timeUntilNextFire = fireRate;
        var bullet = (GameObject)Instantiate(bulletPrefab, firingPoint.position, transform.rotation);
        var bulletSpeed = bulletPrefab.GetComponent<Bullet>().speed;
        var projectileRb = bullet.GetComponent<Rigidbody>();
        var expectedVelocity = bulletDir * bulletSpeed;
        projectileRb.AddForce(expectedVelocity - projectileRb.velocity, ForceMode.VelocityChange);
        



        
    }

}
