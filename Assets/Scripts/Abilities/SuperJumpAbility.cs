using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpAbility : Ability
{

    public float jumpPowerHorizontal;
    public float jumpPowerVertical;

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

        CommonUpdate();
    }

    void BigJump() 
    {
        player.playerVelocity.y += jumpPowerVertical;
        player.playerVelocity.z += jumpPowerHorizontal;
    }
    
}
