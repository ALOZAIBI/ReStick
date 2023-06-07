using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastAura : Ability
{
    //duration that aura is active per cast
    public float duration;
    //duration remaining
    public float durationRemaining=0;
    public bool active = false;
    //wether it targets enemy or ally or both
    public bool enemy;
    public bool ally;
    private Aura aura;

    public override void Start() {
        base.Start();
        updateDescription();
    }
    public override void doAbility() {
        if (available) {
            calculateAmt();
            GameObject temp = Instantiate(prefabObject);
            temp.transform.localScale = new Vector3(rangeAbility*2, rangeAbility*2, rangeAbility*2);
            aura = temp.GetComponent<Aura>();
            //sets the amt 
            aura.amt = amt;
            //sets the caster
            aura.caster = character;
            aura.ally = ally;
            aura.enemy = enemy;
            prefabObject.SetActive(true);
            startCooldown();
            startActiveDuration();
        }
    }

    public override void updateDescription() {
        try {
            calculateAmt();
        }
        catch { /*Debug.Log("Amt not upodated") avoids null character issue*/}
        
        if (amt > 0) {
            description = "Heals nearby characters by " + amt;
        }
        else
            description = "Deals " + amt + " to nearby characters";
    }
    public void startActiveDuration() {
        durationRemaining = duration;
    }

    public void decrementDuration() {
        if (durationRemaining > 0) {
            durationRemaining -= Time.fixedDeltaTime;
        }
        else {
            durationRemaining = 0;
            Destroy(aura.gameObject);
        }
    }

    //as the name states
    private void keepAuraObjectOnCaster() {
        aura.transform.position = character.transform.position;
    }
    void FixedUpdate() {
        cooldown();
        try {
            decrementDuration();

            keepAuraObjectOnCaster();
        }
        catch { /*Avoided error when there is no aura instantiated yet. Before ability is done */}
    }
}
