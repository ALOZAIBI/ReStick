using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

public class CharacterDisplayInventory : CharacterDisplay
{
    //the btn is the cooldownBar itself 

    [SerializeField] public InventoryScreen inventoryScreen;
    [SerializeField] private GameObject notification;
    public GameObject deathSkull;
    private new void Start()
    {
        base.Start();
        btn.onClick.AddListener(viewCharacter);

    }
    private void viewCharacter() {
        UIManager.singleton.inventoryScreen.viewCharacter(character);
    }
    private void Update() {
        deathSkull.SetActive(!character.alive);

        if (character.statPoints > 0) {
            notification.SetActive(true);
        }
        else
            notification.SetActive(false);
    }


}
