using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTarget : Ability
{
    private void Start() {
        updateDescription();
    }
    public override void doAbility() {
        if(available) {
            calculateAmt();
            character.selectTarget(targetStrategy);
            //heals the target
            character.target.HP += amt;
            startCooldown();
        }
    }

    public override void updateDescription() {
        try {
            calculateAmt();
        }
        catch { /* avoids null character issue*/}
        description = "Heals target by " + amt;
    }

    private void FixedUpdate() {
        cooldown();
    }
}
