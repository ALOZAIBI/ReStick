using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnKill : Ability {
    //amt here is the heal amt
    public override void Start() {
        base.Start();
        updateDescription();
    }
    public override void doAbility() {
        calculateAmt();
        character.HP += character.killsLastFrame * amt;
        Debug.Log(character.killsLastFrame+ character.gameObject.name);
    }

    public override void updateDescription() {
        try {
            calculateAmt();
        }
        catch { /* avoids null character issue*/}
        description = "Heals Character by " + amt + " after every kill";
    }
}
