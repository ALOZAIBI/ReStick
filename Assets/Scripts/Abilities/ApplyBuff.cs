using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBuff : Ability
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
        if (character.selectTarget(targetStrategy,rangeAbility) && available) {
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

            buff.INF = INF;
            if (INF > 0) {
                buff.INF += amt * INF;
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

            buff.snare = root;
            buff.silence = silence;
            buff.blind = blind;

            //sets caster and target
            buff.caster = character;
            buff.target = character.target;
            //increases buff duration according to AMT
            buff.duration = buffDuration;
            buff.duration += amt / 6;
            buff.code = abilityName + character.name;
            //applies the buff
            buff.applyBuff();
            refreshDuration();
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
        if (INF != 0)
            description += (INF + (INF * amt)) + " INF ";
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
        description += "for " + ((amt / 6) + buffDuration).ToString("F2") + " seconds";
    }

    //refreshes duration of other stacks of the same buff.
    public void refreshDuration() {
        try {
            foreach(Buff temp in character.target.buffs) {
                //if buff is already applied refresh it's duration
                if (temp.code == abilityName + character.name) {
                    temp.durationRemaining = buffDuration;
                }
            } 
        }
        catch {};
    }

    private void FixedUpdate() {
        cooldown();
    }
}
