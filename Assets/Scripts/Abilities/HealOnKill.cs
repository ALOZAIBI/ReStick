using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnKill : Ability {
    //amt here is the heal amt
    private void Start() {
        description = "Heals Character by " + amt + " after every kill";
    }
    public override void doAbility() {
        character.HP += character.killsLastFrame * amt;
        Debug.Log(character.killsLastFrame+ character.gameObject.name);
    }
}
