using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffOnKill : Item
{
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



    public override void onKill() {
        Buff buff = Instantiate(buffObject).GetComponent<Buff>();

        // Apply stats to the buff based on the number of kills in the last frame
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


        refreshDuration();  

        buff.applyBuff();

    }

    public void refreshDuration() {
        try {
            foreach (Buff temp in character.buffs) {
                //if buff is already applied refresh it's duration
                if (temp.code == itemName + character.name) {
                    temp.durationRemaining = buffDuration;
                }
            }
        }
        catch { Debug.LogError("Error in refrewsh Duration"); };
    }
}
