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

    public bool piloting = false;


    bool eject = false;
    bool thrust = false;


    void Start()
    {
        spaceshipHUD.gameObject.SetActive(false); // should activate HUD through Activate()
    }


    void Update()
    {
        
        if (piloting) {

            GetInput();
            ProcessInput();
            
        }
        
    }

    void GetInput()
    {
        eject = player.GetComponent<MyPlayerController>().player.GetButtonDown("Eject");
        thrust = player.GetComponent<MyPlayerController>().player.GetButtonDown("Thrust");
    }

    void ProcessInput() 
    {
        if (eject) {
            Debug.Log("time to eject!");
            Eject();
        }
    }

    // power up the spaceship
    public void Activate()
    {
        piloting = true; 

        // turn on HUD
        spaceshipHUD.gameObject.SetActive(true); 
        

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
}
