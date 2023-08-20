using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffingAura : Aura
{
    [SerializeField] private CreateFXOnTargetsWithin createFXOnTargetsWithin;

    private new void Start() {
        base.Start();
        //Sets up the FXCreator
        if(createFXOnTargetsWithin!=null) {
            createFXOnTargetsWithin.caster = caster;
            createFXOnTargetsWithin.ally = heal;
            createFXOnTargetsWithin.enemy = damage;
        }
    }
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
            if (heal) {//If this targets both enemies and allies then don't target self
                if ((characterAffected.team == caster.team && !damage)||damage && characterAffected!=caster) {
                    applyBuff(characterAffected);
                    characterAffected.HP += amt * Time.fixedDeltaTime;
                }
            }
            if (saveCharacterInAura) {
                if (!charactersInAura.Contains(characterAffected)) {
                    charactersInAura.Add(characterAffected);
                }
            }
        }
    }
}
