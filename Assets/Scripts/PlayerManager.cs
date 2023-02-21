using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int totalLives = 5;
    //holds the ability Inventory
    public GameObject abilityInventory;
    //holds the abilities that are currently being used by a character in the party
    public GameObject activeAbilities;

    public int gold = 0;
}
