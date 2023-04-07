using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSummon : MonoBehaviour
{

    public float erodeRate = 0.03f;
    public float erodeRefreshRate = 0.01f;
    public float erodeDelay = 1.25f;

    public float castTime = 1.0f;

    public float speed;

    public SkinnedMeshRenderer objectToErode;

    // Start is called before the first frame update
    void Start()
    {

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
