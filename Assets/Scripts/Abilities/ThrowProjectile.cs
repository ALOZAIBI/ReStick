using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectile : Ability
{
    //Summons a projectile
    //can do cool shit depending on the projectile
    //for example a projectile that throws an AOE that heals
    private void Start() {
        updateDescription();
    }
    public override void doAbility() {
        if (available) {
            calculateAmt();
            Debug.Log("ABILITY DONE WHAT");
            //selects target based on the ability's targetting strategy;
            character.selectTarget(targetStrategy);
            //creates the projectile
            GameObject objProjectile = Instantiate(prefabObject, character.transform.position, character.transform.rotation);
            Projectile projectile = objProjectile.GetComponent<Projectile>();
            //sets the shooter to be the caster of this ability
            projectile.shooter = character;
            //sets the damage amount
            projectile.PD = amt;
            //sets the target
            projectile.target = character.target;
            ////sets the projectiles direction
            //projectile.direction = character.target.transform.position - character.transform.position;
            ////normalises the direction so that projectile speed won't be affected by target distance
            //projectile.direction = (10 * projectile.direction).normalized;
            startCooldown();
        }

    }
    public override void updateDescription() {
        try {
            calculateAmt();
        }
        catch { Debug.Log("Calculate AMT didn't work");/* avoids null character issue*/}
        description = "Throws a thing that deals " + amt + " PD to all characters within";
    }

    private void FixedUpdate() {
        cooldown();
    }


}
