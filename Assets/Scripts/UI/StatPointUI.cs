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

    [SerializeField] private Button addDmg;
    [SerializeField] private Button subDmg;

    [SerializeField] private Button addAS;
    [SerializeField] private Button subAS;

    [SerializeField] private Button addMS;
    [SerializeField] private Button subMS;

    [SerializeField] private Button addRNG;
    [SerializeField] private Button subRNG;

    [SerializeField] private Button addLS;
    [SerializeField] private Button subLS;

    [SerializeField] private Button addHP;
    [SerializeField] private Button subHP;

    //the amount that clicking the button adds/removes
    [SerializeField] private float DMGAmt;
    [SerializeField] private float ASAmt;
    [SerializeField] private float MSAmt;
    [SerializeField] private float RNGAmt;
    [SerializeField] private float LSAmt;
    [SerializeField] private float HPAmt;


    [SerializeField] private Button applyChangesBtn;
    [SerializeField] private Button resetChangesBtn;
    //how much stats are modified so far (not applied)
    private float DMGbuffer;
    private float ASbuffer;
    private float MSbuffer;
    private float RNGbuffer;
    private float LSbuffer;
    private float HPbuffer;


    //How much SP used
    private int SPUsedBuffer;

    //thanks chatGPT
    public void hide() {
        // Set all gameObjects to inactive
        statPointTextContainer.SetActive(false);
        statPointDisplay.gameObject.SetActive(false);
        addDmg.gameObject.SetActive(false);
        subDmg.gameObject.SetActive(false);
        addAS.gameObject.SetActive(false);
        subAS.gameObject.SetActive(false);
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
        addDmg.gameObject.SetActive(true);
        subDmg.gameObject.SetActive(true);
        addAS.gameObject.SetActive(true);
        subAS.gameObject.SetActive(true);
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
        addDmg.onClick.AddListener(OnAddDMGButtonClicked);
        subDmg.onClick.AddListener(OnSubDMGButtonClicked);
        addAS.onClick.AddListener(OnAddASButtonClicked);
        subAS.onClick.AddListener(OnSubASButtonClicked);
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
        characterInfoScreen.character.DMG += DMGbuffer;
        characterInfoScreen.character.AS += ASbuffer;
        characterInfoScreen.character.MS += MSbuffer;
        characterInfoScreen.character.Range += RNGbuffer;
        characterInfoScreen.character.LS += LSbuffer;
        characterInfoScreen.character.HPMax += HPbuffer;
        characterInfoScreen.character.HP += HPbuffer;

        characterInfoScreen.character.statPoints -= SPUsedBuffer;

        // Reset the buffer values to zero
        DMGbuffer = 0;
        ASbuffer = 0;
        MSbuffer = 0;
        RNGbuffer = 0;
        LSbuffer = 0;
        HPbuffer = 0;
        SPUsedBuffer = 0;

        characterInfoScreen.displayStats(characterInfoScreen.character);
    }

    private void resetChanges() {
        DMGbuffer = 0;
        ASbuffer = 0;
        MSbuffer = 0;
        RNGbuffer = 0;
        LSbuffer = 0;
        HPbuffer = 0;

        SPUsedBuffer = 0;

        characterInfoScreen.displayStats(characterInfoScreen.character);
    }


    // OnAdd function for DMG
    public void OnAddDMGButtonClicked() {
        if ((characterInfoScreen.character.statPoints - SPUsedBuffer) > 0) {
            DMGbuffer += DMGAmt;
            SPUsedBuffer++;
            fakeStatDisplay();
        }
    }

    // OnSub function for DMG
    public void OnSubDMGButtonClicked() {
        if (DMGbuffer > 0) {
            DMGbuffer -= DMGAmt;
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
        characterInfoScreen.DMG.text = (characterInfoScreen.character.DMG+DMGbuffer).ToString("F1");
        characterInfoScreen.AS.text = (characterInfoScreen.character.AS+ASbuffer).ToString("F1");
        characterInfoScreen.MS.text = (characterInfoScreen.character.MS + MSbuffer).ToString("F1");
        characterInfoScreen.RNG.text = (characterInfoScreen.character.Range + RNGbuffer).ToString("F1");
        characterInfoScreen.LS.text = (characterInfoScreen.character.LS + LSbuffer).ToString("F1");
        characterInfoScreen.healthBar.HPtext.text=((characterInfoScreen.character.HP+HPbuffer).ToString("F1") + "/" + (characterInfoScreen.character.HPMax+HPbuffer).ToString("F1"));
    }

    private void Update() {
        if (characterInfoScreen.character.statPoints > 0) {
            show();
        }
        else
            hide();
    }

}
