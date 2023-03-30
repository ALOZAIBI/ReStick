using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//THIS IS A CHILD OF CAMERA SINCE HIDEUI NEEDS LOCAL POSITION RELATIVE TO PARENT WHICH IS CAMERA

//this is also a gameManager it manages lots of stuff wtf
public class UIManager : MonoBehaviour
{
    //just making it a singleton to make it simpler however this was implemented not early in development so in other places I might not be 
    //accessing uiManager with singleton
    public static UIManager singleton;
    //this is just called in the start function here to prevent it from being destroyed when loading scene
    public GameObject dontDestroys;
    //holds the name of the current saveSlot
    //this is static so that it can be accessible in SaveSystem
    public static string saveSlot;
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

    //Zone Won Screen Stuff
    public Image gameWonScreen;
    public RewardSelect rewardSelectScreen;

    //Map Won Screen Stuff
    public Image mapWonScreen;
    public Button backToWorldBtn;

    //Game Lost Screen Stuff
    public Image gameLostScreen;
    public Button lostToMapBtn;
    public Button lostToRestartBtn;

    //the scene to be loaded
    public string sceneToLoad;

    public TopStatDisplay topStatDisplay;

    public Button pausePlayBtn;

    public TimeControl timeControl;
    //true if paused
    public bool pause=false;
    //used to check if the game was paused before clicking on viewCharacter
    public bool wasPause = false;

    //used to restartZone
    public Zone zone;

    //used to fetch random abilities for rewards and shop
    public AbilityFactory abilityFactory;
    //used to fetch random abilities for rewards and shop
    public CharacterFactory characterFactory;

    //used to deal with Hiding and unHiding UI ELEMENTS
    public HideUI placingScreenHidden;
    public HideUI timeControlHidden;
    public HideUI charInfoScreenHidden;
    public HideUI inventoryScreenHidden;
    public HideUI gameWonScreenHidden;
    public HideUI mapWonScreenHidden;
    public HideUI gameLostScreenHidden;
    public HideUI topStatDisplayHidden;

    private void Awake() {
        singleton = this;
    }
    #region
    private void Start() {
        DontDestroyOnLoad(dontDestroys);

        SaveSystem.initialiseSaveSlots();

        placingScreenHidden = characterPlacingScreen.GetComponent<HideUI>();
        timeControlHidden = timeControlHidden.GetComponent<HideUI>();
        charInfoScreenHidden = characterInfoScreen.GetComponent<HideUI>();
        inventoryScreenHidden = inventoryScreen.GetComponent<HideUI>();
        gameWonScreenHidden = gameWonScreen.GetComponent<HideUI>();
        mapWonScreenHidden = mapWonScreen.GetComponent<HideUI>();
        gameLostScreenHidden = gameLostScreen.GetComponent<HideUI>();
        topStatDisplayHidden = topStatDisplay.GetComponent<HideUI>();

        //
        lostToMapBtn.onClick.AddListener(lostToMap);
        backToWorldBtn.onClick.AddListener(loadScene);

        lostToRestartBtn.onClick.AddListener(restartZone);

        pausePlayBtn.onClick.AddListener(pausePlay);
        closeUIBtn.onClick.AddListener(closeUI);
        openInventoryBtn.onClick.AddListener(openInventory);
    }
    //on first time clicking character Display its info in the topstatDisplay
    //then if character is clicked again or more info button was clicked open the charInfoScreen
    
