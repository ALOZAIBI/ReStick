using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHoming : Projectile
{
    //projectile that homes towards target
    public override void trajectory() {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.fixedDeltaTime);
    }

    //on collision with a character the projectile deals the damage and gets destroyed
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Character") {
            Character victim = collision.GetComponent<Character>();
            //checks that the collision isn't the shooter since the projectile will damage the shooter otherwise
            if (victim != shooter) {
                shooter.damage(victim, DMG, true);
                Destroy(gameObject);
            }
        }
    }

    void FixedUpdate() {
        trajectory();
    }
}
