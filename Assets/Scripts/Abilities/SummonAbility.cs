using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonAbility : Ability
{

    [Tooltip("Prefab of animal summoning object, should have AnimalSummon script attached")]
    public GameObject summonPrefab;

    public float erodeRate = 0.03f;
    public float erodeRefreshRate = 0.01f;
    public float erodeDelay = 1.25f;
    public float speed;
    public SkinnedMeshRenderer objectToErode;

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
            StartCoroutine(SummonCreature());
        }


        CommonUpdate();
    }

    public IEnumerator SummonCreature()
    {
        yield return new WaitForSeconds(castTime);
        GameObject summonedCreature = (GameObject)Instantiate(summonPrefab, transform.position + transform.forward, transform.rotation);
        float creatureSpeed = summonedCreature.GetComponent<SummonAbility>().speed;
        summonedCreature.SetActive(true);
        objectToErode = summonedCreature.GetComponent<SkinnedMeshRenderer>();
        StartCoroutine(summonedCreature.GetComponent<SummonAbility>().ErodeObject());
        summonedCreature.GetComponent<Rigidbody>().velocity = transform.forward * creatureSpeed;
        Destroy(summonedCreature, 3f);

    }

    public IEnumerator ErodeObject()
    {
        yield return new WaitForSeconds(erodeDelay);
        float t = 0;
        while (t<1) {
            t+=erodeRate;
            objectToErode.material.SetFloat("_erode", t);
            
            yield return new WaitForSeconds(erodeRefreshRate);
        }

    }
}
