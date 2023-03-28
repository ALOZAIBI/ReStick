using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


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
    //this is needed for SaveSystem to be able to deserialize it
    public ZoneData() { }
 
    //sends the data to the zone
    public void dataToZone(Zone zone) {
        zone.completed = completed;
        //loads abilityNames
        foreach(string name in abilities) {
            zone.abilityNames.Add(name);
        }
    }
}
