using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFactory : MonoBehaviour
{
    public GameObject playerParty;
    public List<GameObject> characters = new List<GameObject>();
    public List<string> names = new List<string>();
    public int prefabIndex = 0;
    //self explanatory name
    //maybe add parameter that chooses what team the character isin in case I want to create random enemy units as well etc..
    //this is using a simple gameobject as parameter and not the playerParty since sometimes
    //we might want objects that aren't player party like the shop for example
    public void addRandomCharacterAsChild(Transform parent,int amount) {
        for (int i = 0; i < amount; i++) {
            //instantiates a random character as a child of parent
            int index = Random.Range(0, characters.Count);
            Character temp = Instantiate(characters[index], parent).GetComponent<Character>();
            //give it a random name
            index = Random.Range(0, names.Count - 1);
            temp.name = names[index];
            //give it a random color
            temp.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            //give it random size between 1.1 and 1.6
            float size = Random.Range(1.3f, 1.5f);
            temp.transform.localScale = new Vector3(size, size, size);
        }
    }

    public void addCharacterAsChild(CharacterData data,Transform parent) {
        //Debug.Log(characters.Count + data.prefabIndex);
        //instantiate character of prefab as a child of parent
        Character temp = Instantiate(characters[data.prefabIndex],parent).GetComponent<Character>();
        //set it's stats
        temp.prefabIndex = data.prefabIndex;
        temp.name = data.charName;
        temp.PD = data.PD;
        temp.MD = data.MD;
        temp.INF = data.INF;
        temp.HP = data.HP;
        temp.HPMax = data.HPMax;
        temp.AS = data.AS;
        temp.CDR = data.CDR;
        temp.MS = data.MS;
        temp.Range = data.Range;
        temp.LS = data.LS;
        temp.alive = data.alive;
        temp.totalKills = data.totalKills;
        temp.totalDamage = data.totalDamage;
        temp.level = data.level;
        temp.xpProgress = data.xpProgress;
        temp.xpCap = data.xpCap;
        temp.statPoints = data.statPoints;
        temp.team = data.team;
        temp.attackTargetStrategy = data.attackTargetStrategy;
        temp.movementStrategy = data.movementTargetStrategy;
        //set it's color
        Color colorTemp = new Color(data.red,data.green,data.blue);
        temp.GetComponent<SpriteRenderer>().color = colorTemp;
        //set it's size
        temp.transform.localScale = new Vector3(data.size, data.size, data.size);
        //add the abilities
        UIManager.singleton.abilityFactory.addRequestedAbilitiesToCharacter(temp, data.abilities,data.abilityTargetting);
        //if(parent==UIManager.singleton.playerParty.transform)
            //Debug.Log("Character loaded" + temp.name+temp.HP+" PlayerPartyChildCOunt"+UIManager.singleton.playerParty.transform.childCount);
        //create another array for each ability holding the int of targetting strategy
    }
    //on starts adds all children to the characters list and sets the index 
    private void Start() {
        foreach (Transform child in transform) {
            characters.Add(child.gameObject);
            Character temp = child.gameObject.GetComponent<Character>();
            temp.prefabIndex = prefabIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
