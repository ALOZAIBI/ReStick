using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScreen : MonoBehaviour
{
    //contains character displays
    public GameObject Header;
    //contains ability displays
    public GameObject Body;
    public GameObject Footer;
    //object to be instantiated
    public GameObject inventoryCharacterDisplay;
    //the char screen is in body
    public CharacterInfoScreen inventoryCharacterScreen;
    public Character characterSelected;
    public Ability abilitySelected;

    public UIManager uiManager;

    private void Start() {
    }
    public void setupInventoryScreen() {
        setupHeader();
        setupBody();
    }

    private void setupHeader() {
        //loops through children of playerParty
        foreach (Transform child in uiManager.playerParty.transform) {
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                //instantiates a charcaterDisplay
                InventoryCharacterDisplay display = Instantiate(inventoryCharacterDisplay).GetComponent<InventoryCharacterDisplay>();
                display.character = temp;
                //sets this display as a child of header
                display.transform.parent = Header.transform;
                //sets the scale for some reason if I dont do this the scale is set to 167
                display.gameObject.transform.localScale = new Vector3(1, 1, 1);
                
            }
        }
    }

    private void setupBody() {

    }
    public void viewCharacter() {
        Header.SetActive(false);
        inventoryCharacterScreen.gameObject.SetActive(true);
        inventoryCharacterScreen.viewCharacter(characterSelected);
    }
}
