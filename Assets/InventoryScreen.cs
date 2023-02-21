using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public InventoryCharacterInfoScreen inventoryCharacterScreen;
    public Character characterSelected;
    public Ability abilitySelected;

    public Button backToScreenBtn;

    public PlayerManager playerParty;

    private void Start() {
        backToScreenBtn.onClick.AddListener(backToScreen);
    }
    //goes back to landing page
    public void backToScreen() {
        Header.SetActive(true);
        Body.SetActive(true);
        inventoryCharacterScreen.close();
        inventoryCharacterScreen.gameObject.SetActive(false);
        characterSelected = null;
        abilitySelected = null;
        //hide the back button
        backToScreenBtn.gameObject.SetActive(false);
    }
    public void setupInventoryScreen() {
        setupHeader();
        setupBody();
    }
    //displays playerParty Characters
    private void setupHeader() {
        //loops through children of playerParty
        foreach (Transform child in playerParty.transform) {
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

    public void closeHeader() {
        //destroys all ability displays
        foreach (Transform toDestroy in Header.transform) {
            GameObject.Destroy(toDestroy.gameObject);
        }
    }

    private void setupBody() {

    }
    public void viewCharacter() {
        Header.SetActive(false);
        inventoryCharacterScreen.gameObject.SetActive(true);
        inventoryCharacterScreen.viewCharacter(characterSelected);
        //unhides back button
        backToScreenBtn.gameObject.SetActive(true);
    }
}
