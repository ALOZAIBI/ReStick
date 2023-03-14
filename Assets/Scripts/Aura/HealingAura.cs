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
            if (enemy && aTarget.team != caster.team) {
                aTarget.HP += amt * Time.fixedDeltaTime;
                if (aTarget.HP <= 0) {
                    caster.kill(aTarget);
                }
            }

        }
    }
}
