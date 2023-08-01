using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MonoBehaviour
{
    public Button closeScreenBtn;

    [SerializeField] private Button displayCharactersBtn;
    [SerializeField] private Button displayAbilitiesBtn;
    //Prefab to be instantiated
    public GameObject inventoryCharacterDisplay;
    public GameObject abilityDisplay;

    public GameObject charactersScreen;
    public GameObject abilitiesScreen;

    public Transform characterHolder;
    public Transform abilityHolder;

    public CharacterInfoScreen characterInfoScreen;


    private void Start() {
        //backToScreenBtn.onClick.AddListener(backToScreen);
        //uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        closeScreenBtn.onClick.AddListener(closeScreen);

        displayCharactersBtn.onClick.AddListener(displayCharactersScreen);
        displayAbilitiesBtn.onClick.AddListener(displayAbilitiesScreen);

        //This is set in the editor
        //characterInfoScreen.inventoryScreen = true;
        
    }
    public void setupInventoryScreen() {
        displayCharacters();
        characterInfoScreen.gameObject.SetActive(false);
        charactersScreen.SetActive(true);
        abilitiesScreen.SetActive(false);
    }

    private void displayCharactersScreen() {
        close();
        charactersScreen.SetActive(true);
        abilitiesScreen.SetActive(false);
        displayCharacters();
    }
    private void displayAbilitiesScreen() {
        close();
        charactersScreen.SetActive(false);
        abilitiesScreen.SetActive(true);
        displayAbilities();
    }

    private void displayCharacters() {
        //loops through children of playerParty
        foreach (Transform child in UIManager.singleton.playerParty.transform) {
            //Debug.Log("Child" + child.name+child.tag);
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                //instantiates a charcaterDisplay
                InventoryCharacterDisplay display = Instantiate(inventoryCharacterDisplay,characterHolder).GetComponent<InventoryCharacterDisplay>();
                display.character = temp;
                ////sets the scale for some reason if I dont do this the scale is set to 167
                //display.gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    private void displayAbilities() {
        float width = abilityHolder.GetComponent<RectTransform>().rect.width;
        float height = abilityHolder.GetComponent<RectTransform>().rect.height;
        //Sets the size of the grid
        GridLayoutGroup grid = abilityHolder.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(0.3f*width, 0.35f*(height/2));
        grid.spacing = new Vector2(0.025f * width, 0.05f * height);
        //Loops through all abilities in inventory and instantiates them as child of abilityHolder
        foreach(Transform child in UIManager.singleton.playerParty.abilityInventory.transform) {
            Ability temp = child.GetComponent<Ability>();
            //Instantiates ability display
            AbilityDisplay display = Instantiate(abilityDisplay, abilityHolder).GetComponent<AbilityDisplay>();
            display.setupAbilityDisplay(temp);
        }
    }
    private void closeCharacters() {
        //Deletes all children of characterHolder except the ones with the dontDelete tag
        foreach (Transform child in characterHolder) {
            if (child.tag != "dontDelete") {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
    private void closeAbilities() {
        //Deletes all children of abilityHolder except the ones with the dontDelete tag
        foreach (Transform child in abilityHolder) {
            if (child.tag != "dontDelete") {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
    private void close() {
        closeCharacters();
        closeAbilities();
    }

    private void closeScreen() {
        close();
        GetComponent<HideUI>().hidden = true;
    }

    public void viewCharacter(Character charSel) {
        characterInfoScreen.gameObject.SetActive(true);
        characterInfoScreen.viewCharacterFullScreen(charSel);
        characterInfoScreen.time = .23f;
    }
    private void Update() {
    }
}
