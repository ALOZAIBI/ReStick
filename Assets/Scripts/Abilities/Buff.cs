using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this could be a DeBuff as well btw
public class Buff : MonoBehaviour
{
    public bool applied;

    public float DMG;
    public float HP;
    public float AS;
    public float MS;
    public float Range;
    public float LS;

    public float size;

    //to be applied in the future either deals damage over time or heals over time
    //kinda like darius blood for the dmg
    public float dps;
    public Character caster;
    public Character target;

    //code used to verify if a specific buff is already applied. compare buff to be instantiated's code to the target character's buffs
    public string code;

    public float duration;
    public float durationRemaining=0;


    //stars duration timer
    public void startDuration() {
        durationRemaining = duration;
    }
    //decrements duration timer and on end removesbuff
    private void decrementDuration() {
        if (durationRemaining > 0) {
            durationRemaining -= Time.fixedDeltaTime;
        }
        else {
            durationRemaining = 0;
            //if the ability has been applied and time is over then remove it.
            if (applied) {
                removeBuff();
            }
        }
    }
    //applies the buff
    public void applyBuff() {
        if (!applied) {
            target.DMG += DMG;
            //increases HP cap and HP
            target.HPMax += HP;
            target.HP += HP;

            target.AS += AS;
            target.MS += MS;
            target.Range += Range;
            target.LS += LS;
            target.gameObject.transform.localScale += new Vector3(size, size, size);
            //adds this buff to buff list
            target.buffs.Add(this);
            startDuration();
            applied = true;
        }
    }

    //removes the applied stats then deletes this gameobject
    public void removeBuff() {
        target.DMG -= DMG;
        //just decrements HP cap
        target.HPMax -= HP;
        target.AS -= AS;
        target.MS -= MS;
        target.Range -= Range;
        target.LS -= LS;
        target.gameObject.transform.localScale -= new Vector3(size, size, size);
        target.buffs.Remove(this);
        Destroy(gameObject);
    }

    private void FixedUpdate() {
        decrementDuration();
    }
}
