using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

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

    public float initSize;
    public float targetSize;
    public float growSpeed;
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

    public HitFX hitFX;
    //handles the trajectory of the projectile
    public abstract void trajectory();

    public virtual void Start() {
        //destroys projectile after lifetime
        //lifetime thing is not frame independent so if need to be fixed in the future use a custom timer that is incremented
        //woith fixed update think cooldown that is used in character smthn like that
        Destroy(gameObject, lifetime);
        //Starts with initSize
        if(initSize != 0 && targetSize != 0) {
            transform.localScale = new Vector3(initSize, initSize, initSize);
        }
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
    /// <summary>
    /// Creates an instance of the buff and applies it on the victim
    /// </summary>
    /// <param name="victim"></param>
    public void applyBuff(Character victim) {
        if (buff != null) {
            if (buffNotOnCharacter(victim)) {
                //create an instance of the buff
                Buff temp = Instantiate(buff);
                temp.gameObject.SetActive(true);
                temp.target = victim;
                temp.applyBuff();
            }
        }
    }

    //If this has an initSize and targetSize set, this will grow the projectile to that size
    protected void grow() {
        if (initSize != 0 && targetSize != 0 && transform.localScale.x < targetSize) {
            //Grows the projectile by fixeddeltatime to reach targetSize
            transform.localScale += new Vector3(1, 1, 1) * Time.fixedDeltaTime*growSpeed;
        }
    }
    //Instantiates an active HitFX at position
    public void applyHitFX(Character character) {
        applyHitFX(character.transform.position);
    }
    public void applyHitFX(Vector3 position) {
        HitFX temp = Instantiate(hitFX, position,Quaternion.identity);
        temp.gameObject.SetActive(true);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            temp.GetComponent<SpriteRenderer>().color = GetComponent<SpriteShapeRenderer>().color;
        }
        else {
            //Makes the instantiated object's color same as the projectile color
            temp.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
        }
    }
    
}
