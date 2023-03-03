using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffOnKIll : Ability
{
    //on kill instantiate a buff with the following stats, then add it to the character
    public float DMG;
    public float HP;
    public float AS;
    public float MS;
    public float Range;
    public float LS;
    public float size;

    public float buffDuration;


    private void Start() {
        updateDescription();
    }

    public override void doAbility() {
        if (character.killsLastFrame > 0) {
            for (int i = 0; i < character.killsLastFrame; i++) {
                //creates buff for every kill last frame
                Buff buff = Instantiate(prefabObject).GetComponent<Buff>();
                Debug.Log("kills>0"+ buff.transform.parent.name);
                buff.DMG = DMG;
                buff.HP = HP;
                buff.AS = AS;
                buff.MS = MS;
                buff.Range = Range;
                buff.LS = LS;
                buff.size = size;

                buff.caster = character;
                buff.target = character;

                buff.duration = buffDuration;
                buff.applyBuff();
            }
        }
    }

    public override void updateDescription() {
        description = "On Kill give me ";
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
}
