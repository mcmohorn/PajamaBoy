using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MyPlayerController : MonoBehaviour
{

    private int playerId = 0;
    
    public float speed = 3.0f;

    public float jumpPower;

    private bool jump;

    private bool fire;

    private bool taunt;

    private Vector3 moveVector;

    private Player player; // The Rewired Player
    



    void Awake() {
        Debug.Log("awaking player");
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);
    }



    void Update()
    {
        
        GetInput();
        ProcessInput();
        
    }

    void FixedUpdate()
    {
       

    }

    void GetInput()
    {
        // receive movement input
        moveVector.x = player.GetAxis("Move Horizontal"); // get input by name or action id
        moveVector.y = player.GetAxis("Move Vertical");

        // receive button inputs
        jump = player.GetButtonDown("Jump");
        taunt = player.GetButtonDown("Jump");
        fire = player.GetButtonDown("Jump");

        
    }

    private void ProcessInput() 
    {
        // Process movement
        if(moveVector.x != 0.0f || moveVector.y != 0.0f) {
            Debug.Log("moving player");
            GetComponent<Rigidbody>().AddForce(moveVector * speed);

            // handle rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVector), Time.deltaTime * 40f);
        }

        // Process firing
        // if(fire) {
        //     GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.right, transform.rotation);
        //     bullet.GetComponent<Rigidbody>().AddForce(transform.right * bulletSpeed, ForceMode.VelocityChange);
        // }

        // Process Jumping
        if(jump) {
            Debug.Log("jumped");
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }

        // Process Taunting

    }

    


}
