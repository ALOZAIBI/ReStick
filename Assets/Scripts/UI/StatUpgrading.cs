using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using System;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;
using Unity.VisualScripting;

public class StatUpgrading : MonoBehaviour {
    // So what this Script does is when the add and sub buttons are clicked decrease and increase the stat display accordingly.
    //So far this only alters the display. But when the apply button is clicked the stats are added to the character. And if the 
    // reset button is clicked then reset the stats

    //this is set by characterinfoscreen and is only used to check in displayStats if the character to be displayed is new or no. If it is new then it sets applied to false
    public Character lastUsedCharacter;
    public CharacterInfoScreen characterInfoScreen;

    public GameObject PDIcon;

    public GameObject MDIcon;

    public GameObject INFIcon;

    public GameObject ASIcon;

    public GameObject CDRIcon;

    public GameObject MSIcon;

    public GameObject RNGIcon;

   
    public GameObject LSIcon;

    public GameObject HPIcon;

    //the amount that clicking the button adds/removes
    public float PDAmt;
    public float MDAmt;
    public float INFAmt;
    public float ASAmt;
    public float CDRAmt;
    public float MSAmt;
    public float RNGAmt;
    public float LSAmt;
    public float HPAmt;

  

    //how much stats are modified so far (not applied)
    private float PDbuffer;
    private float MDbuffer;
    private float INFbuffer;
    private float ASbuffer;
    private float CDRbuffer;
    private float MSbuffer;
    private float RNGbuffer;
    private float LSbuffer;
    private float HPbuffer;
    //How much SP used
    public int SPUsedBuffer;


    //wether the stats have been appleid or not
    public bool applied;

    [SerializeField]public RectTransform upgradeOptions;

    [SerializeField]public Button applyChangesBtn;
    [SerializeField]public Button resetChangesBtn;

    //The object to be initialised that will display the improvements in the ability
    public GameObject abilityDisplayStatDifferences;
    public List<AbilityDisplayStatDifference> abilityDisplayList;

    //Contains the index of the stat to be upgraded from the stats array
    [SerializeField] private (string,GameObject,float) upgrade1;
    [SerializeField] private bool clickedUpgrade1;
    [SerializeField] private Button upgrade1Btn;
    [SerializeField] private TextMeshProUGUI textUpgrade1;
    [SerializeField] private GameObject iconUpgrade1;

    [SerializeField] private (string, GameObject, float) upgrade2;
    [SerializeField] private bool clickedUpgrade2;
    [SerializeField] private Button upgrade2Btn;
    [SerializeField] private TextMeshProUGUI textUpgrade2;
    [SerializeField] private GameObject iconUpgrade2;

    [SerializeField] private (string, GameObject, float) upgrade3;
    [SerializeField] private bool clickedUpgrade3;
    [SerializeField] private Button upgrade3Btn;
    [SerializeField] private TextMeshProUGUI textUpgrade3;
    [SerializeField] private GameObject iconUpgrade3;

    
    [SerializeField] private Button rerollBtn;
    public bool createdAbilityDisplays;

    //2D array where the first index of each element is the name of the stat and the second is the Icon and the third is the amount
    private (string, GameObject,float)[] stats;
    //So that when we reroll we don't get the same stat again
    private List<int> statsUsedIndices = new List<int>();

    private void Start() {
        applyChangesBtn.onClick.AddListener(applyChanges);
        resetChangesBtn.onClick.AddListener(resetChanges);

        upgrade1Btn.onClick.AddListener(upgrade1Clicked);
        upgrade2Btn.onClick.AddListener(upgrade2Clicked);
        upgrade3Btn.onClick.AddListener(upgrade3Clicked);

        stats = new (string, GameObject, float)[9];
        stats[0] = ("Physical Damage", PDIcon,PDAmt);
        stats[1] = ("Magic Damage", MDIcon,MDAmt);
        stats[2] = ("Influence", INFIcon,INFAmt);
        stats[3] = ("Attack Speed", ASIcon,ASAmt);
        stats[4] = ("Cooldown Reduction", CDRIcon, CDRAmt);
        stats[5] = ("Movement Speed", MSIcon,MSAmt);
        stats[6] = ("Range", RNGIcon,RNGAmt);
        stats[7] = ("Lifesteal", LSIcon,LSAmt);
        stats[8] = ("Health", HPIcon,HPAmt);


    }

