using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePiercing : Projectile
{
    //lifesteal applies at 1/3 effectiveness
    private void Start() {
        base.Start();
        //sets the projectiles direction
        direction = target.transform.position - shooter.transform.position;
        //normalises the direction so that projectile speed won't be affected by target distance
        direction = (10 * direction).normalized;
    }

    //travels in target direction
    public override void trajectory() {
        transform.position = (Vector2)transform.position + (direction * (speed * Time.fixedDeltaTime));
        grow();
    }

    //deals PD once to every enemy hit
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Character") {
            Character victim = collision.GetComponent<Character>();
            //deals damage to everyhing not in the shooters team
            if (victim.team != shooter.team) {
                shooter.damage(victim, 1f * DMG, 0.5f);
                applyHitFX(victim);
            }
        }
    }

    
    private void FixedUpdate() {
        trajectory();
    }
}
