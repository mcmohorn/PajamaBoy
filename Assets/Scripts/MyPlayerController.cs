using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MyPlayerController : MonoBehaviour
{

    private int playerId = 0;
    
    public float speed;
    public float backwardSpeed;
    public float turnSpeed;
    public float accelerationScale;

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

    Vector3 rotationSpeed;
    Vector3 targetVelocity;
    




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

        animator.SetFloat("vy", rb.velocity.y);

    }

    private void ProcessInput()
    {
        // Process movement
        if(moveVector.z != 0.0f) {
                // q1// you were here
            if (moveVector.z < 0) {
                targetVelocity = transform.forward * backwardSpeed * moveVector.z;
            } else {
                targetVelocity = transform.forward * speed * moveVector.z;
            }
           // GetComponent<Rigidbody>().AddForce(moveVector * speed);
            // transform.LookAt(transform.position + moveVector);

            Vector3 force = (targetVelocity - rb.velocity) * accelerationScale;
            rb.AddForce(force);
        }

        // process rotation
        if (moveVector.x != 0.0f) {
            rotationSpeed = new Vector3(0, moveVector.x * turnSpeed , 0);
             Quaternion deltaRotation = Quaternion.Euler(rotationSpeed * Time.fixedDeltaTime);
             rb.MoveRotation(rb.rotation * deltaRotation);
        }
       

        // Process firing
        // if(fire) {
        //     GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.right, transform.rotation);
        //     bullet.GetComponent<Rigidbody>().AddForce(transform.right * bulletSpeed, ForceMode.VelocityChange);
        // }

        // Process Jumping
        if(jump) {
            if (numJumps > 0) {
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower);
                
                numJumps--;
            }
            
        } else {
            rb.AddForce(Physics.gravity, ForceMode.Acceleration);
        }

        // Process Taunting
        if(taunt) {
            Debug.Log("taunted");
        }

        // // apply gravity if not touching a surface
        // if (midair) {
            
        // }

    }

    


}
