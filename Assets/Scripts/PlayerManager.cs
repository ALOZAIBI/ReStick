using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int lifeShards = 5;
    public int maxLifeShards = 7;
    //holds the ability Inventory
    public GameObject abilityInventory;
    //holds the abilities that are currently being used by a character in the party
    public GameObject activeAbilities;

    public GameObject itemInventory;
    public GameObject activeItems;
    public int gold = 0;

    private int initLifeShards;
    private int initGold;

    //Says if there are items or abilities that player just got and hasn't seen yet
    //Will be used for notifications
    public bool newItems = false;
    public bool newAbilities = false;

    public int oldItemCount = 0;
    public int oldAbilityCount = 0;

    /// <summary>
    /// Used to update the bools that determine wether a new item or ability has been added to the inventory
    /// </summary>
    public void setNewStuffNotifications() {
        newItems = itemInventory.transform.childCount > oldItemCount;
        newAbilities = abilityInventory.transform.childCount > oldAbilityCount;

    }
    //Used to update the old counts(Will be used when viewing the inventory)
    public void setOldItemCount() {
        oldItemCount = itemInventory.transform.childCount;
    }
    public void setOldAbilityCount() {
        oldAbilityCount = abilityInventory.transform.childCount;
    }

    private void Start() {
        initLifeShards = lifeShards;
        initGold = gold;
    }
    //Resets to initial values and clear children of activeAbilities and abilityInventory and all characters of the playerParty
    public void Reset() {
        lifeShards = initLifeShards;
        gold = initGold;
        foreach (Transform child in activeAbilities.transform) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in abilityInventory.transform) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in transform) {
            if(child.CompareTag("Character"))
                Destroy(child.gameObject);
        }
    }

    //Sets the character's to not be dropped.
    public void unDrop() {
        foreach (Transform child in transform) {
            if (child.CompareTag("Character")) {
                child.GetComponent<Character>().dropped = false;
            }
        }
    }

    //Returns list of characters that are children of the player party
    public List<Character> getCharacters() {
        List<Character> characters = new List<Character>();
        foreach (Transform child in transform) {
            if (child.CompareTag("Character")) {
                characters.Add(child.GetComponent<Character>());
            }
        }
        return characters;
    }


    //if you're gonan change the number of children the player party has onAwake look at the save slot selector code and modify the amount of hcildren
}
