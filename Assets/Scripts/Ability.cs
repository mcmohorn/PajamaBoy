using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [Tooltip("Cooldown time in seconds")] 
    public float cooldownTime;
    [HideInInspector]
    public float cooldown;

    [HideInInspector]
    public float currentCooldown = 0f;

    [HideInInspector]
    public AbilityButton button;

    [Tooltip("The rewired action name that triggers this ability")]
    public string inputButtonName;

    [Tooltip("The variable that updates in the animator")]
    public string animatorVariableName;

    [HideInInspector]
    public MyPlayerController player;

    [HideInInspector]
    public Canvas playerCanvas;

    
    [Tooltip("Time in between button press and ability happening")]
    public float castTime;
    private bool acting;

    protected bool action;
   

    protected void GetInput() 
    {
        if (player == null) {
            // Debug.Log("Player DNE");
        }
        if (player != null && player.player != null) {
            action = player.player.GetButton(inputButtonName);
        }
        
    }


    // every subclass of Ability should call CommonUpdate at th end of its Update()
    protected void CommonUpdate()
    {

         
        
        //GetInput();
        //ProcessInput();
        //SetAnimatorValues();
        
        
        // update cooldown indicator in the UI
        // if (currentCooldown > 0){
        //     currentCooldown -= Time.deltaTime;
        //     button.timerText.text = ""+Mathf.RoundToInt(currentCooldown)+"\nsec";
        //     button.controlIcon.enabled = false;
        //     button.fill.color = button.inactiveColor;
        // } else {
        //     button.controlIcon.enabled = true;
        //     button.timerText.text = "";
        //     button.fill.color = button.activeColor;
        // }
        // button.fill.fillAmount = Mathf.Lerp (button.fill.fillAmount, (cooldown - currentCooldown) / cooldown, Time.deltaTime * 5);
    }
}
