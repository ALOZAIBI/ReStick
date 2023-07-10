using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    public Button targetSelectionBtn;

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


    //this will be glowing to display that there are stat points available
    public Image upgradeStats;
    public Color upgradeStatsColorPingPong1;
    public Color upgradeStatsColorPingPong2;

    public int pageIndex = -1;
    //-1 is topstatdisplay
    //0 landing page
    //1 target selection

    //to be instantiated this is a different gameobject than the regular abilityDisplay because this one will have diff color
    public GameObject inventoryAbilityDisplay;

    
    public Button addAbilityBtn;
    public Button confirmAddAbilityBtn;
    public Image confirmAddAbilityBtnImage;

    const int MAX_ABILITIES = 5;
    //pageindex 3 = prompt to add ability
    //pageindex 4 = confirm ability adding
    //base page wehn opening charinfoscreen

    private HideUI hideUI;
    //used for initial full screen opening
    [SerializeField] private bool opening;
    private bool closing;

    private bool opened;

    ////Once it is fullscreen it will open the other panels such as abilities/ targetting / upgrading
    //private bool opening2;
    //private bool closing2;

    private bool opened2;
    //this is the main panel itself and maybe I will add another button for clarity later
    [SerializeField] private Button openFullScreenBtn;
    [SerializeField] private Button closeFullScreenBtn;

    [SerializeField] private RectTransform mainPanel;
    //the init position and anchors
    private float mainPanelAnchorL;
    private float mainPanelAnchorR;
    private float mainPanelAnchorT;
    private float mainPanelAnchorB;

    private float mainPanelPositionL;
    private float mainPanelPositionR;
    private float mainPanelPositionT;
    private float mainPanelPositionB;

    [SerializeField] private RectTransform statsPanel;
    private float statsPanelAnchorL;
    private float statsPanelAnchorR;
    private float statsPanelAnchorT;
    private float statsPanelAnchorB;
                  
    private float statsPanelPositionL;
    private float statsPanelPositionR;
    private float statsPanelPositionT;
    private float statsPanelPositionB;

    [SerializeField] private RectTransform portraitPanel;

    private float portraitPanelAnchorL;
    private float portraitPanelAnchorR;
    private float portraitPanelAnchorT;
    private float portraitPanelAnchorB;
                  
    private float portraitPanelPositionL;
    private float portraitPanelPositionR;
    private float portraitPanelPositionT;
    private float portraitPanelPositionB;

    [SerializeField] private RectTransform healthBarPanel;

    private RectTransform targetSelectBtnPanel;

    [SerializeField] private RectTransform abilityPanel;
    [SerializeField] private float transitionTime;
    [SerializeField] private float time;
    [SerializeField] private float time2;


    public void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        //just so that it doesn't transition on start.
        time = transitionTime;

        hideUI = GetComponent<HideUI>();

        saveInitPanels();
        
        targetSelectBtnPanel = targetSelectionBtn.GetComponent<RectTransform>();

        openFullScreenBtn.onClick.AddListener(startOpening);
        closeFullScreenBtn.onClick.AddListener(startClosing);

        targetSelectionBtn.onClick.AddListener(openTargetSelectorNormal);
        openMovementSelectorBtn.onClick.AddListener(openMovementSelectorPage);
        upgradeStatsColorPingPong1 = new Color(upgradeStats.color.r * .5f, upgradeStats.color.g * .5f, upgradeStats.color.b * .5f, .8f);
        upgradeStatsColorPingPong2 = new Color(upgradeStats.color.r * 1.2f, upgradeStats.color.g * 1.2f, upgradeStats.color.b * 1.2f, 1);
        addAbilityBtn.onClick.AddListener(addAbility);
        confirmAddAbilityBtn.onClick.AddListener(confirmAddAbility);
        confirmAddAbilityBtnImage = confirmAddAbilityBtn.GetComponent<Image>();
    }
    private void saveInitPanels() {
        mainPanelAnchorL = mainPanel.GetAnchorLeft();
        mainPanelAnchorR = mainPanel.GetAnchorRight();
        mainPanelAnchorT = mainPanel.GetAnchorTop();
        mainPanelAnchorB = mainPanel.GetAnchorBottom();

        mainPanelPositionL = mainPanel.GetLeft();
        mainPanelPositionR = mainPanel.GetRight();
        mainPanelPositionT = mainPanel.GetTop();
        mainPanelPositionB = mainPanel.GetBottom();

        statsPanelAnchorL = statsPanel.GetAnchorLeft();
        statsPanelAnchorR = statsPanel.GetAnchorRight();
        statsPanelAnchorT = statsPanel.GetAnchorTop();
        statsPanelAnchorB = statsPanel.GetAnchorBottom();

        statsPanelPositionL = statsPanel.GetLeft();
        statsPanelPositionR = statsPanel.GetRight();
        statsPanelPositionT = statsPanel.GetTop();
        statsPanelPositionB = statsPanel.GetBottom();

        portraitPanelAnchorL = portraitPanel.GetAnchorLeft();
        portraitPanelAnchorR = portraitPanel.GetAnchorRight();
        portraitPanelAnchorT = portraitPanel.GetAnchorTop();
        portraitPanelAnchorB = portraitPanel.GetAnchorBottom();

        portraitPanelPositionL = portraitPanel.GetLeft();
        portraitPanelPositionR = portraitPanel.GetRight();
        portraitPanelPositionT = portraitPanel.GetTop();
        portraitPanelPositionB = portraitPanel.GetBottom();
    }
    #region movingUIElementsNStuff
    private void startOpening() {
        if (!opened) {
            closeFullScreenBtn.gameObject.SetActive(true);
            opened = true;
            //resets time so that the transition can start.
            opening = true;
            closing = false;
            time = 0;
        }
    }
    private void startClosing() {
        if (opened) {
            closeFullScreenBtn.gameObject.SetActive(false);
            opened = false;
            time = transitionTime;
            opening = false;
            closing = true;
        }
    }

    private void handleMainPanel() {
        //stretches right anchor to be as far from edge as left anchor is from edge same with bottom
        mainPanel.SetAnchorRight(Mathf.Lerp(mainPanelAnchorR, 1 - mainPanelAnchorL, time/transitionTime));
        mainPanel.SetAnchorBottom(Mathf.Lerp(mainPanelAnchorB, 1 - mainPanelAnchorT, time/transitionTime));
    }

    private float portraitScaleAmount=2;
    private void handlePortraitPanel() {
        //just scales it up
        scalePortraitPanel(portraitScaleAmount);
    }
    
    private void scalePortraitPanel(float amount) {
        portraitPanel.SetRight(Mathf.Lerp(portraitPanelPositionR, portraitPanelPositionR * amount, time/transitionTime));
        portraitPanel.SetBottom(Mathf.Lerp(portraitPanelPositionB, portraitPanelPositionB * amount, time/transitionTime));
    }
    private void handleStatsPanel() {
        //The comments are in the case of expanding the panel but they are also applicable in reverse I guess
        //Puts it below Portrait Panel
        //by finding the scale amount we can find the position of the initial bottom anchor and then set the top to thatposition
        float scaleAmount = (mainPanel.GetAnchorTop() - mainPanel.GetAnchorBottom())/(mainPanelAnchorT-mainPanelAnchorB);
        statsPanel.SetAnchorTop(Mathf.Lerp(statsPanelAnchorT, statsPanelAnchorB*scaleAmount - statsPanelAnchorT, time / transitionTime));
        
        //Stretches it to the left
        statsPanel.SetAnchorLeft(Mathf.Lerp(statsPanelAnchorL, 1 - statsPanelAnchorR, time / transitionTime));

        //sets the bottom to be in the middle of main panel
        statsPanel.SetAnchorBottom(Mathf.Lerp(statsPanelAnchorB, (mainPanel.GetAnchorTop() - mainPanel.GetAnchorBottom())/2, time / transitionTime));

    }

    private void handleHealthBarPanel() {
        //Keep left anchor and right anchor equal to statsPanel
        healthBarPanel.SetAnchorLeft(statsPanel.GetAnchorLeft());
        healthBarPanel.SetAnchorRight(statsPanel.GetAnchorRight());
        //Keep top anchor and bottom anchor on stats panel's bottom anchor
        healthBarPanel.SetAnchorTop(statsPanel.GetAnchorBottom());
        //just to slightly thicken it
        healthBarPanel.SetAnchorBottom(statsPanel.GetAnchorBottom()-statsPanel.GetAnchorBottom()*0.01f);
    }
    private void handlePanels() {
        //this is needed to update stats text position and size as we expand and shrink the panel
        mainPanel.gameObject.RefreshLayoutGroupsImmediateAndRecursive();
        handleMainPanel();
        handleStatsPanel();
        handlePortraitPanel();
        handleTargetSelectorBtnPanel();
        handleHealthBarPanel();
        hideUI.setInitPos();
        
    }

    private void handleTargetSelectorBtnPanel() {
        //Sets the bottom to be the same as portrait Panel. And the top to be in the middle of portrait panel
        targetSelectBtnPanel.SetBottom(portraitPanel.GetBottom());
        targetSelectBtnPanel.SetTop((portraitPanel.GetTop() - portraitPanel.GetBottom()) / 2);
        //sets the left anchor to be to the right of the portrait panel.
        targetSelectBtnPanel.SetAnchorLeft(statsPanelAnchorL*portraitScaleAmount);
        //Grows the right anchor to the right
        targetSelectBtnPanel.SetAnchorRight(Mathf.Lerp(targetSelectBtnPanel.GetAnchorLeft(), statsPanelAnchorR,time/transitionTime));
    }
    public void openTopStatDisplay() {
        close();
        pageIndex = -1;
    }
    public void openLandingPage() {
        close();
        pageIndex = 0;
    }
    public void openTargetSelectionPage() {
        close();
        targetSelector.targetSelection.SetActive(true);
        targetSelector.updateView();
        pageIndex = 1;
    }

    public void openMovementSelectorPage() {
        if (uiManager.zone == null || uiManager.zone.started == false && character.team == (int)Character.teamList.Player) {
            close();
            movementSelector.gameObject.SetActive(true);
            pageIndex = 1;
            movementSelector.updateText();
        }
    }
    #endregion
    //this function displays the information in the characterInfoScreen

    #region displayCharacterInfo
    public void viewCharacterFullScreen(Character currChar) {
        
        
        ////sets the attributes to the character's
        //characterName.text = currChar.name;
        ////sets the image of character
        //characterPortrait.sprite = currChar.GetComponent<SpriteRenderer>().sprite;
        //characterPortrait.color = currChar.GetComponent<SpriteRenderer>().color;
        ////Debug.Log("Is this causing bug?:" + GetComponent<TargetNames>().getName(currChar.attackTargetStrategy));
        ////sets the text of the targetting
        //openTargetSelectionTxt.text = TargetNames.getName(currChar.attackTargetStrategy);
        //openMovementSelectorTxt.text = MovementNames.getName(currChar.movementStrategy);
        //targetSelector.character = currChar;
        //movementSelector.character = currChar;
        //character = currChar;

        ////Tells the abilities that this owns them
        //foreach (Ability temp in currChar.abilities) {
        //    temp.character = currChar;
        //}
        displayNameAndPortrait(currChar);
        openLandingPage();
        displayStats(currChar);
        displayCharacterAbilities(currChar);
        healthBar.manualDisplayHealth();
    }

    public void viewCharacterTopStatDisplay(Character currChar) {
        displayNameAndPortrait(currChar);
        displayStats(currChar);
    }

    private void displayNameAndPortrait(Character currChar) {
        characterName.text = currChar.name;
        //sets the image of character
        characterPortrait.sprite = currChar.GetComponent<SpriteRenderer>().sprite;
        characterPortrait.color = currChar.GetComponent<SpriteRenderer>().color;
    }
    public void updateStats(Character currChar) {

    }
    public void displayCharacterAbilities(Character currChar) {
        //close();
        //foreach (Ability ability in currChar.abilities) {
        //    //updates description
        //    ability.updateDescription();
        //    GameObject temp = Instantiate(abilityDisplay);
        //    //sets the instantiated object as child
        //    temp.transform.parent = abilityDisplayPanel.transform;
        //    AbilityDisplay displayTemp = temp.GetComponent<AbilityDisplay>();
        //    //sets the displays name and description
        //    displayTemp.abilityName.text = ability.abilityName;
        //    displayTemp.description.text = ability.description;
        //    displayTemp.ability = ability;
        //    //if ability has no target hide the target button
        //    if (!ability.hasTarget) {
        //        displayTemp.btn.gameObject.SetActive(false);
        //    }
        //    else
        //        displayTemp.targettingStrategyText.text = TargetNames.getName((ability.targetStrategy));
        //    //sets the cooldownBar fill amount to CD remaining
        //    displayTemp.cooldownBar.fillAmount = (ability.CD - ability.abilityNext) / ability.CD;
        //    //if the ability has no cd anyways(It's a passive)
        //    if (ability.CD == 0)
        //        displayTemp.cooldownText.text = ("Ready");
        //    else
        //    //if the ability is ready
        //    if (ability.abilityNext == 0) 
        //        displayTemp.cooldownText.text = ("Ready "+ability.displayCDAfterChange()+" CD");
        //    else
        //    //shows how much cd remaining 
        //    displayTemp.cooldownText.text = (ability.abilityNext).ToString("F1");
        //    //resetting scale to 1 cuz for somereaosn the scale is 167 otherwise
        //    temp.transform.localScale = new Vector3(1, 1, 1);
        //}
        ////if the character is player and has less than 5 abilities and zone not started and there are abilities to add available
        //if(character.team == (int)Character.teamList.Player &&character.abilities.Count<5&& !uiManager.zoneStarted() && uiManager.playerParty.abilityInventory.transform.childCount>0) {
        //    addAbilityBtn.gameObject.SetActive(true);
        //    //puts the add ability as the last child
        //    addAbilityBtn.transform.SetAsLastSibling();
        //}
        //else
        //    addAbilityBtn.gameObject.SetActive(false);
        //if(abilityDisplayPanel.transform.childCount > 3) {
        //    abilityDisplayPanel.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
        //}
        //else
        //    abilityDisplayPanel.GetComponent<VerticalLayoutGroup>().childControlHeight = false;
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
        //levelBar.fillAmount = (float)currChar.xpProgress / currChar.xpCap;
        //levelProgress.text = currChar.xpProgress + "/" + currChar.xpCap;
    }
    private void displayUpgradeStats(Character currChar) {
        //so that it displays stat points as available/total
        SP.text = "Upgrade Points " + currChar.statPoints.ToString() + "/" + (statPointUI.SPUsedBuffer + currChar.statPoints);


        //displays statPoints if zone hasn't started and if the character has statpoints available
        if ((currChar.statPoints + statPointUI.SPUsedBuffer) > 0 && !uiManager.zoneStarted()) {
            //Debug.Log("showing");
            statPointUI.applied = false;
            statPointUI.show();
        }
        else {
            statPointUI.hide();
        }
        statPointUI.lastUsedCharacter = currChar;
    }
    private void displayInterestingStats(Character currChar) {
        totalKills.text = currChar.totalKills + "";
        totalDamage.text = currChar.totalDamage.ToString("F0");
    }
    public void close() {
        ////destroys all ability displays
        //foreach (Transform toDestroy in abilityDisplayPanel.transform) {
        //    if(toDestroy.tag!="DontDelete")
        //        GameObject.Destroy(toDestroy.gameObject);
        //}

        //targetSelector.targetSelection.SetActive(false);
        //movementSelector.gameObject.SetActive(false);
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

    public void confirmAddAbilityPage() {
        addAbilityBtn.gameObject.SetActive(false);
        confirmAddAbilityBtn.gameObject.SetActive(true);
        pageIndex = 4;
    }
    //displays the abilities in inventory when clicked
    private void addAbility() {
        if (character.abilities.Count >= MAX_ABILITIES) {
            uiManager.tooltip.showMessage("Cannot add ability. Character already has max abilities.");
            return;
        }
        //to destroy all abilityDisplayElements
        close();

        //then display inventory abilities
        displayInventoryAbilities();

        addAbilityBtn.gameObject.SetActive(false);
    }

    public void confirmAddAbility() {
        pageIndex = 4;
        if (character.abilities.Count >= MAX_ABILITIES) {
            uiManager.tooltip.showMessage("Cannot add ability. Character already has max abilities.");
            return;
        }
        //since inventoryCharacterScreen and characterScreen where seperate I have to do some spaghetti code
        //adds the ability to Character
        character.abilities.Add(uiManager.inventoryScreen.abilitySelected);
        //sets the ability's character to this character
        character.initRoundStart();
        //adds ability to activeAbilities in playermanager
        //Debug.Log(uiManager.inventoryScreen.playerParty.activeAbilities.name);
        uiManager.inventoryScreen.abilitySelected.gameObject.transform.parent = uiManager.playerParty.activeAbilities.transform;
        //if in inventory
        if (!uiManager.inventoryScreenHidden.hidden) {
            //if ability was selected first go back to inventory screen landing page
            if (uiManager.inventoryScreen.pageIndex == 2) {
                uiManager.inventoryScreen.openLandingPage();
            }
            else {
                //update the character's ability display
                displayCharacterAbilities(uiManager.inventoryScreen.characterSelected);
            }
        }
        //else if regular character screen
        else
            displayCharacterAbilities(character);
        //saves adding the ability
        if (SceneManager.GetActiveScene().name == "World") {
            uiManager.saveWorldSave();
        }
        else
            uiManager.saveMapSave();
        confirmAddAbilityBtn.gameObject.SetActive(false);
    }


    public void displayInventoryAbilities() {
        //loops thorugh the abilities in abilityInventory
        Debug.Log(uiManager.inventoryScreen.playerParty.abilityInventory.transform.childCount);
        foreach (Transform child in uiManager.inventoryScreen.playerParty.abilityInventory.transform) {
            Debug.Log(child.name + "ABILIRTY INVENTORY");
            Ability ability = child.GetComponent<Ability>();
            GameObject temp = Instantiate(inventoryAbilityDisplay);
            //sets the instantiated object as child
            temp.transform.parent = abilityDisplayPanel.transform;
            InventoryAbilityDisplay displayTemp = temp.GetComponent<InventoryAbilityDisplay>();
            //sets the displays name and description
            displayTemp.abilityName.text = ability.abilityName;
            displayTemp.description.text = ability.description;
            displayTemp.ability = ability;
            displayTemp.glow = true;
            //resetting scale to 1 cuz for somereaosn the scale is 167 otherwise
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    #endregion
    private void FixedUpdate() {
        ////make the upgrade stats color pulsate
        //upgradeStats.color = Color.Lerp(upgradeStatsColorPingPong1, upgradeStatsColorPingPong2, Mathf.PingPong(Time.time, 2));
        ////to make the button glow in and out for emphasis
        //float x = 0.5f + Mathf.PingPong(Time.unscaledTime * 0.5f, 0.7f);
        //confirmAddAbilityBtnImage.color = new Color(x, x, x);
        
    }
    public bool openingFullScreen;
    private void Update() {
        if (uiManager.charInfoScreenHidden.hidden == false)
            viewCharacterTopStatDisplay(character);

        if (opening)
            time += Time.unscaledDeltaTime;
        if (closing)
            time -= Time.unscaledDeltaTime;

        if (time > transitionTime) {
            //no longer in the process of opening
            opening = false;
        }

        if (opening || closing) {
            handlePanels();
        }
    }
}
