using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


[System.Serializable]
public class ShopData
{
    //contains the names of abilities that will then be fetched from ability factory
    public List<string> abilityNames = new List<string>();
    //contains wether an ability has been purchased or not
    public bool[] abilitiyPurchased;
    //same but for character
    public bool[] characterPurchased;

    //this is needed for SaveSystem to be able to deserialize it
    public ShopData() { }

    public ShopData(Shop shop) {
        //gets the ability Names
        foreach (Transform ability in shop.abilityHolder.transform) {
            abilityNames.Add(ability.GetComponent<Ability>().abilityName);
        }
        abilitiyPurchased = shop.abilitiyPurchased;
        characterPurchased = shop.characterPurchased;
    }
    //sends purchaseInfo to shop
    public void purchaseInfoToShop(Shop shop) {
        shop.abilitiyPurchased = abilitiyPurchased;
        shop.characterPurchased = characterPurchased;
    }
}
