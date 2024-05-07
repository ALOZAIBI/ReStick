using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSimpleHit : Projectile
{
    //doesn't apply lifesteal

    //direction is set in the Ability ThrowProjectile
    private void Start() {
        //does base start to make the projectile die after lifetime
        base.Start();
        
        //The angle of the object is set by the ability
    }

    //travels in target direction
    public override void trajectory() {
        //Move the projectile opposite it's y axis
        transform.position += speed * Time.fixedDeltaTime * -transform.up;

        
    }

    //deals damage to first EnemyHit
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Character") {
            Character victim = collision.GetComponent<Character>();
            if (victim.team != shooter.team) {
                applyBuff(victim);
               
                shooter.damage(victim, DMG, 0.75f);
                applyHitFX(victim);
                //Destroy This Projectile After Hit
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate() {
        trajectory();
    }
}
