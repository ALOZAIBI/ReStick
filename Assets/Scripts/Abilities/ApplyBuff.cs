using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBuff : Ability
{
    public float PD;
    public float MD;
    public float HP;
    public float AS;
    public float CDR;
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
        if (character.target!=null && available && buffNotOnTarget()) {
            calculateAmt();
            //creates buff
            Buff buff = Instantiate(prefabObject).GetComponent<Buff>();
            buff.PD = PD;
            buff.MD = MD;
            buff.HP = HP;
            buff.AS = AS;
            buff.CDR = CDR;
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
        if (PD != 0)
            description += PD + " PD ";
        if (MD != 0)
            description += MD + " MD ";
        if (HP != 0)
            description += HP + " HP ";
        if (AS != 0)
            description += AS + " AS ";
        if (CDR != 0)
            description += CDR + " CDR ";
        if (MS != 0)
            description += MS + " MS ";
        if (Range != 0)
            description += Range + " Range ";
        if (LS != 0)
            description += LS + " LS ";
    }

    public bool buffNotOnTarget() {
        try {
            foreach(Buff temp in character.target.buffs) {
                //if buff is already applied refresh it's duration
                if (temp.code == code) {
                    temp.durationRemaining = buffDuration;
                    return false;
                }
            } 
        }
        catch { return true; };
        //otherwise return True which does doAbility()
        return true;
    }

    private void FixedUpdate() {
        cooldown();
    }
}
