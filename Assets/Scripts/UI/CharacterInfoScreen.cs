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
    public TextMeshProUGUI DMG, AS, MS, RNG, LS;
    //cool stats texts
    public TextMeshProUGUI totalKills;
    public CharacterHealthBar healthBar;

    public Image characterPortrait;

    //Used to instantiate AbilityDisplay prefab
    public GameObject abilityDisplay;
    //Instantiate abilityDisplay as child of this
    public GameObject abilityDisplayPanel;

    //Selecting target for attacking and also moving for now.
    public AttackTargetSelector targetSelector;

    //character that is currently being viewed
    public Character character;

    public Button openTargetSelectionBtn;

    //Stat point stuff
    [SerializeField]private StatPointUI statPointUI;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelProgress;
    public Image levelBar;

    public int pageIndex = 0;
    //0 landing page
    //1 target selection
    //3 is in inventoryCharacterInfoScreen

    //base page wehn opening charinfoscreen
    public void openLandingPage() {
        close();
        targetSelector.targetSelection.SetActive(false);
        pageIndex = 0;
    }
    public void openTargetSelectionPage() {
        targetSelector.targetSelection.SetActive(true);
        pageIndex = 1;
    }
    public void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        openTargetSelectionBtn.onClick.AddListener(openTargetSelectorNormal);
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
        targetSelector.target.text = TargetNames.getName(currChar.attackTargetStrategy);
        targetSelector.character = currChar;

        character = currChar;

        openLandingPage();
        displayStats(currChar);
        displayCharacterAbilities(currChar);

    }

    public void displayCharacterAbilities(Character currChar) {
        foreach (Ability ability in currChar.abilities) {
            GameObject temp = Instantiate(abilityDisplay);
            //sets the instantiated object as child
            temp.transform.parent = abilityDisplayPanel.transform;
            AbilityDisplay displayTemp = temp.GetComponent<AbilityDisplay>();
            //sets the displays name and description
            displayTemp.abilityName.text = ability.abilityName;
            displayTemp.description.text = ability.description;
            displayTemp.ability = ability;
            displayTemp.targettingStrategyText.text = TargetNames.getName((ability.targetStrategy));
            //sets the icon fill amount to CD remaining
            displayTemp.icon.fillAmount = (ability.CD - ability.abilityNext) / ability.CD;
            //resetting scale to 1 cuz for somereaosn the scale is 167 otherwise
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    //displays the stats and cool stats of the character and character screen

    private void handleColor(Character currChar) {
        if (currChar.DMG > currChar.zsDMG)
            DMG.color = ColorPalette.buff;
        else
        if (currChar.DMG < currChar.zsDMG)
            DMG.color = ColorPalette.debuff;
        else
            DMG.color = ColorPalette.defaultColor;

        if (currChar.AS > currChar.zsAS)
            AS.color = ColorPalette.buff;
        else
        if (currChar.AS < currChar.zsAS)
            AS.color = ColorPalette.debuff;
        else
            AS.color = ColorPalette.defaultColor;

        if (currChar.MS > currChar.zsMS)
            MS.color = ColorPalette.buff;
        else
        if (currChar.MS < currChar.zsMS)
            MS.color = ColorPalette.debuff;
        else
            MS.color = ColorPalette.defaultColor;

        if (currChar.Range > currChar.zsRange)
            RNG.color = ColorPalette.buff;
        else
        if (currChar.Range < currChar.zsRange)
            RNG.color = ColorPalette.debuff;
        else
            RNG.color = ColorPalette.defaultColor;

        if (currChar.Range > currChar.zsRange)
            RNG.color = ColorPalette.buff;
        else
        if (currChar.Range < currChar.zsRange)
            RNG.color = ColorPalette.debuff;
        else
            RNG.color = ColorPalette.defaultColor;

        if (currChar.LS > currChar.zsLS)
            LS.color = ColorPalette.buff;
        else
        if (currChar.LS < currChar.zsLS)
            LS.color = ColorPalette.debuff;
        else
            LS.color = ColorPalette.defaultColor;
    }
    public void displayStats(Character currChar) {
        

        handleColor(currChar);
        //the empty quotes is to convert float to str
        DMG.text = currChar.DMG.ToString("F1");
        AS.text = currChar.AS.ToString("F1");
        MS.text = currChar.MS.ToString("F1");
        RNG.text = currChar.Range.ToString("F1");
        LS.text = currChar.LS.ToString("F1");

        //displays statPoints if zone hasn't started and if the character has statpoints available
        if (currChar.statPoints > 0) {
            Debug.Log("More than 0 stat points");
            statPointUI.gameObject.SetActive(!uiManager.zone.started);
            statPointUI.fakeStatDisplay();
        }

        totalKills.text = currChar.totalKills + "";
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
