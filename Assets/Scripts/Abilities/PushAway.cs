using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAway : Ability {
    //push away nearby enemies then slow them
    //to detect only character collisions
    public LayerMask mask;


    //prefabObject will hold the buff that will slowthem
    public override bool doAbility() {
        //this ability will only be cast if there are enemies within the radius
        if (available && character.selectTarget((int)Character.TargetList.ClosestEnemy,rangeAbility)) {
            calculateAmt();
            playAnimation("castAoePush");
            return true;
        }
        return false;
    }
    public override void executeAbility() {

        //Holds list of enemies affected by the ability (within rangeability)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(character.transform.position, (rangeAbility), mask);

        applyHitFX(character,rangeAbility*2);

        // Loop through the detected colliders in the layerMask(Characters)
        for (int i = 0; i < colliders.Length; i++) {
            Character temp = colliders[i].GetComponent<Character>();

            if (temp.team != character.team) {
                //apply slow buff
                Buff buff = createBuff();
                buff.MS = (valueAmt.getAmtValueFromName(this, "Slow"));
                buff.caster = character;
                buff.target = temp;
                buff.duration = valueAmt.getAmtValueFromName(this,"SlowDuration");
                buff.applyBuff();
                // Calculate pushback vector from character to the edge of the circle
                Vector2 pushbackVector = colliders[i].transform.position - character.transform.position;
                pushbackVector = pushbackVector.normalized * valueAmt.getAmtValueFromName(this, "PushBackDistance");
                character.damage(temp, valueAmt.getAmtValueFromName(this,"Damage"), 0.33f);
                // Apply pushback vector to the character transform
                colliders[i].transform.position = (Vector2)character.transform.position + pushbackVector;
                interruptDash(temp);
            }
        }

        startCooldown();

    }

    public override void updateDescription() {
        if (character == null) {
            description = "PUSH AWAY ENEMIES " + " And deal"  + "Damage" + " THEN SLOW THEM";
        }
        else {
            calculateAmt();
            description = "PUSH AWAY ENEMIES "+" And deal" +(valueAmt.getAmtValueFromName(this,"Damage"))+"Damage"+" UNITS THEN SLOW THEM";
        }
    }
    public override void Start() {
        base.Start();
        updateDescription();
    }


    private void FixedUpdate() {
        cooldown();
        //if (valueAmt == 0) {
        //    //calculateAmt();
        //}
    }
}
