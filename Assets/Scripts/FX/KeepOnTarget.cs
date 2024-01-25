using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOnTarget : MonoBehaviour
{
    public GameObject target;
    

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position;
    }
}
