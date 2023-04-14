using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScreen : MonoBehaviour
{
    public GameObject abilityDisplayObj;
    public GameObject characterDisplayObj;


    public GameObject abilityArea;
    public GameObject characterArea;
    //so that AbilityDisplayShop can deselect everything else when it is selected
    public List<AbilityDisplayShop> listAbilities = new List<AbilityDisplayShop>();
    //Haven't done yet but should do same as listAbilities
    public List<CharacterDisplayShop> listCharacters = new List<CharacterDisplayShop>();
    public void displayAbilities(Shop shop) {
        //creates ability Displays
        for (int i = 0; i < shop.abilities.Count; i++) {
            //creates the display and makes it a child of abilityArea
            AbilityDisplayShop abilityDisplay = Instantiate(abilityDisplayObj,abilityArea.transform).GetComponent<AbilityDisplayShop>();
            listAbilities.Add(abilityDisplay);
            //gets the ability from shop
            Ability temp = shop.abilities[i];
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
