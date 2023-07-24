using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using System;
using UnityEngine.SceneManagement;

public class StatUpgrading : MonoBehaviour {
    // So what this Script does is when the add and sub buttons are clicked decrease and increase the stat display accordingly.
    //So far this only alters the display. But when the apply button is clicked the stats are added to the character. And if the 
    // reset button is clicked then reset the stats

    //this is set by characterinfoscreen and is only used to check in displayStats if the character to be displayed is new or no. If it is new then it sets applied to false
    public Character lastUsedCharacter;

    [SerializeField] private GameObject statPointTextContainer;
    [SerializeField] private TextMeshProUGUI statPointDisplay;

    [SerializeField] private Button addPD;
    [SerializeField] private Button subPD;

    [SerializeField] private Button addMD;
    [SerializeField] private Button subMD;

    [SerializeField] private Button addINF;
    [SerializeField] private Button subINF;

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
    [SerializeField] private GameObject hpIcon;

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


    [SerializeField] private Button applyChangesBtn;
    [SerializeField] private Button resetChangesBtn;
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


    //wether the stats have been appleid or not
    public bool applied;

    //How much SP used
    [SerializeField] public int SPUsedBuffer;

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
        hpIcon.SetActive(false);
    }

    public void show() {
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
        hpIcon.SetActive(true);
        updateAddColors();
        updateSubColors();
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

        //applyChangesBtn.onClick.AddListener(applyChanges);
        //resetChangesBtn.onClick.AddListener(resetChanges);

        hide();

        Debug.Log("STATUPGRDING HAS STARTED");
        //fakeStatDisplay();
    }

    public void applyChanges() {
        //UIManager.singleton.characterInfoScreen.character.PD += PDbuffer;
        //UIManager.singleton.characterInfoScreen.character.MD += MDbuffer;
        //UIManager.singleton.characterInfoScreen.character.AS += ASbuffer;
        //UIManager.singleton.characterInfoScreen.character.CDR += CDRbuffer;
        //UIManager.singleton.characterInfoScreen.character.MS += MSbuffer;
        //UIManager.singleton.characterInfoScreen.character.Range += RNGbuffer;
        //UIManager.singleton.characterInfoScreen.character.LS += LSbuffer;
        //UIManager.singleton.characterInfoScreen.character.HPMax += HPbuffer;
        //UIManager.singleton.characterInfoScreen.character.HP += HPbuffer;

        //UIManager.singleton.characterInfoScreen.character.statPoints -= SPUsedBuffer;

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

        if (UIManager.singleton.characterInfoScreen.character.Range >= 1.5f) {
            UIManager.singleton.characterInfoScreen.character.usesProjectile = true;
        }

        SPUsedBuffer = 0;

        applied = true;
        //saves the changes
        if (SceneManager.GetActiveScene().name == "World") {
            UIManager.singleton.saveWorldSave();
        }
        else
            UIManager.singleton.saveMapSave();
        UIManager.singleton.characterInfoScreen.viewCharacterFullScreen(UIManager.singleton.characterInfoScreen.character);
    }
    //resets changes when backButton is clicked or CloseUI Button Clicked
    public void resetChanges() {
        UIManager.singleton.characterInfoScreen.character.PD -= PDbuffer;
        UIManager.singleton.characterInfoScreen.character.MD -= MDbuffer;
        UIManager.singleton.characterInfoScreen.character.INF -= INFbuffer;
        UIManager.singleton.characterInfoScreen.character.AS -= ASbuffer;
        UIManager.singleton.characterInfoScreen.character.CDR -= CDRbuffer;
        UIManager.singleton.characterInfoScreen.character.MS -= MSbuffer;
        UIManager.singleton.characterInfoScreen.character.Range -= RNGbuffer;
        UIManager.singleton.characterInfoScreen.character.LS -= LSbuffer;
        UIManager.singleton.characterInfoScreen.character.HPMax -= HPbuffer;
        UIManager.singleton.characterInfoScreen.character.HP -= HPbuffer;
        UIManager.singleton.characterInfoScreen.character.statPoints += SPUsedBuffer;

        if (UIManager.singleton.characterInfoScreen.character.Range < 1.5f) {
            UIManager.singleton.characterInfoScreen.character.usesProjectile = false;
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
        if ((UIManager.singleton.characterInfoScreen.character.statPoints) > 0) {
            UIManager.singleton.characterInfoScreen.character.PD += PDAmt;
            PDbuffer += PDAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for PD
    public void OnSubPDButtonClicked() {
        if (PDbuffer > 0) {
            UIManager.singleton.characterInfoScreen.character.PD -= PDAmt;
            PDbuffer -= PDAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for MD
    public void OnAddMDButtonClicked() {
        if ((UIManager.singleton.characterInfoScreen.character.statPoints) > 0) {
            UIManager.singleton.characterInfoScreen.character.MD += MDAmt;
            MDbuffer += MDAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for MD
    public void OnSubMDButtonClicked() {
        if (MDbuffer > 0) {
            UIManager.singleton.characterInfoScreen.character.MD -= MDAmt;
            MDbuffer -= MDAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for INF
    public void OnAddINFButtonClicked() {
        if ((UIManager.singleton.characterInfoScreen.character.statPoints) > 0) {
            UIManager.singleton.characterInfoScreen.character.INF += INFAmt;
            INFbuffer += INFAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for INF
    public void OnSubINFButtonClicked() {
        if (INFbuffer > 0) {
            UIManager.singleton.characterInfoScreen.character.INF -= INFAmt;
            INFbuffer -= INFAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for AS
    public void OnAddASButtonClicked() {
        if ((UIManager.singleton.characterInfoScreen.character.statPoints) > 0) {
            UIManager.singleton.characterInfoScreen.character.AS += ASAmt;
            ASbuffer += ASAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for AS
    public void OnSubASButtonClicked() {
        if (ASbuffer > 0) {
            UIManager.singleton.characterInfoScreen.character.AS -= ASAmt;
            ASbuffer -= ASAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for CDR
    public void OnAddCDRButtonClicked() {
        if ((UIManager.singleton.characterInfoScreen.character.statPoints) > 0) {
            UIManager.singleton.characterInfoScreen.character.CDR += CDRAmt;
            CDRbuffer += CDRAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for CDR
    public void OnSubCDRButtonClicked() {
        if (CDRbuffer > 0) {
            UIManager.singleton.characterInfoScreen.character.CDR -= CDRAmt;
            CDRbuffer -= CDRAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for MS
    public void OnAddMSButtonClicked() {
        if ((UIManager.singleton.characterInfoScreen.character.statPoints) > 0) {
            UIManager.singleton.characterInfoScreen.character.MS += MSAmt;
            MSbuffer += MSAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for MS
    public void OnSubMSButtonClicked() {
        if (MSbuffer > 0) {
            UIManager.singleton.characterInfoScreen.character.MS -= MSAmt;
            MSbuffer -= MSAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for RNG
    public void OnAddRNGButtonClicked() {
        if ((UIManager.singleton.characterInfoScreen.character.statPoints) > 0) {
            UIManager.singleton.characterInfoScreen.character.Range += RNGAmt;
            RNGbuffer += RNGAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for RNG
    public void OnSubRNGButtonClicked() {
        if (RNGbuffer > 0) {
            UIManager.singleton.characterInfoScreen.character.Range -= RNGAmt;
            RNGbuffer -= RNGAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for LS
    public void OnAddLSButtonClicked() {
        if ((UIManager.singleton.characterInfoScreen.character.statPoints) > 0) {
            UIManager.singleton.characterInfoScreen.character.LS += LSAmt;
            LSbuffer += LSAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for LS
    public void OnSubLSButtonClicked() {
        if (LSbuffer > 0) {
            UIManager.singleton.characterInfoScreen.character.LS -= LSAmt;
            LSbuffer -= LSAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    // OnAdd function for HP
    public void OnAddHPButtonClicked() {
        if ((UIManager.singleton.characterInfoScreen.character.statPoints) > 0) {
            UIManager.singleton.characterInfoScreen.character.HP += HPAmt;
            UIManager.singleton.characterInfoScreen.character.HPMax += HPAmt;
            HPbuffer += HPAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints--;
            SPUsedBuffer++;
            updateStatDisplay();
        }
    }

    // OnSub function for HP
    public void OnSubHPButtonClicked() {
        if (HPbuffer > 0) {
            UIManager.singleton.characterInfoScreen.character.HP -= HPAmt;
            UIManager.singleton.characterInfoScreen.character.HPMax -= HPAmt;
            HPbuffer -= HPAmt;
            UIManager.singleton.characterInfoScreen.character.statPoints++;
            SPUsedBuffer--;
            updateStatDisplay();
        }
    }

    #endregion

    private void updateAddColors() {
        //Colors all the add buttons grey if the character has no stat points to spend
        if (UIManager.singleton.characterInfoScreen.character.statPoints == 0) {
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
    }

    //updates visual to display change to be applied
    public void updateStatDisplay() {
        //Debug.Log("Fakse stats uopdated");
        UIManager.singleton.characterInfoScreen.displayStats(UIManager.singleton.characterInfoScreen.character);
        updateAddColors();
        updateSubColors();
    }

    private void Update() {
    }

}
