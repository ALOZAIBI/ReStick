using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    //Characters are children of this class but the abilities are just in the list
    //
    public List<Ability> abilities = new List<Ability>();


    //contains wether an ability has been purchased or not
    public bool[] abilitiyPurchased;
    //same but for character
    public bool[] characterPurchased;


    public GameObject abilityHolder;
    public GameObject characterHolder;

    public Button openShopBtn;


    //clicky stuff
    private float mouseHoldDuration = 0;
    private bool click = false;
    private void Start() {
        //adds abilities to the shop
        if (SaveSystem.loadShopAbilitiesAndPurchaseInfo(this)) {
            //if there was a save file
        }//if there was no save file
        else {
            int abilityAmount = 6;
            UIManager.singleton.abilityFactory.addRandomAbilityToShop(this, abilityAmount);
            abilitiyPurchased = new bool[abilityAmount];
            //then saves the random abilities so that they are loaded next time
            SaveSystem.saveShopAbilitiesAndPurchaseInfo(this);
        }
        //adds characters to the shop
        SaveSystem.loadShopCharacters(this);
        //checks if there wer no characters added (Which means there was no save)
        if(characterHolder.transform.childCount == 0) {
            //so do add characters.
            int characterAmount = 2;
            SaveSystem.characterNumber = 0;
            UIManager.singleton.characterFactory.addRandomCharacterAsChild(characterHolder.transform,characterAmount);
            characterPurchased = new bool[characterAmount];
            foreach (Transform child  in characterHolder.transform) {
                if(child.tag=="Character")
                   SaveSystem.saveShopCharacters(child.GetComponent<Character>());
            }
            //then saves the purchase info
            SaveSystem.saveShopAbilitiesAndPurchaseInfo(this);
        }

        UIManager.singleton.shopScreen.shop = this;
    }
}
