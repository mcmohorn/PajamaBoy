using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NullSave;

public class AbilityButton : MonoBehaviour
{
    [HideInInspector]
    public Ability ability;

    public RawImage abilityImage;

    public ReIconedImageAction controlAction;

    public Image ring;

    public TextMeshProUGUI timerText;

    RawImage img;


    


    // Update is called once per frame
    void Update()
    {
        if (ability.cooldown > 0) {
            timerText.text = Mathf.Floor(ability.cooldown) + "";
            abilityImage.gameObject.SetActive(false);
            ring.fillAmount = (ability.cooldownTime - ability.cooldown) / ability.cooldownTime;
        } else {
            abilityImage.gameObject.SetActive(true);
            timerText.text = "";
            ring.fillAmount = 1f;
        }
    }
}
