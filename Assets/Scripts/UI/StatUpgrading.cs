using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using System;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;
public class StatUpgrading : MonoBehaviour {
    // So what this Script does is when the add and sub buttons are clicked decrease and increase the stat display accordingly.
    //So far this only alters the display. But when the apply button is clicked the stats are added to the character. And if the 
    // reset button is clicked then reset the stats

    //this is set by characterinfoscreen and is only used to check in displayStats if the character to be displayed is new or no. If it is new then it sets applied to false
    public Character lastUsedCharacter;
    public CharacterInfoScreen characterInfoScreen;

    [SerializeField] private GameObject statPointTextContainer;
    [SerializeField] private TextMeshProUGUI statPointDisplay;

    [SerializeField] public Button addPD;
    [SerializeField] public Button subPD;
    public GameObject PDIcon;

    [SerializeField] public Button addMD;
    [SerializeField] public Button subMD;
    public GameObject MDIcon;

    [SerializeField] public Button addINF;
    [SerializeField] public Button subINF;
    public GameObject INFIcon;

    [SerializeField] public Button addAS;
    [SerializeField] public Button subAS;
    public GameObject ASIcon;

    [SerializeField] public Button addCDR;
    [SerializeField] public Button subCDR;
    public GameObject CDRIcon;

    [SerializeField] public Button addMS;
    [SerializeField] public Button subMS;
    public GameObject MSIcon;

    [SerializeField] public Button addRNG;
    [SerializeField] public Button subRNG;
    public GameObject RNGIcon;

    [SerializeField] public Button addLS;
    [SerializeField] public Button subLS;
    public GameObject LSIcon;

    [SerializeField] public Button addHP;
    [SerializeField] public Button subHP;
    [SerializeField] public GameObject HPIcon;

    //the amount that clicking the button adds/removes
    [SerializeField] private float PDAmt;
    [SerializeField] private float MDAmt;
    [SerializeField] private float INFAmt;
    [SerializeField] private float ASAmt;
    [SerializeField] private float CDRAmt;
    [SerializeField] private float MSAmt;
    [SerializeField] private float RNGAmt;
    [SerializeField] private float LSAmt;
    [SerializeField] private float HPAmt;

    public LayoutGroup textLayoutGroup1;
    public LayoutGroup textLayoutGroup2;

    public LayoutGroup iconLayoutGroup1;
    public LayoutGroup iconLayoutGroup2;

    public LayoutGroup numberLayoutGroup1;
    public LayoutGroup numberLayoutGroup2;

    public LayoutGroup column1;
    public LayoutGroup column2;

    [SerializeField] public Button applyChangesBtn;
    [SerializeField] public Button resetChangesBtn;
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
    [SerializeField] public int SPUsedBuffer;


    //wether the stats have been appleid or not
    public bool applied;

    //The object to be initialised that will display the improvements in the ability
    public GameObject abilityDisplayStatDifferences;
    public List<AbilityDisplayStatDifference> abilityDisplayList;

    public bool createdAbilityDisplays;
    //thanks chatGPT
    public void hide() {
        // Set all gameObjects to inactive
        //statPointTextContainer.SetActive(false);
        //statPointDisplay.gameObject.SetActive(false);
        addPD.gameObject.SetActive(false);
        subPD.gameObject.SetActive(false);
        addMD.gameObject.SetActive(false);
        subMD.gameObject.SetActive(false);
        addINF.gameObject.SetActive(false);
        subINF.gameObject.SetActive(false);
        addAS.gameObject.SetActive(false);
        subAS.gameObject.SetActive(false);
        addCDR.gameObject.SetActive(false);
        subCDR.gameObject.SetActive(false);
        addMS.gameObject.SetActive(false);
        subMS.gameObject.SetActive(false);
        addRNG.gameObject.SetActive(false);
        subRNG.gameObject.SetActive(false);
        addLS.gameObject.SetActive(false);
        subLS.gameObject.SetActive(false);
        addHP.gameObject.SetActive(false);
        subHP.gameObject.SetActive(false);
        HPIcon.SetActive(false);

        applyChangesBtn.gameObject.SetActive(false);
        resetChangesBtn.gameObject.SetActive(false);
        closeAbilityDisplays();
    }

