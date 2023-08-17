using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnKill : Ability {
    //amt here is the heal amt
    public override void Start() {
        base.Start();
        updateDescription();
    }
    //Heals character depending on the target's level, max Heal at target level 5
    public override void doAbility() {
        calculateAmt();
        foreach(int i in character.averageLevelOfKillsLastFrame) {
            character.HP+= Mathf.Clamp01(((i - 0.4f) * 0.25f)) *valueAmt.getAmtValueFromName(this,"Heal");
        }
        Debug.Log(character.killsLastFrame+ character.gameObject.name);
    }

    public override void updateDescription() {
        if (character != null) {
            calculateAmt();
            description = "Heals Character by " + valueAmt.getAmtValueFromName(this,"Heal") + " after every kill";
        }
        else {
            description = "Heals Character after every kill";
        }
    }
}
