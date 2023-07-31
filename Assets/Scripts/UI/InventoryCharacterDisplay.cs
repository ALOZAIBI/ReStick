using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

public class InventoryCharacterDisplay : MonoBehaviour
{
    public Character character;
    //the btn is the cooldownBar itself 
    [SerializeField] private Button btn;
    [SerializeField] public InventoryScreen inventoryScreen;
    [SerializeField] private Image characerPortrait;
    [SerializeField] private CharacterHealthBar healthBar;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private GameObject notification;
    public GameObject deathSkull;
    void Start()
    {
        btn.onClick.AddListener(viewCharacter);
        //sets the image
        characerPortrait.sprite = character.GetComponent<SpriteRenderer>().sprite;
        characerPortrait.color = character.GetComponent<SpriteRenderer>().color;
        //sets the HPbar
        healthBar.character = character;
        //sets the name
        name.text = character.name;
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
