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
    
    public void addCharactersToShop(Transform parent,int amount) {
        for (int i = 0; i < amount; i++) {
            //instantiates a random character as a child of parent
            int index = Random.Range(0, characters.Count);
            Character temp = Instantiate(characters[0], parent).GetComponent<Character>();
            //give it a random name
            index = Random.Range(0, names.Count - 1);
            temp.name = names[index];
        }
    }

    public void addDefaultCharacter(Transform parent) {
        Character temo = Instantiate(characters[0], parent).GetComponent<Character>();
        //Give it a random name
        int index = Random.Range(0, names.Count - 1);
        temo.name = names[index];
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

        //xpCap has to be before xpProgress, otherwise the character will level up a lot due to the setter of xpProgress
        temp.xpCap = data.xpCap;
        temp.xpProgress = data.xpProgress;

        temp.statPoints = data.statPoints;
        temp.team = data.team;
        temp.attackTargetStrategy = data.attackTargetStrategy;
        temp.movementStrategy = data.movementTargetStrategy;

        temp.hasArchetype = data.hasArchetype;
        temp.archetypeName = data.archetypeName;
        //add the abilities
        UIManager.singleton.abilityFactory.addRequestedAbilitiesToCharacter(temp, data.abilities,data.abilityTargetting);
        UIManager.singleton.itemFactory.addRequestedAbilitiesToCharacter(temp, data.items);
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