    public void viewCharacter(Character charSel) {
        //if the character to be viewed is already selected
        if (charSel == topStatDisplay.character) {
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
        topStatDisplay.moreInfoBtn.gameObject.SetActive(false);
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
            sceneToLoad = sceneName;
            //the rewardSelectScreen contains the Button. The button waits for an ability to be selected. Once it is selected
            //the button can be clicked to add it to inventory and go back to mapSceneName
        //}
    }

    public void displayGameLost(string sceneName) {
        gameLostScreenHidden.hidden = false;
        pausePlayBtn.gameObject.SetActive(false);
        timeControlHidden.hidden = true;
        sceneToLoad = sceneName;
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
    public void loadScene() {

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
        SceneManager.LoadScene(sceneToLoad);
        closeUI();
        
    }
    //Once player loses a zone and chooses not to restart
    public void lostToMap() {
        //This is kinda inefficient since in the case that this function is called in zoneWonScreen then we would be loading what we just saved
        //so A way to optimize is to load only if it this function is called from zone lost to map
        loadMapSave();

        loadScene();
    }
    //once player completes all maps then goes back to world
    public void wonToWorld() {

        //save the world save then updates the gamestate to not be in a map
        saveWorldSave();
        SaveSystem.saveGameState("", false);
        loadScene();
        pausePlayBtn.gameObject.SetActive(false);
        timeControlHidden.hidden = true;
    }
    //tis is triggered by a button;
    //loads the previous mapSave then reloads the scene
    private void restartZone() {
        //resets position of camera
        cam.transform.position = new Vector3(0, 0, cam.transform.position.z);

        hideCharacter();
        clearBuffs();
        //hides the screen and shows pause again
        gameLostScreenHidden.hidden = true;
        pausePlayBtn.gameObject.SetActive(true);
        openInventoryBtn.gameObject.SetActive(true);
        loadMapSave();
        DontDestroyOnLoad(playerParty);
        SceneManager.LoadScene(zone.zoneName);
    }
    public void closeUI() {
        //closes all UIScreens
        charInfoScreenHidden.hidden = true;
        inventoryScreenHidden.hidden = true;
        inventoryScreen.closeHeader();
        inventoryScreen.closeBody();
        gameWonScreenHidden.hidden = true;
        mapWonScreenHidden.hidden=true;

        //unhides placing screen if zone not started
        try {
            if (!zone.started) {
                placingScreenHidden.hidden = false;
            }
        } catch { }

        topStatDisplay.moreInfoBtn.gameObject.SetActive(true);


        //characterPlacingScreen.gameObject.SetActive(false);
        //Hides the close Button and shows the pause Button
        closeUIBtn.gameObject.SetActive(false);
        //and shows open inventory btn again
        openInventoryBtn.gameObject.SetActive(true);
        
        try {
            Debug.Log("IUN ZONE"+zone.zoneName);
            if (zone.started == true) {
            //if zone has started show these
                pausePlayBtn.gameObject.SetActive(true);
                timeControlHidden.hidden = false;
            }
            //else if zone not accessible i.e in map
        }catch{
            Debug.Log("IUN MAP");
            pausePlayBtn.gameObject.SetActive(false);
            timeControlHidden.hidden = true;
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
    //returns if zone started or not
    public bool zoneStarted() {
        //this test is done when game isn;t in zone and hence zone.started cant be tested 
        if (zone == null) {
            Debug.Log("Zone not started");
            return false;
        }
        else {
            Debug.Log("Other not started");
            return zone.started;
        }
    }
    private void Update() {
        if(zone == null) {
            try {
                zone = GameObject.FindGameObjectWithTag("Zone").GetComponent<Zone>();
            }
            catch { }
        }
        ////display gold if in zone with goldgainedsofar in zone if possible
        //try {
        //    goldtext.text = "G:" + (playerParty.gold + zone.goldSoFar);
        //}
        //catch { goldtext.text = "G:" + playerParty.gold; }
        //hide();
        //Debug.Log(saveSlot);

    }
    #endregion

    //saves all characters in playerParty and saves inventory
    public void saveWorldSave() {
        SaveSystem.characterNumber = 0;
        //save character world
        foreach(Transform child in playerParty.transform) {
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                SaveSystem.saveCharacterInWorld(temp);
            }
        }
        //save inventory world
        SaveSystem.saveInventoryInWorld();
    }
    public void loadWorldSave() {
        //deletes all characters then reloads them back in
        deleteAllCharacters();
        SaveSystem.loadCharactersInWorld();
        //load inventory
        SaveSystem.loadInventoryInWorld();
    }
    public void saveMapSave() {
        SaveSystem.characterNumber = 0;
        //save character in map
        foreach (Transform child in playerParty.transform) {
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                SaveSystem.saveCharacterInMap(temp);
            }
        }
        //save inventory in map
        SaveSystem.saveInventoryInMap();
    }
    public void loadMapSave() {
        deleteAllCharacters();
        SaveSystem.loadCharactersInMap();
        SaveSystem.loadInventoryInMap();
    }
    //this is to be called before loading characters So that there are no duplicates
    public void deleteAllCharacters() {
        foreach (Transform child in playerParty.transform) {
            if (child.tag == "Character") {
                Debug.Log("TO BE DESTROYED" + child.name);
                Destroy(child.gameObject);
            }
        }
    }

    
}
