using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBuff : Ability
{
    //code used to verify if a specific buff is already applied. compare buff to be instantiated's code to the target character's buffs
    public string code;
    public float buffDuration;

    private void Start() {
        updateDescription();
        code = Random.state+"";
    }
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

    public override void updateDescription() {
        Buff temp = prefabObject.GetComponent<Buff>();
        description = "Give target ";
        if (temp.DMG != 0)
            description += temp.DMG + " DMG ";
        if (temp.HP != 0)
            description += temp.HP + " HP ";
        if (temp.AS != 0)
            description += temp.AS + " AS ";
        if (temp.MS != 0)
            description += temp.MS + " MS ";
        if (temp.Range != 0)
            description += temp.Range + " Range";
        if (temp.LS != 0)
            description += temp.LS + " LS ";
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
