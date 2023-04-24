using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class InventoryData
{
    public int gold;
    public List<string> abilityNames;


    //this is needed for SaveSystem to be able to deserialize it
    public InventoryData() {
        abilityNames = new List<string>();
    }
}
