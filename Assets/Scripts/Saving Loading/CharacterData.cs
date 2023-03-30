using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class CharacterData
{
    public int prefabIndex;

    public string charName;

    //Stats
    public float PD;
    public float MD;
    public float HP;
    public float HPMax;
    public float CDR;
    public float MS;
    public float Range;
    public float LS;

    public bool alive;

    //Interesting Stats
    public int totalKills;

    //level stuff
    public int level;
    //how much xp in current level
    public int xpProgress;
    //how much xp needed to level up
    public int xpCap;
    //points that can be used on stats (gained wen leveling up)
    public int statPoints;

    //Character's team
    public int team;

    //Current targeting strategy
    public int attackTargetStrategy;
    public int movementTargetStrategy;

    //contains the names of abilities that will then be fetched from ability factory
    public string[] abilities;

    public float size;

    public CharacterData(Character character) {
        prefabIndex = character.prefabIndex;
        charName = character.name;
        PD = character.PD;
        MD = character.MD;
        HP = character.HP;
        HPMax = character.HPMax;
        CDR = character.CDR;
        MS = character.MS;
        Range = character.Range;
        LS = character.LS;
        alive = character.alive;
        totalKills = character.totalKills;
        level = character.level;
        xpProgress = character.xpProgress;
        xpCap = character.xpCap;
        statPoints = character.statPoints;
        team = character.team;
        attackTargetStrategy = character.attackTargetStrategy;
        movementTargetStrategy = character.movementTargetStrategy;

        abilities = new string[character.abilities.Count];
        for(int i = 0; i < character.abilities.Count; i++) {
            abilities[i] = character.abilities[i].abilityName;
        }
        //taking x is enough since the scale is square
        size = character.gameObject.transform.localScale.x;
    }

    //this is needed for SaveSystem to be able to deserialize it
    public CharacterData() { }

    
}