using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    //this is set in initroundstart in character
    public Character character;

    public string abilityName;
    public string description;

    //abilities cd
    public float CD;
    //the range of said ability
    public float rangeAbility;
    //checked if available in the ability's script
    public bool available=true;
    public float abilityNext = 0;

    //player can choose to add delay before an ability is executed
    public float delay;

    //used to calculate amt     //consider adding more ratios such as HPMax HP MS AS etc...
    public float baseAmt;
    public float PDRatio;
    public float MDRatio;
    public float INFRatio;
    public float HPMaxRatio;
    public float HPRatio;
    public float LVLRatio;//scales with level
    public float MSRatio;
    public float ASRatio;
    //amt = baseamt+charPD*PDratio+charMD*MDratio
    //the float value used in an ability what it is used for depends on the ability
    public float amt;

    //to be used if this ability uses target(In order to display target in abilityDisplay) for example in healing aura there is no target
    //this still isn't used
    public bool hasTarget;

    //someAbilities can instantiate prefabs or object
    public GameObject prefabObject;

    public enum raritiesList {
        Common,
        Rare,
        Epic,
        Legendary
    }
    public int rarity;


    //executes this ability
    public abstract void doAbility();

    //call this when ability level increases to update the description to show the new stats
    public abstract void updateDescription();
    //Abilities targetStrategy
    public int targetStrategy;

    //call this in doAbility();
    /// <summary>
    /// Calculates amt by adding stat ratios
    /// </summary>
    public void calculateAmt() {
        //after loading characters for the first time the cahracter's abilities don't recognize who their caster is yet so calculateAmt wouldnt work
        //so here we manually tell the characterInfoScreen to tell whichever character is currently being viewed to tell it's abilities that it owns them
        //what a fucked explanation lmao
        if(character == null) {
            //Debug.Log("TYOLD EM");
            //if we're doing this to regular character screen
            if(UIManager.singleton.inventoryScreenHidden.hidden)
                UIManager.singleton.characterInfoScreen.character.initRoundStart();
            else//we're doing this to inventory Character Screen
                UIManager.singleton.inventoryScreen.inventoryCharacterScreen.character.initRoundStart();
    
        }
        amt = baseAmt + character.PD * PDRatio + character.MD * MDRatio + character.INF * INFRatio + character.HPMax * HPMaxRatio+ character.HP*HPRatio + character.level*LVLRatio + character.MS*MSRatio + character.AS*ASRatio;
        //for example in teh case of damagin aura it should make the amt more negative instead of positive
        if(baseAmt < 0) {
            amt = -amt;
        }
    }

    //if an ability has a cooldown call this inside doAbility()
    public void startCooldown() {
        abilityNext = CD - CD*character.CDR;
        available = false;
    }
    //and put this in the fixedupdate function
    public void cooldown() {
        if (abilityNext > 0) {
            abilityNext -= Time.fixedDeltaTime;
        }
        else {
            abilityNext = 0;
            available = true;
        }
    }

    //just to be able to display the CD after the CDR stat has been updated (Since increasing CDR shouldn't change base CD)
    public string displayCDAfterChange() {
        try {
            return (CD - CD * character.CDR).ToString("F1");
        }
        catch {
            //catch happens when an ability's character hasn't been set yet
            return (CD.ToString("F1"));
        }
    }
}