    public void show() {
        createdAbilityDisplays = false;
        // Set all gameObjects to active
        //statPointTextContainer.SetActive(true);
        //statPointDisplay.gameObject.SetActive(true);
        addPD.gameObject.SetActive(true);
        subPD.gameObject.SetActive(true);
        addMD.gameObject.SetActive(true);
        subMD.gameObject.SetActive(true);
        addINF.gameObject.SetActive(true);
        subINF.gameObject.SetActive(true);
        addAS.gameObject.SetActive(true);
        subAS.gameObject.SetActive(true);
        addCDR.gameObject.SetActive(true);
        subCDR.gameObject.SetActive(true);
        addMS.gameObject.SetActive(true);
        subMS.gameObject.SetActive(true);
        addRNG.gameObject.SetActive(true);
        subRNG.gameObject.SetActive(true);
        addLS.gameObject.SetActive(true);
        subLS.gameObject.SetActive(true);
        addHP.gameObject.SetActive(true);
        subHP.gameObject.SetActive(true);
        HPIcon.SetActive(true);
        applyChangesBtn.gameObject.SetActive(true);
        resetChangesBtn.gameObject.SetActive(true);
        updateAddColors();
        updateSubColors();
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
        // Add onClick listeners for all buttons
        addPD.onClick.AddListener(OnAddPDButtonClicked);
        subPD.onClick.AddListener(OnSubPDButtonClicked);
        addMD.onClick.AddListener(OnAddMDButtonClicked);
        subMD.onClick.AddListener(OnSubMDButtonClicked);
        addINF.onClick.AddListener(OnAddINFButtonClicked);
        subINF.onClick.AddListener(OnSubINFButtonClicked);
        addAS.onClick.AddListener(OnAddASButtonClicked);
        subAS.onClick.AddListener(OnSubASButtonClicked);
        addCDR.onClick.AddListener(OnAddCDRButtonClicked);
        subCDR.onClick.AddListener(OnSubCDRButtonClicked);
        addMS.onClick.AddListener(OnAddMSButtonClicked);
        subMS.onClick.AddListener(OnSubMSButtonClicked);
        addRNG.onClick.AddListener(OnAddRNGButtonClicked);
        subRNG.onClick.AddListener(OnSubRNGButtonClicked);
        addLS.onClick.AddListener(OnAddLSButtonClicked);
        subLS.onClick.AddListener(OnSubLSButtonClicked);
        addHP.onClick.AddListener(OnAddHPButtonClicked);
        subHP.onClick.AddListener(OnSubHPButtonClicked);

        applyChangesBtn.onClick.AddListener(applyChanges);
        resetChangesBtn.onClick.AddListener(resetChanges);

        hide();
        //fakeStatDisplay();
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

        hide();
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

    #region Buttons
    // OnAdd function for PD
    public void OnAddPDButtonClicked() {
        if ((characterInfoScreen.character.statPoints) > 0) {
            characterInfoScreen.character.PD += PDAmt;
            PDbuffer += PDAmt;
            characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for PD
    public void OnSubPDButtonClicked() {
        if (PDbuffer > 0) {
            characterInfoScreen.character.PD -= PDAmt;
            PDbuffer -= PDAmt;
            characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for MD
    public void OnAddMDButtonClicked() {
        if ((characterInfoScreen.character.statPoints) > 0) {
            characterInfoScreen.character.MD += MDAmt;
            MDbuffer += MDAmt;
            characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for MD
    public void OnSubMDButtonClicked() {
        if (MDbuffer > 0) {
            characterInfoScreen.character.MD -= MDAmt;
            MDbuffer -= MDAmt;
            characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for INF
    public void OnAddINFButtonClicked() {
        if ((characterInfoScreen.character.statPoints) > 0) {
            characterInfoScreen.character.INF += INFAmt;
            INFbuffer += INFAmt;
            characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for INF
    public void OnSubINFButtonClicked() {
        if (INFbuffer > 0) {
            characterInfoScreen.character.INF -= INFAmt;
            INFbuffer -= INFAmt;
            characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for AS
    public void OnAddASButtonClicked() {
        if ((characterInfoScreen.character.statPoints) > 0) {
            characterInfoScreen.character.AS += ASAmt;
            ASbuffer += ASAmt;
            characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for AS
    public void OnSubASButtonClicked() {
        if (ASbuffer > 0) {
            characterInfoScreen.character.AS -= ASAmt;
            ASbuffer -= ASAmt;
            characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for CDR
    public void OnAddCDRButtonClicked() {
        if ((characterInfoScreen.character.statPoints) > 0) {
            characterInfoScreen.character.CDR += CDRAmt;
            CDRbuffer += CDRAmt;
            characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for CDR
    public void OnSubCDRButtonClicked() {
        if (CDRbuffer > 0) {
            characterInfoScreen.character.CDR -= CDRAmt;
            CDRbuffer -= CDRAmt;
            characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for MS
    public void OnAddMSButtonClicked() {
        if ((characterInfoScreen.character.statPoints) > 0) {
            characterInfoScreen.character.MS += MSAmt;
            MSbuffer += MSAmt;
            characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for MS
    public void OnSubMSButtonClicked() {
        if (MSbuffer > 0) {
            characterInfoScreen.character.MS -= MSAmt;
            MSbuffer -= MSAmt;
            characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for RNG
    public void OnAddRNGButtonClicked() {
        if ((characterInfoScreen.character.statPoints) > 0) {
            characterInfoScreen.character.Range += RNGAmt;
            RNGbuffer += RNGAmt;
            characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for RNG
    public void OnSubRNGButtonClicked() {
        if (RNGbuffer > 0) {
            characterInfoScreen.character.Range -= RNGAmt;
            RNGbuffer -= RNGAmt;
            characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for LS
    public void OnAddLSButtonClicked() {
        if ((characterInfoScreen.character.statPoints) > 0) {
            characterInfoScreen.character.LS += LSAmt;
            LSbuffer += LSAmt;
            characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for LS
    public void OnSubLSButtonClicked() {
        if (LSbuffer > 0) {
            characterInfoScreen.character.LS -= LSAmt;
            LSbuffer -= LSAmt;
            characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for HP
    public void OnAddHPButtonClicked() {
        if ((characterInfoScreen.character.statPoints) > 0) {
            characterInfoScreen.character.HP += HPAmt;
            characterInfoScreen.character.HPMax += HPAmt;
            HPbuffer += HPAmt;
            characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for HP
    public void OnSubHPButtonClicked() {
        if (HPbuffer > 0) {
            characterInfoScreen.character.HP -= HPAmt;
            characterInfoScreen.character.HPMax -= HPAmt;
            HPbuffer -= HPAmt;
            characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    #endregion

    private void updateAddColors() {
        //Colors all the add buttons grey if the character has no stat points to spend
        if (characterInfoScreen.character.statPoints == 0) {
            addPD.GetComponent<Image>().SetAlpha(0.2f);
            addMD.GetComponent<Image>().SetAlpha(0.2f);
            addINF.GetComponent<Image>().SetAlpha(0.2f);
            addAS.GetComponent<Image>().SetAlpha(0.2f);
            addCDR.GetComponent<Image>().SetAlpha(0.2f);
            addMS.GetComponent<Image>().SetAlpha(0.2f);
            addRNG.GetComponent<Image>().SetAlpha(0.2f);
            addLS.GetComponent<Image>().SetAlpha(0.2f);
            addHP.GetComponent<Image>().SetAlpha(0.2f);
        } else {
            addPD.GetComponent<Image>().SetAlpha(1f);
            addMD.GetComponent<Image>().SetAlpha(1f);
            addINF.GetComponent<Image>().SetAlpha(1f);
            addAS.GetComponent<Image>().SetAlpha(1f);
            addCDR.GetComponent<Image>().SetAlpha(1f);
            addMS.GetComponent<Image>().SetAlpha(1f);
            addRNG.GetComponent<Image>().SetAlpha(1f);
            addLS.GetComponent<Image>().SetAlpha(1f);
            addHP.GetComponent<Image>().SetAlpha(1f);
        }
    }
    private void updateSubColors() {
        //Colors the sub buttons grey if that stat's buffer is 0
        if (PDbuffer == 0) {
            subPD.GetComponent<Image>().SetAlpha(0.2f);
        } else {
            subPD.GetComponent<Image>().SetAlpha(1f);
        }
        if (MDbuffer == 0) {
            subMD.GetComponent<Image>().SetAlpha(0.2f);
        } else {
            subMD.GetComponent<Image>().SetAlpha(1f);
        }
        if (INFbuffer == 0) {
            subINF.GetComponent<Image>().SetAlpha(0.2f);
        } else {
            subINF.GetComponent<Image>().SetAlpha(1f);
        }
        if (ASbuffer == 0) {
            subAS.GetComponent<Image>().SetAlpha(0.2f);
        } else {
            subAS.GetComponent<Image>().SetAlpha(1f);
        }
        if (CDRbuffer == 0) {
            subCDR.GetComponent<Image>().SetAlpha(0.2f);
        } else {
            subCDR.GetComponent<Image>().SetAlpha(1f);
        }
        if (MSbuffer == 0) {
            subMS.GetComponent<Image>().SetAlpha(0.2f);
        } else {
            subMS.GetComponent<Image>().SetAlpha(1f);
        }
        if (RNGbuffer == 0) {
            subRNG.GetComponent<Image>().SetAlpha(0.2f);
        } else {
            subRNG.GetComponent<Image>().SetAlpha(1f);
        }
        if (LSbuffer == 0) {
            subLS.GetComponent<Image>().SetAlpha(0.2f);
        } else {
            subLS.GetComponent<Image>().SetAlpha(1f);
        }
        if (HPbuffer == 0) {
            subHP.GetComponent<Image>().SetAlpha(0.2f);
        } else {
            subHP.GetComponent<Image>().SetAlpha(1f);
        }

        if(SPUsedBuffer == 0) {
            resetChangesBtn.GetComponent<Image>().SetAlpha(0.2f);
        } else {
            resetChangesBtn.GetComponent<Image>().SetAlpha(1f);
        }
    }

    //updates visual to display change to be applied
    public void updateStatDisplay() {
        //Debug.Log("Fakse stats uopdated");
        unFocusAbilityIconHolder();
        characterInfoScreen.displayStats(characterInfoScreen.character);
        characterInfoScreen.displayCharacterAbilities(characterInfoScreen.character);
        updateAddColors();
        updateSubColors();

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
