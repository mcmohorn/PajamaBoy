using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MyPlayerController : MonoBehaviour
{
    
    
    
    [Tooltip("Toggle on/off to show helpful Raycasts etc.")]
    public bool debugTools;

    [Tooltip("Main camera for the scene")]
    public Camera mainCamera;

    // standard unity characer movement things
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 4.0f;
    private float jumpHeight = 3.0f;
    private float gravityValue = -9.81f;
    
   
    public float turnSpeed;

    private bool taunt;

    private bool fire;
    private bool jump;

    private int maxJumps =1;
    private int numJumps =0;

    private Vector3 moveVector = new Vector3(0,0,0);
    private Player player; // The Rewired Player
    private int playerId = 0;
    private bool jumping = false;

    Animator animator;


    Vector3 rotationSpeed;
    Vector3 targetVelocity;
    Vector3 newForward = new Vector3(0,0,0);

    

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        controller.minMoveDistance = 0;  // it was at .001 and jumping wouldn't work
    }


    void Awake() {
        if (debugTools) Debug.Log("awaking player");
        
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        GetInput();
        ProcessInput();
        SetAnimatorValues();
        
    }

    
    void GetInput()
    {
        // receive movement input
        moveVector.x = player.GetAxis("MoveHorizontal");
        moveVector.z = player.GetAxis("MoveVertical");

        // receive button inputs
        jump = player.GetButtonDown("Jump");
        fire = player.GetButtonDown("Fire");
        taunt = player.GetButtonDown("Taunt");
        
    }


    void SetAnimatorValues()
    {

        animator.SetBool("isJumping", jumping);

        if (!animator.GetBool("isTaunting")) {
            animator.SetBool("isTaunting", taunt);
        }

        if(moveVector.x != 0.0f || moveVector.z != 0.0f) {
            animator.SetBool("isMoving", true);
        } else {
            animator.SetBool("isMoving", false);
        }

        animator.SetFloat("vy", controller.velocity.y);

    }

    private void ProcessInput()
    {
        newForward = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z); 

        Quaternion angle=Quaternion.FromToRotation(Vector3.forward, newForward);

        if (debugTools) {

            Debug.DrawRay(transform.position, moveVector*3.0f, Color.red);
            Debug.DrawRay(transform.position, transform.forward, Color.blue);
            Debug.DrawRay(transform.position, newForward, Color.black);
        }


        groundedPlayer = controller.isGrounded;

        if (groundedPlayer) {
            jumping = false;
        }

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(moveVector.x, 0, moveVector.z);
        move = angle * move;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            
            gameObject.transform.forward = newForward;
            // ? change this up use transform .lookat? lerp
        }


    // Changes the height position of the player..
        if (jump && groundedPlayer)
        {
            jumping = true;
            Debug.Log("JUMP");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);


        // Process firing
        // if(fire) {
        //     GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.right, transform.rotation);
        //     bullet.GetComponent<Rigidbody>().AddForce(transform.right * bulletSpeed, ForceMode.VelocityChange);
        // }


        
        // Process Jumping
        if(jump) {
            Debug.Log("should jump... grounded?" + groundedPlayer );
            
        } else {

        }

        // Process Taunting
        if(taunt) {
            Debug.Log("taunted");
        }



    }

    


}
