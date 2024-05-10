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

    public bool createdAbilityDisplays;

    public void showUpgrades() {
        applyChangesBtn.gameObject.SetActive(true);
        resetChangesBtn.gameObject.SetActive(true);
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
    private void Start() {
        applyChangesBtn.onClick.AddListener(applyChanges);
        resetChangesBtn.onClick.AddListener(resetChanges);
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

        if (characterInfoScreen.character.Range >= 1.5f) {
            characterInfoScreen.character.usesProjectile = true;
        }

        SPUsedBuffer = 0;

        applied = true;
        //saves the changes
        if (SceneManager.GetActiveScene().name == "World") {
            UIManager.singleton.saveWorldSave();
        }
        else
            UIManager.singleton.saveMapSave();

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
        characterInfoScreen.character.statPoints += SPUsedBuffer;

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
