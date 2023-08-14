using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public abstract class Ability : MonoBehaviour
{
    //this is set in initroundstart in character
    public Character character;
    //This is used to save who the target is when the ability is triggered(Before the animation starts). Because in some cases the animation can start then the target moves out of range and hence becomes null on character.target. Also because character.target might be different from the target of the ability
    [SerializeField]public Character lockedTarget;
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

    //There are X values in abilities so each Ratio would be an array of size X. The 0 element of the array would be for the first value then 1 then 2 etc...X
    //Examples of Values: Dmg, Slow Amount, Buff Duration, etc...
    //used to calculate values
    
    [HideInInspector]public List<string> valueNames= new List<string>();
    [HideInInspector]public List<float> baseAmt = new List<float>();
    [HideInInspector]public List<float> PDRatio = new List<float>();
    [HideInInspector]public List<float> MDRatio = new List<float>();
    [HideInInspector]public List<float> INFRatio = new List<float>();
    [HideInInspector]public List<float> HPMaxRatio = new List<float>();
    [HideInInspector]public List<float> HPRatio = new List<float>();
    [HideInInspector]public List<float> LVLRatio = new List<float>();//scales with level
    [HideInInspector]public List<float> MSRatio = new List<float>();
    [HideInInspector]public List<float> ASRatio = new List<float>();

    //amt = baseamt+charPD*PDratio+charMD*MDratio
    //the float value used in an ability what it is used for depends on the ability
    public List<float> valueAmt = new List<float>();

    //to be used if this ability uses target(In order to display target in abilityDisplay) for example in healing aura there is no target
    //this still isn't used
    public bool hasTarget;

    //someAbilities can instantiate prefabs or object
    public GameObject prefabObject;


    //some abilities apply buffs
    public Buff buffPrefab;
    //Returns the AmtValue of the respective ValueName
    
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

    //Used for target Selection
    public bool canTargetAlly;
    public bool canTargetEnemy;

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
    [HideInInspector]public int numberOfValues;
    public virtual void Start() {
        abilityType = (int)abilityTypeList;
        rarity = ((int)Rarities);

        color = ColorPalette.singleton.getIndicatorColor(abilityType);
    }
    //use onValidate only in values that aren't supposed to change when game starts.
#if UNITY_EDITOR
    public void OnValidate() {
        abilityType = (int)abilityTypeList;
        rarity = (int)Rarities;   
        //Sets size of the lists to be equal to numberOfValues
        while (valueNames.Count != numberOfValues) {
            if(valueNames.Count>numberOfValues)
                valueNames.RemoveAt(valueNames.Count-1);
            else
            valueNames.Add("");
        }
        while (baseAmt.Count != numberOfValues) {
            if (baseAmt.Count > numberOfValues)
                baseAmt.RemoveAt(baseAmt.Count - 1);
            else
                baseAmt.Add(0);
        }
        while (PDRatio.Count != numberOfValues) {
            if (PDRatio.Count > numberOfValues)
                PDRatio.RemoveAt(PDRatio.Count - 1);
            else
                PDRatio.Add(0);
        }
        while (MDRatio.Count != numberOfValues) {
            if (MDRatio.Count > numberOfValues)
                MDRatio.RemoveAt(MDRatio.Count - 1);
            else
                MDRatio.Add(0);
        }
        while (INFRatio.Count != numberOfValues) {
            if (INFRatio.Count > numberOfValues)
                INFRatio.RemoveAt(INFRatio.Count - 1);
            else
                INFRatio.Add(0);
        }
        while (HPMaxRatio.Count != numberOfValues) {
            if (HPMaxRatio.Count > numberOfValues)
                HPMaxRatio.RemoveAt(HPMaxRatio.Count - 1);
            else
                HPMaxRatio.Add(0);
        }
        while (HPRatio.Count != numberOfValues) {
            if (HPRatio.Count > numberOfValues)
                HPRatio.RemoveAt(HPRatio.Count - 1);
            else
                HPRatio.Add(0);
        }
        while (LVLRatio.Count != numberOfValues) {
            if (LVLRatio.Count > numberOfValues)
                LVLRatio.RemoveAt(LVLRatio.Count - 1);
            else
                LVLRatio.Add(0);
        }
        while (MSRatio.Count != numberOfValues) {
            if (MSRatio.Count > numberOfValues)
                MSRatio.RemoveAt(MSRatio.Count - 1);
            else
                MSRatio.Add(0);
        }
        while (ASRatio.Count != numberOfValues) {
            if (ASRatio.Count > numberOfValues)
                ASRatio.RemoveAt(ASRatio.Count - 1);
            else
                ASRatio.Add(0);
        }
        while (valueAmt.Count != numberOfValues) {
            if (valueAmt.Count > numberOfValues)
                valueAmt.RemoveAt(valueAmt.Count - 1);
            else
                valueAmt.Add(0);
        }

    }
    #endif
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
            //Debug.Log("animation should play"+character.name+abilityName);
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
                UIManager.singleton.characterInfoScreen.character.initRoundStart();
            }
            for (int i = 0; i < valueNames.Count; i++) {
                valueAmt[i] = baseAmt[i] + character.PD * PDRatio[i] + character.MD * MDRatio[i] + character.INF * INFRatio[i] + character.HPMax * HPMaxRatio[i] + character.HP * HPRatio[i] + character.level * LVLRatio[i] + character.MS * MSRatio[i] + character.AS * ASRatio[i];
                ////for example in teh case of damagin aura it should make the amt more negative instead of positive
                //if (baseAmt[i] < 0) {
                //    valueAmt[i] = -valueAmt[i];
                //}
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
    public float getCDAfterChange() {
        try {
            return (CD - CD * character.CDR);
        }
        catch {
            //catch happens when an ability's character hasn't been set yet
            return (CD);
        }
    }   

    
}
