using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float maxHealth;
    float health;
    public int team;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void TakeDamage(float amt) 
    {
        // health -= amt;



        if(health <= 0) {

        }
    }
}
