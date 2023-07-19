using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAway : Ability {
    //push away nearby enemies then slow them
    //the radius of affected enemies
    public float pushBackDistance;
    public float slowAmount;
    public float slowDuration;
    //to detect only character collisions
    public LayerMask mask;

    //prefabObject will hold the buff that will slowthem
    public override void doAbility() {
        //this ability will only be cast if there are enemies within the radius
        if (available && character.selectTarget((int)Character.TargetList.ClosestEnemy,rangeAbility)) {
            calculateAmt();
            playAnimation("castAoePush");
        }
    }
    public override void executeAbility() {

        //Holds list of enemies affected by the ability (within rangeability)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(character.transform.position, (rangeAbility), mask);

        // Loop through the detected colliders in the layerMask(Characters)
        for (int i = 0; i < colliders.Length; i++) {
            Character temp = colliders[i].GetComponent<Character>();

            if (temp.team != character.team) {
                //apply slow buff
                Buff buff = createBuff();
                buff.MS = slowAmount - (amt);
                buff.caster = character;
                buff.target = temp;
                buff.duration = slowDuration;
                buff.applyBuff();
                // Calculate pushback vector from character to the edge of the circle
                Vector2 pushbackVector = colliders[i].transform.position - character.transform.position;
                pushbackVector = pushbackVector.normalized * (pushBackDistance + amt * 2);
                character.damage(temp, amt * 20, false);
                // Apply pushback vector to the character transform
                colliders[i].transform.position = (Vector2)character.transform.position + pushbackVector;
            }
        }

        startCooldown();

    }

    public override void updateDescription() {
        if (character == null) {
            description = "PUSH AWAY ENEMIES " + (pushBackDistance).ToString("F1") + " And deal"  + "Damage" + " UNITS THEN SLOW THEM";
        }
        else {
            calculateAmt();
            description = "PUSH AWAY ENEMIES "+(pushBackDistance + amt*2).ToString("F1")+" And deal" +(amt*10)+"Damage"+" UNITS THEN SLOW THEM";
        }
    }
    public override void Start() {
        base.Start();
        updateDescription();
    }


    private void FixedUpdate() {
        cooldown();
        if (amt == 0) {
            //calculateAmt();
        }
    }
}
