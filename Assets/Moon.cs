using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{

    public GameObject focus;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         Vector3 lookPoint = transform.position - focus.transform.position;
         lookPoint.y = focus.transform.position.y;
         transform.LookAt(focus.transform.position);
    }
}
