using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    //these values are set by the calling character
    public Character target;
    //the instantiating character
    public Character shooter;
    public float speed;
    public float lifetime;
    public float dmg;
    public float LS;

    //This is stikll not in use
    //some projectile's (Ability projectiles) have a description that will be read by the throwProjectile ability
    public string description;

    //direction that the projectile is to be fired
    //direction = targ.position - transform. position;
    public Vector2 direction;

    //handles the trajectory of the projectile
    public abstract void trajectory();

    public virtual void Start() {
        //destroys projectile after lifetime
        //lifetime thing is not frame independent so if need to be fixed in the future use a custom timer that is incremented
        //woith fixed update think cooldown that is used in character smthn like that
        Destroy(gameObject, lifetime);
    }

}
