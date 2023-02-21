using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAura : Aura {
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Character") {
            Character aTarget = collision.GetComponent<Character>();
            if (aTarget.team == caster.team) {
                aTarget.HP += amt * Time.fixedDeltaTime;
            }
        }
    }
}
