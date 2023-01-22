using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlacingScreen : MonoBehaviour
{
    public UIManager uiManager;

    //the prefab to be instantiated
    public GameObject characterDisplay;
    
    //displays the characters
    public void displayCharacters() {
        //deletes all created instances before recreating to account for dead characters etc..
        close();
        //loops through children of playerParty
        foreach (Transform child in uiManager.playerParty.transform) {
            //Debug.Log(child.name);
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                if (temp.alive) {
                    //instantiates a charcaterDisplay
                    CharacterDisplay display = Instantiate(characterDisplay).GetComponent<CharacterDisplay>();
                    display.character = temp;
                    //sets this display as a child 
                    display.transform.parent = transform;
                    //sets the scale for some reason if I dont do this the scale is set to 167
                    display.gameObject.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }

    //deletes all the created instances
    public void close() {
        foreach(Transform child in transform) {
            //Debug.Log("destroying "+child.name);
            GameObject.Destroy(child.gameObject);
        }
    }
}
