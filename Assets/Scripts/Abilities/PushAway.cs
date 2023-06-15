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
        if (available) {
            //true if enemy is within the radius(rangeAbility)(it has been pushed back)
            bool enemyAffected=false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(character.transform.position, (rangeAbility),mask);

            // Loop through the detected colliders in the layerMask(Characters)
            for (int i = 0; i < colliders.Length; i++) {
                Character temp = colliders[i].GetComponent<Character>();
                
                if (temp.team != character.team) {
                    //apply slow buff
                    Buff buff = Instantiate(prefabObject).GetComponent<Buff>();
                    buff.MS = slowAmount - (amt);
                    buff.caster = character;
                    buff.target = temp;
                    buff.duration = slowDuration;
                    buff.applyBuff();
                    enemyAffected = true;
                    // Calculate pushback vector from character to the edge of the circle
                    Vector2 pushbackVector = colliders[i].transform.position - character.transform.position;
                    pushbackVector = pushbackVector.normalized * (pushBackDistance+amt*2);
                    character.damage(temp, amt*10, false);
                    // Apply pushback vector to the character transform
                    colliders[i].transform.position = (Vector2)character.transform.position + pushbackVector;
                }
            }
            if (enemyAffected) {
                startCooldown();
            }
        }
    }

    public override void updateDescription() {
        try {
            calculateAmt();
        }
        catch { /* avoids null character issue*/}
        description = "PUSH AWAY ENEMIES "+(pushBackDistance + amt*2).ToString("F1")+" And deal" +(amt*10)+"Damage"+" UNITS THEN SLOW THEM";
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
