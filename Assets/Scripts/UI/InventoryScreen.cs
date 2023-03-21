using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MonoBehaviour
{
    //contains character displays
    public GameObject Header;
    public GameObject pickCharacterToolTip;
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

    public UIManager uiManager;

    public Button backToScreenBtn;

    public PlayerManager playerParty;

    public int pageIndex = 0;
    //t
    private void Start() {
        backToScreenBtn.onClick.AddListener(backToScreen);
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }
    public void openLandingPage() {
        backToScreenBtn.gameObject.SetActive(false);
        inventoryCharacterScreen.gameObject.SetActive(false);
        AbilityHeader.SetActive(false);
        pickCharacterToolTip.SetActive(false);

        //shows the close button
        uiManager.closeUIBtn.gameObject.SetActive(true);

        Body.SetActive(true);
        Header.SetActive(true);
        pageIndex = 0;

        closeHeader();
        closeBody();

        setupHeader();
        setupBody();
    }

    public void openAbilityPickedPage() {
        //deletes other ability displays
        closeBody();
        makeCharDisplayGlow();
        backToScreenBtn.gameObject.SetActive(true);
        inventoryCharacterScreen.gameObject.SetActive(false);
        AbilityHeader.SetActive(true);
        pickCharacterToolTip.SetActive(true);

        //hides the close button
        uiManager.closeUIBtn.gameObject.SetActive(false);

        Body.SetActive(false);
        Header.SetActive(true);

        pageIndex = 1;
    }

    //when ability is picked then character is picked
    public void openAbilityCharacterPage() {
        backToScreenBtn.gameObject.SetActive(true);
        inventoryCharacterScreen.gameObject.SetActive(true);
        AbilityHeader.SetActive(true);
        pickCharacterToolTip.SetActive(false);

        //hides the close button
        uiManager.closeUIBtn.gameObject.SetActive(false);

        Body.SetActive(true);
        Header.SetActive(false);

        pageIndex = 2;
    }
    //when character is selected first
    public void openCharacterSelectedPage() {
        backToScreenBtn.gameObject.SetActive(true);
        inventoryCharacterScreen.gameObject.SetActive(true);
        AbilityHeader.SetActive(false);
        pickCharacterToolTip.SetActive(false);

        //hides the close button
        uiManager.closeUIBtn.gameObject.SetActive(false);

        Body.SetActive(true);
        Header.SetActive(false);

        pageIndex = 3;
    }

    //goes back to landing page
    public void backToScreen() {
        //if ability selected first then character selected.
        //clicking the back button in this case keeps ability in ability header and prompts to pick character so basically 
        //going back 1 step its intuitive trust me.

        //if character selected back brings back to landingPage
        switch (pageIndex) {
            case 1:
                openLandingPage();
                break;
            case 2:
                openAbilityPickedPage();
                break;
            case 3:
                Debug.Log("Fixing bug");
                openLandingPage();
                break;
            default:
                openLandingPage();
                break;
        }
    }
    public void setupInventoryScreen() {
        openLandingPage();
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
            if(toDestroy.tag=="Button")
                GameObject.Destroy(toDestroy.gameObject);
        }
    }

    public void makeCharDisplayGlow() {
        foreach (Transform toDestroy in Header.transform) {
            if (toDestroy.tag == "Button") {
                toDestroy.GetComponent<InventoryCharacterDisplay>().glow = true;
            }
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
        //deletes the abilities
        closeBody();
        inventoryCharacterScreen.viewCharacter(characterSelected);
        openCharacterSelectedPage();
    }
}
