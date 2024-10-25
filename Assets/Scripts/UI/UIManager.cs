using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Tilemaps;
//THIS IS A CHILD OF CAMERA SINCE HIDEUI NEEDS LOCAL POSITION RELATIVE TO PARENT WHICH IS CAMERA

//this is also a gameManager it manages lots of stuff wtf
//[DefaultExecutionOrder(-100)]
public class UIManager : MonoBehaviour
{
    //just making it a singleton to make it simpler however this was implemented not early in development so in other places I might not be 
    //accessing uiManager with singleton
    public static UIManager singleton;
    //this is just called in the start function here to prevent it from being destroyed when loading scene
    public GameObject dontDestroys;
    //holds the name of the current saveSlot
    //this is static so that it can be accessible in SaveSystem
    public  string saveSlot;
    //used to reset cam position when changing scenes 
    public Camera cam;
    public CameraMovement camMov;

    //Btn used to close all UI
    public Button closeUIBtn;

    public Tooltip tooltip;

    public GameObject menuUI;
    public Button openInventoryBtn;
    //notifies if there are abilities in inventory
    public Image openInventoryNotification;

    public InventoryScreen inventoryScreen;

    public ShopScreen shopScreen;
    public Button openShopButton;

    //Character Screen Stuff
    public CharacterInfoScreen characterInfoScreen;
    

    //The playerCharacters are children of this
    public PlayerManager playerParty;

    //currently selected character
    public Character character;

    public ManualTargetting manualTargetting;

    //screen that pops up oin lelve start
    public CharacterPlacingScreen characterPlacingScreen;

    
    //Zone Won Screen Stuff
    public GameWonScreen gameWonScreen;

    //Map Won Screen Stuff
    public Image mapWonScreen;
    public Button wonToWorldBtn;

    //Game Lost Screen Stuff
    public Image gameLostScreen;
    public Button lostToMapBtn;
    public Button lostToRestartBtn;

    //the scene to be loaded
    public string sceneToLoad;

    public GameOverScreen gameOverScreen;

    public GameObject topBarUI;

    //displays how much gold player has
    public GoldText goldtext;
    
    //displays how many life shards the player has remaining
    public TextMeshProUGUI lifeShardText;

    public TopStatDisplay topStatDisplay;

    public Button pausePlayBtn;
    //uk the arrow icon and the || pause icon thing
    public Image pausePauseImage;
    public Image pausePlayImage;

    public Button exitBtn;
    public Button retryBtn;

    public TimeControl timeControl;
    //true if paused
    public bool pause=false;
    //used to check if the game was paused before clicking on viewCharacter
    public bool wasPause = false;

    public Button showAbilityIndicatorBtn;
    public Image eyeOffImage;
    public Image eyeOnImage;
    public bool showAbilityIndicator = true;

    public Tutorial tutorial;

    //This is an image that is on the top most ui layer. So if you wanna emphasize or focus on a UI element put it as a child of this.
    public Image focus;
    public RectTransform focusRect;
    public float focusOpacity;

    public ScreenBlink screenBlink;

    //used to restartZone
    public Zone zone;
    //This will be used for procGen ( To determine difficulty + to set the seed)
    public int zoneNumber;

    //needed to determine what closeUI does
    public bool inZone;

    //used to fetch random abilities for rewards and shop
    public AbilityFactory abilityFactory;
    //used to fetch random abilities for rewards and shop
    public CharacterFactory characterFactory;
    //Used to fetch random items for rewards and shop
    public ItemFactory itemFactory;

    public MiscBonusFactory miscBonusFactory;

    //Used to fetch archetypes
    public Transform archetypeList;
    //used to deal with Hiding and unHiding UI ELEMENTS
    [HideInInspector]public HideUI placingScreenHidden;
    [HideInInspector]public HideUI timeControlHidden;
    [HideInInspector]public HideUI charInfoScreenHidden;
    [HideInInspector]public HideUI inventoryScreenHidden;
    [HideInInspector]public HideUI zoneWonScreenHidden;
    [HideInInspector]public HideUI mapWonScreenHidden;
    [HideInInspector]public HideUI zoneLostScreenHidden;
    [HideInInspector]public HideUI gameOverScreenHidden;
    [HideInInspector]public HideUI topStatDisplayHidden;
    [HideInInspector]public HideUI shopScreenHidden;
    [HideInInspector]public HideUI menuUIHidden;

