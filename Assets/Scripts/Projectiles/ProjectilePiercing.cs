using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePiercing : Projectile
{
    //lifesteal applies at 1/3 effectiveness
    private void Start() {
        base.Start();
        angle();
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
                shooter.damage(victim, 1f * DMG, 0.33f);
                applyHitFX(victim);
            }
        }
    }

    //angles the attack towards the target
    public void angle() {
        float angle = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle+90);
    }
    private void FixedUpdate() {
        trajectory();
    }
}
