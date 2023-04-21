using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuffinAbility : Ability
{

    [Tooltip("Prefab of muffin to throw")]
    public GameObject muffinPrefab;

    private GameObject activeMuffin;

    [Tooltip("Time it takes to release the muffin")]
    public float tossTime;

    public float throwForceForward;
    public float throwForceUp;

    public float muffinLifetime;

    private bool tossing = false;

    Vector3 throwVector;

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
            StartCoroutine(ThrowMuffin());
        }

        if (activeMuffin && tossing) {
            activeMuffin.transform.position = player.rightHandTransform.position;
        }


        CommonUpdate();
    }

    public IEnumerator ThrowMuffin()
    {
        throwVector = player.transform.forward;
        throwVector.z *= throwForceForward;
        throwVector.y *= throwForceUp;
        yield return new WaitForSeconds(castTime);
        tossing = true;
        activeMuffin = (GameObject)Instantiate(muffinPrefab, player.rightHandTransform.position, player.rightHandTransform.rotation);
        yield return new WaitForSeconds(tossTime);
        tossing = false;
        activeMuffin.GetComponent<Rigidbody>().AddForce(throwVector);
        activeMuffin.GetComponent<CapsuleCollider>().enabled = true; 

        Destroy(activeMuffin, muffinLifetime);
         //StartCoroutine(DestroyMuffin());
    }

    // public IEnumerator DestroyMuffin()
    // {

    //     yield return new WaitForSeconds(muffinLifetime);

    // }

   
}
