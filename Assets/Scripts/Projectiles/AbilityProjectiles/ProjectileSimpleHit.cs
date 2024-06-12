using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSimpleHit : Projectile
{

    //If homing is set to true, keep updating the angle of the projectile to the target
    [SerializeField]private bool homing = false;

    //direction is set in the Ability ThrowProjectile
    private void Start() {
        //does base start to make the projectile die after lifetime
        base.Start();

        if (homing) {
            Debug.Log("Homing");
            //DEbugs the information of the projectile
            Debug.Log("Target: " + target);
            Debug.Log("Shooter: " + shooter);
            Debug.Log("Speed: " + speed);
            setAngleToFollowTarget();
        }
        
        //The angle of the object is set by the ability
    }

    //travels in target direction
    public override void trajectory() {
        if (homing) {
            setAngleToFollowTarget();
        }
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
