using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public Character character;
    public string abilityName;
    public string description;

    //abilities cd
    public float CD;

    //checked if available in the ability's script
    public bool available=true;
    public float abilityNext = 0;

    //player can choose to add delay before an ability is executed
    public float delay;

    //the float value used in an ability what it is used for depends on the ability
    public float amt;

    //someAbilities can instantiate prefabs or object
    public GameObject prefabObject;


    //executes this ability
    public abstract void doAbility();
    //Abilities targetStrategy
    public int targetStrategy;


    //if an ability has a cooldown call this inside doAbility()
    public void startCooldown() {
        abilityNext = CD;
        available = false;
    }
    //and put this in the fixedupdate function
    public void cooldown() {
        if (abilityNext > 0) {
            abilityNext -= Time.fixedDeltaTime;
        }
        else {
            abilityNext = 0;
            available = true;
        }
    }
 
}
