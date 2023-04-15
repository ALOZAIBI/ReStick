using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


[System.Serializable]
public class ShopData
{
    //contains the names of abilities that will then be fetched from ability factory
    public string []abilities;
    //contains wether an ability has been purchased or not
    public bool[] abilitiyPurchased;
    //same but for character
    public bool[] characterPurchased;

    //this is needed for SaveSystem to be able to deserialize it
    public ShopData() { }


}
