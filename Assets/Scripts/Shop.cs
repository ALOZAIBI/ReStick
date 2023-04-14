using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    //Characters are children of this class but the abilities are just in the list
    //
    public List<Ability> abilities = new List<Ability>();

    

    //the thing that holds the buttons and stuff
    public GameObject display;

    public Button openShopBtn;
    public Button closeDisplayBtn;

    //clicky stuff
    private float mouseHoldDuration = 0;
    private bool click = false;
    private void Start() {
        //adds abilities to the shop
        if (SaveSystem.loadShopAbilities(this)) {
            //abilities have been loaded but not yet instantiated. They will be instantiated when a purchase is done
        }
        else {
            UIManager.singleton.abilityFactory.addRandomAbilityToShop(this, 6);
            //then saves the random abilities so that they are loaded next time
            SaveSystem.saveShopAbilities(this);
        }
        //adds characters to the shop
        SaveSystem.loadShopCharacters(this);
        Debug.Log("CHild coubnt shiop" + transform.childCount);
        //checks if this has exactly 1 child(the canvas) if it doesn't it means there were no characters loaded
        if(transform.childCount == 1) {
            //so do add characters.
            UIManager.singleton.characterFactory.addRandomCharacterAsChild(transform,2);
            foreach(Transform child  in transform) {
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
