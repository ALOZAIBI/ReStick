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
        character.HP+= character.killsLastFrame * valueAmt.getAmtValueFromName(this,"Heal");   
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
