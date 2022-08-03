using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;

    public float verticalOffset;
    public float horizontalOffset;



	void LateUpdate () {


		transform.position = player.transform.position + Vector3.up * verticalOffset - (horizontalOffset) * player.transform.forward;

		transform.LookAt(player.transform.position + player.transform.up);

	}
}
