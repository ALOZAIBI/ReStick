using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoScreen : MonoBehaviour
{
    public UIManager uiManager;
    public TextMeshProUGUI characterName;
    //stats texts
    public TextMeshProUGUI PD, MD, INF, AS, CDR, MS, RNG, LS,SP;
    //cool stats texts
    public TextMeshProUGUI totalKills;
    public TextMeshProUGUI totalDamage;
    public CharacterHealthBar healthBar;

    public Image characterPortrait;

    //Used to instantiate AbilityDisplay prefab
    public GameObject abilityDisplay;
    //Instantiate abilityDisplay as child of this
    public GameObject abilityDisplayPanel;

    //Selecting target for attacking and also moving for now.
    public AttackTargetSelector targetSelector;

    public Button openTargetSelectionBtn;

    public TextMeshProUGUI openTargetSelectionTxt;

    //Selecting target for attacking and also moving for now.
    public MovementStrategySelector movementSelector;

    public Button openMovementSelectorBtn;

    public TextMeshProUGUI openMovementSelectorTxt;


    //character that is currently being viewed
    public Character character;


    //Stat point stuff
    [SerializeField]public StatPointUI statPointUI;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelProgress;
    public Image levelBar;

    public GameObject footer;

    public int pageIndex = 0;
    //0 landing page
    //1 target selection
    //3 is in inventoryCharacterInfoScreen

    //base page wehn opening charinfoscreen
    public void openLandingPage() {
        close();
        targetSelector.targetSelection.SetActive(false);
        movementSelector.gameObject.SetActive(false);
        footer.SetActive(true);
        pageIndex = 0;
    }
    public void openTargetSelectionPage() {
        close();
        targetSelector.targetSelection.SetActive(true);
        targetSelector.updateView();
        footer.SetActive(false);
        pageIndex = 1;
    }

    public void openMovementSelectorPage() {
        if (uiManager.zone == null || uiManager.zone.started == false && character.team == (int)Character.teamList.Player) {
            close();
            movementSelector.gameObject.SetActive(true);
            footer.SetActive(false);
            pageIndex = 1;
            movementSelector.updateText();
        }
    }
    public void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        openTargetSelectionBtn.onClick.AddListener(openTargetSelectorNormal);
        openMovementSelectorBtn.onClick.AddListener(openMovementSelectorPage);
    }
    //this function displays the information in the characterInfoScreen
    public void viewCharacter(Character currChar) {
        
        
        //sets the attributes to the character's
        characterName.text = currChar.name;
        //sets the image of character
        characterPortrait.sprite = currChar.GetComponent<SpriteRenderer>().sprite;
        characterPortrait.color = currChar.GetComponent<SpriteRenderer>().color;
        //Debug.Log("Is this causing bug?:" + GetComponent<TargetNames>().getName(currChar.attackTargetStrategy));
        //sets the text of the targetting
        openTargetSelectionTxt.text = TargetNames.getName(currChar.attackTargetStrategy);
        openMovementSelectorTxt.text = MovementNames.getName(currChar.movementStrategy);
        targetSelector.character = currChar;
        movementSelector.character = currChar;
        character = currChar;

        //Tells the abilities that this owns them
        foreach (Ability temp in currChar.abilities) {
            temp.character = currChar;
        }

        openLandingPage();
        displayStats(currChar);
        displayCharacterAbilities(currChar);
        healthBar.manualDisplayHealth();
    }

    public void displayCharacterAbilities(Character currChar) {
        close();
        foreach (Ability ability in currChar.abilities) {
            //updates description
            ability.updateDescription();
            GameObject temp = Instantiate(abilityDisplay);
            //sets the instantiated object as child
            temp.transform.parent = abilityDisplayPanel.transform;
            AbilityDisplay displayTemp = temp.GetComponent<AbilityDisplay>();
            //sets the displays name and description
            displayTemp.abilityName.text = ability.abilityName;
            displayTemp.description.text = ability.description;
            displayTemp.ability = ability;
            //if ability has no target hide the target button
            if (!ability.hasTarget) {
                displayTemp.btn.gameObject.SetActive(false);
            }
            else
                displayTemp.targettingStrategyText.text = TargetNames.getName((ability.targetStrategy));
            //sets the cooldownBar fill amount to CD remaining
            displayTemp.cooldownBar.fillAmount = (ability.CD - ability.abilityNext) / ability.CD;
            //if the ability has no cd anyways(It's a passive)
            if (ability.CD == 0)
                displayTemp.cooldownText.text = ("Ready");
            else
            //if the ability is ready
            if (ability.abilityNext == 0) 
                displayTemp.cooldownText.text = ("Ready "+ability.displayCDAfterChange()+" CD");
            else
            //shows how much cd remaining 
            displayTemp.cooldownText.text = (ability.abilityNext).ToString("F1");
            //resetting scale to 1 cuz for somereaosn the scale is 167 otherwise
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
        if(currChar.abilities.Count > 3) {
            abilityDisplayPanel.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
        }
        else
            abilityDisplayPanel.GetComponent<VerticalLayoutGroup>().childControlHeight = false;
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
        CDR.text = (currChar.CDR*100).ToString("F1");
        MS.text = currChar.MS.ToString("F1");
        RNG.text = currChar.Range.ToString("F1");
        LS.text = (currChar.LS*100).ToString("F1");
        SP.text = currChar.statPoints.ToString();

        
        //displays statPoints if zone hasn't started and if the character has statpoints available
        if ((currChar.statPoints + statPointUI.SPUsedBuffer)>0 && !uiManager.zoneStarted()) {
            //Debug.Log("showing");
            statPointUI.applied = false;
            statPointUI.show();
        }
        else {
                statPointUI.hide();
        }

        statPointUI.lastUsedCharacter = currChar;

        totalKills.text = currChar.totalKills + "";
        totalDamage.text = currChar.totalDamage.ToString("F0");
        //fills the HP bar correctly
        healthBar.character = currChar;

        //
        levelText.text = "LVL: "+currChar.level;
        levelBar.fillAmount = (float)currChar.xpProgress / currChar.xpCap;
        levelProgress.text = currChar.xpProgress + "/"+currChar.xpCap;
    }

    public void close() {
        //destroys all ability displays
        foreach (Transform toDestroy in abilityDisplayPanel.transform) {
            GameObject.Destroy(toDestroy.gameObject);
        }
    }

    
    //opens target selector for normal attacks
    public void openTargetSelectorNormal() {
        Debug.Log("OPen target selector");
        if (uiManager.zone == null || uiManager.zone.started == false && character.team == (int)Character.teamList.Player) {
            //to indicate that it isnt for an ability
            targetSelector.isAbilityTargetSelector = false;
            openTargetSelectionPage();
        }
    }

    //this function is called in abilityDisplay
    public void openTargetSelectorAbility() {                   //change back to false. only true for testingf purposes
        if (uiManager.zone == null || uiManager.zone.started == false && character.team == (int)Character.teamList.Player) {
            //to indicate that it is for an ability
            targetSelector.isAbilityTargetSelector = true;
            openTargetSelectionPage();
        }
    }


}
