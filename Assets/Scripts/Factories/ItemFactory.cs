using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{

    //Contains all items
    public List<GameObject> items = new List<GameObject>();

    //List per rarity
    public List<GameObject> common = new List<GameObject>();

    public List<GameObject> rare = new List<GameObject>();

    public List<GameObject> epic = new List<GameObject>();

    public List<GameObject> legendary = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform) {
            items.Add(child.gameObject);
            Item temp = child.GetComponent<Item>();
            if (temp.rarity == (int)Item.RaritiesList.Common)
                common.Add(child.gameObject);
            if (temp.rarity == (int)Item.RaritiesList.Rare) 
                  rare.Add(child.gameObject);
            if (temp.rarity == (int)Item.RaritiesList.Epic) 
                  epic.Add(child.gameObject);
            if (temp.rarity == (int)Item.RaritiesList.Legendary)
                legendary.Add(child.gameObject);

        }
    }

    //returns the gameObject from the abilities list given the ability's name
    public GameObject objectFromName(string name) {
        foreach (GameObject obj in items) {
            if (obj.GetComponent<Item>().itemName == name) {
                return obj;
            }
        }
        return null;
    }

    public void addRequestedAbilitiesToCharacter(Character character, string[] itemNames) {
        int count = 0;
        foreach (string name in itemNames) {
            //fetches the item frmo the list
            Item item = objectFromName(name).GetComponent<Item>();
            //creates the item and adds it as child
            Item temp = Instantiate(item, UIManager.singleton.playerParty.activeItems.transform);
            //adds it to character
            character.items.Add(temp);

            count++;
        }
    }

    public void addRequestedItemsToInventory(List<string> abilityNames) {
        foreach (string name in abilityNames) {
            //Debug.Log("ABILIT DEBUG+"+name);
            GameObject obj = objectFromName(name);
            Instantiate(obj, UIManager.singleton.playerParty.itemInventory.transform);
        }
    }

    public void addRequestedItemsToShop(Shop shop,List<string> itemNames) {
        foreach(string name in itemNames) {
            GameObject obj = objectFromName(name);
            Instantiate(obj, shop.itemHolder.transform);
        }
    }
    //Returns a random item based on the rarity
    public GameObject randomItem() {
        int randomRarity = Random.Range(0, 100);
        int randomItem;
        if (randomRarity < 7) {
            randomItem = Random.Range(0, legendary.Count);
            return legendary[randomItem];
        }
        else if (randomRarity < 22) {
            randomItem = Random.Range(0, epic.Count);
            return epic[randomItem];
        }
        else if (randomRarity < 65) {
            randomItem = Random.Range(0, rare.Count);
            return rare[randomItem];
        }
        else {
            randomItem = Random.Range(0, common.Count);
            return common[randomItem];
        }
        
    }


}
