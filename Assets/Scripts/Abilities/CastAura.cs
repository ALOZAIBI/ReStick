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
    Aura aura;
    public override void doAbility() {
        if (available) {
            aura = Instantiate(prefabObject).GetComponent<Aura>();
            //sets the amt 
            aura.amt = amt;
            //sets the caster
            aura.caster = character;
            prefabObject.SetActive(true);
            startCooldown();
            startActiveDuration();
        }
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
