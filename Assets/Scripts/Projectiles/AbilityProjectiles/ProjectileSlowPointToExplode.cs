using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSlowPointToExplode : Projectile
{
    //this ability is basically ursus shock (Bartholomew Kuma)
    //Travels to target without dealing PD then explodes(size expands quickly) and deals damage ontriggerEnter

    public bool exploded;
    public Vector2 targetPosition;

    //size before explosion
    public float ogSize;
    //maxsize after explosion
    public int size;
    public float explosionDuration;
    public float timeSinceExplosion=0;

    public float implosionDuration;
    public float timeSinceImplosion = 0;

    public bool sizeAttained;

    public override void trajectory() {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition,speed * Time.fixedDeltaTime);
    }
    private void explosion() {
        
        timeSinceExplosion += Time.fixedDeltaTime;
        float time = timeSinceExplosion / explosionDuration;
        //in this case we took localScale.x because why the fuck not x y z scale should all be the same anyways
        float val = Mathf.Lerp(transform.localScale.x, size, time);
        //to modify transform.localscale need to sue a vector 3
        transform.localScale = new Vector3(val, val, val);
        exploded = true;
    }

    private void implosion() {
        timeSinceImplosion += Time.fixedDeltaTime;
        float time = timeSinceImplosion / implosionDuration;
        //in this case we took localScale.x because why the fuck not x y z scale should all be the same anyways
        float val = Mathf.Lerp(transform.localScale.x, ogSize, time);
        //to modify transform.localscale need to sue a vector 3
        transform.localScale = new Vector3(val, val, val);
        exploded = true;

    }

    private void OnTriggerStay2D(Collider2D collision) {
        //if exploded deal damage to everything inside
        if (exploded) {
            if (collision.tag == "Character") {
                Character victim = collision.GetComponent<Character>();
                //deals damage to everyhing not in the shooters team
                if (victim.team != shooter.team) {
                    victim.HP -= PD * Time.fixedDeltaTime;
                    if (victim.HP <= 0) {
                        shooter.kill(victim);
                    }
                }
            }
        }
    }
    private void Start() {
        //doesn't inherit base start since we don't want the destroy(lifetime) to start on cast but rather on explosion
        targetPosition = target.transform.position;
        ogSize = transform.localScale.x;
    }

    private void FixedUpdate() {

        //if projectile reached targetPosition
        if (Vector2.SqrMagnitude((Vector2)transform.position - targetPosition) < 0.000001) {

            //if maximum size was attained mark it as true
            if (transform.localScale == new Vector3(size, size, size)) {
                sizeAttained = true;
            }
            
            //if maxSize attained start implosion
            if (sizeAttained) {
                implosion();
                //if implosion completed destroy the object
                if (transform.localScale.x == ogSize) {
                    Destroy(gameObject);
                }
            }
            //otherwise continue explosion
            else
                explosion();
        }
        else
            trajectory();

    }
}