    public CoinManager coinManager;

    public Button debuggingBtn;

    private void Awake() {
        singleton = this;
    }
    #region
    private void Start() {
        DontDestroyOnLoad(dontDestroys);

        SaveSystem.initialiseSaveSlots();
        //On start load to Main Menu. We don't manually start at Main Menu because when we want to restart the game there would be 2 dontdestroys
        SceneManager.LoadScene("MainMenu");

        placingScreenHidden = characterPlacingScreen.GetComponent<HideUI>();
        timeControlHidden = timeControlHidden.GetComponent<HideUI>();
        charInfoScreenHidden = characterInfoScreen.GetComponent<HideUI>();
        inventoryScreenHidden = inventoryScreen.GetComponent<HideUI>();
        zoneWonScreenHidden = gameWonScreen.GetComponent<HideUI>();
        mapWonScreenHidden = mapWonScreen.GetComponent<HideUI>();
        gameOverScreenHidden = gameOverScreen.GetComponent<HideUI>();
        zoneLostScreenHidden = gameLostScreen.GetComponent<HideUI>();
        topStatDisplayHidden = topStatDisplay.GetComponent<HideUI>();
        shopScreenHidden = shopScreen.GetComponent<HideUI>();
        menuUIHidden = menuUI.GetComponent<HideUI>();

        //
        lostToMapBtn.onClick.AddListener(backToMap);
        exitBtn.onClick.AddListener(backToMap);
        wonToWorldBtn.onClick.AddListener(wonToWorld);

        lostToRestartBtn.onClick.AddListener(restartZone);
        retryBtn.onClick.AddListener(restartZone);

        showAbilityIndicatorBtn.onClick.AddListener(showAbilityIndicatorFunc);
        pausePlayBtn.onClick.AddListener(pausePlay);
        closeUIBtn.onClick.AddListener(closeUIButton);
        openInventoryBtn.onClick.AddListener(openInventory);
        openShopButton.onClick.AddListener(openShop);

        camMov = Camera.main.GetComponent<CameraMovement>();

        debuggingBtn.onClick.AddListener(debug);
    }
    //Debugtype 0 only does to allies, 1 only does to enemies
    int debugType = 0;
    public void debug() {
        foreach(Character c in zone.charactersInside) {
            if (debugType == 0) {
                if (c.team == (int)Character.teamList.Player)
                    c.displayDebugText();
            }
            else {
                if (c.team != (int)Character.teamList.Player)
                    c.displayDebugText();
            }
        }
        debugType = (debugType + 1) % 2;

        zone.tileMapToShowFully.GetComponent<TilemapRenderer>().enabled = debugType == 0;
    }
    //on first time clicking character Display its info in the topstatDisplay
    //then if character is clicked again or more info button was clicked open the charInfoScreen
    public void viewCharacter(Character charSel) {
        //if the character to be viewed is already selected
        if (charSel.getSelected()) {
            viewCharacterInfo(charSel);
            Debug.Log("FullSCreening");
        }
        //if character wasn't already selected
        else {
            character = charSel;
            viewTopstatDisplay(charSel);
            Debug.Log("TopStatting");
        }
    }
    //in some cases we want it to only viewTopStat
    public void viewTopstatDisplay(Character charSel) {
        character = charSel;
        characterInfoScreen.character = character;
        charInfoScreenHidden.hidden = false;
        //characterInfoScreen.displayTopStatAbilities();
    }
    public void viewCharacterInfo(Character currChar) {
        //opens the screen and pauses the game
        charInfoScreenHidden.hidden = false;
        //pausePlay(true);
        ////hides placing screen
        //placingScreenHidden.hidden = true;
        ////the close button pops up and the pause button+time control is hidden
        //closeUIBtn.gameObject.SetActive(false);
        //timeControlHidden.hidden = true;
        //pausePlayBtn.gameObject.SetActive(false);
        characterInfoScreen.viewCharacterFullScreen(currChar);
    }

