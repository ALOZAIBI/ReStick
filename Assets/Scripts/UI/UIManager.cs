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
    public CharacterScreen characterScreen;
    

    //The playerCharacters are children of this
    public GameObject playerParty;
    //screen that pops up oin lelve start
    public CharacterPlacingScreen characterPlacingScreen;
    
    

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
        characterScreen.viewCharacter(currChar);
    }

    //displays the game won screen and prompts the player to click to go back to the scene with name sceneName
    public void displayGameWon(string sceneName) {
        gameWonScreen.gameObject.SetActive(true);
        pausePlayBtn.gameObject.SetActive(false);
        mapSceneName = sceneName;
    }

   
    public void displayCharacterPlacing() {
        //activate the screen and pause
        characterPlacingScreen.gameObject.SetActive(true);
        pause = true;
        Time.timeScale = 0;

        characterPlacingScreen.displayCharacters();
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
        characterPlacingScreen.gameObject.SetActive(false);
        //Hides the close Button and shows the pause Button
        closeUIBtn.gameObject.SetActive(false);
        pausePlayBtn.gameObject.SetActive(true);
        //go back to the game paused or unpaused determined by if it waspaused before UI was opened
        pause = wasPause;
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

        characterScreen.close();
        characterPlacingScreen.close();
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
