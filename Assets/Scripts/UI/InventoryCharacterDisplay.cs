using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryCharacterDisplay : MonoBehaviour
{
    public Character character;
    //the btn is the icon itself 
    [SerializeField] private Button btn;
    [SerializeField] public InventoryScreen inventoryScreen;
    [SerializeField] private Image characerPortrait;
    [SerializeField] private CharacterHealthBar healthBar;
    [SerializeField] private TextMeshProUGUI name;
    void Start()
    {
        inventoryScreen = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().inventoryScreen;
        btn.onClick.AddListener(viewCharacter);
        //sets the image
        characerPortrait.sprite = character.GetComponent<SpriteRenderer>().sprite;
        characerPortrait.color = character.GetComponent<SpriteRenderer>().color;
        //sets the HPbar
        healthBar.character = character;
        //sets the name
        name.text = character.name;
    }

    public void viewCharacter() {
        inventoryScreen.characterSelected = character;
        inventoryScreen.viewCharacter();
        //and also open CharacterScreen with character
    }
    
}
