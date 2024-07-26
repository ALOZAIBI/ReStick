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
    public bool hasTarget;

    //someAbilities can instantiate prefabs or object
    public GameObject prefabObject;


    //some abilities apply buffs
    public Buff buffPrefab;

    /// <summary>
    /// When the animation is done playing if this is set to true then execute of the ability will be called
    /// </summary>
    public bool executeAbilityOnEvent = true;
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

    //Some abilities have multiple steps and play a different animation for each step, so in these abilities the execute ability function will increment step and the cooldown function will reset step.
    [SerializeField]protected int step;

    [SerializeField]protected HitFX hitFX;
    [SerializeField]protected Color hitFXColor;
    ////Buff Values
    //public float PD;
    //public float MD;
    //public float INF;
    //public float HP;
    //public float AS;
    //public float CDR;
    //public float MS;
    //public float Range;
    //public float LS;

    //public float size;

    ////used to root silence and blind etc..
    //public bool root;
    //public bool silence;
    //public bool blind;
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

    public Archetype.ArchetypeList archetypePrimary;
    public Archetype.ArchetypeList archetypeSecondary;

    public int abilityType;
    [HideInInspector]public int numberOfValues;
    [SerializeField] protected string animationToPlay;
    public virtual void Start() {
        abilityType = (int)abilityTypeList;
        rarity = ((int)Rarities);

        color = ColorPalette.singleton.getTypeColor(abilityType);
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
    //executes this ability returns true if the ability was executed(To be used for UI activation)
    public abstract bool doAbility();

  
    //Some abilities need to reset on round start or on cooldown
    public virtual void reset() {

    }
    //Used to trigger an item's afterAbility effect
    //Used to play the UI activation
    protected void startAbilityActivation(bool visualOnly = false) {
        //Plays the UI Activation animation, if this is the character currently selected
        if (character == UIManager.singleton.characterInfoScreen.character) {
            //Find the index of the ability in the character's ability list
            for (int i = 0; i < character.abilities.Count; i++) {
                if (character.abilities[i] == this) {
                    UIManager.singleton.characterInfoScreen.displayAbilityActivation(i);
                    break;
                }
            }
        }
        //character's item's afterAbility effect(Can pass this as parameter if needed)
        if(!visualOnly)
            character.itemAfterAbility();
    }
    public virtual void executeAbility() {
        //Some abilities need to play an animation before executing. Once the animation is played call this function.
       startAbilityActivation();
    }
    /// <summary>
    /// Sends the ability to be cast and the animation that will cast it
    /// </summary>
    /// <param name="animation"></param>
    public void playAnimation(string animation) {
        character.animationManager.cast(this,animation); 
    }
    //call this when ability level increases to update the description to show the new stats
    public abstract void updateDescription();

    public virtual List<Character> excludeTargets() {
        return null;
    }
    
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
        reset();
        //starts the CD
        abilityNext = CD - CD*character.CDR;
        available = false;
        step = 0;
        if(character.currentDashingAbility == this) {
            character.currentDashingAbility = null;
        }
        
    }

    protected bool canUseDash() {
        //if the character is not dashing or is dashing with this ability then enable doing the ability
        if(character.currentDashingAbility == null || character.currentDashingAbility == this) {
            return true;
        }
        else {
            return false;
        }
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
    //Used to interrupt dashes
    protected void interruptDash(Character victim) {
        //Since dashes disable navmeshagent, we need to reenable it when interrupting.
        victim.agent.enabled = true;
        if (victim.currentDashingAbility != null) {
            victim.animationManager.forceStop();
            victim.currentDashingAbility.step = 0;
            Debug.Log("The ability" + victim.currentDashingAbility.abilityName + " has been interrupted." + " of character " + victim.name);
            victim.currentDashingAbility.startCooldown();
        }
            
        //Later when you have a dash animation, the animation would hold the ability that is being cast in the buffer, so we need to put that ability on cooldown since it has been interuppted.
    }

    //Instantiates an active HitFX at position
    public void applyHitFX(Character character) {
        applyHitFX(character.transform.position);
    }
    public void applyHitFX(Vector3 position) {
        HitFX temp = Instantiate(hitFX, position, Quaternion.identity);
        temp.gameObject.SetActive(true);
        //Makes the instantiated object's color same as the projectile color
        temp.GetComponent<SpriteRenderer>().color = hitFXColor;
    }
    public void applyHitFX(Character character, float size) {
        applyHitFX(character.transform.position, size);
    }
    public void applyHitFX(Vector3 position,float size) {
        HitFX temp = Instantiate(hitFX, position, Quaternion.identity);
        temp.transform.localScale = new Vector3(size, size, size);
        temp.gameObject.SetActive(true);
        //Makes the instantiated object's color same as the projectile color
        temp.GetComponent<SpriteRenderer>().color = hitFXColor;
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
    
    //So that after cloning CD's are random
    public void setRandomCD() {
        abilityNext = Random.Range(0, CD - CD * character.CDR);
        if(abilityNext > 0) {
            available = false;
        }
        else {
            available = true;
        }
    }

    
}
