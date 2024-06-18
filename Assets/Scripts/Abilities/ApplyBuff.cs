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

    public override void Start() {
        base.Start();
        updateDescription();
    }
    public override bool doAbility() {
        //Added the check if interruptible to not make this function called every frame.
        if (character.animationManager.interruptible && available) {
            //selects target
            if (character.selectTarget(targetStrategy,rangeAbility,excludeTargets())) {
                calculateAmt();
                playAnimation("castRaise");
                return true;
            }
        }

        return false;
    }
    public override void executeAbility() {
        base.executeAbility();
        //creates buff
        Buff buff = Instantiate(prefabObject, character.target.transform.position, Quaternion.identity).GetComponent<Buff>();
        buff.PD = PD;
        if (PD > 0) {
            buff.PD += valueAmt.getAmtValueFromName(this, "BuffStrength") * PD;
        }

        buff.MD = MD;
        if (MD > 0) {
            buff.MD += valueAmt.getAmtValueFromName(this, "BuffStrength") * MD;
        }

        buff.INF = INF;
        if (INF > 0) {
            buff.INF += valueAmt.getAmtValueFromName(this, "BuffStrength") * INF;
        }

        buff.HP = HP;
        if (HP > 0) {
            buff.HP += valueAmt.getAmtValueFromName(this, "BuffStrength") * HP;
        }

        buff.AS = AS;
        if (AS > 0) {
            buff.AS += valueAmt.getAmtValueFromName(this, "BuffStrength") * AS;
        }

        buff.CDR = CDR;
        if (CDR > 0) {
            buff.CDR += valueAmt.getAmtValueFromName(this, "BuffStrength") * CDR;
        }

        buff.MS = MS;
        if (MS > 0) {
            buff.MS += valueAmt.getAmtValueFromName(this, "BuffStrength") * MS;
        }

        buff.Range = Range;
        if (Range > 0) {
            buff.Range += valueAmt.getAmtValueFromName(this, "BuffStrength") * Range;
        }

        buff.LS = LS;
        if (LS > 0) {
            buff.LS += valueAmt.getAmtValueFromName(this, "BuffStrength") * LS;
        }

        buff.size = size;
        if (size > 0) {
            buff.size += valueAmt.getAmtValueFromName(this, "BuffStrength") * size;
        }

        buff.snare = root;
        buff.silence = silence;
        buff.blind = blind;

        //sets caster and target
        buff.caster = character;
        buff.target = character.target;
        //increases buff duration according to AMT
        buff.duration = valueAmt.getAmtValueFromName(this, "Duration");
        buff.code = getCodeString();
        //applies the buff
        buff.applyBuff();
        refreshDuration();
        //cooldown this ability
        startCooldown();
    }

    //Excludes targets that already have the buff or a similar debuff(root,silence, blind)
    public override List<Character> excludeTargets() {

        List<Character> alreadyHaveBuff = new List<Character>();
        foreach (Character c in character.zone.charactersInside) {

            foreach (Buff b in c.buffs) {
                if (b.code == getCodeString() || (b.snare && root && true) || (b.silence && silence && true) || (b.blind && blind && true)) {
                    alreadyHaveBuff.Add(c);
                    break;
                }
            }

        }
        return alreadyHaveBuff;
    }

    public override void updateDescription() {
        if (character != null) {

            calculateAmt();
            description = "Give target ";
            if (PD != 0)
                description += (PD + (PD * valueAmt.getAmtValueFromName(this,"BuffStrength"))) + " PD ";
            if (MD != 0)
                description += (MD + (MD * valueAmt.getAmtValueFromName(this, "BuffStrength"))) + " MD ";
            if (INF != 0)
                description += (INF + (INF * valueAmt.getAmtValueFromName(this, "BuffStrength"))) + " INF ";
            if (HP != 0)
                description += (HP + (HP * valueAmt.getAmtValueFromName(this, "BuffStrength"))) + " HP ";
            if (AS != 0)
                description += (AS + (AS * valueAmt.getAmtValueFromName(this, "BuffStrength"))) + " AS ";
            if (CDR != 0)
                description += (CDR + (CDR * valueAmt.getAmtValueFromName(this, "BuffStrength"))) + " CDR ";
            if (MS != 0)
                description += (MS + (MS * valueAmt.getAmtValueFromName(this, "BuffStrength"))) + " MS ";
            if (Range != 0)
                description += (Range + (Range * valueAmt.getAmtValueFromName(this, "BuffStrength"))) + " Range ";
            if (LS != 0)
                description += (LS + (LS * valueAmt.getAmtValueFromName(this, "BuffStrength"))) + " LS ";

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
            description += "for " + (valueAmt.getAmtValueFromName(this, "Duration")).ToString("F2") + " seconds";
        } else {
            description = "Give target ";
            if (PD != 0)
                description += (PD) + " PD ";
            if (MD != 0)
                description += (MD) + " MD ";
            if (INF != 0)
                description += (INF)+ " INF ";
            if (HP != 0)
                description += (HP) + " HP ";
            if (AS != 0)
                description += (AS) + " AS ";
            if (CDR != 0)
                description += (CDR ) + " CDR ";
            if (MS != 0)
                description += (MS) + " MS ";
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

    public string getCodeString() {
        return abilityName + character.name;
    }
    //refreshes duration of other stacks of the same buff.
    public void refreshDuration() {
        try {
            foreach(Buff temp in character.target.buffs) {
                //if buff is already applied refresh it's duration
                if (temp.code == getCodeString()) {
                    temp.durationRemaining = valueAmt.getAmtValueFromName(this, "Duration");
                }
            } 
        }
        catch {};
    }

    private void FixedUpdate() {
        cooldown();
    }
}
