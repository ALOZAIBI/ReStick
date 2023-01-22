using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class UIManager : MonoBehaviour
{
    //used to reset cam position when changing scenes 
    public Camera cam;

    //Btn used to close all UI
    public Button closeUIBtn;

    //Character Screen Stuff
    public Image characterScreen;
    public TextMeshProUGUI characterName;
    //stats texts
    public TextMeshProUGUI DMG, AS, MS, RNG, LS;
    //cool stats texts
    public TextMeshProUGUI totalKills;
    public CharacterHealthBar healthBar;

    public Image characterPortrait;

    
    //Used to instantiate AbilityDisplay prefab
    public GameObject abilityDisplay;
    //Instantiate abilityDisplay as child of this
    public GameObject abilityDisplayPanel;

    //Game Won Screen Stuff
    public Image gameWonScreen;
    public Button parentSceneBtn;
    public string mapSceneName;

    public Button pausePlayBtn;
    //true if paused
    public bool pause=false;
    //used to check if the game was paused before clicking on viewCharacter
    public bool wasPause = false;

    private void Start() {
        parentSceneBtn.onClick.AddListener(loadMap);
        pausePlayBtn.onClick.AddListener(pausePlay);
        closeUIBtn.onClick.AddListener(closeUI);
    }

    //This function is called in Character with currChar being the character that triggered it. Find Index of character to be able to scroll
    //to next character
    public void viewCharacter(Character currChar) {
        //opens the screen and pauses the game
        characterScreen.gameObject.SetActive(true);
        pause = true;
        Time.timeScale = 0;
        //the close button pops up and the pause button is hidden
        closeUIBtn.gameObject.SetActive(true);
        pausePlayBtn.gameObject.SetActive(false);
        //sets the attributes to the character's
        characterName.text = currChar.name;
        //sets the image of character
        characterPortrait.sprite = currChar.GetComponent<SpriteRenderer>().sprite;
        characterPortrait.color = currChar.GetComponent<SpriteRenderer>().color;
        displayStats(currChar);
        displayCharacterAbilities(currChar);
    }

    public void displayCharacterAbilities(Character currChar) {
        foreach(Ability ability in currChar.abilities) {
            GameObject temp = Instantiate(abilityDisplay);
            //sets the instantiated object as child
            temp.transform.parent = abilityDisplayPanel.transform;
            AbilityDisplay displayTemp = temp.GetComponent<AbilityDisplay>();
            //sets the displays name and description
            displayTemp.abilityName.text = ability.abilityName;
            displayTemp.description.text = ability.description;
            //resetting scale to 1 cuz for somereaosn the scale is 167 otherwise
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    //displays the stats and cool stats of the character and character screen
    private void displayStats(Character currChar) {
        //the empty quotes is to convert float to str
        DMG.text = currChar.DMG+"";
        AS.text = currChar.AS + "";
        MS.text = currChar.MS + "";
        RNG.text = currChar.Range + "";
        LS.text = currChar.LS + "";
        totalKills.text = currChar.totalKills+"";
        //fills the HP bar correctly
        healthBar.character = currChar;
    }

    //displays the game won screen and prompts the player to click to go back to the scene with name sceneName
    public void displayGameWon(string sceneName) {
        gameWonScreen.gameObject.SetActive(true);
        pausePlayBtn.gameObject.SetActive(false);
        mapSceneName = sceneName;
    }
    //this is triggered by a button
    //loads map and reset cam position
    private void loadMap() {
        //resets position of axes except Z 
        cam.transform.position = new Vector3(0, 0, cam.transform.position.z);
        gameWonScreen.gameObject.SetActive(false);
        pausePlayBtn.gameObject.SetActive(true);
        SceneManager.LoadScene(mapSceneName);
    }
    private void closeUI() {
        //closes all UIScreens
        characterScreen.gameObject.SetActive(false);
        gameWonScreen.gameObject.SetActive(false);
        //Hides the close Button and shows the pause Button
        closeUIBtn.gameObject.SetActive(false);
        pausePlayBtn.gameObject.SetActive(true);
        //go back to the game paused or unpaused determined by if it waspaused before UI was opened
        pause = wasPause;
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

        //destroys all ability displays
        foreach(Transform toDestroy in abilityDisplayPanel.transform) {
            GameObject.Destroy(toDestroy.gameObject);
        }
    }
    private void pausePlay() {
        //Flips the pause switch then pauses or unpauses
        pause = !pause;
        if (pause) {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;
        //wasPause = pause
        wasPause = pause;
    }
}
