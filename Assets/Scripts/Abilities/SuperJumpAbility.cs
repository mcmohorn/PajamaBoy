using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpAbility : Ability
{

    public float jumpPowerHorizontal;
    public float jumpPowerVertical;

    public Vector3 jumpVector;
    private bool isJumping;

    void Start()
    {
        jumpVector = new Vector3(0,0,0);
        isJumping = false;
    }

    void Update()
    {
        GetInput();

        if (player.debugTools) {
            Debug.DrawRay(player.transform.position, jumpVector, Color.red);
        }

        if (isJumping && player.controller.isGrounded) {
            player.animator.SetBool(animatorVariableName, false);
            isJumping = false;
        }

        if (cooldown > 0) {
            // player.animator.SetBool(animatorVariableName, false);
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
        isJumping = true;
        jumpVector = player.transform.forward;
        jumpVector.y += jumpPowerVertical;
        jumpVector *= jumpPowerHorizontal;

        player.playerVelocity += jumpVector;
    }
    
}
