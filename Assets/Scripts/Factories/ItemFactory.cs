using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{

    //Contains all items
    public List<GameObject> items = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform) {
            items.Add(child.gameObject);
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

    //Returns a random item from the list
    public GameObject randomItem() {
        return items[Random.Range(0, items.Count)];
    }


}
