using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemPrompt : MonoBehaviour
{
    Vector3 targetDir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // targetDir = gameObject.transform.position - Camera.main.transform.position;
        // Transform target = transform;
        // target.position += targetDir;
        transform.LookAt(Camera.main.transform);
        this.transform.Rotate(0,180,0);
    }
}
