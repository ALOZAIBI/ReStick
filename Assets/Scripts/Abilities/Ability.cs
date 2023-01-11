using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public Character character;
    //ability's description
    public string description;
    public float CD;

    public bool available;

    //player can choose to add delay before an ability is executed
    public float delay;

    //the int value used in a skill
    public int amt;


    //executes this ability
    public abstract void doAbility();

    //Use StartCoroutine(StartCooldown()); at the end of doAbility to make the ability go on CD if it is an ability that goes on CD


    public IEnumerator StartCooldown() {
        available = false;
        yield return new WaitForSeconds(CD);
        available = true;
            
    }
}
