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
            //creates the healing effect
            KeepOnTarget fx= Instantiate(prefabObject, character.target.transform.position, Quaternion.identity).GetComponent<KeepOnTarget>();
            fx.target = character.target.gameObject;
            Destroy(fx.gameObject, 1.3f);

            startCooldown();
        }
    }

    public override void updateDescription() {
        if (character != null) {
            calculateAmt();
            description = "Heals target by " + amt;
        }
        else
            description = "Heals target";
        
    }

    private void FixedUpdate() {
        cooldown();
    }
}