    private void setUpgrades() {
        //Get a random int from length of statsArray without it being in statsUsedIndices
        int randomIndex = UnityEngine.Random.Range(0, stats.Length);
        while (statsUsedIndices.Contains(randomIndex)) {
            randomIndex = UnityEngine.Random.Range(0, stats.Length);
        }
        statsUsedIndices.Add(randomIndex);
        upgrade1 = stats[randomIndex];
        textUpgrade1.text = "+ " + upgrade1.Item3 +" "+upgrade1.Item1;
        iconUpgrade1 = upgrade1.Item2;

        randomIndex = UnityEngine.Random.Range(0, stats.Length);
        while (statsUsedIndices.Contains(randomIndex)) {
            randomIndex = UnityEngine.Random.Range(0, stats.Length);
        }
        statsUsedIndices.Add(randomIndex);
        upgrade2 = stats[randomIndex];
        textUpgrade2.text = "+ " + upgrade2.Item3 + " " + upgrade2.Item1;
        iconUpgrade2 = upgrade2.Item2;

        randomIndex = UnityEngine.Random.Range(0, stats.Length);
        while (statsUsedIndices.Contains(randomIndex)) {
            randomIndex = UnityEngine.Random.Range(0, stats.Length);
        }
        statsUsedIndices.Add(randomIndex);
        upgrade3 = stats[randomIndex];
        textUpgrade3.text = "+ " + upgrade3.Item3 + " " + upgrade3.Item1;
        iconUpgrade3 = upgrade3.Item2;
    }

