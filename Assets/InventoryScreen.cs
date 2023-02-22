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
    public GameObject AbilityHeader;
    //object to be instantiated
    public GameObject inventoryCharacterDisplay;
    public GameObject inventoryAbilityDisplay;
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
        //if ability selected first then character selected.
        //clicking the back button in this case keeps ability in ability header and prompts to pick character so basically 
        //going back 1 step its intuitive trust me.
        if (abilitySelected != null && characterSelected != null && AbilityHeader.activeSelf ) {
            characterSelected = null;
            Body.SetActive(false);
            Header.SetActive(true);
            inventoryCharacterScreen.close();
            closeHeader();
            setupHeader();
        }
        else {
            Header.SetActive(true);
            Body.SetActive(true);
            AbilityHeader.SetActive(false);
            inventoryCharacterScreen.close();
            closeHeader();
            inventoryCharacterScreen.gameObject.SetActive(false);
            characterSelected = null;
            abilitySelected = null;
            //hide the back button
            backToScreenBtn.gameObject.SetActive(false);
            //sets up the stuff again.
            setupInventoryScreen();
        }
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

    public void closeAbilityHeader() {
        //destroys all ability displays
        foreach (Transform toDestroy in AbilityHeader.transform) {
            GameObject.Destroy(toDestroy.gameObject);
        }
    }

    //removes ability displayys from body.
    public void closeBody() {
        foreach (Transform toDestroy in Body.transform) {
            //destroys all inventoryAbilityDisplays
            if (toDestroy.GetComponent<InventoryAbilityDisplay>() != null) {
                Debug.Log("This will be detstroyed"+toDestroy.name);
                GameObject.Destroy(toDestroy.gameObject);
            }
        }
    }

    private void setupBody() {
        //loops thorugh the abilities in abilityInventory
        foreach (Transform child in playerParty.abilityInventory.transform) {
            Ability ability = child.GetComponent<Ability>();
            GameObject temp = Instantiate(inventoryAbilityDisplay);
            //sets the instantiated object as child
            temp.transform.parent = Body.transform;
            InventoryAbilityDisplay displayTemp = temp.GetComponent<InventoryAbilityDisplay>();
            //sets the displays name and description
            displayTemp.abilityName.text = ability.abilityName;
            displayTemp.description.text = ability.description;
            displayTemp.ability = ability;
            //resetting scale to 1 cuz for somereaosn the scale is 167 otherwise
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    public void viewCharacter() {
        Header.SetActive(false);
        //deletes the abilities
        closeBody();
        //then displays inventoryscreen
        inventoryCharacterScreen.gameObject.SetActive(true);
        inventoryCharacterScreen.viewCharacter(characterSelected);
        //unhides back button
        backToScreenBtn.gameObject.SetActive(true);
    }
}
