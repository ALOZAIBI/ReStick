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
            if (PD > 0) {
                buff.PD += amt * PD;
            }

            buff.MD = MD;
            if (MD > 0) {
                buff.MD += amt * MD;
            }

            buff.HP = HP;
            if (HP > 0) {
                buff.HP += amt * HP;
            }

            buff.AS = AS;
            if (AS > 0) {
                buff.AS += amt * AS;
            }

            buff.CDR = CDR;
            if (CDR > 0) {
                buff.CDR += amt * CDR;
            }

            buff.MS = MS;
            if (MS > 0) {
                buff.MS += amt * MS;
            }

            buff.Range = Range;
            if (Range > 0) {
                buff.Range += amt * Range;
            }

            buff.LS = LS;
            if (LS > 0) {
                buff.LS += amt * LS;
            }

            buff.size = size;
            if (size > 0) {
                buff.size += amt * size;
            }



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
        calculateAmt();
        description = "Give target ";
        if (PD != 0)
            description += (PD+(PD*amt)) + " PD ";
        if (MD != 0)
            description += (MD+(MD*amt)) + " MD ";
        if (HP != 0)
            description += (HP + (HP * amt)) + " HP ";
        if (AS != 0)
            description += (AS + (AS * amt)) + " AS ";
        if (CDR != 0)
            description += (CDR + (CDR * amt)) + " CDR ";
        if (MS != 0)
            description += (MS + (MS * amt)) + " MS ";
        if (Range != 0)
            description += (Range + (Range * amt)) + " Range ";
        if (LS != 0)
            description += (LS + (LS * amt)) + " LS ";
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
