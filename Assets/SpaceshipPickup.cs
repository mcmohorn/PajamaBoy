using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipPickup : MonoBehaviour
{

    public Canvas promptCanvas;
    // Start is called before the first frame update
    void Start()
    {
        promptCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player")) {
            //promptCanvas.SetActive(true);
            promptCanvas.gameObject.SetActive(true);
            if (other.gameObject.GetComponent<MyPlayerController>()) {
                other.gameObject.GetComponent<MyPlayerController>().interactTarget = gameObject;
            }

        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.CompareTag("Player")) {
            //promptCanvas.SetActive(false);
            promptCanvas.gameObject.SetActive(false);
            if (other.gameObject.GetComponent<MyPlayerController>()) {
                other.gameObject.GetComponent<MyPlayerController>().interactTarget = null;
            }
        }
    }
}
