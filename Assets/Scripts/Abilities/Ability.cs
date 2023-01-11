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
    public float abilityNext = 0;

    //player can choose to add delay before an ability is executed
    public float delay;

    //the int value used in a skill
    public int amt;


    //executes this ability
    public abstract void doAbility();



    public void startCooldown() {
        abilityNext = CD;
        available = false;
    }
    public void cooldown() {
        if (abilityNext > 0) {
            abilityNext -= Time.deltaTime;
        }
        else {
            abilityNext = 0;
            available = true;
        }
    }
    private void Update() {
        
    }
}
