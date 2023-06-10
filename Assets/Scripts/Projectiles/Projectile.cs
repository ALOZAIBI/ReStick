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
    public float DMG;
    public float LS;

    //debuff or buff that this projectile applies
    //the buff will be added by the ability creating the projectile
    public Buff buff;

    //This is stikll not in use
    //some projectile's (Ability projectiles) have a description that will be read by the throwProjectile ability
    public string description;

    //direction that the projectile is to be fired
    //direction = targ.position - transform. position;
    public Vector2 direction;

    //name of the ability that created this projectile. This is used in BuffNotOnTarget check
    public string castingAbilityName;

    //handles the trajectory of the projectile
    public abstract void trajectory();

    public virtual void Start() {
        //destroys projectile after lifetime
        //lifetime thing is not frame independent so if need to be fixed in the future use a custom timer that is incremented
        //woith fixed update think cooldown that is used in character smthn like that
        Destroy(gameObject, lifetime);
    }

    //returns true if no buff on character and if there is the sameBuff on Character simply refresh it's duration
    public bool buffNotOnCharacter(Character victim) {
        try {
            foreach (Buff temp in victim.buffs) {
                //if buff is already applied refresh it's duration
                if (temp.code == castingAbilityName + shooter.name) {
                    temp.durationRemaining = buff.duration;
                    return false;
                }
            }
        }
        catch { return true; };
        return true;
    }

    public void applyBuff(Character victim) {
        if (buff != null) {
            if (buffNotOnCharacter(victim)) {
                //create an instance of the buff
                Buff temp = Instantiate(buff);
                temp.target = victim;
                temp.applyBuff();
            }
        }
    }

}
