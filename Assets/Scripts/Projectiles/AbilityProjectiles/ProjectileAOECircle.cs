using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAOECircle : Projectile
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

    //deals damage to enemies that this passes over
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Character") {
            Character victim = collision.GetComponent<Character>();
            //deals damage to everyhing not in the shooters team
            if (victim.team != shooter.team) {
                victim.HP -= dmg*Time.fixedDeltaTime;
                if (victim.HP <= 0) {
                    shooter.kill(victim);
                }
            }
        }
    }
    private void FixedUpdate() {
        trajectory();
    }
}
