using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOnTarget : MonoBehaviour
{
    //For now in all cases the target is also a characte but maybe that will change? Maybe need to put an fx on a Projectile maybe?
    public GameObject target;

    public Character character;

    private void Start() {
        //Checks if target is character
        if(target.GetComponent<Character>() != null) {
            character = target.GetComponent<Character>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position;
    }
}
