using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    //Characters are children of this class but the abilities are just in the list
    //
    public List<Ability> abilities = new List<Ability>();


    //contains wether an ability has been purchased or not
    public bool[] abilitiyPurchased;
    //same but for character
    public bool[] characterPurchased;

    //the thing that holds the buttons and stuff
    public GameObject display;

    public GameObject abilityHolder;
    public GameObject characterHolder;

    public Button openShopBtn;
    public Button closeDisplayBtn;

    //clicky stuff
    private float mouseHoldDuration = 0;
    private bool click = false;
    private void Start() {
        //adds abilities to the shop
        if (SaveSystem.loadShopAbilitiesAndPurchaseInfo(this)) {
            //if there was a save file
        }//if there was no save file
        else {
            int abilityAmount = 6;
            UIManager.singleton.abilityFactory.addRandomAbilityToShop(this, abilityAmount);
            abilitiyPurchased = new bool[abilityAmount];
            //then saves the random abilities so that they are loaded next time
            SaveSystem.saveShopAbilitiesAndPurchaseInfo(this);
        }
        //adds characters to the shop
        SaveSystem.loadShopCharacters(this);
        Debug.Log("CHild coubnt shiop" + transform.childCount);
        //checks if there wer no characters added (Which means there was no save)
        if(characterHolder.transform.childCount == 0) {
            //so do add characters.
            int characterAmount = 2;
            SaveSystem.characterNumber = 0;
            UIManager.singleton.characterFactory.addRandomCharacterAsChild(characterHolder.transform,characterAmount);
            characterPurchased = new bool[characterAmount];
            foreach(Transform child  in characterHolder.transform) {
                if(child.tag=="Character")
                   SaveSystem.saveShopCharacters(child.GetComponent<Character>());
            }
        }
        
        openShopBtn.onClick.AddListener(openShop);
        closeDisplayBtn.onClick.AddListener(close);
    }

    private void close() {
        display.SetActive(false);
    }

    private void openShop() {
        UIManager.singleton.openShop(this);
    }

    //onclick load specified scene
    private void OnMouseDown() {
        click = true;
    }

    //checks wether the click is held or not
    private void mouseClickedNotHeld() {

        if (click) {
            //counts mouse down to determine if click or hold
            if (Input.GetMouseButton(0)) {
                mouseHoldDuration += Time.unscaledDeltaTime;
            }
            //if click 
            else if (mouseHoldDuration < 0.2f) {
                //open shop
                display.SetActive(true);
                click = false;
            }
            //if HOLD
            else {
                //reset values
                mouseHoldDuration = 0;
                click = false;
            }
        }
    }

    private void Update() {
        mouseClickedNotHeld();
    }
    //when closing the shop save the abilites and characters and if they were sold or not
}
