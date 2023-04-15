using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScreen : MonoBehaviour
{
    public GameObject abilityDisplayObj;
    public GameObject characterDisplayObj;


    public GameObject abilityArea;
    public GameObject characterArea;

    //cost of purchase of abilities
    public int commonCost;
    public int rareCost;
    public int epicCost;
    public int legendaryCost;

    //so that AbilityDisplayShop can deselect everything else when it is selected
    public List<AbilityDisplayShop> listAbilities = new List<AbilityDisplayShop>();
    //Haven't done yet but should do same as listAbilities
    public List<CharacterDisplayShop> listCharacters = new List<CharacterDisplayShop>();

    //returns cost of ability This is based on rarity
    private int costOfAbility(Ability ability) {
        switch (ability.rarity) {
            case (int)Ability.raritiesList.Common:
                return commonCost;
            case (int)Ability.raritiesList.Rare:
                return rareCost;
            case (int)Ability.raritiesList.Epic:
                return epicCost;
            case (int)Ability.raritiesList.Legendary:
                return legendaryCost;
            default:
                throw new Exception("RarityUnkown");
        }
    }
    //closes the abilityDisplays
    private void closeAbilities() {
        foreach(Transform child in abilityArea.transform) {
                Destroy(child.gameObject);
        }
    }
    private void closeCharacters() {
        foreach (Transform child in characterArea.transform) {
            Destroy(child.gameObject);
        }
    }

    public void close() {
        closeAbilities();
        closeCharacters();
    }
    public void displayAbilities(Shop shop) {
        //creates ability Displays
        for (int i = 0; i < shop.abilities.Count; i++) {
            //creates the display and makes it a child of abilityArea
            AbilityDisplayShop abilityDisplay = Instantiate(abilityDisplayObj,abilityArea.transform).GetComponent<AbilityDisplayShop>();
            listAbilities.Add(abilityDisplay);
            //gets the ability from shop
            Ability temp = shop.abilities[i];
            abilityDisplay.price.text = costOfAbility(temp)+"";
            abilityDisplay.ability = temp;
            abilityDisplay.abilityName.text = temp.abilityName;
            abilityDisplay.description.text = temp.description;
            //sets the scale for some reason if I dont do this the scale is set to 167
            abilityDisplay.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void displayCharacters(Shop shop) {
        //creates character Displays
        foreach(Transform child in shop.transform) {
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();   
                CharacterDisplayShop characterDisplay = Instantiate(characterDisplayObj,characterArea.transform).GetComponent<CharacterDisplayShop>();
                listCharacters.Add(characterDisplay);
                characterDisplay.character = temp;
            }
        }
    }
}
