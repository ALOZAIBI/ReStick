using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectArchetype : MonoBehaviour
{
    public List<Archetype> archetypeList = new List<Archetype>();

    //Hold the chances of each archetype being picked. (The value changes based on the character's upgrade stats)
    public List<int> pickChance = new List<int>();

    public Character character;

    public Character baseCharacter;

    public int PWR;
    public int MGC;
    public int INF;
    public int HP;
    public int AS;
    public int CDR;
    public int SPD;
    public int Range;
    public int LS;

    private void Start() {
        baseCharacter = UIManager.singleton.characterFactory.transform.GetChild(0).GetComponent<Character>();
        //Make pickChance same size as archetypeLiost
        for (int i = 0; i < archetypeList.Count; i++) {
            pickChance.Add(0);
        }
        errorCheck();
    }
    private void errorCheck() {
        //Checks if the sum of values of each archetype is more than 10, if so there is an error
        foreach(Archetype archetype in archetypeList) {
            if (archetype.PWR + archetype.MGC + archetype.INF + archetype.HP + archetype.AS + archetype.CDR + archetype.SPD + archetype.Range + archetype.LS > 10) {
                Debug.LogError("Archetype " + archetype.archetypeName + " has more than 10 points in total");
            }
        }
    }
    //Checks which stats were upgraded in character and how many times.
    public void getStatsUpgraded(Character character) {
        this.character = character;
        //Compares to base character to find out how many times each stat was upgraded
        PWR = (int)((character.PD - baseCharacter.PD) / UIManager.singleton.characterInfoScreen.statUpgrading.PDAmt);
        MGC = (int)((character.MD - baseCharacter.MD) / UIManager.singleton.characterInfoScreen.statUpgrading.MDAmt);
        INF = (int)((character.INF - baseCharacter.INF) / UIManager.singleton.characterInfoScreen.statUpgrading.INFAmt);
        //We did this to HP since HP increases per level as well
        HP = (int)(((character.HPMax - 12*character.level) - baseCharacter.HPMax+12) / UIManager.singleton.characterInfoScreen.statUpgrading.HPAmt);
        AS = (int)((character.AS - baseCharacter.AS) / UIManager.singleton.characterInfoScreen.statUpgrading.ASAmt);
        CDR = (int)((character.CDR - baseCharacter.CDR) / UIManager.singleton.characterInfoScreen.statUpgrading.CDRAmt);
        SPD = (int)((character.MS - baseCharacter.MS) / UIManager.singleton.characterInfoScreen.statUpgrading.MSAmt);
        Range = (int)((character.Range - baseCharacter.Range) / UIManager.singleton.characterInfoScreen.statUpgrading.RNGAmt);
        LS = (int)((character.LS - baseCharacter.LS) / UIManager.singleton.characterInfoScreen.statUpgrading.LSAmt);


    }

    //Sets the pick chance for each archetype
    public void setPickChance() {

        for (int i = 0; i < archetypeList.Count; i++) {
            pickChance[i] = 0;
        }
        //Sets the pickChance for each archetype
        for(int i = 0; i < archetypeList.Count; i++) {
            //increase pick chance for each stat if it is non negative.(Sometimes it would be negative like the case of BigNStrong
            pickChance[i] += archetypeList[i].PWR > 0? archetypeList[i].PWR * PWR : 0;
            pickChance[i] += archetypeList[i].MGC > 0 ? archetypeList[i].MGC * MGC : 0;
            pickChance[i] += archetypeList[i].INF > 0 ? archetypeList[i].INF * INF : 0;
            pickChance[i] += archetypeList[i].HP > 0 ? archetypeList[i].HP * HP : 0;
            pickChance[i] += archetypeList[i].AS > 0 ? archetypeList[i].AS * AS : 0;
            pickChance[i] += archetypeList[i].CDR > 0 ? archetypeList[i].CDR * CDR : 0;
            pickChance[i] += archetypeList[i].SPD > 0 ? archetypeList[i].SPD * SPD : 0;
            pickChance[i] += archetypeList[i].Range > 0 ? archetypeList[i].Range * Range : 0;
            pickChance[i] += archetypeList[i].LS > 0 ? archetypeList[i].LS * LS : 0;
        }
    }
}
