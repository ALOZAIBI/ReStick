using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    //this is set in initroundstart in character
    public Character character;

    public string abilityName;
    public string description;

    //used for indicators and for ability color.
    public Color color;

    public Color buffFXColor;

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


    //some abilities apply buffs
    public Buff buffPrefab;
    public enum RaritiesList {
        Common,
        Rare,
        Epic,
        Legendary
    }
    [SerializeField] public RaritiesList Rarities;
    public int rarity;

    //Abilities targetStrategy
    public int targetStrategy;

    public enum AbilityTypeList {
        PhysicalDamage,
        MagicDamage,
        Heal,
        Buff,
        Debuff,
        SelfBuff,
        CrowdControl,
        Special,
        HealthDamage,
        Other
    }
    [SerializeField] AbilityTypeList abilityTypeList;
    public int abilityType;
    public virtual void Start() {
        abilityType = (int)abilityTypeList;
        rarity = ((int)Rarities);

        color = ColorPalette.singleton.getIndicatorColor(abilityType);
    }
    //use onValidate only in values that aren't supposed to change when game starts.
    private void OnValidate() {
        abilityType = (int)abilityTypeList;
        rarity = (int)Rarities;   
    }
    //executes this ability
    public abstract void doAbility();
    public virtual void executeAbility() {
        //Some abilities need to play an animation before executing. Once the animation is played call this function.
    }
    /// <summary>
    /// Sends the ability to be cast and the animation that will cast it
    /// </summary>
    /// <param name="animation"></param>
    public void playAnimation(string animation) {
        //if there is no ability queued To Be Cast and we are allowed to interrupt the current animation (or there is no animation playing)
        if(character.animationManager.abilityBuffer == null && character.animationManager.interruptible) {
            Debug.Log("animation should play"+character.name+abilityName);
            character.animationManager.cast(this,animation);
        }
            
    }
    //call this when ability level increases to update the description to show the new stats
    public abstract void updateDescription();

    
    //call this in doAbility();
    /// <summary>
    /// Calculates amt by adding stat ratios
    /// </summary>
    public void calculateAmt() {
        //after loading characters for the first time the cahracter's abilities don't recognize who their caster is yet so calculateAmt wouldnt work
        //so here we manually tell the characterInfoScreen to tell whichever character is currently being viewed to tell it's abilities that it owns them
        //what a fucked explanation lmao
        try {
            if (character == null) {
                //Debug.Log("TYOLD EM"+abilityName);
                //if we're doing this to regular character screen
                if (UIManager.singleton.inventoryScreenHidden.hidden)
                    UIManager.singleton.characterInfoScreen.character.initRoundStart();
                else//we're doing this to inventory Character Screen
                    UIManager.singleton.inventoryScreen.inventoryCharacterScreen.character.initRoundStart();

            }
            amt = baseAmt + character.PD * PDRatio + character.MD * MDRatio + character.INF * INFRatio + character.HPMax * HPMaxRatio + character.HP * HPRatio + character.level * LVLRatio + character.MS * MSRatio + character.AS * ASRatio;
            //for example in teh case of damagin aura it should make the amt more negative instead of positive
            if (baseAmt < 0) {
                amt = -amt;
            }
        }
        catch { /*when starting the game character will be null but also characterinfoscreen.character will be null so this is just to avoid the error when starting the game for the first time*/}
    }

    //if an ability has a cooldown call this inside doAbility()
    public void startCooldown() {
        //starts the CD
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
    public Buff createBuff() {
        if(buffPrefab==null) {
            Debug.Log("BuffPrefab is null");
        }
        Buff buff = Instantiate(buffPrefab).GetComponent<Buff>();
        buff.gameObject.SetActive(false);
        return buff;
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
