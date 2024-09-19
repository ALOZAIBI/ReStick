using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePiercing : Projectile
{
    public bool stopAfterHitting = false;
    private bool timerToStopMoving = false;
    private float timer = 0f;

    //lifesteal applies at 1/3 effectiveness
    private void Start() {
        base.Start();
        //sets the projectiles direction
        direction = target.transform.position - shooter.transform.position;
        //normalises the direction so that projectile speed won't be affected by target distance
        direction = (10 * direction).normalized;

        setAngle();
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
                shooter.damage(victim, 1f * DMG, 1f);
                applyHitFX(victim);
            }
            //If the victim hit is the target, the projectil will stop moving in .1 seconds and stops dealing dmg, then gets destroyed after a second
            if(stopAfterHitting && victim == target) {
                Destroy(gameObject, 1f);
                timerToStopMoving = true;
            }
        }
    }

    
    private void FixedUpdate() {
        trajectory();
        if (timerToStopMoving) {
            timer += Time.fixedDeltaTime;
            if (timer >= 0.1f) {
                speed = 0;
                DMG = 0;
            }
        }
    }
}
