using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using System;

public class StatPointUI : MonoBehaviour {
    // So what this Script does is when the add and sub buttons are clicked decrease and increase the stat display accordingly.
    //So far this only alters the display. But when the apply button is clicked the stats are added to the character. And if the 
    // reset button is clicked then reset the stats

    //this is initialized within characterInfoScreen
    public CharacterInfoScreen characterInfoScreen;

    [SerializeField] private GameObject statPointTextContainer;
    [SerializeField] private TextMeshProUGUI statPointDisplay;

    [SerializeField] private Button addPD;
    [SerializeField] private Button subPD;

    [SerializeField] private Button addMD;
    [SerializeField] private Button subMD;

    [SerializeField] private Button addAS;
    [SerializeField] private Button subAS;

    [SerializeField] private Button addCDR;
    [SerializeField] private Button subCDR;

    [SerializeField] private Button addMS;
    [SerializeField] private Button subMS;

    [SerializeField] private Button addRNG;
    [SerializeField] private Button subRNG;

    [SerializeField] private Button addLS;
    [SerializeField] private Button subLS;

    [SerializeField] private Button addHP;
    [SerializeField] private Button subHP;

    //the amount that clicking the button adds/removes
    [SerializeField] private float PDAmt;
    [SerializeField] private float MDAmt;
    [SerializeField] private float ASAmt;
    [SerializeField] private float CDRAmt;
    [SerializeField] private float MSAmt;
    [SerializeField] private float RNGAmt;
    [SerializeField] private float LSAmt;
    [SerializeField] private float HPAmt;


    [SerializeField] private Button applyChangesBtn;
    [SerializeField] private Button resetChangesBtn;
    //how much stats are modified so far (not applied)
    private float PDbuffer;
    private float MDbuffer;
    private float ASbuffer;
    private float CDRbuffer;
    private float MSbuffer;
    private float RNGbuffer;
    private float LSbuffer;
    private float HPbuffer;


    //How much SP used
    [SerializeField] private int SPUsedBuffer;

    //thanks chatGPT
    public void hide() {
        // Set all gameObjects to inactive
        statPointTextContainer.SetActive(false);
        statPointDisplay.gameObject.SetActive(false);
        addPD.gameObject.SetActive(false);
        subPD.gameObject.SetActive(false);
        addMD.gameObject.SetActive(false);
        subMD.gameObject.SetActive(false);
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
    }

    public void show() {
        // Set all gameObjects to active
        statPointTextContainer.SetActive(true);
        statPointDisplay.gameObject.SetActive(true);
        addPD.gameObject.SetActive(true);
        subPD.gameObject.SetActive(true);
        addMD.gameObject.SetActive(true);
        subMD.gameObject.SetActive(true);
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
    }

    private void Start() {
        // Add onClick listeners for all buttons
        addPD.onClick.AddListener(OnAddPDButtonClicked);
        subPD.onClick.AddListener(OnSubPDButtonClicked);
        addMD.onClick.AddListener(OnAddMDButtonClicked);
        subMD.onClick.AddListener(OnSubMDButtonClicked);
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

        //fakeStatDisplay();
    }

    private void applyChanges() {
        characterInfoScreen.character.PD += PDbuffer;
        characterInfoScreen.character.MD += MDbuffer;
        characterInfoScreen.character.AS += ASbuffer;
        characterInfoScreen.character.CDR += CDRbuffer;
        characterInfoScreen.character.MS += MSbuffer;
        characterInfoScreen.character.Range += RNGbuffer;
        characterInfoScreen.character.LS += LSbuffer;
        characterInfoScreen.character.HPMax += HPbuffer;
        characterInfoScreen.character.HP += HPbuffer;

        characterInfoScreen.character.statPoints -= SPUsedBuffer;

        // Reset the buffer values to zero
        PDbuffer = 0;
        MDbuffer = 0;
        ASbuffer = 0;
        CDRbuffer = 0;
        MSbuffer = 0;
        RNGbuffer = 0;
        LSbuffer = 0;
        HPbuffer = 0;

        SPUsedBuffer = 0;

        characterInfoScreen.viewCharacter(characterInfoScreen.character);
    }

    private void resetChanges() {
        PDbuffer = 0;
        MDbuffer = 0;
        ASbuffer = 0;
        CDRbuffer = 0;
        MSbuffer = 0;
        RNGbuffer = 0;
        LSbuffer = 0;
        HPbuffer = 0;

        SPUsedBuffer = 0;

        characterInfoScreen.displayStats(characterInfoScreen.character);
    }


    // OnAdd function for PD
    public void OnAddPDButtonClicked() {
        if ((characterInfoScreen.character.statPoints - SPUsedBuffer) > 0) {
            PDbuffer += PDAmt;
            SPUsedBuffer++;
            fakeStatDisplay();
        }
    }

    // OnSub function for PD
    public void OnSubPDButtonClicked() {
        if (PDbuffer > 0) {
            PDbuffer -= PDAmt;
            SPUsedBuffer--;
            fakeStatDisplay();
        }
    }

