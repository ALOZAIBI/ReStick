using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOnTarget : MonoBehaviour
{
    public GameObject target;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = target.transform.position;
    }
}