    private void upgrade1Clicked() {
        if (clickedUpgrade1) {
            return;
        }
        //Reset the other upgrades
        resetChanges();
        switch (upgrade1.Item1) {
            case "Physical Damage":
                PDbuffer += upgrade1.Item3;
                break;

            case "Magic Damage":
                MDbuffer += upgrade1.Item3;
                break;

            case "Influence":
                INFbuffer += upgrade1.Item3;
                break;

            case "Attack Speed":
                ASbuffer += upgrade1.Item3;
                break;

            case "Cooldown Reduction":
                CDRbuffer += upgrade1.Item3;
                break;

            
            case "Movement Speed":
                MSbuffer += upgrade1.Item3;
                break;

            case "Range":
                RNGbuffer += upgrade1.Item3;
                break;

            case "Lifesteal":
                LSbuffer += upgrade1.Item3;
                break;

             case "Health":
                HPbuffer += upgrade1.Item3;
                break;
        }
        clickedUpgrade1 = true;
        SPUsedBuffer++;
        characterInfoScreen.character.statPoints--;
        addBufferToCharacter();
        updateStatDisplay();
    }
    private void upgrade2Clicked() {
        if(clickedUpgrade2) {
            return;
        }
        //Reset the other upgrades
        resetChanges();
        switch (upgrade2.Item1) {
            case "Physical Damage":
                PDbuffer += upgrade2.Item3;
                break;

            case "Magic Damage":
                MDbuffer += upgrade2.Item3;
                break;

            case "Influence":
                INFbuffer += upgrade2.Item3;
                break;

            case "Attack Speed":
                ASbuffer += upgrade2.Item3;
                break;

            case "Cooldown Reduction":
                CDRbuffer += upgrade2.Item3;
                break;

            case "Movement Speed":
                MSbuffer += upgrade2.Item3;
                break;

            case "Range":
                RNGbuffer += upgrade2.Item3;
                break;

            case "Lifesteal":
                LSbuffer += upgrade2.Item3;
                break;

            case "Health":
                HPbuffer += upgrade2.Item3;
                break;
        }
        clickedUpgrade2 = true;
        characterInfoScreen.character.statPoints--;
        SPUsedBuffer++;
        addBufferToCharacter();
        updateStatDisplay();

    }
    private void upgrade3Clicked() {
        if (clickedUpgrade3) {
            return;
        }
        //Reset the other upgrades
        resetChanges();
        switch (upgrade3.Item1) {
            case "Physical Damage":
                PDbuffer += upgrade3.Item3;
                break;

            case "Magic Damage":
                MDbuffer += upgrade3.Item3;
                break;

            case "Influence":
                INFbuffer += upgrade3.Item3;
                break;

            case "Attack Speed":
                ASbuffer += upgrade3.Item3;
                break;

            case "Cooldown Reduction":
                CDRbuffer += upgrade3.Item3;
                break;

            case "Movement Speed":
                MSbuffer += upgrade3.Item3;
                break;

            case "Range":
                RNGbuffer += upgrade3.Item3;
                break;

            case "Lifesteal":
                LSbuffer += upgrade3.Item3;
                break;

            case "Health":
                HPbuffer += upgrade3.Item3;
                break;
        }
        //Use a stat point
        characterInfoScreen.character.statPoints--;
        SPUsedBuffer++;
        clickedUpgrade3 = true; 
        addBufferToCharacter();
        updateStatDisplay();

    }
    private void addBufferToCharacter() {
        characterInfoScreen.character.PD += PDbuffer;
        characterInfoScreen.character.MD += MDbuffer;
        characterInfoScreen.character.INF += INFbuffer;
        characterInfoScreen.character.AS += ASbuffer;
        characterInfoScreen.character.CDR += CDRbuffer;
        characterInfoScreen.character.MS += MSbuffer;
        characterInfoScreen.character.Range += RNGbuffer;
        characterInfoScreen.character.LS += LSbuffer;
        characterInfoScreen.character.HP += HPbuffer;
        characterInfoScreen.character.HPMax += HPbuffer;

    }
    public void showUpgrades() {
        applyChangesBtn.gameObject.SetActive(true);
        resetChangesBtn.gameObject.SetActive(true);

        setUpgrades();
    }

    public void focusAbilityIconHolder() {
        foreach(RectTransform rectTransform in characterInfoScreen.abilityDisplays) {
            AbilityDisplay abilityDisplay = rectTransform.GetComponent<AbilityDisplay>();
            abilityDisplay.iconHolder.transform.SetParent(UIManager.singleton.focus.transform);
        }
    }
    public void unFocusAbilityIconHolder() {
        foreach (RectTransform rectTransform in characterInfoScreen.abilityDisplays) {
            AbilityDisplay abilityDisplay = rectTransform.GetComponent<AbilityDisplay>();
            abilityDisplay.iconHolder.transform.SetParent(rectTransform);
        }
    }

