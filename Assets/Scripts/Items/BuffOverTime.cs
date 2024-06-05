using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffOverTime : Item
{

    [SerializeField] private float timeSinceLastBuff;
    [SerializeField] private float timeBetweenBuffs;

    [SerializeField] private Buff buffObject;

    public float buffPD;
    public float buffMD;
    public float buffINF;
    public float buffHP;
    public float buffAS;
    public float buffCDR;
    public float buffMS;
    public float buffRange;
    public float buffLS;
    public float buffsize;

    public float buffDuration;


    public override void onZoneStart() {
        timeSinceLastBuff = 0;
    }

    public override void continuous() {
        timeSinceLastBuff += Time.fixedDeltaTime;
        if (timeSinceLastBuff >= timeBetweenBuffs) {
            timeSinceLastBuff = 0;

            Buff buff = Instantiate(buffObject).GetComponent<Buff>();

            buff.PD = buffPD;
            buff.MD = buffMD;
            buff.INF = buffINF;
            buff.HP = buffHP;
            buff.AS = buffAS;
            buff.CDR = buffCDR;
            buff.MS = buffMS;
            buff.Range = buffRange;
            buff.LS = buffLS;
            buff.size = buffsize;

            buff.caster = character;
            buff.target = character;
            //code used to identify duplicate buffs to refresh duration when a new stack is added
            buff.code = itemName + character.name;
            buff.duration = buffDuration;


            buff.applyBuff();
        }
    }

}
