using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZoneData
{
    public string zoneName;
    //contains the names of abilities that will then be fetched from ability factory
    public string []abilities;
    //wether zone has been completed or not
    public bool completed;

    public ZoneData(Zone zone) {
        zoneName = zone.zoneName;
        //creates array containing all abilityRewards' names
        abilities = new string[zone.abilityRewardPool.Count];
        for(int i = 0; i < zone.abilityRewardPool.Count; i++) {
            abilities[i] = zone.abilityRewardPool[i].GetComponent<Ability>().abilityName;
        }

        completed = zone.completed;
    }
}
