using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBuff : Ability
{
    //code used to verify if a specific buff is already applied. compare buff to be instantiated's code to the target character's buffs
    public string code;
    public float buffDuration;

    public override void doAbility() {
        //selects target
        character.selectTarget(targetStrategy);
        if (available && buffNotOnTarget()) {
            //creates buff
            Buff buff = Instantiate(prefabObject).GetComponent<Buff>();
            //sets caster and target
            buff.caster = character;
            buff.target = character.target;
            buff.duration = buffDuration;
            buff.code = code;
            //adds buff to target's buff list
            character.target.buffs.Add(buff);
            //applies the buff
            buff.applyBuff();
            //cooldown this ability
            startCooldown();
        }
    }

    public bool buffNotOnTarget() {
        try {
            Debug.Log("try");
            foreach(Buff temp in character.target.buffs) {
                Debug.Log(temp.code);
                //if buff is already applied refresh it's duration
                if (temp.code == code) {
                    temp.durationRemaining = buffDuration;
                    Debug.Log("try first if");
                    return false;
                }
            } 
        }
        catch { Debug.Log("catch)"); return true; };
        Debug.Log("Last true");
        //otherwise return True which does doAbility()
        return true;
    }

    private void FixedUpdate() {
        cooldown();
    }
}
