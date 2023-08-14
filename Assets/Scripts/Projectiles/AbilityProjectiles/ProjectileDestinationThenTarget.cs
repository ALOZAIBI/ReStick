using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this projectile flies to destination then after Time time fly towards Target.
public class ProjectileDestinationThenTarget : Projectile
{
    public Vector2 destination;


    public float toDestinationSpeed;
    public bool arrivedDestination=false;

    public float delayWanted;
    public float delayCurrent;

    
    //doesn't apply lifesteal

    //direction is set in the Ability ThrowProjectile
    private void Start() {
        //does base start to make the projectile die after lifetime
        base.Start();
    }

    //travels in target direction
    public override void trajectory() {

        //travel to destination
        if (!arrivedDestination) {
            transform.position = Vector2.MoveTowards(transform.position, destination, toDestinationSpeed*Time.fixedDeltaTime);
            //checks if arrived to destination
            if ((Vector2)transform.position == destination)
                arrivedDestination = true;
        }
        //once it has already arrived to destination start Delay then after Delay fly towards target
        else {
            //if target dies travel to nearest enemy to shoter after a delay that is half the usual the delay
            if (target.alive == false) {
                destination = transform.position;
                arrivedDestination = false;
                delayCurrent = delayWanted/2;
                shooter.selectTarget((int)Character.TargetList.ClosestEnemy);
                target = shooter.target;
            }
            else
            if(delayCurrent < delayWanted) {
                delayCurrent += Time.fixedDeltaTime;
            }
            else
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.fixedDeltaTime);
        }
    }

    //deals damage to first EnemyHit
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Character") {
            Character victim = collision.GetComponent<Character>();
            if (victim.team != shooter.team) {
                shooter.damage(victim, DMG, 0.33f);
                //Destroy This Projectile After Hit
                applyHitFX(victim);
                Destroy(gameObject);
            }
        }
    }
    private void FixedUpdate() {
        trajectory();
    }
}
