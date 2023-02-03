using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//THIS IS A CHILD OF CAMERA SINCE HIDEUI NEEDS LOCAL POSITION RELATIVE TO PARENT WHICH IS CAMERA
public class UIManager : MonoBehaviour
{
    //used to reset cam position when changing scenes 
    public Camera cam;

    //Btn used to close all UI
    public Button closeUIBtn;

    //Character Screen Stuff
    public CharacterScreen characterScreen;
    

    //The playerCharacters are children of this
    public PlayerManager playerParty;
    //screen that pops up oin lelve start
    public CharacterPlacingScreen characterPlacingScreen;
    
    

    //Game Won Screen Stuff
    public Image gameWonScreen;
    public Button wonToMapBtn;
    public RewardSelect rewardSelectScreen;

    //Game Lost Screen Stuff
    public Image gameLostScreen;
    public Button lostToMapBtn;
    public Button lostToRestartBtn;

    //the scene to be loaded
    public string mapSceneName;


    public Button pausePlayBtn;

    public TimeControl timeControl;
    //true if paused
    public bool pause=false;
    //used to check if the game was paused before clicking on viewCharacter
    public bool wasPause = false;

    //used to restartZone
    public Zone zone;

    //to know if hidden or not
    public HideUI placingScreenHidden;
    private void Start() {
        placingScreenHidden = characterPlacingScreen.GetComponent<HideUI>();

        //
        wonToMapBtn.onClick.AddListener(loadMap);
        lostToMapBtn.onClick.AddListener(loadMap);

        lostToRestartBtn.onClick.AddListener(restartZone);

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
        //hides placing screen
        placingScreenHidden.hidden = true;
        timeControl.gameObject.SetActive(false);
        //the close button pops up and the pause button+time control is hidden
        closeUIBtn.gameObject.SetActive(true);
        timeControl.gameObject.SetActive(false);
        pausePlayBtn.gameObject.SetActive(false);
        characterScreen.viewCharacter(currChar);
    }

    //so that displayGameWon isn't executed infinitely
    private bool displayed=false;
    //displays the game won screen and prompts the player to click to go back to the scene with name sceneName
    public void displayGameWon(string sceneName) {
        if (!displayed) {
            Debug.Log("Disaplyed");
            //gameWonScreen.gameObject.SetActive(true);
            rewardSelectScreen.gameObject.SetActive(true);
            rewardSelectScreen.displayAbilities();
            pausePlayBtn.gameObject.SetActive(false);
            mapSceneName = sceneName;
            displayed = true;
            //turn display back to false when gamewonscreen button is clicked
        }
    }

    public void displayGameLost(string sceneName) {
        gameLostScreen.gameObject.SetActive(true);
        pausePlayBtn.gameObject.SetActive(false);
        mapSceneName = sceneName;
    }


    public void displayCharacterPlacing() {
        //hides pausePlay and timeControl
        pausePlayBtn.gameObject.SetActive(false);
        timeControl.gameObject.SetActive(false);
        //activate the screen and pause
        characterPlacingScreen.gameObject.SetActive(true);

        pausePlay();

        characterPlacingScreen.displayCharacters();
    }
    //this is triggered by a button
    //loads map and reset cam position
    private void loadMap() {
        //resets position of camera
        cam.transform.position = new Vector3(0, 0, cam.transform.position.z);
        gameWonScreen.gameObject.SetActive(false);
        pausePlayBtn.gameObject.SetActive(true);
        //characters are set to inactive in Scene Select        
        SceneManager.LoadScene(mapSceneName);
        
    }
    //tis is triggered by a button;
    //reloads zone decrease zoneLives and if no more zone lives decrease total lives and reset zone Lives.(Notebook Page 24)
    //for now only deal with total lives.
    private void restartZone() {
        //resets position of camera
        cam.transform.position = new Vector3(0, 0, cam.transform.position.z);

        //sets all playerCharacters to inactive then heals to full hp and make alive
        foreach (Transform child in playerParty.transform) {
            if (child.tag == "Character") {
                child.gameObject.SetActive(false);
                Character currChar = child.GetComponent<Character>();
                currChar.HP = currChar.HPMax;
                currChar.alive = true;
            }
        }
        //hides the screen and shows pause again
        gameLostScreen.gameObject.SetActive(false);
        //shows the pauseplaybtn
        pausePlayBtn.gameObject.SetActive(true);
        DontDestroyOnLoad(playerParty);
        SceneManager.LoadScene(zone.zoneName);
    }
    private void closeUI() {
        //closes all UIScreens
        characterScreen.gameObject.SetActive(false);
        gameWonScreen.gameObject.SetActive(false);

        //unhides placing screen
        placingScreenHidden.hidden = false;

        //characterPlacingScreen.gameObject.SetActive(false);
        //Hides the close Button and shows the pause Button
        closeUIBtn.gameObject.SetActive(false);
        pausePlayBtn.gameObject.SetActive(true);
        timeControl.gameObject.SetActive(true);
        //go back to the game paused or unpaused determined by if it waspaused before UI was opened
        pause = wasPause;
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = timeControl.currTimeScale;

        characterScreen.close();
        //characterPlacingScreen.close();
    }
    public void pausePlay() {
        //Flips the pause switch then pauses or unpauses
        pause = !pause;
        if (pause) {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = timeControl.currTimeScale;
        //wasPause = pause
        wasPause = pause;
    }

    
    //to deal with hiding and unhiding UI
    private void hide() {
        //if characterScreen is up hide placing screen otherwise don't hide shit
        if (characterScreen.gameObject.activeSelf && characterPlacingScreen.gameObject.activeSelf ) {
            placingScreenHidden.hidden = true;
        }
        else {
            placingScreenHidden.hidden = false;
        }
    }
    private void Update() {
        if(zone == null) {
            zone = GameObject.FindGameObjectWithTag("Zone").GetComponent<Zone>();
        }
        //hide();
    }
}
