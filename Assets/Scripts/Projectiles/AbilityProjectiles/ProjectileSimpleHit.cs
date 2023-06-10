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
        
        //sets the projectiles direction
        direction = target.transform.position - shooter.transform.position;
        //normalises the direction so that projectile speed won't be affected by target distance
        direction = (10 * direction).normalized;
    }

    //travels in target direction
    public override void trajectory() {
        transform.position = (Vector2)transform.position + (direction * (speed * Time.fixedDeltaTime));
    }

    //deals damage to first EnemyHit
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Character") {
            Character victim = collision.GetComponent<Character>();
            if (victim.team != shooter.team) {
                applyBuff(victim);
               
                shooter.damage(victim, DMG, false);
                //Destroy This Projectile After Hit
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate() {
        trajectory();
    }
}
