using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    //Characters are children of this class but the abilities are just in the list
    public List<Ability> abilities = new List<Ability>();

    public List<Item> items = new List<Item>();

    public bool[] itemPurchased;
    //contains wether an ability has been purchased or not
    public bool[] abilitiyPurchased;
    //same but for character
    public bool[] characterPurchased;

    public GameObject itemHolder;
    public GameObject abilityHolder;
    public GameObject characterHolder;


    //clicky stuff
    private float mouseHoldDuration = 0;
    private bool click = false;

    //Deletes the children of all holders
    private void clean() {
        foreach(Transform child in itemHolder.transform) 
            Destroy(child.gameObject);
        
        foreach (Transform child in abilityHolder.transform)
            Destroy(child.transform);

        foreach (Transform child in characterHolder.transform)
            Destroy(child.transform);
    }
    public void setup() {

        clean();

        //adds abilities to the shop
        int abilityAmount = 3;
        UIManager.singleton.abilityFactory.addRandomAbilityToShop(this, abilityAmount);
        abilitiyPurchased = new bool[abilityAmount];




        //Add 3 items
        int itemAmount = 3;
        for(int i = 0; i < itemAmount; i++) {
            Instantiate(UIManager.singleton.itemFactory.randomItem(), itemHolder.transform);
        }

        itemPurchased = new bool[itemAmount];

        int characterAmount = 1;
        SaveSystem.characterNumber = 0;
        UIManager.singleton.characterFactory.addCharactersToShop(characterHolder.transform,characterAmount);
        characterPurchased = new bool[characterAmount];
    }
}
