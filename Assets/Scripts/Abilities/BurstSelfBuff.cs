using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstSelfBuff : Ability
{
    public float PD;
    public float MD;
    public float INF;
    public float HP;
    public float AS;
    public float CDR;
    public float MS;
    public float Range;
    public float LS;

    public float size;

    //used to root silence and blind etc..
    public bool root;
    public bool silence;
    public bool blind;

    //code used to verify if a specific buff is already applied. compare buff to be instantiated's code to the target character's buffs
    public string code;
    public float buffDuration;

    public override void Start() {
        base.Start();
        updateDescription();
    }
    public override void doAbility() {
        //selects target
        if (available && buffNotOnTarget()) {
            //AA Reset 
            character.AtkNext = 0;

            calculateAmt();
            //creates buff
            Buff buff = Instantiate(prefabObject).GetComponent<Buff>();
            buff.PD = PD;
            if (PD > 0) {
                buff.PD += amt;
            }

            buff.MD = MD;
            if (MD > 0) {
                buff.MD += amt;
            }

            buff.INF = INF;
            if (INF > 0) {
                buff.INF += amt;
            }

            buff.HP = HP;
            if (HP > 0) {
                buff.HP += amt;
            }

            buff.AS = AS;
            if (AS > 0) {
                
                buff.AS += amt;
            }

            buff.CDR = CDR;
            if (CDR > 0) {
                buff.CDR += amt;;
            }

            buff.MS = MS;
            if (MS > 0) {
                buff.MS += amt;
            }

            buff.Range = Range;
            if (Range > 0) {
                buff.Range += amt;
            }

            buff.LS = LS;
            if (LS > 0) {
                buff.LS += amt*0.01f;
            }

            buff.size = size;
            if (size > 0) {
                buff.size += amt;
            }


            //sets caster and target
            buff.caster = character;
            buff.target = character;
            //increases buff duration according to AMT
            buff.duration = buffDuration;
            buff.code = abilityName+character.name;
            //applies the buff
            buff.applyBuff();
            //cooldown this ability
            startCooldown();
        }
    }

    public override void updateDescription() {
        calculateAmt();
        description = "Give Me ";
        if (PD != 0)
            description += (PD+amt)+ " PD ";
        if (MD != 0)
            description += (MD+amt) + " MD ";
        if (INF != 0)
            description += (INF + amt) + " INF ";
        if (HP != 0)
            description += (HP +amt) + " HP ";
        if (AS != 0)
            description += (AS +amt) + " AS ";
        if (CDR != 0)
            description += (CDR + amt) + " CDR ";
        if (MS != 0)
            description += (MS +amt) + " MS ";
        if (Range != 0)
            description += (Range + (amt)) + " Range ";
        if (LS != 0)
            description += (LS +amt*0.01f) + " LS ";

        if (root || silence || blind)
            description += ". ";
        if (root && silence && blind) {
            description += "Stun target ";
        }
        else {
            if (root)
                description += "Root target ";
            if (silence)
                description += "Silence target";
            if (blind)
                description += "Blind target";
        }
        description += "for " + ((amt / 10) + buffDuration).ToString("F2") + " seconds";
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
