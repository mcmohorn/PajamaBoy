using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Spaceship : MonoBehaviour
{

    public Canvas pickupUI;
    public CinemachineVirtualCamera cam;
    public GameObject player;

    public Transform ejectionPoint;

    public bool piloting = false;


    bool interact = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (piloting) {
            if (interact) {
                Debug.Log("time to eject!");
                Eject();
            }
        }
        
    }

    void GetInput()
    {
        
        interact = player.GetComponent<MyPlayerController>().player.GetButtonDown("Interact");
        
    }

    void Eject()
    {
        piloting = false;

        // reposition player and spawn
        player.transform.position = ejectionPoint.position;
        player.transform.rotation = transform.rotation;
        player.SetActive(true);

        // switch cameras
        cam.Priority = 0;
        player.GetComponent<MyPlayerController>().mainCamera.GetComponent<CinemachineVirtualCamera>().Priority = 1;
    }
}
