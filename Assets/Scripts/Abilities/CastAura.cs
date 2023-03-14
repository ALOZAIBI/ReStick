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
    Aura aura;

    private void Start() {
        updateDescription();
    }
    public override void doAbility() {
        if (available) {
            aura = Instantiate(prefabObject).GetComponent<Aura>();
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
        decrementDuration();
        keepAuraObjectOnCaster();
    }
}
