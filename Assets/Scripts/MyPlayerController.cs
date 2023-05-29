using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Rewired;

public class MyPlayerController : MonoBehaviour
{
    
    [Tooltip("Toggle on/off logging and stuff")]
    public bool debugTools;

    [Tooltip("Main camera for the scene")]
    public Camera mainCamera;

    [Tooltip("The 2D canvas for drawing HUD elements")]
    public Canvas mainCanvas;

    [Tooltip("Main menu element")]
    public RectTransform menu;

    public SkinnedMeshRenderer bodyRenderer;

    // standard unity characer movement things
    public CharacterController controller;
    public Vector3 playerVelocity;
    private bool groundedPlayer;


    [Tooltip("How fast the player can move")]
    public float playerSpeed = 4.0f;

    [Tooltip("Factor by which running is faster than walking")]
    public float sprintMultiplier = 2.0f;

    [Tooltip("How high the player can jump")]
    public float jumpHeight = 3.0f;

    public float gravityValue = -9.81f;

    private float abilityButtonSpacing = 50f;
    private float abilityButtonSpacingFromBottom = 50f;

    public Transform rightHandTransform;
    public Transform leftHandTransform;

    // can hide this
    [HideInInspector]
    public GameObject interactTarget;
    
   
    public float turnSpeed; // (look sensitivity, accessed by camera controller)

    // actions
    private bool taunt;
    private bool fire;
    private bool jump;
    private bool sprint;
    private bool start;
    private bool interact;

    private Vector3 moveVector = new Vector3(0,0,0);
    public Player player; // The Rewired Player
    private int playerId = 0;
    private bool jumping = false;

    public Animator animator;

    public bool disabled = false; // coi;d 


    Vector3 rotationSpeed;
    Vector3 targetVelocity;
    Vector3 newForward = new Vector3(0,0,0);

    [Tooltip("UI Prefab instantiated for each ability")]
    public GameObject abilityButtonPrefab;

    private Ability[] abilities;

    Rigidbody rb;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        controller.minMoveDistance = 0;  // it was at .001 and jumping wouldn't work
        SetupUI();
        ResumeGame();
        rb = GetComponent<Rigidbody>();
    }

    private void SetupUI()
    {
        mainCanvas.gameObject.SetActive(true);
        abilities = gameObject.GetComponents<Ability>();
        Debug.Log("instantiating " + abilities.Length + " abilities");

        // float abw = Screen.Width *0.04f;

        // instantiate AbilityButton prefabs in the players canvas
        for (int index = 0; index < abilities.Length; index++)
        {
            var ability = abilities[index];
            ability.player = gameObject.GetComponent<MyPlayerController>();
            GameObject abilityButton = Instantiate(abilityButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
    

            var abilityButtonWidth = abilityButton.GetComponent<RectTransform>().sizeDelta.x;

            Debug.Log("ability buttons have width"+ abilityButtonWidth);

            var targetWidth = Screen.width * 0.037f;

            float scaleFactor = targetWidth/abilityButtonWidth * 1.0f;
            
            // abilityButton.GetComponent<RectTransform>().localScale = new Vector3(scaleFactor, scaleFactor, 1);
            Debug.Log("ability want to have: "+ targetWidth);
            Debug.Log("scale should be : "+ scaleFactor);
            Debug.Log("screen width : "+ Screen.width);

            Vector3 offset = new Vector3((abilityButtonWidth + abilityButtonSpacing) * -1.0f*index, abilityButtonSpacingFromBottom, 0);
            abilityButton.GetComponent<RectTransform>().localPosition += offset;
            abilityButton.transform.SetParent (mainCanvas.transform, false);
            
            ability.button = abilityButton.GetComponent<AbilityButton>();
            ability.button.ability = ability;
            ability.button.timerText.text = "";
            ability.button.abilityImage.texture = ability.icon;
            ability.button.controlAction.actionName = ability.actionName;
            // ability.button.controlAction.actionId = ability.actionId;
            


        }

    }


    void Awake() {
        if (debugTools) Debug.Log("awaking player");
        
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);

        animator = GetComponent<Animator>();
 
    }

    void Update()
    {
        if (!disabled) {
            GetInput();
            ProcessInput();
            SetAnimatorValues();
        }
        
    }

    
    void GetInput()
    {
        // receive movement input
        moveVector.x = player.GetAxis("MoveHorizontal");
        moveVector.z = player.GetAxis("MoveVertical");

        // receive button inputs
        jump = player.GetButtonDown("Jump");
        // fire = player.GetButtonDown("Fire");
        taunt = player.GetButtonDown("Taunt");
        sprint = player.GetButton("Sprint");
        start = player.GetButtonDown("Start");
        interact = player.GetButtonDown("Interact");
        
    }


    void SetAnimatorValues()
    {

        animator.SetBool("isJumping", jumping);
        animator.SetBool("isSprinting", sprint);
        
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
            playerVelocity.z = 0f;
            playerVelocity.x = 0f;
        }

        Vector3 move = new Vector3(moveVector.x, 0, moveVector.z);
        move = angle * move;

        float currSpeed = playerSpeed;

        if (sprint) {
            currSpeed = playerSpeed * sprintMultiplier;
        } 


        controller.Move(move * Time.deltaTime * currSpeed);

        if (move != Vector3.zero)
        {
            
            gameObject.transform.forward = newForward;
            // ? change this up use transform .lookat? lerp
        }


        if (jump && groundedPlayer)
        {
            jumping = true;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);


        // Process firing
        // if(fire) {
        //     GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.right, transform.rotation);
        //     bullet.GetComponent<Rigidbody>().AddForce(transform.right * bulletSpeed, ForceMode.VelocityChange);
        // }

        // Process Taunting
        if(taunt) {
            Debug.Log("taunted");
        }

        if (start) {
            if (Time.timeScale > 0) {
                PauseGame();

            } else {
                ResumeGame();
            }
        }

        if (interact && interactTarget) {
            // interact with targetted object

            if (interactTarget.GetComponent<Spaceship>()) {
                Debug.Log("get in da spacehsip");
                interactTarget.GetComponent<Spaceship>().player = gameObject;
                interactTarget.GetComponent<Spaceship>().Activate();
                //interactTarget.GetComponent<Spaceship>().cam.Priority = 1;
                
                
                interactTarget.GetComponent<SpaceshipPickup>().promptCanvas.gameObject.SetActive(false); // could move to activate
                // mainCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
                disabled = true;
                mainCanvas.gameObject.SetActive(false);
                bodyRenderer.enabled = false;
                SwitchToControlMap("Spaceship");
                // rb.constraints = RigidbodyConstraints.FreezePosition; // maybe there's a better way to disable rigidbody stuff for now,
                // could Destroy and add it back
                //gameObject.SetActive(false);
            }


        }

        

    }

    void PauseGame ()
    {
        Time.timeScale = 0;
        menu.gameObject.SetActive(true);
    }

    void ResumeGame ()
    {
        Time.timeScale = 1;
        menu.gameObject.SetActive(false);

    }

    public void SwitchToControlMap(string controlMapName)
    {
        Debug.Log("switching to " + controlMapName);
        // Disable all controller maps first for all controllers of all types
        player.controllers.maps.SetAllMapsEnabled(false);


        // The manual way - iterate all Controller Maps in a category and set the state manually
        foreach(ControllerMap map in player.controllers.maps.GetAllMapsInCategory(controlMapName)) {
            map.enabled = true; // set the enabled state on the map
        } 
        

        // Enable maps for the current game mode for all controlllers of all types
        //player.controllers.maps.SetMapsEnabled(true, controlMapName);
    }

    
}
