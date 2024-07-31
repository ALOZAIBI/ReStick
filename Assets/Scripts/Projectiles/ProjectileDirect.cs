using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDirect : Projectile {
    //projectile that homes but doesn't hit except the target
    public override void trajectory() {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.fixedDeltaTime);
    }
    //on collision with target deal damage then destroy the projectile
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Character") {
            Character victim = collision.GetComponent<Character>();
            if(victim == target) {
                shooter.damage(victim, DMG, true);
                Destroy(gameObject);
                applyHitFX(victim);
            }

        }
    }
    private void FixedUpdate() {
        trajectory();
    }
}
