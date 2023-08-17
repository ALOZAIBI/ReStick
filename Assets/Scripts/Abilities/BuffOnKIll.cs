using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffOnKIll : Ability
{
    //on kill instantiate a buff with the following stats, then add it to the character
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

    //code used to identify duplicate buffs to refresh duration when a new stack is added
    public string code;

    //gives buff to character on kill
    //can stack infintely
    //when a new stack is added refresh duration of previous stacks

    public override void Start() {
        code = Random.Range(-50, 500) + "";
        base.Start();
        updateDescription();
    }
    public override void doAbility() {
        if (character.killsLastFrame > 0) {
            calculateAmt();
            Debug.Log("There was a kill last frame");
            foreach(int i in character.averageLevelOfKillsLastFrame){
                //creates buff for every kill last frame
                Buff buff = Instantiate(prefabObject).GetComponent<Buff>();
                //Debug.Log("kills>0"+ buff.transform.parent.name);
                //Debug.Log("Arrived here");
                buff.PD = Mathf.Clamp01(((i - 0.4f) * 0.25f)) * PD;
                if (PD > 0) {
                    buff.PD += Mathf.Clamp01(((i - 0.4f) * 0.25f)) * valueAmt.getAmtValueFromName(this, "BuffStrength") * PD;
                }

                buff.MD = Mathf.Clamp01(((i - 0.4f) * 0.25f)) * MD;
                if (MD > 0) {
                    buff.MD += Mathf.Clamp01(((i - 0.4f) * 0.25f)) * valueAmt.getAmtValueFromName(this, "BuffStrength") * MD;
                }

                buff.INF = Mathf.Clamp01(((i - 0.4f) * 0.25f)) * INF;
                if (INF > 0) {
                    buff.INF += Mathf.Clamp01(((i - 0.4f) * 0.25f)) * valueAmt.getAmtValueFromName(this, "BuffStrength") * INF;
                }

                buff.HP = Mathf.Clamp01(((i - 0.4f) * 0.25f)) * HP;
                if (HP > 0) {
                    buff.HP += Mathf.Clamp01(((i - 0.4f) * 0.25f)) * valueAmt.getAmtValueFromName(this, "BuffStrength") * HP;
                }

                buff.AS = Mathf.Clamp01(((i - 0.4f) * 0.25f)) * AS;
                if (AS > 0) {
                    buff.AS += Mathf.Clamp01(((i - 0.4f) * 0.25f)) * valueAmt.getAmtValueFromName(this, "BuffStrength") * AS;
                }

                buff.CDR = Mathf.Clamp01(((i - 0.4f) * 0.25f)) * CDR;
                if (CDR > 0) {
                    buff.CDR += Mathf.Clamp01(((i - 0.4f) * 0.25f)) * valueAmt.getAmtValueFromName(this, "BuffStrength") * CDR;
                }

                buff.MS = Mathf.Clamp01(((i - 0.4f) * 0.25f)) * MS;
                if (MS > 0) {
                    buff.MS += Mathf.Clamp01(((i - 0.4f) * 0.25f)) * valueAmt.getAmtValueFromName(this, "BuffStrength") * MS;
                }

                buff.Range = Mathf.Clamp01(((i - 0.4f) * 0.25f)) * Range;
                if (Range > 0) {
                    buff.Range += Mathf.Clamp01(((i - 0.4f) * 0.25f)) * valueAmt.getAmtValueFromName(this, "BuffStrength") * Range;
                }

                buff.LS = Mathf.Clamp01(((i - 0.4f) * 0.25f)) * LS;
                if (LS > 0) {
                    buff.LS += Mathf.Clamp01(((i - 0.4f) * 0.25f)) * valueAmt.getAmtValueFromName(this, "BuffStrength") * LS;
                }

                buff.size = Mathf.Clamp01(((i - 0.4f) * 0.25f)) * size;
                if (size > 0) {
                    buff.size += Mathf.Clamp01(((i - 0.4f) * 0.25f)) * valueAmt.getAmtValueFromName(this, "BuffStrength") * size;
                }

                buff.caster = character;
                buff.target = character;

                buff.code = abilityName + character.name;


                buff.duration = valueAmt.getAmtValueFromName(this,"Duration");

                //refreshes duration of buff when a new stack is added
                foreach (Buff temp in character.buffs) {
                    if (temp.code == code) {
                        temp.durationRemaining = valueAmt.getAmtValueFromName(this,"Duration");
                        Debug.Log("try first if");
                    }
                }
                buff.applyBuff();
                refreshDuration();
            }
        }
    }

    public override void updateDescription() {
        if (character != null) {
            calculateAmt();
            description = "On Kill give me ";
            if (PD != 0)
                description += PD + PD * valueAmt.getAmtValueFromName(this,"BuffStrength") + " PD ";
            if (MD != 0)
                description += MD + MD * valueAmt.getAmtValueFromName(this, "BuffStrength") + " MD ";
            if (INF != 0)
                description += INF + INF * valueAmt.getAmtValueFromName(this, "BuffStrength") + " INF ";
            if (HP != 0)
                description += HP + HP * valueAmt.getAmtValueFromName(this, "BuffStrength") + " HP ";
            if (AS != 0)
                description += AS + AS * valueAmt.getAmtValueFromName(this, "BuffStrength") + " AS ";
            if (MS != 0)
                description += MS + MS * valueAmt.getAmtValueFromName(this, "BuffStrength") + " MS ";
            if (Range != 0)
                description += Range + Range * valueAmt.getAmtValueFromName(this, "BuffStrength") + " Range ";
            if (LS != 0)
                description += LS + LS * valueAmt.getAmtValueFromName(this, "BuffStrength") + " LS ";
        }
        else {
              description = "On Kill give me ";
            if (PD != 0)
                description += PD + " PD ";
            if (MD != 0)
                description += MD + " MD ";
            if (INF != 0)
                description += INF + " INF ";
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
    }

    public void refreshDuration() {
        try {
            foreach (Buff temp in character.buffs) {
                //if buff is already applied refresh it's duration
                if (temp.code == abilityName + character.name) {
                    temp.durationRemaining = valueAmt.getAmtValueFromName(this,"Duration");
                }
            }
        }
        catch {};
    }
}
