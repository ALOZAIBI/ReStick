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

    //if the character was ranged before the buff.
    public bool initRanged;


    private void Start() {
        initRanged = target.usesProjectile;
    }
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

    //Prevents debuffs from setting target's AS below 0.1 and MS below 0.5
    private void setMinimumAS() {
        if(target.AS + AS < 0.1f && target.AS >= 0.1f) {
            AS = target.AS - 0.1f;
            AS = -AS;
        }
    }
    private void setMinimumMS() {
        if (target.MS + MS < 0.8f && target.MS >= 0.8f) {
            MS = target.MS - 0.8f;
            MS = -MS;
        }
    }
    //applies the buff
    //In some cases we'll just increase the HPmax and not the HP (such as in buffOnCharacterAround when resetting the buff(otherwise the buff would heal the character))
    public void applyBuff(bool increaseHP = true) {

        if (!applied) {
            gameObject.SetActive(true);
            target.PD += PD;
            target.MD += MD;
            target.INF += INF;
            //increases HP cap and HP
            target.HPMax += HP;
            if (increaseHP)
                target.HP += HP;

            setMinimumAS();
            target.AS += AS;
            target.CDR += CDR;
            setMinimumMS();
            target.MS += MS;
            target.Range += Range;
            //make ranged if this gives range.
            //target.usesProjectile = Range > 0 ? true : initRanged;
            target.LS += LS;
            //increment by 1 if affected
            target.snare += snare ? 1:0;
            target.silence += silence ? 1:0;
            target.blind += blind ? 1:0;
            if (snare || silence) {
                interruptDash(target);
            }
            target.gameObject.transform.localScale += new Vector3(size, size, size);
            //increase range with size otherwise character becomes too big and pushes it's target with it's collision and can't hit
            target.Range += 0.75f*size;
            //adds this buff to buff list if it is not already there
            if(!target.buffs.Contains(this))
                target.buffs.Add(this);
            startDuration();
            applied = true;
            Debug.Log("Applying buff on " + target.name);
        }
    }

    private void interruptDash(Character victim) {
        //Since dashes disable navmeshagent, we need to reenable it when interrupting.
        victim.agent.enabled = true;
        if (victim.currentDashingAbility != null) {
            victim.currentDashingAbility.startCooldown();
            victim.animationManager.forceStop();
        }
    }

    //public string getDescription() {
    //    string description = "Gives";
    //    if(PD!=0) {
    //        description += " " + PD + " PWR";
    //    }
    //    if (MD != 0) {
    //        description += " " + MD + " MGC";
    //    }
    //    if (INF != 0) {
    //        description += " " + INF + " INF";
    //    }
    //    if (HP != 0) {
    //        description += " " + HP + " HP";
    //    }
    //    if (AS != 0) {
    //        description += " " + AS + " AS";
    //    }
    //    if (CDR != 0) {
    //        description += " " + CDR + " CDR";
    //    }
    //    if (MS != 0) {
    //        description += " " + MS + " SPD";
    //    }
    //    if (Range != 0) {
    //        description += " " + Range + " Range";
    //    }
    //    if (LS != 0) {
    //        description += " " + LS + " LS";
    //    }
    //    if (snare) {
    //        description += " Snare";
    //    }
    //    if (silence) {
    //        description += " Silence";
    //    }
    //    if (blind) {
    //        description += " Blind";
    //    }

    //    return description;
    //}
    //public string getDescription(float buffStrength) {
    //    string description = "Gives";
    //    if (PD != 0) {
    //        description += " " + PD + PD*buffStrength + " PWR";
    //    }
    //    if (MD != 0) {
    //        description += " " + MD + MD*buffStrength + " MGC";
    //    }
    //    if (INF != 0) {
    //        description += " " + INF + INF*buffStrength + " INF";
    //    }
    //    if (HP != 0) {
    //        description += " " + HP + HP * buffStrength + " HP";
    //    }
    //    if (AS != 0) {
    //        description += " " + AS + AS * buffStrength + " AS";
    //    }
    //    if (CDR != 0) {
    //        description += " " + CDR +CDR * buffStrength + " CDR";
    //    }
    //    if (MS != 0) {
    //        description += " " + MS + MS * buffStrength + " SPD";
    //    }
    //    if (Range != 0) {
    //        description += " " + Range + Range * buffStrength + " Range";
    //    }
    //    if (LS != 0) {
    //        description += " " + LS + LS * buffStrength + " LS";
    //    }
    //    if (snare) {
    //        description += " Snare";
    //    }
    //    if (silence) {
    //        description += " Silence";
    //    }
    //    if (blind) {
    //        description += " Blind";
    //    }

    //    return description;
    //}

    //In some cases we want to removeThe stats without deleting the buff Object, like when cloning a target that has a buff on it. The clone will initially have the buff(the same object that the cloned target has) so if we delete the object it iwll be removed from both the clone and the original target.

    //In that case removeFromBuffs should be true
    //However in the cxase of buffoncharacterAround we want to keep updating the buff so we only remove the stats withtout removing the buff from thtet lsit of buffs

    public void removeBuffAppliedStats(Character toBeRemovedFrom,bool removeFromBuffs=true) {
        toBeRemovedFrom.PD -= PD;
        toBeRemovedFrom.MD -= MD;
        toBeRemovedFrom.INF -= INF;
        //just decrements HP cap
        toBeRemovedFrom.HPMax -= HP;
        toBeRemovedFrom.AS -= AS;
        toBeRemovedFrom.CDR -= CDR;
        toBeRemovedFrom.MS -= MS;
        toBeRemovedFrom.Range -= Range;
        //toBeRemovedFrom.usesProjectile = initRanged;
        toBeRemovedFrom.LS -= LS;

        toBeRemovedFrom.snare -= snare ? 1 : 0;
        toBeRemovedFrom.silence -= silence ? 1 : 0;
        toBeRemovedFrom.blind -= blind ? 1 : 0;

        toBeRemovedFrom.gameObject.transform.localScale -= new Vector3(size, size, size);
        toBeRemovedFrom.Range -= 0.75f * size;//see apply buff
        if (removeFromBuffs)
            toBeRemovedFrom.buffs.Remove(this);

        applied = false;
        Debug.Log("Buff:" + code + "Not in stats anymore ");
    }
    //removes the applied stats then deletes this gameobject
    public void removeBuff() {
        removeBuffAppliedStats(target);
        Destroy(gameObject);
        Debug.Log("Buff:" + code + "Removed !!");
    }

    //keeps the visual fx on target
    private void keepOnTarget() {
        if (target == null || !target.alive) {
            gameObject.SetActive(false);
        }
        else {
            gameObject.SetActive(true);
            transform.position = target.transform.position;
            transform.localScale = target.transform.localScale;
        }
    }
    private void FixedUpdate() {
        decrementDuration();
        keepOnTarget();
    }
}
