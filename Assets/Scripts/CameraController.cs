using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;

    public float verticalOffset;
    public float horizontalOffset;

    public float targetOffset;


	void LateUpdate () {


		transform.position = player.transform.position + Vector3.up * verticalOffset - (horizontalOffset) * Vector3.forward;

		transform.LookAt(player.transform.position + player.transform.forward * targetOffset);

	}
}
