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

    //connects to UImanager
    private void Start() {
        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        uIManager.displayCharacterPlacing();

        playerParty = GameObject.FindGameObjectWithTag("PlayerParty").GetComponent<PlayerManager>();
        zoneName = gameObject.scene.name;
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
        }
    }
    void FixedUpdate()
    {
        //counts characters alive
        foreach (Character temp in charactersInside) {
            if (temp.alive) {
                if (temp.team != (int)Character.teamList.Player)
                    enemiesAlive++;
                if (temp.team == (int)Character.teamList.Player)
                    alliesAlive++;
            }
        }
        zoneWon();
        zoneLost();
    }
}
