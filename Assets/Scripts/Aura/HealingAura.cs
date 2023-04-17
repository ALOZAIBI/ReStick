using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAura : Aura {
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Character") {
            Character aTarget = collision.GetComponent<Character>();
            //wether this targets ally or enemy or both is determined in the superclass (aura)
            if (ally && aTarget.team == caster.team) {
                aTarget.HP += amt * Time.fixedDeltaTime;
            }
            //in this case if the aura targets enemy it is most likely to damage so we can call the damage script here
            if (enemy && aTarget.team != caster.team) {
                caster.damage(aTarget,-amt * Time.fixedDeltaTime,false);
            }

        }
    }
}
