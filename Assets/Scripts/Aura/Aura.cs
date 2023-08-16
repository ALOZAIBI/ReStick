using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Aura : MonoBehaviour
{
    public Character caster;

    public float amt;
    //wether this targets enemy otr ally or both 
    public bool enemy;
    public bool ally;

    public bool damage;
    public bool heal;

    public Buff buff;

    public string castingAbilityName;

    //We will use this list in the DashALotThenSheath
    public bool saveCharacterInAura;
    public List<Character> charactersInAura = new List<Character>();

    //returns true if no buff on character and if there is the sameBuff on Character simply refresh it's duration
    public bool buffNotOnCharacter(Character victim) {
        try {
            foreach (Buff temp in victim.buffs) {
                //if buff is already applied refresh it's duration
                if (temp.code == castingAbilityName + caster.name) {
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
                Debug.Log("IOssue");
            }
        }
    }

    private void FixedUpdate() {
        //If the character is silenced or dead then destroy the aura
        if (caster == null || caster.silence>0 || !caster.alive) {
            Destroy(gameObject);
        }
    }

}