    public void applyChanges() {
        // Reset the buffer values to zero
        PDbuffer = 0;
        MDbuffer = 0;
        INFbuffer = 0;
        ASbuffer = 0;
        CDRbuffer = 0;
        MSbuffer = 0;
        RNGbuffer = 0;
        LSbuffer = 0;
        HPbuffer = 0;

        SPUsedBuffer = 0;

        if (characterInfoScreen.character.Range >= 1.5f) {
            characterInfoScreen.character.usesProjectile = true;
        }



        applied = true;
        //saves the changes
        if (SceneManager.GetActiveScene().name == "World") {
            UIManager.singleton.saveWorldSave();
        }
        else
            UIManager.singleton.saveMapSave();
        
        //Unclick the upgrades
        clickedUpgrade1 = false;
        clickedUpgrade2 = false;
        clickedUpgrade3 = false;

        //Clear the stats used
        statsUsedIndices.Clear();
        //hide();
        unFocusAbilityIconHolder();
        characterInfoScreen.startUnfocusing();


    }
    //resets changes when backButton is clicked or CloseUI Button Clicked
    public void resetChanges() {
        characterInfoScreen.character.PD -= PDbuffer;
        characterInfoScreen.character.MD -= MDbuffer;
        characterInfoScreen.character.INF -= INFbuffer;
        characterInfoScreen.character.AS -= ASbuffer;
        characterInfoScreen.character.CDR -= CDRbuffer;
        characterInfoScreen.character.MS -= MSbuffer;
        characterInfoScreen.character.Range -= RNGbuffer;
        characterInfoScreen.character.LS -= LSbuffer;
        characterInfoScreen.character.HPMax -= HPbuffer;
        characterInfoScreen.character.HP -= HPbuffer;

        //Undo the use of stat point
        characterInfoScreen.character.statPoints+=SPUsedBuffer;

        if (characterInfoScreen.character.Range < 1.5f) {
            characterInfoScreen.character.usesProjectile = false;
        }

        PDbuffer = 0;
        MDbuffer = 0;
        INFbuffer = 0;
        ASbuffer = 0;
        CDRbuffer = 0;
        MSbuffer = 0;
        RNGbuffer = 0;
        LSbuffer = 0;
        HPbuffer = 0;

        SPUsedBuffer = 0;

        applied = false;

        //Clear the stats used
        statsUsedIndices.Clear();

        //Unclick the upgrades
        clickedUpgrade1 = false;
        clickedUpgrade2 = false;
        clickedUpgrade3 = false;

        updateStatDisplay();
    }

    //updates visual to display change to be applied
    public void updateStatDisplay() {
        //Debug.Log("Fakse stats uopdated");
        unFocusAbilityIconHolder();
        characterInfoScreen.displayStats(characterInfoScreen.character);
        characterInfoScreen.displayCharacterAbilities(characterInfoScreen.character);

        focusAbilityIconHolder();
        createdAbilityDisplays = true;
        

        showOrHideAbilityDisplays();
    }

    //Creates abilityDisplays and puts them in their place
    public void createAbilityDisplayStatDifferences() {
        closeAbilityDisplays();
        //Goes through all abilities
        for(int i = 0; i < characterInfoScreen.character.abilities.Count; i++) {
            //Instantiates as child of abilitiesPanel so that the rectTransform is correctly scaled and positioned
            abilityDisplayList.Add(Instantiate(abilityDisplayStatDifferences.GetComponent<AbilityDisplayStatDifference>(),characterInfoScreen.abilitiesPanel));
            abilityDisplayList[i].setupAbilityDisplay(characterInfoScreen.character.abilities[i]);
            RectTransform rectTransform = abilityDisplayList[i].GetComponent<RectTransform>();


            rectTransform.SetAnchorTop(characterInfoScreen.abilityPlaceholders[i].GetAnchorTop());
            rectTransform.SetAnchorBottom(characterInfoScreen.abilityPlaceholders[i].GetAnchorBottom());
            rectTransform.SetAnchorLeft(characterInfoScreen.abilityPlaceholders[i].GetAnchorLeft());
            rectTransform.SetAnchorRight(characterInfoScreen.abilityPlaceholders[i].GetAnchorRight());
            rectTransform.SetStretchToAnchors();

            abilityDisplayList[i].transform.SetParent(UIManager.singleton.focus.transform);
            Debug.Log("Created a display"+rectTransform.name);
        }
    }
    private void showOrHideAbilityDisplays() {
        for(int i = 0;i<abilityDisplayList.Count;i++) {
            abilityDisplayList[i].showOrHide(characterInfoScreen.character.abilities[i].valueAmt);
        }
    }
    private void closeAbilityDisplays() {
        //Deletes all abilityDisplays GameObjects
        for(int i = 0; i < abilityDisplayList.Count; i++) {
            Destroy(abilityDisplayList[i].gameObject);
        }
        //Clears the list
        abilityDisplayList.Clear();
    }
    private void Update() {
    }

}
