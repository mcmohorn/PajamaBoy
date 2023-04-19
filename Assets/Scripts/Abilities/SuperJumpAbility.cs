using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpAbility : Ability
{

    // [Tooltip("Prefab of animal summoning object, should have AnimalSummon script attached")]
    // public GameObject summonPrefab;

    public float jumpAngle;
    public float jumpPower;

    Vector3 jumpDirection;

    void Start()
    {
        jumpDirection = new Vector3(0,0,0);
    }

    void Update()
    {
        GetInput();

        if (cooldown > 0) {
            player.animator.SetBool(animatorVariableName, false);
            cooldown -= Time.deltaTime;  
        }

        else if (action) {
            cooldown = cooldownTime;
            player.animator.SetBool(animatorVariableName, true);
            BigJump();

        }
        jumpDirection = Quaternion.Euler(-1.0f*jumpAngle, 0, 0) * player.transform.forward * jumpPower;
        Debug.DrawRay(transform.position+Vector3.up, jumpDirection, Color.red);

        CommonUpdate();
    }

    void BigJump() {
        jumpDirection = Quaternion.Euler(-1.0f*jumpAngle, 0, 0) * player.transform.forward * jumpPower;

        player.GetComponent<Rigidbody>().AddForce(new Vector3(0, 10,10));
    }
    
}
