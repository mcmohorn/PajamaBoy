using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CameraController : MonoBehaviour {

    	public GameObject player;

		private Player rewiredPlayer;

		public float startHeight;

		private float lookSpeed;
    	public float radiusX;
		public float radiusY;
		float verticalOffset;
		float horizontalOffset;

		public Vector3 targetOffset;



		Vector3 origin;

		Vector2 lookVector;


void Start() {
	horizontalOffset = 0;
	lookSpeed = player.GetComponent<MyPlayerController>().turnSpeed;
	rewiredPlayer = ReInput.players.GetPlayer(0);
}

void GetInput()
    {
        // receive movement input
        lookVector.x = rewiredPlayer.GetAxis("LookHorizontal"); // get input by name or action id
        lookVector.y = rewiredPlayer.GetAxis("LookVertical");
           
    }


	void LateUpdate () {
		GetInput();

		origin = player.transform.position;

		if (lookVector.x != 0) {
			horizontalOffset += (lookVector.x * lookSpeed);
		}

		verticalOffset = startHeight + radiusY*Mathf.Sin(Mathf.PI/2.0f*lookVector.y);

		transform.position =  new Vector3(origin.x + radiusX*Mathf.Sin(horizontalOffset), origin.y + verticalOffset, origin.z - radiusX*Mathf.Cos(horizontalOffset));

		transform.LookAt(player.transform.position + targetOffset);

	}
}
