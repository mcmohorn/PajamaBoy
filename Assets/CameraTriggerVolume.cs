using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(BoxCollider))]

public class CameraTriggerVolume : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cam;
    BoxCollider coll;


    void Awake () 
    {
        coll = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Switch cameras");
        }
    }
}
