using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeAttack : MonoBehaviour
{
    //these values are set by the calling character
    public Character target;
    //the instantiating character
    public Character character;
    //Speed of the growth of the attack
    public float speed;
    //How long the attack lasts
    public float lifetime;
    //Damage of the attack
    public float DMG;
    //LS Amount
    public float LS;
    //maximum range of the attack
    public float range;
    //an object that is used in some melee attacks to be able to change the object's origin (pivot) https://www.youtube.com/watch?v=NsUJDqEY8tE
    public GameObject emptyPivot;
    //debuff or buff that this attack applies
    //the buff will be added by the ability creating the attack
    public Buff buff;

    //This is stikll not in use
    //some attack's  have a description that will be read by the castMeleeAttack ability
    public string description;


    //name of the ability that created this attack. This is used in BuffNotOnTarget check
    public string castingAbilityName;

    //handles the attack. In some cases it's a circle AOE that grows, in others its a box that grows towards axis it's been angled at
    public abstract void attackHandler();

    
    public virtual void Start() {

    }

    //returns true if no buff on character and if there is the sameBuff on Character simply refresh it's duration
    public bool buffNotOnCharacter(Character victim) {
        try {
            foreach (Buff temp in victim.buffs) {
                //if buff is already applied refresh it's duration
                if (temp.code == castingAbilityName + character.name) {
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
                temp.gameObject.SetActive(true);
                temp.target = victim;
                temp.applyBuff();
            }
        }
    }

}