    // OnAdd function for MD
    public void OnAddMDButtonClicked() {
        if ((characterInfoScreen.character.statPoints - SPUsedBuffer) > 0) {
            MDbuffer += MDAmt;
            SPUsedBuffer++;
            fakeStatDisplay();
        }
    }

    // OnSub function for MD
    public void OnSubMDButtonClicked() {
        if (MDbuffer > 0) {
            MDbuffer -= MDAmt;
            SPUsedBuffer--;
            fakeStatDisplay();
        }
    }

    // OnAdd function for AS
    public void OnAddASButtonClicked() {
        if ((characterInfoScreen.character.statPoints - SPUsedBuffer) > 0) {
            ASbuffer += ASAmt;
            SPUsedBuffer++;
            fakeStatDisplay();
        }
    }

    // OnSub function for AS
    public void OnSubASButtonClicked() {
        if (ASbuffer > 0) {
            ASbuffer -= ASAmt;
            SPUsedBuffer--;
            fakeStatDisplay();
        }
    }

    // OnAdd function for CDR
    public void OnAddCDRButtonClicked() {
        if ((characterInfoScreen.character.statPoints - SPUsedBuffer) > 0) {
            CDRbuffer += CDRAmt;
            SPUsedBuffer++;
            fakeStatDisplay();
        }
    }

    // OnSub function for CDR
    public void OnSubCDRButtonClicked() {
        if (CDRbuffer > 0) {
            CDRbuffer -= CDRAmt;
            SPUsedBuffer--;
            fakeStatDisplay();
        }
    }

    // OnAdd function for MS
    public void OnAddMSButtonClicked() {
        if ((characterInfoScreen.character.statPoints - SPUsedBuffer) > 0) {
            MSbuffer += MSAmt;
            SPUsedBuffer++;
            fakeStatDisplay();
        }
    }

    // OnSub function for MS
    public void OnSubMSButtonClicked() {
        if (MSbuffer > 0) {
            MSbuffer -= MSAmt;
            SPUsedBuffer--;
            fakeStatDisplay();
        }
    }

    // OnAdd function for RNG
    public void OnAddRNGButtonClicked() {
        if ((characterInfoScreen.character.statPoints - SPUsedBuffer) > 0) {
            RNGbuffer += RNGAmt;
            SPUsedBuffer++;
            fakeStatDisplay();
        }
    }

    // OnSub function for RNG
    public void OnSubRNGButtonClicked() {
        if (RNGbuffer > 0) {
            RNGbuffer -= RNGAmt;
            SPUsedBuffer--;
            fakeStatDisplay();
        }
    }

    // OnAdd function for LS
    public void OnAddLSButtonClicked() {
        if ((characterInfoScreen.character.statPoints - SPUsedBuffer) > 0) {
            LSbuffer += LSAmt;
            SPUsedBuffer++;
            fakeStatDisplay();
        }
    }

    // OnSub function for LS
    public void OnSubLSButtonClicked() {
        if (LSbuffer > 0) {
            LSbuffer -= LSAmt;
            SPUsedBuffer--;
            fakeStatDisplay();
        }
    }

    // OnAdd function for HP
    public void OnAddHPButtonClicked() {
        if ((characterInfoScreen.character.statPoints - SPUsedBuffer) > 0) {
            HPbuffer += HPAmt;
            SPUsedBuffer++;
            fakeStatDisplay();
        }
    }

    // OnSub function for HP
    public void OnSubHPButtonClicked() {
        if (HPbuffer > 0) {
            HPbuffer -= HPAmt;
            SPUsedBuffer--;
            fakeStatDisplay();
        }
    }




    //updates visual to display change to be applied
    public void fakeStatDisplay() {
        Debug.Log("Fakse stats uopdated");
        statPointDisplay.text = characterInfoScreen.character.statPoints-SPUsedBuffer + "";
        characterInfoScreen.PD.text = (characterInfoScreen.character.PD+PDbuffer).ToString("F1");
        characterInfoScreen.MD.text = (characterInfoScreen.character.MD + MDbuffer).ToString("F1");
        characterInfoScreen.AS.text = (characterInfoScreen.character.AS+ASbuffer).ToString("F1");
        characterInfoScreen.CDR.text = ((characterInfoScreen.character.CDR + CDRbuffer)*100).ToString("F1");
        characterInfoScreen.MS.text = (characterInfoScreen.character.MS + MSbuffer).ToString("F1");
        characterInfoScreen.RNG.text = (characterInfoScreen.character.Range + RNGbuffer).ToString("F1");
        characterInfoScreen.LS.text = ((characterInfoScreen.character.LS + LSbuffer)*100).ToString("F1");
        characterInfoScreen.healthBar.HPtext.text=((characterInfoScreen.character.HP+HPbuffer).ToString("F1") + "/" + (characterInfoScreen.character.HPMax+HPbuffer).ToString("F1"));
        //updates the abilities to display new description (since we migth have changed the MD AND PD which would change abiltiy amt)
        characterInfoScreen.displayCharacterAbilities(characterInfoScreen.character);
    }

    private void Update() {
    }

}