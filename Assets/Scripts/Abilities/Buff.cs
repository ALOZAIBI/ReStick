using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this could be a DeBuff as well btw
public class Buff : MonoBehaviour
{
    public bool applied;

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
    public bool snare;// no movement
    public bool silence;// no abilities
    public bool blind;// no auto attacks

    //to be applied in the future either deals damage over time or heals over time
    //kinda like darius blood for the PD
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
            target.PD += PD;
            target.MD += MD;
            target.INF += INF;
            //increases HP cap and HP
            target.HPMax += HP;
            target.HP += HP;

            target.AS += AS;
            target.CDR += CDR;
            target.MS += MS;
            target.Range += Range;
            target.LS += LS;

            target.snare = snare;
            target.silence = silence;
            target.blind = blind;

            target.gameObject.transform.localScale += new Vector3(size, size, size);
            //increase range with size otherwise character becomes too big and pushes it's target with it's collision and can't hit
            target.Range += 0.75f*size;
            //adds this buff to buff list
            target.buffs.Add(this);
            startDuration();
            applied = true;
            //Debug.Log("Applying buff on " + target.name);
        }
    }

    //removes the applied stats then deletes this gameobject
    public void removeBuff() {
        target.PD -= PD;
        target.MD -= MD;
        target.INF -= INF;
        //just decrements HP cap
        target.HPMax -= HP;
        target.AS -= AS;
        target.CDR -= CDR;
        target.MS -= MS;
        target.Range -= Range;
        target.LS -= LS;

        target.snare = false;
        target.silence = false;
        target.blind = false;

        target.gameObject.transform.localScale -= new Vector3(size, size, size);
        target.Range -= 0.75f*size;//see apply buff
        target.buffs.Remove(this);
        Destroy(gameObject);
    }

    private void FixedUpdate() {
        decrementDuration();
    }
}
