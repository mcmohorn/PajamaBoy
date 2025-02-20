using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("Speed of the bullet")]
    public float speed;

    [Tooltip("Seconds until bullet disappears on its own")]
    public float maxLifetime;

    [Tooltip("Damage on impact with something with health")]
    public float damage;

    [Tooltip("Heat cost per bullet ")]
    public float heat;
    // Start is called before the first frame update
    void Start()
    {
       //  StartCoroutine(Disappear);
        Destroy(gameObject, maxLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) 
    {
        

        if (other.gameObject.GetComponent<Health>()) {
            Debug.Log("Bullet hit something with health "+ other.gameObject.name);
            // take damage 
        } else {
            Debug.Log("Bullet hit something without health"+ other.gameObject.name);
        }

        // take health from target game object
        Destroy(gameObject);
    }

    // public IEnumerator Disappear()
    // {
    //     yield return new WaitForSeconds(castTime);
        
    // }
}
