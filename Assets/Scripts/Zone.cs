using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Zone : MonoBehaviour
{
    //List of characters inside the Zone
    public List<Character> charactersInside = new List<Character>();

    //the map that this zone belongs to 
    public string belongsToMap;

    [SerializeField] private UIManager uIManager;
    [SerializeField] private GameObject playerParty;
    //connects to UImanager
    private void Start() {
        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        uIManager.displayCharacterPlacing();
        
    }
    //to detect which players in the zone
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Character")) {
            charactersInside.Add(collision.GetComponent<Character>());
        }
        
    }

    //if there are no enemies alive and there is atleast 1 playerCharacter alive show win screen
    private void zoneWon() {

        int enemiesAlive = 0;
        int alliesAlive = 0;
        foreach(Character temp in charactersInside) {
            if(temp.alive) {
                if(temp.team != (int)Character.teamList.Player)
                    enemiesAlive++;
                if (temp.team == (int)Character.teamList.Player)
                    alliesAlive++;
            }
        }
        if (enemiesAlive == 0 && alliesAlive>0) {
            Debug.Log("Game Over");
            uIManager.displayGameWon(belongsToMap);
        }
    }
    void FixedUpdate()
    {
        zoneWon();
    }
}