    public void hideCharacter() {
        charInfoScreenHidden.hidden = true;
        characterInfoScreen.character = null;
    }
    //displays the game won screen and prompts the player to click to go back to the scene with name sceneName
    public void displayGameWon(string sceneName) {
        //if (!displayed) {
            //displayGameWon and display the rewards
            zoneWonScreenHidden.hidden = false;
            timeControlHidden.hidden = true;
            gameWonScreen.zoneWon();
            pausePlayBtn.gameObject.SetActive(false);
            exitBtn.gameObject.SetActive(false);
            retryBtn.gameObject.SetActive(false);
            sceneToLoad = sceneName;
        Debug.Log("GAQMEWON"+exitBtn.isActiveAndEnabled);
            //the rewardSelectScreen contains the Button. The button waits for an ability to be selected. Once it is selected
            //the button can be clicked to add it to inventory and go back to mapSceneName
        //}
    }

    public void displayZoneLost(string sceneName) {
        zoneLostScreenHidden.hidden = false;
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
    //loads map and removes buffs
    public void loadScene() {


        //clearBuffs();
        //resets position of camera
        //Camera.main.transform.position = new Vector3(0, 0, -10);
        zoneWonScreenHidden.hidden = true;
        zoneLostScreenHidden.hidden = true;
        hideCharacter();
        //pausePlayBtn.gameObject.SetActive(true);
        //hides timecontrol
        timeControlHidden.hidden = true;

        

        //characters are set to inactive in Scene Select        
        UIManager.singleton.loadSceneBlink(sceneToLoad);
        closeUI();
        
    }
    //Once player loses a zone and chooses not to restart
    public void backToMap() {
        //If the zone already started then exiting will cost a life shard.(To prevent exploiting) (Player exits as soon as about to lose)
        if (zoneStarted()) {
            playerParty.lifeShards -= 1;
            SaveSystem.updateLifeShardsInMap();
            checkGameOver();
        }
        //This is kinda inefficient since in the case that this function is called in zoneWonScreen then we would be loading what we just saved
        //so A way to optimize is to load only if it this function is called from zone lost to map
        //Camera.main.transform.position = new Vector3(0, 0, -10);

        //Removes manual targeting from all allied characters
        foreach (Transform child in playerParty.transform) {
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                temp.endManualTarget();
            }
        }

        placingScreenHidden.hidden = true;
        loadMapSave();
        sceneToLoad = zone.belongsToMap;
        inZone = false;
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
    //From GameOverScreen we want to restart without losing a life shard after checking zone started so we pass false
    private void restartZone() {
        restartZone(true);
    }
    public void restartZone(bool checkZoneStarted = true) {
        //If the zone already started then restarting will cost a life shard.(To prevent exploiting) (Player restars as soon as about to lose)
        if(zoneStarted() && checkZoneStarted) {
            playerParty.lifeShards -= 1;
            SaveSystem.updateLifeShardsInMap();
            checkGameOver();
        }
        //resets position of camera
        //cam.transform.position = new Vector3(0, 0, cam.transform.position.z);

        //Removes manual targeting from all allied characters
        foreach (Transform child in playerParty.transform) {
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                temp.endManualTarget();
            }
        }

        hideCharacter();
        clearBuffs();
        //hides the screen and shows pause again
        zoneLostScreenHidden.hidden = true;
        pausePlayBtn.gameObject.SetActive(true);
        
        loadMapSave();
        DontDestroyOnLoad(playerParty);
        UIManager.singleton.loadSceneBlink(zone.zoneName);
    }

