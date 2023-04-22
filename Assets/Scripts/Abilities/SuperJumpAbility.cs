using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpAbility : Ability
{

    public float jumpPowerHorizontal;
    public float jumpPowerVertical;

    public Vector3 jumpVector;

    void Start()
    {
        jumpVector = new Vector3(0,0,0);
    }

    void Update()
    {
        GetInput();

        Debug.DrawRay(player.transform.position, jumpVector, Color.red);

        if (cooldown > 0) {
            player.animator.SetBool(animatorVariableName, false);
            cooldown -= Time.deltaTime;
        }

        else if (action) {
            cooldown = cooldownTime;
            player.animator.SetBool(animatorVariableName, true);
            BigJump();
        }

        CommonUpdate();
    }

    void BigJump() 
    {
        jumpVector = player.transform.forward;
        jumpVector.y += jumpPowerVertical;
        jumpVector *= jumpPowerHorizontal;


        
        player.playerVelocity += jumpVector;
    }
    
}
