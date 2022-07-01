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

    private int numJumps;
    public int maxJumps;

    private bool fire;

    private bool taunt;

    private Vector3 moveVector;

    private Player player; // The Rewired Player

    Animator animator;

    Rigidbody rb;
    



    void Awake() {
        Debug.Log("awaking player");
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        numJumps = maxJumps;
    }



    void Update()
    {
        
        GetInput();
        ProcessInput();
        SetAnimatorValues();
        
    }

    void FixedUpdate()
    {
       

    }

    void GetInput()
    {
        // receive movement input
        moveVector.x = player.GetAxis("MoveHorizontal"); // get input by name or action id
        moveVector.z = player.GetAxis("MoveVertical");

        // receive button inputs
        jump = player.GetButtonDown("Jump");
        
        fire = player.GetButtonDown("Fire");

        
        taunt = player.GetButtonDown("Taunt");
        

        
    }

    private void OnCollisionEnter(Collision other) 
    {
        
        if (other.gameObject.GetComponent<Surface>()) {
            Debug.Log("Landed on  " + other.gameObject.name);
            numJumps = maxJumps;

        }

    }

    void SetAnimatorValues()
    {
        if (numJumps < maxJumps) {
            animator.SetBool("isJumping", true);
        } else {
           animator.SetBool("isJumping", false); 
        }


        if (!animator.GetBool("isTaunting")) {
            animator.SetBool("isTaunting", taunt);
        }

        if(moveVector.x != 0.0f || moveVector.z != 0.0f) {
            animator.SetBool("isMoving", true);
        } else {
            animator.SetBool("isMoving", false);
        }

    }

    private void ProcessInput()
    {
        // Process movement
        if(moveVector.x != 0.0f || moveVector.z != 0.0f) {
            GetComponent<Rigidbody>().AddForce(moveVector * speed);
            transform.LookAt(transform.position + moveVector);
        }

        // Process firing
        // if(fire) {
        //     GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.right, transform.rotation);
        //     bullet.GetComponent<Rigidbody>().AddForce(transform.right * bulletSpeed, ForceMode.VelocityChange);
        // }

        // Process Jumping
        if(jump) {
            if (numJumps > 0) {
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                numJumps--;
            }
            
        }

        // Process Taunting
        if(taunt) {
            Debug.Log("taunted");
        }

    }

    


}