    public bool checkGameOver() {

        if (playerParty.lifeShards < 0) {
            gameOverScreen.setup();
            gameOverScreenHidden.hidden = false;
            return true;
        }
        return false;
    }
    //closes all uiScreen except some depending on if currently in zone or not so make sure to set the inZone boolean before calling loadSCene which calls closeUI
    public void closeUI() {
        //closes all UIScreens
        charInfoScreenHidden.hidden = true;
        shopScreenHidden.hidden = true;
        shopScreen.closeBuyScreen();    
        inventoryScreenHidden.hidden = true;
        zoneWonScreenHidden.hidden = true;
        mapWonScreenHidden.hidden = true;

        //the try catch was initially used since I didn't have the inZone boolean so I wasnt sure if zone was accessible so I'm pretty sure it's safe to remove
        if (inZone) {
            exitBtn.gameObject.SetActive(true);
            retryBtn.gameObject.SetActive(true);
            try {
        //unhides placing screen if zone not started
                if (!zone.started) {
                    placingScreenHidden.hidden = false;
                    
                }
                else {
                    //if zone has started show these
                    pausePlayBtn.gameObject.SetActive(true);
                    timeControlHidden.hidden = false;
                }
            }
            catch { }
        }
        //if in map
        else {
            pausePlayBtn.gameObject.SetActive(false);
            timeControlHidden.hidden = true;
            exitBtn.gameObject.SetActive(false);
            retryBtn.gameObject.SetActive(false);
        }

        topStatDisplay.moreInfoBtn.gameObject.SetActive(true);


        //Hides the close Button and shows the pause Button
        closeUIBtn.gameObject.SetActive(false);
        //and shows open inventory btn again
        
        
        
        characterInfoScreen.closeAbilityDisplays();
        //characterPlacingScreen.close();
    }
    //we made this a seperate function so that it can also applyChanges without messing up the loadscene function. If the applyChanges was in the regular closeUI it would've made loadScene function also save which causes a duplicate character bug
    public void closeUIButton() {
        closeUI();
        //applies the statPoints 
        if (!zoneStarted()) {
            characterInfoScreen.statUpgrading.applyChanges();
            //inventoryScreen.inventoryCharacterScreen.statUpgrading.applyChanges();
        }
    }
    //i should improve the pausePlay function to take a bool pausePlay(true) makes the game paused pausePlay(false) makes the game continue 
    //if no parameter is given just flip the switch so pausePlay() would go from paused to unpaused and vice versa

    public void pausePlay() {
        //Flips the pause switch then pauses or unpauses
        wasPause = pause;
        pause = !pause;
        if (pause) {
            pausePlayImage.gameObject.SetActive(true);
            pausePauseImage.gameObject.SetActive(false);
            Time.timeScale = 0;
            retryBtn.gameObject.SetActive(true);
            exitBtn.gameObject.SetActive(true);
        }
        else {
            Time.timeScale = timeControl.currTimeScale;
            pausePlayImage.gameObject.SetActive(false);
            pausePauseImage.gameObject.SetActive(true);
            retryBtn.gameObject.SetActive(false);
            exitBtn.gameObject.SetActive(false);
        }
        //wasPause = pause
    }
    /// <summary>
    /// If true pause the game
    /// </summary>
    /// <param name="yesPause"></param>
    public void pausePlay(bool yesPause) {
        if (yesPause) {
            wasPause = pause;
            pause = yesPause;
            pausePlayImage.gameObject.SetActive(true);
            pausePauseImage.gameObject.SetActive(false);
            Time.timeScale = 0;
            retryBtn.gameObject.SetActive(true);
            exitBtn.gameObject.SetActive(true);
        }
        else {
            wasPause = pause;
            pause = yesPause;
            pausePlayImage.gameObject.SetActive(false);
            pausePauseImage.gameObject.SetActive(true);
            Time.timeScale = timeControl.currTimeScale;
            retryBtn.gameObject.SetActive(false);
            exitBtn.gameObject.SetActive(false);
        }
    }

    public void openInventory() {
        //closeUIBtn.gameObject.SetActive(true);
        //openInventoryBtn.gameObject.SetActive(false);
        inventoryScreenHidden.hidden = false;
        inventoryScreen.setupInventoryScreen(); 
    }
    
    public void openShop() {
        //closeUIBtn.gameObject.SetActive(true);
        shopScreen.shop.setup();
        shopScreenHidden.hidden = false;
        shopScreen.setupShopScreen();
    }


