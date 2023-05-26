using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTarget : Ability
{
    public override void Start() {
        base.Start();
        updateDescription();
    }
    public override void doAbility() {
        if(available&&character.selectTarget(targetStrategy, rangeAbility)) {
            calculateAmt();
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
