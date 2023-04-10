using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private UIManager uIManager;

    [SerializeField] private PlayerManager playerParty;

    private int enemiesAlive = 0;

    private int alliesAlive = 0;

    //gold to be added to playerparty on level completion
    public int goldSoFar=0;

    //list of gameObject that contain ability that can be rewarded here
    public List<GameObject> abilityRewardPool = new List<GameObject>();

    //contains the abilities to be rewarded as children
    public GameObject abilityContainer;

    //string containing ability names. This list is filled when zone save file is loaded.
    //These string names will be used to fetch abilities from ability factory
    public List<string> abilityNames = new List<string>();

    //connects to UImanager
    private void Start() {
        abilityContainer = GameObject.FindGameObjectWithTag("ZoneRewards");

        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        uIManager.displayCharacterPlacing();
        uIManager.hideCharacter();

        playerParty = GameObject.FindGameObjectWithTag("PlayerParty").GetComponent<PlayerManager>();
        zoneName = gameObject.scene.name;

        //loads Zone if possible
        if (SaveSystem.loadZone(this)) {
            //then extracts the ability names from the save file then fetches them from ability factory to add to reward pool
            uIManager.abilityFactory.addRequestedAbilityToZone(this, abilityNames);
        }
        //if there is no saveFile for this zone. get abilities from ablity factory to add to reward pool
        else {
            //adds 3 random abilities to the zone
            uIManager.abilityFactory.addRandomAbilityToZone(this, 3);
        }
        
    }
    //to detect which players in the zone
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Character")) {
            charactersInside.Add(collision.GetComponent<Character>());
        }
    }

    //if there are no enemies alive and there is atleast 1 playerCharacter alive show win screen
    private void zoneWon() {

        if (enemiesAlive == 0 && alliesAlive>0) {
            uIManager.displayGameWon(belongsToMap);
            //marks zone as completed then saves
            completed = true;
            //then pauses the game
            uIManager.pausePlay(true);
        }
        
    }
    //if all player Character's in play died decrease totallives and displaygamelost
    private void zoneLost() {

        if(started) {
            //if there's a single player character in play and alive leave this function
            foreach (Transform child in playerParty.transform) {
                if (child.tag == "Character") {
                    Character currChar = child.GetComponent<Character>();
                    //if in play and alive
                    if (currChar.gameObject.activeSelf && currChar.alive)
                        return;
                }
            }
            //otherwise zone is lost
            uIManager.displayGameLost(belongsToMap);
            playerParty.totalLives--;
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
        zoneWon();
        zoneLost();
    }
}
