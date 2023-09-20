using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HospitalScreen : MonoBehaviour
{
    public TextMeshProUGUI characterName;

    public Transform characterPlayerPartyArea;

    public GameObject characterHospitalDisplayPrefab;
    //stats texts
    public TextMeshProUGUI PD, MD, INF, AS, CDR, MS, RNG, LS, SP;
    public CharacterHealthBar healthBar;

    [SerializeField] private TextMeshProUGUI selectCharacterTxt;

    public Image characterPortrait;
    //character that is currently being viewed
    public Character character;


    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelProgress;
    public Image levelBar;

    public Button heal10PercentBtn;
    public Button reviveOrHealToFullBtn;

    public Button healAndRevivePartyBtn;

    [SerializeField]private Button closeBtn;

    [SerializeField] private float costPerHp;
    [SerializeField] private int costForRevive;

    //this is used since if I use calculate cost then reduce that amount of gold then do calcualteAmount to heal it would be inaccurate since the latter depends on how much gold you have.
    private int costOfClicked;

    //reviving revives the character and gives it 50 hp;


    public void Start() {
        heal10PercentBtn.onClick.AddListener(heal10Percent);
        reviveOrHealToFullBtn.onClick.AddListener(reviveOrHealToFull);
        closeBtn.onClick.AddListener(close);
    }
    private void close() {
        UIManager.singleton.hospitalScreenHidden.hidden = true;
    }
    //Displays "Select a character to Heal" and hides character portrait
    private void displayPlaceHolder() {
        characterPortrait.gameObject.SetActive(false);
        selectCharacterTxt.gameObject.SetActive(true);
    }
    public void hidePlaceHolder() {
        characterPortrait.gameObject.SetActive(true);
        selectCharacterTxt.gameObject.SetActive(false);
    }
    public void setupHospitalScreen() {
        displayPlayerParty();
        displayPlaceHolder();
    }
    private void closeCharactersPlayerParty() {
        foreach (Transform child in characterPlayerPartyArea.transform) {
            if (!child.CompareTag("DontDelete"))
                Destroy(child.gameObject);
        }
    }
    private void displayPlayerParty() {
        //deletes all created instances before recreating to account for dead characters etc..
        closeCharactersPlayerParty();
        //loops through children of playerParty
        foreach (Transform child in UIManager.singleton.playerParty.transform) {
            //Debug.Log(child.name);
            if (child.CompareTag("Character")) {
                Character temp = child.GetComponent<Character>();

                //instantiates a charcaterDisplay
                CharacterDisplayShopHospitalTraining display = Instantiate(characterHospitalDisplayPrefab).GetComponent<CharacterDisplayShopHospitalTraining>();
                display.character = temp;
                //sets this display as a child 
                display.transform.parent = characterPlayerPartyArea.transform;
                //sets the scale for some reason if I dont do this the scale is set to 167
                display.gameObject.transform.localScale = new Vector3(1, 1, 1);

            }
        }
    }
    private int calculateCost(Character characterToHeal,int percentage) {
        return (int)(calculateAmountToHeal(characterToHeal,percentage) * costPerHp);
    }
    
    private float calculateAmountToHeal(Character characterToHeal, int percentage) {
        float chunkOfHP = characterToHeal.HPMax * percentage / 100;
        //if it would overheal calculate the amount to heall till full
        if (chunkOfHP + characterToHeal.HP > characterToHeal.HPMax) {
            chunkOfHP = characterToHeal.HPMax - characterToHeal.HP;
        }
        //if player can't afford to heal the percentage heal as much as you can with the money man has
        if (UIManager.singleton.playerParty.gold < (chunkOfHP) * costPerHp) {
            costOfClicked = (int)((UIManager.singleton.playerParty.gold / costPerHp) * costPerHp);
            return (UIManager.singleton.playerParty.gold / costPerHp);
        }
        else {
            costOfClicked = (int)(chunkOfHP * costPerHp);
            return chunkOfHP;
        }
    }
    public void heal10Percent() {
        if (UIManager.singleton.playerParty.gold >= calculateCost(character,10)) {
            character.HP += calculateAmountToHeal(character, 10);
            UIManager.singleton.playerParty.gold -= costOfClicked;
            updateButtons();
            healthBar.manualDisplayHealth();
        }
        UIManager.singleton.saveMapSave();
        //UIManager.singleton.shopScreen.displayPlayerParty();
    }

    public void reviveOrHealToFull() {
        if (!character.alive && UIManager.singleton.playerParty.gold >= costForRevive) {
            character.alive = true;
            character.HP = 50;
            UIManager.singleton.playerParty.gold -= costForRevive;
        }
        else if(character.alive && UIManager.singleton.playerParty.gold >= calculateCost(character,100)){
            Debug.Log("Healing to full");
            character.HP += calculateAmountToHeal(character,100);
            UIManager.singleton.playerParty.gold -= costOfClicked;
        }
        updateButtons();
        healthBar.manualDisplayHealth();
        UIManager.singleton.saveMapSave();
        //to update display 
        //UIManager.singleton.shopScreen.displayPlayerParty();
    }

    public void updateButtons() {
        //if dead 
        if (!character.alive) {
            heal10PercentBtn.interactable = false;
            heal10PercentBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Man is dead can't heal";

            reviveOrHealToFullBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Revive \n Price:" + costForRevive;
            reviveOrHealToFullBtn.interactable = true;

        }//if not full hp
        else if(character.HP != character.HPMax){
            heal10PercentBtn.interactable = true;
            heal10PercentBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Heal " + calculateAmountToHeal(character,10).ToString("F1") + "\nPrice:" + calculateCost(character,10);

            reviveOrHealToFullBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Heal fully\nPrice:" + calculateCost(character,100);
            reviveOrHealToFullBtn.interactable = true;
        }//if full hp
        else {
            heal10PercentBtn.interactable=false;
            heal10PercentBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Looks Healthy";
            reviveOrHealToFullBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Looks very alive";
            reviveOrHealToFullBtn.interactable = false;
        }

    }
    //this function displays the information in the characterInfoScreen
    public void viewCharacter(Character currChar) {


        //sets the attributes to the character's
        characterName.text = currChar.name;
        //sets the image of character
        characterPortrait.sprite = currChar.GetComponent<SpriteRenderer>().sprite;
        characterPortrait.color = currChar.GetComponent<SpriteRenderer>().color;

        character = currChar;

        healthBar.character = currChar;
        updateButtons();
    }

    //displays the stats and cool stats of the character and character screen

    private void handleColor(Character currChar) {
        if (currChar.PD > currChar.zsPD)
            PD.color = ColorPalette.singleton.buff;
        else
        if (currChar.PD < currChar.zsPD)
            PD.color = ColorPalette.singleton.debuff;
        else
            PD.color = ColorPalette.singleton.defaultColor;
        //PD.color = Color.green;

        if (currChar.MD > currChar.zsMD)
            MD.color = ColorPalette.singleton.buff;
        else
       if (currChar.MD < currChar.zsMD)
            MD.color = ColorPalette.singleton.debuff;
        else
            MD.color = ColorPalette.singleton.defaultColor;

        if (currChar.INF > currChar.zsINF)
            INF.color = ColorPalette.singleton.buff;
        else
       if (currChar.INF < currChar.zsINF)
            INF.color = ColorPalette.singleton.debuff;
        else
            INF.color = ColorPalette.singleton.defaultColor;

        if (currChar.AS > currChar.zsAS)
            AS.color = ColorPalette.singleton.buff;
        else
        if (currChar.AS < currChar.zsAS)
            AS.color = ColorPalette.singleton.debuff;
        else
            AS.color = ColorPalette.singleton.defaultColor;

        if (currChar.CDR > currChar.zsCDR)
            CDR.color = ColorPalette.singleton.buff;
        else
        if (currChar.CDR < currChar.zsCDR)
            CDR.color = ColorPalette.singleton.debuff;
        else
            CDR.color = ColorPalette.singleton.defaultColor;

        if (currChar.MS > currChar.zsMS)
            MS.color = ColorPalette.singleton.buff;
        else
        if (currChar.MS < currChar.zsMS)
            MS.color = ColorPalette.singleton.debuff;
        else
            MS.color = ColorPalette.singleton.defaultColor;

        if (currChar.Range > currChar.zsRange)
            RNG.color = ColorPalette.singleton.buff;
        else
        if (currChar.Range < currChar.zsRange)
            RNG.color = ColorPalette.singleton.debuff;
        else
            RNG.color = ColorPalette.singleton.defaultColor;

        if (currChar.Range > currChar.zsRange)
            RNG.color = ColorPalette.singleton.buff;
        else
        if (currChar.Range < currChar.zsRange)
            RNG.color = ColorPalette.singleton.debuff;
        else
            RNG.color = ColorPalette.singleton.defaultColor;

        if (currChar.LS > currChar.zsLS)
            LS.color = ColorPalette.singleton.buff;
        else
        if (currChar.LS < currChar.zsLS)
            LS.color = ColorPalette.singleton.debuff;
        else
            LS.color = ColorPalette.singleton.defaultColor;

        //colors the healthbar according to team
        switch (character.team) {
            case ((int)Character.teamList.Enemy1):
                healthBar.health.color = ColorPalette.singleton.enemyHealthBar;
                break;
            case ((int)Character.teamList.Player):
                healthBar.health.color = ColorPalette.singleton.allyHealthBar;
                break;
            default:
                break;
        }
    }
    public void displayStats(Character currChar) {


        handleColor(currChar);
        //the empty quotes is to convert float to str
        PD.text = currChar.PD.ToString("F1");
        MD.text = currChar.MD.ToString("F1");
        INF.text = currChar.INF.ToString("F1");
        AS.text = currChar.AS.ToString("F1");
        CDR.text = (currChar.CDR * 100).ToString("F1");
        MS.text = currChar.MS.ToString("F1");
        RNG.text = currChar.Range.ToString("F1");
        LS.text = (currChar.LS * 100).ToString("F1");


        //fills the HP bar correctly
        healthBar.character = currChar;

        levelText.text = "LVL: " + currChar.level;
        levelBar.fillAmount = (float)currChar.xpProgress / currChar.xpCap;
        levelProgress.text = currChar.xpProgress + "/" + currChar.xpCap;
    }


}
