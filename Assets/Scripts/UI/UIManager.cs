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

    public Button openInventoryBtn;

    public InventoryScreen inventoryScreen;

    //Character Screen Stuff
    public CharacterInfoScreen characterInfoScreen;
    

    //The playerCharacters are children of this
    public PlayerManager playerParty;

    //currently selected character
    public Character character;

    //screen that pops up oin lelve start
    public CharacterPlacingScreen characterPlacingScreen;

    //displays how much gold player has
    public TextMeshProUGUI goldtext;

    //Game Won Screen Stuff
    public Image gameWonScreen;
    public RewardSelect rewardSelectScreen;

    //Game Lost Screen Stuff
    public Image gameLostScreen;
    public Button lostToMapBtn;
    public Button lostToRestartBtn;

    //the scene to be loaded
    public string mapSceneName;

    public TopStatDisplay topStatDisplay;

    public Button pausePlayBtn;

    public TimeControl timeControl;
    //true if paused
    public bool pause=false;
    //used to check if the game was paused before clicking on viewCharacter
    public bool wasPause = false;

    //used to restartZone
    public Zone zone;

    //used to deal with Hiding and unHiding UI ELEMENTS
    public HideUI placingScreenHidden;
    public HideUI timeControlHidden;
    public HideUI charInfoScreenHidden;
    public HideUI inventoryScreenHidden;
    public HideUI gameWonScreenHidden;
    public HideUI gameLostScreenHidden;
    public HideUI topStatDisplayHidden;
    private void Start() {
        placingScreenHidden = characterPlacingScreen.GetComponent<HideUI>();
        timeControlHidden = timeControlHidden.GetComponent<HideUI>();
        charInfoScreenHidden = characterInfoScreen.GetComponent<HideUI>();
        inventoryScreenHidden = inventoryScreen.GetComponent<HideUI>();
        gameWonScreenHidden = gameWonScreen.GetComponent<HideUI>();
        gameLostScreenHidden = gameLostScreen.GetComponent<HideUI>();
        topStatDisplayHidden = topStatDisplay.GetComponent<HideUI>();

        //
        lostToMapBtn.onClick.AddListener(loadMap);

        lostToRestartBtn.onClick.AddListener(restartZone);

        pausePlayBtn.onClick.AddListener(pausePlay);
        closeUIBtn.onClick.AddListener(closeUI);
        openInventoryBtn.onClick.AddListener(openInventory);
    }
    //on first time clicking character Display its info in the topstatDisplay
    //then if character is clicked again or more info button was clicked open the charInfoScreen
    
    public void viewCharacter(Character charSel) {
        //if the character to be viewed is already selected or zone hasn't started open character info
        if (charSel == topStatDisplay.character || !zone.started) {
            viewCharacterInfo(charSel);
        }
        //if character wasn't already selected
        else {
            character = charSel;
            topStatDisplay.character = character;
            topStatDisplayHidden.hidden = false;
        }
    }
    public void viewCharacterInfo(Character currChar) {
        //opens the screen and pauses the game
        charInfoScreenHidden.hidden = false;
        pause = true;
        Time.timeScale = 0;
        //hides placing screen
        placingScreenHidden.hidden = true;
        //the close button pops up and the pause button+time control is hidden
        closeUIBtn.gameObject.SetActive(true);
        timeControlHidden.hidden = true;
        pausePlayBtn.gameObject.SetActive(false);
        characterInfoScreen.viewCharacter(currChar);
    }

    public void hideCharacter() {
        topStatDisplayHidden.hidden = true;
        topStatDisplay.character = null;
    }
    //displays the game won screen and prompts the player to click to go back to the scene with name sceneName
    public void displayGameWon(string sceneName) {
        //if (!displayed) {
            //displayGameWon and display the rewards
            gameWonScreenHidden.hidden = false;
            rewardSelectScreen.displayAbilities();
            pausePlayBtn.gameObject.SetActive(false);
            mapSceneName = sceneName;
            //the rewardSelectScreen contains the Button. The button waits for an ability to be selected. Once it is selected
            //the button can be clicked to add it to inventory and go back to mapSceneName
        //}
    }

    public void displayGameLost(string sceneName) {
        gameLostScreenHidden.hidden = false;
        pausePlayBtn.gameObject.SetActive(false);
        timeControlHidden.hidden = true;
        mapSceneName = sceneName;
    }


    public void displayCharacterPlacing() {
        //hides pausePlay and timeControl
        pausePlayBtn.gameObject.SetActive(false);
        timeControlHidden.hidden = true;
        //activate the screen and pause
        placingScreenHidden.hidden = false;

        pausePlay(true);
        wasPause = true;

        characterPlacingScreen.displayCharacters();
    }
    //this is triggered by a button
    //loads map and reset cam position
    public void loadMap() {
        clearBuffs();
        //resets position of camera
        cam.transform.position = new Vector3(0, 0, cam.transform.position.z);
        gameWonScreenHidden.hidden = true;
        gameLostScreenHidden.hidden = true;
        hideCharacter();
        //pausePlayBtn.gameObject.SetActive(true);
        //hides timecontrol
        timeControlHidden.hidden = true;
        //unhides inventory but it will be hidden again when start button is clicked in characte rplacing
        openInventoryBtn.gameObject.SetActive(true);
        //characters are set to inactive in Scene Select        
        SceneManager.LoadScene(mapSceneName);
        
    }
    //tis is triggered by a button;
    //reloads zone decrease zoneLives and if no more zone lives decrease total lives and reset zone Lives.(in the Zone script)(Notebook Page 24)
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
        hideCharacter();
        clearBuffs();
        //hides the screen and shows pause again
        gameLostScreenHidden.hidden = true;
        pausePlayBtn.gameObject.SetActive(true);
        openInventoryBtn.gameObject.SetActive(true);
        DontDestroyOnLoad(playerParty);
        SceneManager.LoadScene(zone.zoneName);
    }
    private void closeUI() {
        //closes all UIScreens
        charInfoScreenHidden.hidden = true;
        inventoryScreenHidden.hidden = true;
        inventoryScreen.closeHeader();
        inventoryScreen.closeBody();
        gameWonScreenHidden.hidden = true;

        //unhides placing screen if zone not started
        try {
            if (!zone.started) {
                placingScreenHidden.hidden = false;
            }
        } catch { }
        

        //characterPlacingScreen.gameObject.SetActive(false);
        //Hides the close Button and shows the pause Button
        closeUIBtn.gameObject.SetActive(false);
        //and shows open inventory btn again
        openInventoryBtn.gameObject.SetActive(true);
        //if game has started show these
        if (zone.started == true) {
            pausePlayBtn.gameObject.SetActive(true);
            timeControlHidden.hidden = false;
        }
        //go back to the game paused or unpaused determined by if it waspaused before UI was opened
        pausePlay(wasPause);


        characterInfoScreen.close();
        //characterPlacingScreen.close();
    }
    //i should improve the pausePlay function to take a bool pausePlay(true) makes the game paused pausePlay(false) makes the game continue 
    //if no parameter is given just flip the switch so pausePlay() would go from paused to unpaused and vice versa

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
    /// <summary>
    /// If true pause the game
    /// </summary>
    /// <param name="yesPause"></param>
    public void pausePlay(bool yesPause) {
        if (yesPause) {
            wasPause = pause;
            pause = yesPause;
            Time.timeScale = 0;
            Debug.Log("game should pause");
        }
        else {
            wasPause = pause;
            pause = yesPause;
            Time.timeScale = timeControl.currTimeScale;
        }
    }

    public void openInventory() {
        closeUIBtn.gameObject.SetActive(true);
        openInventoryBtn.gameObject.SetActive(false);
        inventoryScreenHidden.hidden = false;
        inventoryScreen.setupInventoryScreen();
    }
    
    //removes buffs from player characters. To be called in loadZone and on Restart
    public void clearBuffs() {
        foreach (Transform child in playerParty.transform) {
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                if (temp.buffs.Count > 0) {
                    //the .ToArray is needed to prevent the error of collection is modified while accessing it
                    foreach (Buff buff in temp.buffs.ToArray()) {
                        buff.removeBuff();
                    }
                }
            }
        }
    }
    private void Update() {
        if(zone == null) {
            try {
                zone = GameObject.FindGameObjectWithTag("Zone").GetComponent<Zone>();
            }
            catch { }
        }
        //display gold if in zone with goldgainedsofar in zone if possible
        try {
            goldtext.text = "G:" + (playerParty.gold + zone.goldSoFar);
        }
        catch { goldtext.text = "G:" + playerParty.gold; }
        //hide();

    }
}
