using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbilityFactory : MonoBehaviour
{
    public List<GameObject> abilities = new List<GameObject>();

    //on starts adds all children to the abilities list
    private void Start() {
        foreach(Transform child in transform) {
            abilities.Add(child.gameObject);
        }
    }
    //returns the gameObject from the abilities list given the ability's name
    private GameObject objectFromName(string name) {
        foreach(GameObject obj in abilities) {
            if(obj.GetComponent<Ability>().abilityName == name) {
                return obj;
            }
        }
        return null;
    }

    private void addsObjectToZone(Zone zone,GameObject obj) {
        //creates a copy of the gameobject
        GameObject temp = Instantiate(obj);
        //adds it to the Zone's ability container
        temp.transform.parent = zone.abilityContainer.transform;
        //then adds it to the zone's rewardPool list
        zone.abilityRewardPool.Add(temp);
    }
    //name self explanatory innit
    //adds amount amonut of abilities to the zone
    public void addRandomAbilityToZone(Zone zone,int amount) {
        for (int i = 0; i < amount; i++) {
            Debug.Log("adding 1 ability");
            //gets a random ability
            int index = Random.Range(0, abilities.Count - 1);
            addsObjectToZone(zone, abilities[index]);
        }
    }

    //adds the abilityNames' respective ability to the zone
    public void addRequestedAbilityToZone(Zone zone, List<string> abilityNames) {
        foreach(string name in abilityNames) {
            GameObject obj = objectFromName(name);
            addsObjectToZone(zone, obj);
        }
    }

}
