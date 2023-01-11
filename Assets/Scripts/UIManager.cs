using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    ////Character that is to be displayed in characterScreen;
    //public Character currChar;

    //Btn used to close all UI
    public Button closeUIBtn;

    //Character Screen Stuff
    public Image characterScreen;
    public TextMeshProUGUI characterName;
    //stats texts
    public TextMeshProUGUI DMG, AS, MS, RNG, LS;
    //cool stats texts
    public TextMeshProUGUI totalKills;

    public Button pausePlayBtn;
    //true if paused
    public bool pause=false;

    private void Start() {
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
        displayStats(currChar);
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
    }
    private void closeUI() {
        //closes all UIScreens
        characterScreen.gameObject.SetActive(false);
        //Hides the close Button and shows the pause Button
        closeUIBtn.gameObject.SetActive(false);
        pausePlayBtn.gameObject.SetActive(true);
    }
    private void pausePlay() {
        //Flips the pause switch then pauses or unpauses
        pause = !pause;
        if (pause) {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;
    }
}
