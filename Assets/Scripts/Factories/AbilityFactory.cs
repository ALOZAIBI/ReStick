using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbilityFactory : MonoBehaviour
{
    //contains all abilities
    public List<GameObject> abilities = new List<GameObject>();

    //contains only common
    public List<GameObject> common = new List<GameObject>();
    
    public List<GameObject> rare = new List<GameObject>();

    public List<GameObject> epic = new List<GameObject>();

    public List<GameObject> legendary = new List<GameObject>();


    //on starts adds all children to the abilities list
    private void Start() {
        foreach(Transform child in transform) {
            abilities.Add(child.gameObject);
            Ability temp = child.GetComponent<Ability>();
            if (temp.rarity == (int)Ability.raritiesList.Common)
                common.Add(child.gameObject);
            if (temp.rarity == (int)Ability.raritiesList.Rare) {
                rare.Add(child.gameObject);
            }
            if (temp.rarity == (int)Ability.raritiesList.Epic) {
                epic.Add(child.gameObject);
            }
            if (temp.rarity == (int)Ability.raritiesList.Legendary) {
                legendary.Add(child.gameObject);
            }
        }
    }
    //takes Drop Rates as paramter
    //then returns an ability GameObject with the droprate of each rarity taken into account
    private GameObject randomAbility( int rareDR, int epicDR, int legendaryDR) {
        int randomRarity = Random.Range(0, 100);
        int randomAbility;
        if (randomRarity < legendaryDR) {
            randomAbility = Random.Range(0, legendary.Count);
            return legendary[randomAbility];
        }
        else if (randomRarity < epicDR) {
            randomAbility = Random.Range(0, epic.Count);
            return epic[randomAbility];
        }
        else if (randomRarity < rareDR) {
            randomAbility = Random.Range(0, rare.Count);
            return rare[randomAbility];
        }
        else {
            randomAbility = Random.Range(0, common.Count);
            return common[randomAbility];
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
    public void addRandomAbilityToShop(Shop shop,int amount) {
        for(int i = 0;i < amount; i++) {
            Instantiate(randomAbility(30,15,5),shop.abilityHolder.transform);
        }
    }
    public void addRequestedAbilitiesToShop(Shop shop, List<string> abilityNames) {
        foreach(string abilityName in abilityNames) {
            GameObject obj = objectFromName(abilityName);
            Instantiate(obj,shop.abilityHolder.transform);
        }
    }
    //name self explanatory innit
    //adds amount amonut of abilities to the zone
    public void addRandomAbilityToZone(Zone zone,int amount) {
        for (int i = 0; i < amount; i++) {

            addsObjectToZone(zone, randomAbility(30, 15, 5));
        }
    }

    //adds the abilityNames' respective ability to the zone
    public void addRequestedAbilityToZone(Zone zone, List<string> abilityNames) {
        foreach(string name in abilityNames) {
            GameObject obj = objectFromName(name);
            addsObjectToZone(zone, obj);
        }
    }

    public void addRequestedAbilitiesToCharacter(Character character, string[] abilityNames, int[] abilityTargetting) {
        int count = 0;
        foreach(string name in abilityNames) {
            //fetches the ability frmo the list
            Ability ability = objectFromName(name).GetComponent<Ability>();
            //creates the ability and adds it as child
            Ability temp =Instantiate(ability, UIManager.singleton.playerParty.activeAbilities.transform);
            //sets the target strategy
            Debug.Log("Target Strategy setting from : to "+ temp.targetStrategy + ":" + abilityTargetting[count]);
            temp.targetStrategy = abilityTargetting[count];
            Debug.Log("Target Strategy set to" + temp.targetStrategy + ":" + abilityTargetting[count]);
            //adds it to character
            character.abilities.Add(temp);

            count++;
        }
    }
    public void addRequestedAbilitiesToInventory(List<string> abilityNames) {
        foreach(string name in abilityNames) {
            //Debug.Log("ABILIT DEBUG+"+name);
            GameObject obj = objectFromName(name);
            Instantiate(obj, UIManager.singleton.playerParty.abilityInventory.transform);
        }
    }
}
