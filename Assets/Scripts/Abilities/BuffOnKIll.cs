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

    public float buffDuration;
    //code used to identify duplicate buffs to refresh duration when a new stack is added
    public string code;

    //gives buff to character on kill
    //can stack infintely
    //when a new stack is added refresh duration of previous stacks
    private void Start() {
        code = Random.Range(-50, 500) + "";
        updateDescription();
    }

    public override void doAbility() {
        if (character.killsLastFrame > 0) {
            calculateAmt();
            Debug.Log("There was a kill last frame");
            for (int i = 0; i < character.killsLastFrame; i++) {
                //creates buff for every kill last frame
                Buff buff = Instantiate(prefabObject).GetComponent<Buff>();
                //Debug.Log("kills>0"+ buff.transform.parent.name);
                //Debug.Log("Arrived here");
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

                buff.caster = character;
                buff.target = character;

                //buff.code = code;


                buff.duration = buffDuration;

                //refreshes duration of buff when a new stack is added
                foreach (Buff temp in character.buffs) {
                    if (temp.code == code) {
                        temp.durationRemaining = buffDuration;
                        Debug.Log("try first if");
                    }
                }
                buff.applyBuff();
            }
        }
    }

    public override void updateDescription() {
        try {
            calculateAmt();
        }
        catch { /* avoids null character issue*/}
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
