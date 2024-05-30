using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class InventoryData
{
    public int gold;
    public int lifeShards;
    public List<string> abilityNames;
    public List<string> itemNames;


    //this is needed for SaveSystem to be able to deserialize it
    public InventoryData() {
        abilityNames = new List<string>();
        itemNames = new List<string>();
    }
}
