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

    public override void Start() {
        base.Start();
        updateDescription();
    }
    public override void doAbility() {
        //selects target
        if (available && buffNotOnTarget()) {
            calculateAmt();
            playAnimation("castRaise");
        }
    }

    public override void executeAbility() {
        //AA Reset 
        character.AtkNext = 0;
        //creates buff
        Buff buff = Instantiate(prefabObject).GetComponent<Buff>();
        buff.PD = PD;
        if (PD > 0) {
            buff.PD += PD*valueAmt.getAmtValueFromName(this, "BuffStrength");
        }

        buff.MD = MD;
        if (MD > 0) {
            buff.MD += MD*valueAmt.getAmtValueFromName(this, "BuffStrength");
        }

        buff.INF = INF;
        if (INF > 0) {
            buff.INF += INF*valueAmt.getAmtValueFromName(this, "BuffStrength");
        }

        buff.HP = HP;
        if (HP > 0) {
            buff.HP += HP*valueAmt.getAmtValueFromName(this, "BuffStrength");
        }

        buff.AS = AS;
        if (AS > 0) {

            buff.AS += AS*valueAmt.getAmtValueFromName(this, "BuffStrength");
        }

        buff.CDR = CDR;
        if (CDR > 0) {
            buff.CDR += CDR*valueAmt.getAmtValueFromName(this, "BuffStrength"); ;
        }

        buff.MS = MS;
        if (MS > 0) {
            buff.MS += MS*valueAmt.getAmtValueFromName(this, "BuffStrength");
        }

        buff.Range = Range;
        if (Range > 0) {
            buff.Range += Range*valueAmt.getAmtValueFromName(this, "BuffStrength");
        }

        buff.LS = LS;
        if (LS > 0) {
            buff.LS += LS*valueAmt.getAmtValueFromName(this, "BuffStrength");
        }

        buff.size = size;
        if (size > 0) {
            buff.size += size*valueAmt.getAmtValueFromName(this, "BuffStrength");
        }


        //sets caster and target
        buff.caster = character;
        buff.target = character;
        //increases buff duration according to AMT
        buff.duration = valueAmt.getAmtValueFromName(this, "Duration");
        buff.code = abilityName + character.name;
        //applies the buff
        buff.applyBuff();
        //cooldown this ability
        startCooldown();
    }

    public override void updateDescription() {
        if (character != null) {
            calculateAmt();
            description = "Give Me ";
            if (PD != 0)
                description += (PD + valueAmt.getAmtValueFromName(this, "BuffStrength")) + " PWR ";
            if (MD != 0)
                description += (MD + valueAmt.getAmtValueFromName(this, "BuffStrength")) + " MGC ";
            if (INF != 0)
                description += (INF + valueAmt.getAmtValueFromName(this, "BuffStrength")) + " INF ";
            if (HP != 0)
                description += (HP + valueAmt.getAmtValueFromName(this, "BuffStrength")) + " HP ";
            if (AS != 0)
                description += (AS + valueAmt.getAmtValueFromName(this, "BuffStrength")) + " AS ";
            if (CDR != 0)
                description += (CDR + valueAmt.getAmtValueFromName(this, "BuffStrength")) + " CDR ";
            if (MS != 0)
                description += (MS + valueAmt.getAmtValueFromName(this, "BuffStrength")) + " SPD ";
            if (Range != 0)
                description += (Range + (valueAmt.getAmtValueFromName(this, "BuffStrength"))) + " Range ";
            if (LS != 0)
                description += (LS + valueAmt.getAmtValueFromName(this, "BuffStrength")) + " LS ";

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
            description += "for " + ((valueAmt.getAmtValueFromName(this, "Duration"))).ToString("F2") + " seconds";
        }
        else {
            description = "Give Me ";
            if (PD != 0)
                description += (PD) + " PWR ";
            if (MD != 0)
                description += (MD) + " MGC ";
            if (INF != 0)
                description += (INF) + " INF ";
            if (HP != 0)
                description += (HP) + " HP ";
            if (AS != 0)
                description += (AS) + " AS ";
            if (CDR != 0)
                description += (CDR) + " CDR ";
            if (MS != 0)
                description += (MS) + " SPD ";
            if (Range != 0)
                description += (Range) + " Range ";
            if (LS != 0)
                description += (LS) + " LS ";

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
        }
    }

    public bool buffNotOnTarget() {
        try {
            foreach(Buff temp in character.target.buffs) {
                //if buff is already applied refresh it's duration
                if (temp.code == code) {
                    temp.durationRemaining = valueAmt.getAmtValueFromName(this, "Duration");
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
