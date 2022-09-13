using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CameraController : MonoBehaviour {

    public GameObject player;

		private Player rewiredPlayer;

    public float verticalOffset;

		public float lookSpeed;
    public float radius;

		float horizontalOffset;


		Vector3 origin;

		Vector2 lookVector;


void Start() {
	horizontalOffset = 0;
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

		transform.position =  new Vector3(origin.x + radius*Mathf.Sin(horizontalOffset), origin.y + verticalOffset, origin.z - radius*Mathf.Cos(horizontalOffset));

// 		transform.position = player.transform.position + new Vector3( Vector3.up * verticalOffset - (horizontalOffset) * player.transform.forward;

		transform.LookAt(player.transform.position + player.transform.up);

	}
}
