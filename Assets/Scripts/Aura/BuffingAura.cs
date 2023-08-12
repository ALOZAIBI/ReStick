using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffingAura : Aura
{
    //deals damage to enemies that this passes over
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Character") {
            Character characterAffected = collision.GetComponent<Character>();
            if (damage) {
                //deals damage to everyhing not in the casters team
                if (characterAffected.team != caster.team) {
                    applyBuff(characterAffected);
                    caster.damage(characterAffected, amt * Time.fixedDeltaTime, 0.33f);
                }
            }
            if (heal) {
                if (characterAffected.team == caster.team) {
                    applyBuff(characterAffected);
                    characterAffected.HP += amt * Time.fixedDeltaTime;
                }
            }
        }
    }
}
