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

    public int gold = 0;

    private int initLifeShards;
    private int initGold;

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


    //if you're gonan change the number of children the player party has onAwake look at the save slot selector code and modify the amount of hcildren
}
