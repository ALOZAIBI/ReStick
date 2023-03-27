using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffOnKIll : Ability
{
    //on kill instantiate a buff with the following stats, then add it to the character
    public float PD;
    public float MD;
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
                buff.MD = MD;
                buff.HP = HP;
                buff.AS = AS;
                buff.CDR = CDR;
                buff.MS = MS;
                buff.Range = Range;
                buff.LS = LS;
                buff.size = size;

                buff.caster = character;
                buff.target = character;

                buff.code = code;

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
