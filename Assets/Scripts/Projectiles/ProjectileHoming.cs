using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHoming : Projectile
{
    private void Start() {
        //destroys projectile after lifetime
        Destroy(gameObject, lifetime);
    }
    public override void trajectory() {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.fixedDeltaTime);
    }

    //on collision with a character the projectile deals the damage and gets destroyed
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Character") {
            Character victim = collision.GetComponent<Character>();
            //checks that the collision isn't the shooter since the projectile will damage the shooter otherwise
            if (victim != shooter) {
                victim.HP -= dmg;
                //heals shooter thanks to life steal
                shooter.HP += dmg * LS;
                if (victim.HP <= 0) {
                    shooter.totalKills++;
                    shooter.killsLastFrame++;
                }
                Destroy(gameObject);
            }
        }
    }

    void FixedUpdate() {
        trajectory();
    }
}
