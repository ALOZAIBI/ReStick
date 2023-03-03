using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBuff : Ability
{
    public float DMG;
    public float HP;
    public float AS;
    public float MS;
    public float Range;
    public float LS;

    public float size;


    //code used to verify if a specific buff is already applied. compare buff to be instantiated's code to the target character's buffs
    public string code;
    public float buffDuration;

    private void Start() {
        updateDescription();
        code = Random.Range(-50,500) +"";
    }
    public override void doAbility() {
        //selects target
        character.selectTarget(targetStrategy);
        if (available && buffNotOnTarget()) {
            //creates buff
            Buff buff = Instantiate(prefabObject).GetComponent<Buff>();
            buff.DMG = DMG;
            buff.HP = HP;
            buff.AS = AS;
            buff.MS = MS;
            buff.Range = Range;
            buff.LS = LS;
            buff.size = size;

            //TO BE DONE
            //multiply buff stats by ability amt

            //sets caster and target
            buff.caster = character;
            buff.target = character.target;
            buff.duration = buffDuration;
            buff.code = code;
            //applies the buff
            buff.applyBuff();
            //cooldown this ability
            startCooldown();
        }
    }

    public override void updateDescription() {
        description = "Give target ";
        if (DMG != 0)
            description += DMG + " DMG ";
        if (HP != 0)
            description += HP + " HP ";
        if (AS != 0)
            description += AS + " AS ";
        if (MS != 0)
            description += MS + " MS ";
        if (Range != 0)
            description += Range + " Range ";
        if (LS != 0)
            description += LS + " LS ";
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
