using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MonoBehaviour
{
    public Button closeScreenBtn;

    public GameObject inventoryCharacterDisplay;
    public Transform characterHolder;

    public CharacterInfoScreen characterInfoScreen;


    private void Start() {
        //backToScreenBtn.onClick.AddListener(backToScreen);
        //uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        closeScreenBtn.onClick.AddListener(closeScreen);

        //This is set in the editor
        //characterInfoScreen.inventoryScreen = true;
        
    }
    public void setupInventoryScreen() {
        displayCharacters();
        characterInfoScreen.gameObject.SetActive(false);
    }

    private void displayCharacters() {
        //deletes all created instances before recreating to account for dead characters etc..
        closeCharacters();
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
    private void closeCharacters() {
        //Deletes all children of characterHolder except the ones with the dontDelete tag
        foreach (Transform child in characterHolder) {
            if (child.tag != "dontDelete") {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
    private void closeScreen() {
        GetComponent<HideUI>().hidden = true;
    }

    public void viewCharacter(Character charSel) {
        characterInfoScreen.gameObject.SetActive(true);
        characterInfoScreen.viewCharacterFullScreen(charSel);
        characterInfoScreen.time = .23f;
    }
}
