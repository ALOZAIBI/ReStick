using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Zone : MonoBehaviour
{
    //this is needed for restarting the scene in UIMANAGER
    public string zoneName;
    //List of characters inside the Zone
    public List<Character> charactersInside = new List<Character>();

    //the map that this zone belongs to 
    public string belongsToMap;

    //zone started or not. This is used to be able to move playerCharacter's before zone has started
    public bool started = false;

    //zone completed or not
    public bool completed = false;

    //If true this zone will for sure give reward
    public bool forceReward = false;

    [SerializeField] private UIManager uIManager;

    [SerializeField] private PlayerManager playerParty;

    private int enemiesAlive = 0;

    private int alliesAlive = 0;

    //gold to be added to playerparty on level completion
    public int goldSoFar=0;

    //list of gameObject that contain ability that can be rewarded here
    public List<GameObject> abilityRewardPool = new List<GameObject>();

    //list of gameObject that contain items that can be rewarded here
    public List<GameObject> itemRewardPool = new List<GameObject>();

    //contains the abilities to be rewarded as children
    public GameObject abilityContainer;


    //string containing ability names. This list is filled when zone save file is loaded.
    //These string names will be used to fetch abilities from ability factory
    public List<string> abilityNames = new List<string>();

    //this is used to prevent a bug where the update loop goes through zone won twice which results in duplicate ability reward displays
    private bool zoneDone;

    [SerializeField] public Tilemap placeableOverlay;
    [SerializeField] private TileBase overlayTile;

    public Tilemap tileMapToShowFully;
    public Tilemap tileMapToFocus;
    public int zoomFocusAmount;

    //connects to UImanager
    private void Start() {
        abilityContainer = GameObject.FindGameObjectWithTag("ZoneRewards");

        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        uIManager.displayCharacterPlacing();
        uIManager.hideCharacter();

        uIManager.showAbilityIndicator = true;

        uIManager.retryBtn.gameObject.SetActive(true);
        uIManager.exitBtn.gameObject.SetActive(true);
        uIManager.menuUIHidden.hidden = true;
        playerParty = GameObject.FindGameObjectWithTag("PlayerParty").GetComponent<PlayerManager>();
        zoneName = gameObject.scene.name;

        //loads Zone if possible
        if (SaveSystem.loadZone(this)) {
            //then extracts the ability names from the save file then fetches them from ability factory to add to reward pool
            uIManager.abilityFactory.addRequestedAbilityToZone(this, abilityNames);
        }
        //if there is no saveFile for this zone.
        else {
            //if there are no rewards initialized in the editor or if it's the tutorial level and tutorial already completed 
            if(abilityContainer.transform.childCount == 0 || uIManager.tutorial.upgradingStatsTutorialDone) {
                //adds 3 random abilities to the zone
                uIManager.abilityFactory.addRandomAbilityToZone(this, 3);
            }
            //adds the abilities in the reward pool to the zone (Some levels like the tutorial levels I want specific abilities as reward that are initialized in the editor)
            else { 
                uIManager.abilityFactory.addAlreadyInitializedRewardAbilityToZone(this);
            }

        }

        playerParty.unDrop();

        drawPlaceableOverlay();
        CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
        cameraMovement.tilemapToDisplayFully = tileMapToShowFully;
        cameraMovement.tilemapToFocus = tileMapToFocus;
        if(zoomFocusAmount!=0)
            cameraMovement.zoomTarget = zoomFocusAmount;
        cameraMovement.showMapIntoZoom();
        //Makes sure camera not focusing anything
        cameraMovement.characterToFocusOn = null;

        //Tutorial Stuff
        uIManager.tutorial.beginDraggingCharactersTutorial();
        uIManager.tutorial.beginAddingAbilityTutorial();
        uIManager.tutorial.beginUpgradingStatsTutorial();

        uIManager.coinManager.setUp();

        if(zoneName == "ProcGen" && uIManager.zoneNumber == 0)
            forceReward = true;
    }
    //draws an overlay using placeable Tile over the placeable tilemap
    private void drawPlaceableOverlay() {
        //fetch Tilemap with tag PlaceableArea
        Tilemap placeable = GameObject.FindGameObjectWithTag("PlaceableArea").GetComponent<Tilemap>();

        Color color = placeable.color;
        color.a = 0;
        placeable.color = color;
        //hides the placeable tilemap by making it's order in layer -10
        placeable.GetComponent<Renderer>().sortingOrder = -10;
        //duplicates it and places it as a child of the grid which is the parent of the placeable tilemap
        placeableOverlay = Instantiate(placeable,transform.parent);
        //shows the overlay by making it's order in layer 1
        placeableOverlay.GetComponent<Renderer>().sortingOrder = 1;
        //clears the overlay
        placeableOverlay.ClearAllTiles();
        //get bounds of placeable
        BoundsInt bounds = placeable.cellBounds;

        //Saves the placeableOverlay so that it can be shown when dragging another character in after zone has started.

        //loop through all the tiles in the placeableOverlay and set them to overlayTile
        foreach (Vector3Int pos in bounds.allPositionsWithin) {
            //checks if tile is not null in placeable
            if (placeable.HasTile(pos)) {
                placeableOverlay.SetTile(pos, overlayTile);
            }
        }
        color.a = 1;
        placeableOverlay.color = color;
    }
    ////to detect which players in the zone
    //private void OnTriggerEnter2D(Collider2D collision) {
    //    if (collision.CompareTag("Character")) {
    //        charactersInside.Add(collision.GetComponent<Character>());
    //    }
    //}


    //if there are no enemies alive and there is atleast 1 playerCharacter alive show win screen
    private void zoneWon() {
        if (enemiesAlive == 0 && alliesAlive > 0) {
            endZone();
            uIManager.coinManager.zoneEnded = true;
            //pauses the game and displays game won
            uIManager.pausePlay(true);
            uIManager.displayGameWon(belongsToMap);
            //marks zone as completed then saves
            completed = true;
            zoneDone = true;

        }
    }
    //if all player Character's in play died decrease totallives and displaygamelost
    private void zoneLost() {

        if(started) {
            //if there's a single player character in play and alive leave this function
            foreach(Character child in charactersInside) {
                if (child.GetComponent<Character>().team == (int)Character.teamList.Player && child.GetComponent<Character>().alive)
                    return;
            }


            endZone();

            //otherwise zone is lost
            playerParty.lifeShards--;
            SaveSystem.updateLifeShardsInMap();
            uIManager.placingScreenHidden.hidden = true;

            uIManager.pausePlay(true);
            //started is re set to false to prevent totallives to decrement infintely
            started = false;

            //if !completed keep it that way otherwise if it is completed also keep it that way so in other words
            //what is written below is useless but keep it for now I guess just to know where I should be saving data
            if (!completed) {
                //marks zone as completed then saves
                completed = false;
                SaveSystem.saveZone(this);
            }
            //If game over display Game over screen otherwise blink back to level select
            if (!uIManager.checkGameOver()) {
                uIManager.backToMap();
            }
        }
    }

    public void endZone() {
        //revive dead characters and set them to 20 % health
        //Removes manual targeting from all allied characters and reset manual targeting cd
        foreach (Transform child in UIManager.singleton.playerParty.transform) {
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                temp.endManualTarget();
                temp.manualTargettingCDRemaining = 0;
                if(!temp.alive) {
                    temp.alive = true;
                    temp.HP= 0.2f*temp.HPMax ;
                }
            }
        }

        uIManager.timeControl.resetTime();
        
        uIManager.clearBuffs();
        //Removes all the auras
        Aura[] auras = FindObjectsOfType<Aura>();
        foreach (Aura aura in auras) {
            Destroy(aura.gameObject);
        }



    }

    void FixedUpdate()
    {
        //counts characters alive
        //using temp since otherwise I'd have to reinitalise the values to 0 and I dont want that since I might use enemiesAlive and alliesAlive as a display and I don't want it randomly resseting to 0
        int enemiesTemp=0;
        int alliesTemp=0;
        foreach (Character temp in charactersInside) {
            if (temp.alive) {
                if (temp.team != (int)Character.teamList.Player)
                    enemiesTemp++;
                if (temp.team == (int)Character.teamList.Player)
                    alliesTemp++;
            }
        }
        enemiesAlive = enemiesTemp;
        alliesAlive = alliesTemp;
        if(!zoneDone)
        zoneWon();
        zoneLost();
    }
}
