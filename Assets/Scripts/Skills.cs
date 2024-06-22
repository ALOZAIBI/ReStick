using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//Includes skills that will be reused by abilities/items etc...
public static class Skills
{
    //public static Skills singleton;

    //private void Awake() {
    //    if(singleton == null || singleton !=this)
    //        singleton = this;
    //}

    //Returns the summoned character
    /// <summary>
    /// Summon a character
    /// </summary>
    /// <param name="toSummon">The Character to summon</param>
    /// <param name="summoner">The Character doing the summoning</param>
    /// <param name="strength">Strength of the summoned relative to  summoner</param>
    /// <param name="size">Size of the summoned relative to summoner</param>
    /// <param name="posRange">The number used to randomly position summoned relative to summoner</param>
    /// <param name="setRandomCD">If the summoned should have the CDs randomly set, If false all abilities will be available</param>
    /// <returns></returns>
    public static Character summon(this Character summoner,Character toSummon,float strength,float size=1,float posRange=2,bool setRandomCD = false) {
        //Debug.Log("Beginning summoning" + summoner.name + toSummon.name);
        Character summoned = Object.Instantiate(toSummon.gameObject, summoner.transform.position + new Vector3(Random.Range(-posRange, posRange), Random.Range(-posRange, posRange), -1), summoner.transform.rotation).GetComponent<Character>();

        summoned.summoned = true;
        summoned.summoner = summoner;

        summoned.team = summoner.team;
        summoned.movementStrategy = (int)Character.MovementStrategies.Default;

        //Removes the buffs from the summoned Character
        //the .ToArray is used to prevent the error of modifying the list while iterating through it.
        foreach (Buff tempBuff in summoned.buffs.ToArray()) {
            tempBuff.removeBuffAppliedStats(summoned);
        }
        //Clones the abilities
        int index = 0;
        foreach (Ability ability in toSummon.abilities) {
            Ability temp = Object.Instantiate(ability);
            temp.reset();
            if(setRandomCD)
                temp.setRandomCD();
            else {
                temp.available = true;
                temp.abilityNext = 0;
            }
            summoned.abilities[index] = temp;
            index++;
        }

        //Clones the items
        index = 0;
        foreach (Item item in toSummon.items) {
            Item temp = Object.Instantiate(item);
            temp.reset();
            summoned.items[index] = temp;
            index++;
        }

        //Modifies the stats of the summoned character
        if(strength > 0) {
            summoned.PD = summoner.PD * strength;
            summoned.MD = summoner.MD * strength;
            summoned.INF = summoner.INF * strength;
            summoned.HPMax = summoner.HPMax * strength;
        }
        summoned.HP = summoned.HPMax;

        //Modifies the size
        if(size > 0) {
            summoned.transform.localScale = summoned.transform.localScale * size;
        }

        summoned.zone = summoner.zone;
        //Adds the clone to the zone
        summoner.zone.charactersInside.Add(summoned);

        summoned.alive = true;
        summoned.dieNextFrame = false;

        //Debug.Log("Summoning complete" + summoner.name + toSummon.name);

        Debug.Log("DBG:" + summoned.name + " Summoned Renderer:" + summoned.GetComponent<Renderer>().enabled + " Active" + summoned.isActiveAndEnabled + "Layer I'm in" + summoned.gameObject.layer);
        summoned.DebugClone(summoned.gameObject);
        return summoned;
    }

}
