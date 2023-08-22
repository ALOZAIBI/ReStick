using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectArchetype : MonoBehaviour {
    //The archetypes are children of this
    public List<Archetype> archetypeList = new List<Archetype>();

    //Hold the chances of each archetype being picked. (The value changes based on the character's upgrade stats)
    public List<int> pickChance = new List<int>();

    public Character character;

    public Character baseCharacter;

    //Holds the maximum 3 pickChance archetypes
    public List<int> selectedArchetypes = new List<int>(new int[3]);

    public int PWR;
    public int MGC;
    public int INF;
    public int HP;
    public int AS;
    public int CDR;
    public int SPD;
    public int Range;
    public int LS;

    public void setupAndView(Character character) {
        setupArchetypes(character);
        viewArchetypes();
    }
    private void setupArchetypes(Character character) {
        //set archetypeList from archetypeHolder
        archetypeList.Clear();
        for (int i = 0; i < UIManager.singleton.archetypeList.childCount; i++) {
            archetypeList.Add(UIManager.singleton.archetypeList.GetChild(i).GetComponent<Archetype>());
        }
        baseCharacter = UIManager.singleton.characterFactory.transform.GetChild(0).GetComponent<Character>();
        pickChance.Clear();
        //Make pickChance same size as archetypeLiost
        for (int i = 0; i < archetypeList.Count; i++) {
            pickChance.Add(0);
        }
        errorCheck();

        getStatsUpgraded(character);
        setPickChance();
        randomizePickChance(6);
        saveMaximumPickChance();
    }
    private void errorCheck() {
        //Checks if the sum of values of each archetype is more than 10, if so there is an error
        foreach (Archetype archetype in archetypeList) {
            if (archetype.PWR + archetype.MGC + archetype.INF + archetype.HP + archetype.AS + archetype.CDR + archetype.SPD + archetype.Range*0 + archetype.LS > 10) {
                Debug.LogError("Archetype " + archetype.archetypeName + " has more than 10 points in total");
            }
        }
    }
    //Checks which stats were upgraded in character and how many times.
    private void getStatsUpgraded(Character character) {
        this.character = character;
        //Compares to base character to find out how many times each stat was upgraded
        PWR = (int)((character.PD - baseCharacter.PD) / UIManager.singleton.characterInfoScreen.statUpgrading.PDAmt);
        MGC = (int)((character.MD - baseCharacter.MD) / UIManager.singleton.characterInfoScreen.statUpgrading.MDAmt);
        INF = (int)((character.INF - baseCharacter.INF) / UIManager.singleton.characterInfoScreen.statUpgrading.INFAmt);
        //We did this to HP since HP increases per level as well
        HP = (int)(((character.HPMax - 12 * character.level) - baseCharacter.HPMax + 12) / UIManager.singleton.characterInfoScreen.statUpgrading.HPAmt);
        AS = (int)((character.AS - baseCharacter.AS) / UIManager.singleton.characterInfoScreen.statUpgrading.ASAmt);
        CDR = (int)((character.CDR - baseCharacter.CDR) / UIManager.singleton.characterInfoScreen.statUpgrading.CDRAmt);
        SPD = (int)((character.MS - baseCharacter.MS) / UIManager.singleton.characterInfoScreen.statUpgrading.MSAmt);
        Range = (int)((character.Range - baseCharacter.Range) / UIManager.singleton.characterInfoScreen.statUpgrading.RNGAmt);
        LS = (int)((character.LS - baseCharacter.LS) / UIManager.singleton.characterInfoScreen.statUpgrading.LSAmt);


    }

    //Sets the pick chance for each archetype
    private void setPickChance() {

        for (int i = 0; i < archetypeList.Count; i++) {
            pickChance[i] = 0;
        }
        //Sets the pickChance for each archetype
        for (int i = 0; i < archetypeList.Count; i++) {
            //increase pick chance for each stat if it is non negative.(Sometimes it would be negative like the case of BigNStrong
            pickChance[i] += archetypeList[i].PWR > 0 ? archetypeList[i].PWR * PWR : archetypeList[i].PWR;
            pickChance[i] += archetypeList[i].MGC > 0 ? archetypeList[i].MGC * MGC : archetypeList[i].MGC;
            pickChance[i] += archetypeList[i].INF > 0 ? archetypeList[i].INF * INF : archetypeList[i].INF;
            pickChance[i] += archetypeList[i].HP > 0 ? archetypeList[i].HP * HP : archetypeList[i].HP;
            pickChance[i] += archetypeList[i].AS > 0 ? archetypeList[i].AS * AS : archetypeList[i].AS;
            pickChance[i] += archetypeList[i].CDR > 0 ? archetypeList[i].CDR * CDR : archetypeList[i].CDR;
            pickChance[i] += archetypeList[i].SPD > 0 ? archetypeList[i].SPD * SPD : archetypeList[i].SPD;
            pickChance[i] += archetypeList[i].Range > 0 ? archetypeList[i].Range / 3 * Range : archetypeList[i].Range;
            pickChance[i] += archetypeList[i].LS > 0 ? archetypeList[i].LS * LS : archetypeList[i].LS;
        }
    }

    private void randomizePickChance(int n) {
        //Increase or decrease the pickChance of the non maximum by Random.Range(-n, n)
        for (int i = 0; i < pickChance.Count; i++) {
            pickChance[i] += Random.Range(-n, n);
        }
    }
    private void saveMaximumPickChance() {
        //Sets selectedArchetypes to -1 to signify that it hasn't been set yet
        for (int i = 0; i < 3; i++) {
            selectedArchetypes[i] = -1;
        }
        //Saves the maximum 3 pickChance archetypes to selectedArchetypes
        for (int i = 0; i < 3; i++) {
            int maxIndex = 0;
            int max = 0;
            for (int j = 0; j < pickChance.Count; j++) {
                //If there's a new maximum and it hasn't been selected yet, set it as the new maximum
                if (pickChance[j] > max && !selectedArchetypes.Contains(j)) {
                    max = pickChance[j];
                    maxIndex = j;
                }
            }
            selectedArchetypes[i] = maxIndex;
        }

    }
    #region UIStuff

    [SerializeField] private List<Image> portraits = new List<Image>(new Image[3]);

    [SerializeField] private List<TextMeshProUGUI> archetypeNames = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);

    [SerializeField] private List<TextMeshProUGUI> PWRText = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> MGCText = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> INFText = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> HPText = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> ASText = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> CDRText = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> SPDText = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> RangeText = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> LSText = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);

    [SerializeField] private List<StatIcon> PWRIcons = new List<StatIcon>(new StatIcon[3]);
    [SerializeField] private List<StatIcon> MGCIcons = new List<StatIcon>(new StatIcon[3]);
    [SerializeField] private List<StatIcon> INFIcons = new List<StatIcon>(new StatIcon[3]);
    [SerializeField] private List<StatIcon> HPIcons = new List<StatIcon>(new StatIcon[3]);
    [SerializeField] private List<StatIcon> ASIcons = new List<StatIcon>(new StatIcon[3]);
    [SerializeField] private List<StatIcon> CDRIcons = new List<StatIcon>(new StatIcon[3]);
    [SerializeField] private List<StatIcon> SPDIcons = new List<StatIcon>(new StatIcon[3]);
    [SerializeField] private List<StatIcon> RangeIcons = new List<StatIcon>(new StatIcon[3]);
    [SerializeField] private List<StatIcon> LSIcons = new List<StatIcon>(new StatIcon[3]);

    [SerializeField] private List<TextMeshProUGUI> PWRAmt = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> MGCAmt = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> INFAmt = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> HPAmt = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> ASAmt = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> CDRAmt = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> SPDAmt = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> RangeAmt = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);
    [SerializeField] private List<TextMeshProUGUI> LSAmt = new List<TextMeshProUGUI>(new TextMeshProUGUI[3]);

    public CharacterInfoScreen characterInfoScreen;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button confirmBtn;

    //To indicate that nothing has been selected
    private int selected = -1;

    //Listener is set in the inspector
    public void cancel() {
        characterInfoScreen.startUnfocusing();
    }

    public void confirm() {
        applyArchetype();
        character.hasArchetype = true;
        cancel();
    }

    private void applyArchetype() {
        character.prefabIndex = archetypeList[selectedArchetypes[selected]].prefabIndex;
        character.PD += archetypeList[selectedArchetypes[selected]].PWR*UIManager.singleton.characterInfoScreen.statUpgrading.PDAmt;
        character.MD += archetypeList[selectedArchetypes[selected]].MGC*UIManager.singleton.characterInfoScreen.statUpgrading.MDAmt;
        character.INF += archetypeList[selectedArchetypes[selected]].INF*UIManager.singleton.characterInfoScreen.statUpgrading.INFAmt;
        character.HP += archetypeList[selectedArchetypes[selected]].HP*UIManager.singleton.characterInfoScreen.statUpgrading.HPAmt;
        character.HPMax += archetypeList[selectedArchetypes[selected]].HP * UIManager.singleton.characterInfoScreen.statUpgrading.HPAmt;
        character.AS += archetypeList[selectedArchetypes[selected]].AS*UIManager.singleton.characterInfoScreen.statUpgrading.ASAmt;
        character.CDR += archetypeList[selectedArchetypes[selected]].CDR*UIManager.singleton.characterInfoScreen.statUpgrading.CDRAmt;
        character.MS += archetypeList[selectedArchetypes[selected]].SPD*UIManager.singleton.characterInfoScreen.statUpgrading.MSAmt;
        character.Range += archetypeList[selectedArchetypes[selected]].Range*UIManager.singleton.characterInfoScreen.statUpgrading.RNGAmt;
        character.LS += archetypeList[selectedArchetypes[selected]].LS*UIManager.singleton.characterInfoScreen.statUpgrading.LSAmt;

        //saves the archetype
        if (SceneManager.GetActiveScene().name == "World") {
            UIManager.singleton.saveWorldSave();
        }
        else
            UIManager.singleton.saveMapSave();

        ////Loads the characters so that it applies the new prefabIndex
        //if (SceneManager.GetActiveScene().name == "World") {
        //    UIManager.singleton.loadWorldSave();
        //}
        //else
        //    UIManager.singleton.loadMapSave();
    }

    public void selectArchetype1() {
        selected = 0;
        confirmBtn.interactable = true;
    }
    public void selectArchetype2() {
        selected = 1;
        confirmBtn.interactable = true;
    }
    public void selectArchetype3() {
        selected = 2;
        confirmBtn.interactable = true;
    }

    private void viewArchetypes() {
        selected = -1;
        confirmBtn.interactable = false;
        viewNames();
        viewPortraits();
        viewStats();
    }
    private void viewNames() {
        archetypeNames[0].text = archetypeList[selectedArchetypes[0]].archetypeName;
        archetypeNames[1].text = archetypeList[selectedArchetypes[1]].archetypeName;
        archetypeNames[2].text = archetypeList[selectedArchetypes[2]].archetypeName;
    }

    private void viewPortraits() {
        portraits[0].sprite = UIManager.singleton.characterFactory.transform.GetChild(archetypeList[selectedArchetypes[0]].prefabIndex).GetComponent<SpriteRenderer>().sprite;
        portraits[1].sprite = UIManager.singleton.characterFactory.transform.GetChild(archetypeList[selectedArchetypes[1]].prefabIndex).GetComponent<SpriteRenderer>().sprite;
        portraits[2].sprite = UIManager.singleton.characterFactory.transform.GetChild(archetypeList[selectedArchetypes[2]].prefabIndex).GetComponent<SpriteRenderer>().sprite;
    }
    private void viewStats() {
        //Enables and disables the stat stuff according to wether or not the archetype has that stat
        for(int i = 0; i < 3; i++) {
            if (archetypeList[selectedArchetypes[i]].PWR == 0) {
                PWRText[i].gameObject.SetActive(false);
                PWRIcons[i].gameObject.SetActive(false);
                PWRAmt[i].gameObject.SetActive(false);
            }
            else {
                PWRText[i].gameObject.SetActive(true);
                PWRIcons[i].gameObject.SetActive(true);
                PWRAmt[i].gameObject.SetActive(true);
                amtToSign(PWRAmt[i],archetypeList[selectedArchetypes[i]].PWR);
            }

            if (archetypeList[selectedArchetypes[i]].MGC == 0) {
                MGCText[i].gameObject.SetActive(false);
                MGCIcons[i].gameObject.SetActive(false);
                MGCAmt[i].gameObject.SetActive(false);
            }
            else {
                MGCText[i].gameObject.SetActive(true);
                MGCIcons[i].gameObject.SetActive(true);
                MGCAmt[i].gameObject.SetActive(true);
                amtToSign(MGCAmt[i], archetypeList[selectedArchetypes[i]].MGC);
            }

            if (archetypeList[selectedArchetypes[i]].INF == 0) {
                INFText[i].gameObject.SetActive(false);
                INFIcons[i].gameObject.SetActive(false);
                INFAmt[i].gameObject.SetActive(false);
            }
            else {
                INFText[i].gameObject.SetActive(true);
                INFIcons[i].gameObject.SetActive(true);
                INFAmt[i].gameObject.SetActive(true);
                amtToSign(INFAmt[i], archetypeList[selectedArchetypes[i]].INF);
            }

            if (archetypeList[selectedArchetypes[i]].HP == 0) {
                HPText[i].gameObject.SetActive(false);
                HPIcons[i].gameObject.SetActive(false);
                HPAmt[i].gameObject.SetActive(false);
            }
            else {
                HPText[i].gameObject.SetActive(true);
                HPIcons[i].gameObject.SetActive(true);
                HPAmt[i].gameObject.SetActive(true);
                amtToSign(HPAmt[i], archetypeList[selectedArchetypes[i]].HP);
            }

            if (archetypeList[selectedArchetypes[i]].AS == 0) {
                ASText[i].gameObject.SetActive(false);
                ASIcons[i].gameObject.SetActive(false);
                ASAmt[i].gameObject.SetActive(false);
            }
            else {
                ASText[i].gameObject.SetActive(true);
                ASIcons[i].gameObject.SetActive(true);
                ASAmt[i].gameObject.SetActive(true);
                amtToSign(ASAmt[i], archetypeList[selectedArchetypes[i]].AS);
            }

            if (archetypeList[selectedArchetypes[i]].CDR == 0) {
                CDRText[i].gameObject.SetActive(false);
                CDRIcons[i].gameObject.SetActive(false);
                CDRAmt[i].gameObject.SetActive(false);
            }
            else {
                CDRText[i].gameObject.SetActive(true);
                CDRIcons[i].gameObject.SetActive(true);
                CDRAmt[i].gameObject.SetActive(true);
                amtToSign(CDRAmt[i], archetypeList[selectedArchetypes[i]].CDR);
            }

            if (archetypeList[selectedArchetypes[i]].SPD == 0) {
                SPDText[i].gameObject.SetActive(false);
                SPDIcons[i].gameObject.SetActive(false);
                SPDAmt[i].gameObject.SetActive(false);
            }
            else {
                SPDText[i].gameObject.SetActive(true);
                SPDIcons[i].gameObject.SetActive(true);
                SPDAmt[i].gameObject.SetActive(true);
                amtToSign(SPDAmt[i], archetypeList[selectedArchetypes[i]].SPD);
            }

            if (archetypeList[selectedArchetypes[i]].Range == 0) {
                RangeText[i].gameObject.SetActive(false);
                RangeIcons[i].gameObject.SetActive(false);
                RangeAmt[i].gameObject.SetActive(false);
            }
            else {
                RangeText[i].gameObject.SetActive(true);
                RangeIcons[i].gameObject.SetActive(true);
                RangeAmt[i].gameObject.SetActive(true);
                amtToSign(RangeAmt[i], Mathf.CeilToInt(archetypeList[selectedArchetypes[i]].Range/2));
            }

            if (archetypeList[selectedArchetypes[i]].LS == 0) {
                LSText[i].gameObject.SetActive(false);
                LSIcons[i].gameObject.SetActive(false);
                LSAmt[i].gameObject.SetActive(false);
            }
            else {
                LSText[i].gameObject.SetActive(true);
                LSIcons[i].gameObject.SetActive(true);
                LSAmt[i].gameObject.SetActive(true);
                amtToSign(LSAmt[i], archetypeList[selectedArchetypes[i]].LS);
            }
        }
    }

    private void amtToSign(TextMeshProUGUI text,int amount) {
        string sign = "";
        if (amount > 0) {
            for (int i = 0; i < amount / 2; i++) {
                sign += "+ ";
                text.color = ColorPalette.singleton.buff;
            }
        }
        else
            for(int i = 0; i < (amount*-1) / 2; i++) {
                sign += "- ";
                text.color = ColorPalette.singleton.debuff;
            }

        text.text = sign;
    }

    #endregion
}
