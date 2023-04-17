using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityButton : MonoBehaviour
{
    [HideInInspector]
    public Ability ability;

    // public 

    public Image abilityIcon;

    public Image ring;

    public TextMeshProUGUI timerText;


    // Update is called once per frame
    void Update()
    {
        if (ability.cooldown > 0) {
            timerText.text = Mathf.Floor(ability.cooldown) + "";
            abilityIcon.gameObject.SetActive(false);
            ring.fillAmount = (ability.cooldownTime - ability.cooldown) / ability.cooldownTime;
        } else {
            abilityIcon.gameObject.SetActive(true);
            timerText.text = "";
            ring.fillAmount = 1f;
        }
    }
}