    //removes buffs from player characters. To be called in loadZone and on Restart
    public void clearBuffs() {
        foreach (Transform child in playerParty.transform) {
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                if (temp.buffs.Count > 0) {
                    //the .ToArray is needed to prevent the error of collection is modified while accessing it
                    foreach (Buff buff in temp.buffs.ToArray()) {
                        Debug.Log("Buff:"+buff.code + "Removed from" + temp.name);
                        buff.removeBuff();
                    }
                }
            }
        }
    }
    public void updateGoldtextToCurrent() {
        //Update the goldToDisplay to the actual currentGold
        goldtext.goldToDisplay = playerParty.gold;
        goldtext.updateView();
    }
    //returns if zone started or not
    public bool zoneStarted() {
        //this test is done when game isn;t in zone and hence zone.started cant be tested 
        if (zone == null) {
            //Debug.Log("Zone not started");
            return false;
        }
        else {
            //Debug.Log("Other not started");
            return zone.started;
        }
    }

    private void inventoryNotification() {
        //if (playerParty.abilityInventory.transform.childCount > 0) {
        //    openInventoryNotification.gameObject.SetActive(true);
        //}
        //else
        //    openInventoryNotification.gameObject.SetActive(false);
    }
    private void Update() {
        inventoryNotification();
        if(zone == null) {
            try {
                zone = FindObjectOfType<Zone>();
            }
            catch { }
        }

        //goldtext.text = playerParty.gold.ToString();
        
        lifeShardText.text = playerParty.lifeShards.ToString();
        ////display gold if in zone with goldgainedsofar in zone if possible
        //try {
        //    goldtext.text = "G:" + (playerParty.gold + zone.goldSoFar);
        //}
        //catch { goldtext.text = "G:" + playerParty.gold; }
        //hide();
        //Debug.Log(saveSlot);

    }

    private void showAbilityIndicatorFunc() {
        showAbilityIndicator = !showAbilityIndicator;
        eyeOffImage.gameObject.SetActive(showAbilityIndicator);
        eyeOnImage.gameObject.SetActive(!showAbilityIndicator);
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
        deleteAllInventory();
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
        deleteAllInventory();
        deleteAllPlayerAbilities();
        deleteAllPlayerItems();
        SaveSystem.loadCharactersInMap();
        SaveSystem.loadInventoryInMap();
    }

    //this is used before loading in characters otherwise there's a glitch where there will be to many abilities whenever you restart. Since loading character doesn't delete it's abilities
    public void deleteAllPlayerAbilities() {
        foreach(Transform child in playerParty.activeAbilities.transform) { 
        Destroy(child.gameObject);
        }
    }
    public void deleteAllPlayerItems() {
        foreach (Transform child in playerParty.activeItems.transform) {
            Destroy(child.gameObject);
        }
    }
    //this is to be called before loading characters So that there are no duplicates
    public void deleteAllCharacters() {
        foreach (Transform child in playerParty.transform) {
            if (child.tag == "Character") {
                //Since destroy destroys at the end of this update loop, but characterPlacingScreen will display all the playerparty children, with the character tag. So we will change the tag
                child.tag = "Untagged";
                Destroy(child.gameObject);
            }
        }
    }

    public void deleteAllInventory() {
        foreach (Transform child in playerParty.abilityInventory.transform) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in playerParty.itemInventory.transform) {
            Destroy(child.gameObject);
        }
    }

    //LoadingScene extension to allow for ui blink during scene transition
    public void loadSceneBlink(string sceneName) {
        screenBlink.startBlinkClose();
        StartCoroutine(loadingScene(sceneName));
    }

    private IEnumerator loadingScene(string sceneName) {
        //Waits for blink to close before starting scene loading
        while (screenBlink.blinkClose) { yield return null; } ;
        
        //Start scene loading
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        //Pauses scene activation so that all the start and awake functions are called normally in the new scene
        async.allowSceneActivation = false;


        while(!async.isDone) {
            // Check if the load has finished
            if (async.progress >= 0.9f) {
                //Activate the Scene
                async.allowSceneActivation = true;
                //Debug.Log("SCene activation now allowed");
                screenBlink.startBlinkOpen();
            }
            yield return null;
        }
       
    }


}
