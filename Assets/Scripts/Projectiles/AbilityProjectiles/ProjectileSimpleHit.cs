using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSimpleHit : Projectile
{

    //If homing is set to true, keep updating the angle of the projectile to the target
    [SerializeField]private bool homing = false;

    //If indirectAngle is set to true, the projectile will start a bit off angle then correct its course
    [SerializeField] private bool indirectAngle = false;

    [SerializeField] private float speedOfCorrection = 100;

    private float currAngle = 0;

    //direction is set in the Ability ThrowProjectile
    private void Start() {
        //does base start to make the projectile die after lifetime
        base.Start();

        if (indirectAngle) {
            //Set the angle to be a random angle
            currAngle = Random.Range(-180, 180);
        }
        if (homing) {
            setAngle(currAngle);
        }
        
        //The angle of the object is set by the ability
    }

    //travels in target direction
    public override void trajectory() {
        if (homing) {
            //Actually make it homing
            setAngle(currAngle);
            //If I am homing and the target is dead, destroy the projectile
            if (!target.alive) {
                Destroy(gameObject);
            }
        }
        //Move the projectile opposite it's y axis
        transform.position += speed * Time.fixedDeltaTime * -transform.up;

        //If it's indirect angle, slowly correct the angle
        if (indirectAngle) {
            if (currAngle < 0) {
                currAngle += Time.fixedDeltaTime * speedOfCorrection;
            }
            else if (currAngle > 0) {
                currAngle -= Time.fixedDeltaTime * speedOfCorrection;
            }
        }
        
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
