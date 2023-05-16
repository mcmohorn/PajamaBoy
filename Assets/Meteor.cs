using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Meteor : MonoBehaviour
{

    Rigidbody rb;
    public Rigidbody rockBody;
    public float rockRotation;
    Vector3 dir;
    public ParticleSystem explosionPS;
    public ParticleSystem explosionPS2;

    public float delayUntilExplosion;
    public float delayUntilExplosion2;

    public float delayUntilNextScene;

    public string nextScene;

    public float meteorVelocity;

    void Start()
    {
        dir = new Vector3(0,0,meteorVelocity);
        rb = GetComponent<Rigidbody>();

        Invoke("Explode", delayUntilExplosion);
        Invoke("Explode2", delayUntilExplosion2);
        Invoke("NextScene", delayUntilNextScene);
    }

    void Update()
    {
        rb.AddForce(dir);
        rockBody.AddForce(dir);
        rockBody.AddTorque(transform.up * rockRotation);
        
    }

    void Explode() 
    {
        explosionPS.Play();
        Destroy(explosionPS, explosionPS.main.duration);
    }

    void Explode2() 
    {
        explosionPS2.Play();
        Destroy(explosionPS2, explosionPS2.main.duration);
    }

    void NextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
