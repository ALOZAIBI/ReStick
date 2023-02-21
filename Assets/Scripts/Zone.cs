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

    [SerializeField] private UIManager uIManager;

    [SerializeField] private PlayerManager playerParty;

    private int enemiesAlive = 0;
    //initialized to one so that
    private int alliesAlive = 0;

    //gold to be added to playerparty on level completion
    public int goldSoFar=0;

    //list of gameObject that contain ability that can be rewarded here
    public List<GameObject> abilityRewardPool = new List<GameObject>();

    //connects to UImanager
    private void Start() {
        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        uIManager.displayCharacterPlacing();

        playerParty = GameObject.FindGameObjectWithTag("PlayerParty").GetComponent<PlayerManager>();
        zoneName = gameObject.scene.name;

        //fill the abilityRewardPool
        foreach(Transform objPool in transform) {
            if(objPool.tag == "ZoneRewards") {
                foreach(GameObject tempAb in objPool) {
                    Debug.Log("Ability Name" + tempAb.name);
                    abilityRewardPool.Add(tempAb);
                }
            }
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
            Debug.Log("Game Over");
            uIManager.displayGameWon(belongsToMap);
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
            //started is re set to false to prevent totallives to decrement infintely
            started = false;
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
