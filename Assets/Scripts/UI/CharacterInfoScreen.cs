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
    //StatIcons
    public GameObject PDIcon, MDIcon, INFIcon, HPIcon;
    //cool stats texts
    public TextMeshProUGUI totalKills;
    public TextMeshProUGUI totalDamage;
    public CharacterHealthBar healthBar;

    public Image characterPortrait;

    //Used to instantiate AbilityDisplay prefab
    public GameObject abilityDisplay;

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
    public SlicedFilledImage levelBar;


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

    [SerializeField] private bool opened;

    //this is used to pause(waspause) once closing is done
    private bool willHandlePause;
    private bool closed;
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

    [SerializeField] private RectTransform xpPanel;
    private float xpPanelAnchorL;
    private float xpPanelAnchorR;
    private float xpPanelAnchorT;
    private float xpPanelAnchorB;

    private float xpPanelPositionL;
    private float xpPanelPositionR;
    private float xpPanelPositionT;
    private float xpPanelPositionB;

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

    [SerializeField] private RectTransform abilitiesPanel;

    private float abilitiesPanelAnchorL;
    private float abilitiesPanelAnchorR;
    private float abilitiesPanelAnchorT;
    private float abilitiesPanelAnchorB;

    private float abilitiesPanelPositionL;
    private float abilitiesPanelPositionR;
    private float abilitiesPanelPositionT;
    private float abilitiesPanelPositionB;

    [SerializeField] private RectTransform healthBarPanel;
    private float healthBarPanelAnchorL;
    private float healthBarPanelAnchorR;
    private float healthBarPanelAnchorT;
    private float healthBarPanelAnchorB;
    private float healthBarPanelPositionL;
    private float healthBarPanelPositionR;
    private float healthBarPanelPositionT;
    private float healthBarPanelPositionB;

    private RectTransform targetSelectBtnPanel;

    [SerializeField] private float transitionTime;
    [SerializeField] private float time;
    [SerializeField] private float time2;

    //to be able to place the ability displays here
    [SerializeField] private List<RectTransform> abilityPlaceholders= new List<RectTransform>();

    //to be able to delete all abilityDisplays
    [SerializeField] private List<GameObject> abilityDisplays = new List<GameObject>();

    [SerializeField] private List<Button> abilityTargetting = new List<Button>();
    [SerializeField] private List<TextMeshProUGUI> abilityTargettingText = new List<TextMeshProUGUI>();
    //The children of this will be the stat Icons
    [SerializeField] private List<Transform> abilityTargettingIconParent = new List<Transform>();


    public void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        //just so that it doesn't transition on start.
        time = transitionTime;

        hideUI = GetComponent<HideUI>();

        saveInitPanels();
        
        targetSelectBtnPanel = targetSelectionBtn.GetComponent<RectTransform>();

        openFullScreenBtn.onClick.AddListener(viewCharacterFullScreen);
        closeFullScreenBtn.onClick.AddListener(startClosing);

        //targetSelectionBtn.onClick.AddListener(openTargetSelectorNormal);
        //openMovementSelectorBtn.onClick.AddListener(openMovementSelectorPage);
        //upgradeStatsColorPingPong1 = new Color(upgradeStats.color.r * .5f, upgradeStats.color.g * .5f, upgradeStats.color.b * .5f, .8f);
        //upgradeStatsColorPingPong2 = new Color(upgradeStats.color.r * 1.2f, upgradeStats.color.g * 1.2f, upgradeStats.color.b * 1.2f, 1);
        //addAbilityBtn.onClick.AddListener(addAbility);
        //confirmAddAbilityBtn.onClick.AddListener(confirmAddAbility);
        //confirmAddAbilityBtnImage = confirmAddAbilityBtn.GetComponent<Image>();
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

        xpPanelAnchorL = xpPanel.GetAnchorLeft();
        xpPanelAnchorR = xpPanel.GetAnchorRight();
        xpPanelAnchorT = xpPanel.GetAnchorTop();
        xpPanelAnchorB = xpPanel.GetAnchorBottom();

        xpPanelPositionL = xpPanel.GetLeft();
        xpPanelPositionR = xpPanel.GetRight();
        xpPanelPositionT = xpPanel.GetTop();
        xpPanelPositionB = xpPanel.GetBottom();

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

        abilitiesPanelAnchorL = abilitiesPanel.GetAnchorLeft();
        abilitiesPanelAnchorR = abilitiesPanel.GetAnchorRight();
        abilitiesPanelAnchorT = abilitiesPanel.GetAnchorTop();
        abilitiesPanelAnchorB = abilitiesPanel.GetAnchorBottom();

        abilitiesPanelPositionL = abilitiesPanel.GetLeft();
        abilitiesPanelPositionR = abilitiesPanel.GetRight();
        abilitiesPanelPositionT = abilitiesPanel.GetTop();
        abilitiesPanelPositionB = abilitiesPanel.GetBottom();

        healthBarPanelAnchorL = healthBarPanel.GetAnchorLeft();
        healthBarPanelAnchorR = healthBarPanel.GetAnchorRight();
        healthBarPanelAnchorT = healthBarPanel.GetAnchorTop();
        healthBarPanelAnchorB = healthBarPanel.GetAnchorBottom();

        healthBarPanelPositionL = healthBarPanel.GetLeft();
        healthBarPanelPositionR = healthBarPanel.GetRight();
        healthBarPanelPositionT = healthBarPanel.GetTop();
        healthBarPanelPositionB = healthBarPanel.GetBottom();
    }
    #region movingUIElementsNStuff
    private void startOpening() {
        if (!opened) {
            setPanelStuffActive(true);
            opened = true;
            //resets time so that the transition can start.
            opening = true;
            closing = false;
            time = 0;
            uiManager.pausePlay(true);
        }
    }

    private void setPanelStuffActive(bool bol) {
        closeFullScreenBtn.gameObject.SetActive(bol);
        levelProgress.gameObject.SetActive(bol);
        openTargetSelectionTxt.gameObject.SetActive(bol);
    }
    private void startClosing() {
        if (opened) {
            setPanelStuffActive(false);
            opened = false;
            time = transitionTime;
            opening = false;
            closing = true;
            willHandlePause = true;
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

    private void handleXpPanel() {
        //The comments are in the case of expanding the panel but they are also applicable in reverse I guess
        //Puts it below Portrait Panel (Where stats panel was initially)
        //by finding the scale amount we can find the position of the initial bottom anchor and then set the top to thatposition
        float scaleAmount = (mainPanel.GetAnchorTop() - (1-mainPanel.GetAnchorTop())) / (mainPanelAnchorT - mainPanelAnchorB);
        xpPanel.SetAnchorTop(Mathf.Lerp(xpPanelAnchorT, statsPanelAnchorB * scaleAmount - (statsPanelAnchorT), time / transitionTime));

        //Stretches it to the right
        xpPanel.SetAnchorRight(Mathf.Lerp(xpPanelAnchorR, 1 - xpPanelAnchorL, time / transitionTime));

        
        //sets the bottom to make the height of the panel twice it's inital height
        float initHeight = xpPanelAnchorT - xpPanelAnchorB;
        xpPanel.SetAnchorBottom(Mathf.Lerp(xpPanelAnchorB, xpPanel.GetAnchorTop() - initHeight/4, time / transitionTime));
    }
    private void handleStatsPanel() {
        //Puts it below XP Panel
        //Sets the top to be the bottom of xpPanel
        statsPanel.SetAnchorTop(Mathf.Lerp(statsPanelAnchorT, xpPanel.GetAnchorBottom(), time / transitionTime));
        
        //Stretches it to the left
        statsPanel.SetAnchorLeft(Mathf.Lerp(statsPanelAnchorL, 1 - statsPanelAnchorR, time / transitionTime));

        //sets the bottom to be in the top third kinda of main panel
        statsPanel.SetAnchorBottom(Mathf.Lerp(statsPanelAnchorB, (mainPanel.GetAnchorTop() - mainPanel.GetAnchorBottom())/1.5f, time / transitionTime));

    }

    private void handleAbilitiesPanel() {
        //Sets the top to be the bottom of healthBarPanel with some padding
        abilitiesPanel.SetAnchorTop(Mathf.Lerp(abilitiesPanelAnchorT, healthBarPanel.GetAnchorBottom()-0.015f, time / transitionTime));
        //Sets the bottom to be slightly above the bottom of the mainPanel
        abilitiesPanel.SetAnchorBottom(Mathf.Lerp(abilitiesPanelAnchorB, mainPanel.GetAnchorBottom(), time / transitionTime));

        //stretches it to the left
        abilitiesPanel.SetAnchorLeft(Mathf.Lerp(abilitiesPanelAnchorL, 1 - abilitiesPanelAnchorR, time / transitionTime));
    }

    private void handleHealthBarPanel() {
        //Keep left anchor and right anchor equal to statsPanel
        healthBarPanel.SetAnchorLeft(Mathf.Lerp(healthBarPanelAnchorL, statsPanel.GetAnchorLeft(),time/transitionTime));
        healthBarPanel.SetAnchorRight(Mathf.Lerp(healthBarPanelAnchorR, statsPanel.GetAnchorRight(),time/transitionTime));
        //Keep top anchor and bottom anchor on stats panel's bottom anchor with some padding
        healthBarPanel.SetAnchorTop(Mathf.Lerp(healthBarPanelAnchorT, statsPanel.GetAnchorBottom()-0.007f,time/transitionTime));
        //To keep the bottom anchor from going all the way to the bottom
        healthBarPanel.SetAnchorBottom(Mathf.Lerp(healthBarPanelAnchorB, statsPanel.GetAnchorBottom()-0.03f,time/transitionTime));
    }
    private void handlePanels() {
        //this is needed to update stats text position and size as we expand and shrink the panel
        handleXpPanel();
        handleStatsPanel();
        handleMainPanel();
        handlePortraitPanel();
        handleTargetSelectorBtnPanel();
        handleHealthBarPanel();
        handleAbilitiesPanel();
        hideUI.setInitPos();
        mainPanel.gameObject.RefreshLayoutGroupsImmediateAndRecursive();
        
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
    private void viewCharacterFullScreen() {
        viewCharacterFullScreen(character);
    }
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
        character = currChar;
        displayNameAndPortrait(currChar);
        //openLandingPage();
        displayStats(currChar);
        displayCharacterAbilities(currChar);
        startOpening();

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
        close();
        int count = 0;
        foreach (Ability ability in currChar.abilities) {
            //updates description
            ability.updateDescription();
            //Instantiates it as child of the abilityDisplayPanel
            GameObject displayObject = Instantiate(this.abilityDisplay, abilitiesPanel.transform);
            abilityDisplays.Add(displayObject);
            //copies the anchors of the placeholder
            RectTransform displayRect = displayObject.GetComponent<RectTransform>();
            displayRect.SetAnchorBottom(abilityPlaceholders[count].GetAnchorBottom());
            displayRect.SetAnchorTop(abilityPlaceholders[count].GetAnchorTop());
            displayRect.SetAnchorLeft(abilityPlaceholders[count].GetAnchorLeft());
            displayRect.SetAnchorRight(abilityPlaceholders[count].GetAnchorRight());

            AbilityDisplay abilityDisplay = displayObject.GetComponent<AbilityDisplay>();
            //sets the displays name and description
            abilityDisplay.abilityName.text = ability.abilityName;
            abilityDisplay.description.text = ability.description;
            abilityDisplay.ability = ability;
            //if ability has target display the targetting and the respective text and icon
            if (ability.hasTarget) {
                abilityTargetting[count].gameObject.SetActive(true);
                //Deletes the old icon by deleting children of abilityTargettingIconParent
                foreach (Transform child in abilityTargettingIconParent[count]) {
                    Destroy(child.gameObject);
                }

                switch (ability.targetStrategy) {
                    case (int)Character.TargetList.HighestPDAlly:
                    case (int)Character.TargetList.LowestPDAlly:
                        //Sets Color
                        abilityTargetting[count].image.color = ColorPalette.singleton.allyHealthBar;
                        //instantiate staticon as child of abilityTargettingIcon
                        Instantiate(PDIcon, abilityTargettingIconParent[count]);
                        break;
                    case (int)Character.TargetList.HighestPDEnemy:
                    case (int)Character.TargetList.LowestPDEnemy:
                        //Sets Color
                        abilityTargetting[count].image.color = ColorPalette.singleton.enemyHealthBar;
                        //instantiate staticon as child of abilityTargettingIcon
                        Instantiate(PDIcon, abilityTargettingIconParent[count]);
                        break;

                    case (int)Character.TargetList.HighestMDAlly:
                    case (int)Character.TargetList.LowestMDAlly:
                        //Sets Color
                        abilityTargetting[count].image.color = ColorPalette.singleton.allyHealthBar;
                        //instantiate staticon as child of abilityTargettingIcon
                        Instantiate(MDIcon, abilityTargettingIconParent[count]);
                        break;
                    case (int)Character.TargetList.HighestMDEnemy:
                    case (int)Character.TargetList.LowestMDEnemy:
                        //Sets Color
                        abilityTargetting[count].image.color = ColorPalette.singleton.enemyHealthBar;
                        //instantiate staticon as child of abilityTargettingIcon
                        Instantiate(MDIcon, abilityTargettingIconParent[count]);
                        break;

                    case (int)Character.TargetList.HighestHPAlly:
                    case (int)Character.TargetList.LowestHPAlly:
                        //Sets Color
                        abilityTargetting[count].image.color = ColorPalette.singleton.allyHealthBar;
                        //instantiate staticon as child of abilityTargettingIcon
                        Instantiate(HPIcon, abilityTargettingIconParent[count]);
                        break;

                    case (int)Character.TargetList.HighestHPEnemy:
                    case (int)Character.TargetList.LowestHPEnemy:
                        //Sets Color
                        abilityTargetting[count].image.color = ColorPalette.singleton.enemyHealthBar;
                        //instantiate staticon as child of abilityTargettingIcon
                        Instantiate(HPIcon, abilityTargettingIconParent[count]);
                        break;

                    case (int)Character.TargetList.HighestINFAlly:
                    case (int)Character.TargetList.LowestINFAlly:
                        //Sets Color
                        abilityTargetting[count].image.color = ColorPalette.singleton.allyHealthBar;
                        //instantiate staticon as child of abilityTargettingIcon
                        Instantiate(INFIcon, abilityTargettingIconParent[count]);
                        break;

                    case (int)Character.TargetList.HighestINFEnemy:
                    case (int)Character.TargetList.LowestINFEnemy:
                        //Sets Color
                        abilityTargetting[count].image.color = ColorPalette.singleton.enemyHealthBar;
                        //instantiate staticon as child of abilityTargettingIcon
                        Instantiate(INFIcon, abilityTargettingIconParent[count]);
                        break;

                    case (int)Character.TargetList.ClosestAlly:
                        //Sets Color
                        abilityTargetting[count].image.color = ColorPalette.singleton.allyHealthBar;
                        break;
                    case (int)Character.TargetList.ClosestEnemy:
                        //Sets Color
                        abilityTargetting[count].image.color = ColorPalette.singleton.enemyHealthBar;
                        break;

                    default:
                        Debug.Log("No appropriate strategy or closest");
                        break;
                }
                abilityTargettingText[count].text = TargetNames.getName((ability.targetStrategy));

                //Stretches the icon by setting the anchors of the children of abilityTargettingIconParent to stretch
                foreach (Transform child in abilityTargettingIconParent[count]) {
                    RectTransform iconRect = child.GetComponent<RectTransform>();
                    iconRect.SetAnchorsStretch();
                }
                
                
            }
            else {
                abilityTargetting[count].gameObject.SetActive(false);
            }

            ////if ability has no target hide the target button
            //if (!ability.hasTarget) {
            //    abilityDisplay.btn.gameObject.SetActive(false);
            //}
            //else
            //    abilityDisplay.targettingStrategyText.text = TargetNames.getName((ability.targetStrategy));
            //sets the cooldownBar fill amount to CD remaining
            abilityDisplay.cooldownBar.fillAmount = (ability.CD - ability.abilityNext) / ability.CD;
            //if the ability has no cd anyways(It's a passive)
            if (ability.CD == 0)
                abilityDisplay.cooldownText.text = ("");
            else
            //if the ability is ready
            if (ability.abilityNext == 0)
                abilityDisplay.cooldownText.text = (ability.displayCDAfterChange());
            else
                //shows how much cd remaining 
                abilityDisplay.cooldownText.text = (ability.abilityNext).ToString("F1");
            //resetting scale to 1 cuz for somereaosn the scale is 167 otherwise
            displayObject.transform.localScale = new Vector3(1, 1, 1);

            count++;
        }
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
        healthBar.manualDisplayHealth();

        levelText.text = "LVL: " + currChar.level;
        levelBar.fillAmount = (float)currChar.xpProgress / currChar.xpCap;
        levelProgress.text = currChar.xpProgress + "/" + currChar.xpCap;
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
        //destroys all ability displays
        for(int i = 0;i<abilityDisplays.Count;i++) {
            //Destroys the ability display then refills the placeholder with a background
            Destroy(abilityDisplays[i]);
            Debug.Log("Destroyed" + abilityDisplays[i].name);
            foreach(Transform child in abilityPlaceholders[i].transform) {
                child.gameObject.SetActive(true);
            }
        }
        abilityDisplays.Clear();

        //targetSelector.targetSelection.SetActive(false);
        //movementSelector.gameObject.SetActive(false);
    }

    //The onclick is set in the editor
    public void ability1Clicked() {
        Debug.Log("Ability1Clicked");
    }

    public void ability2Clicked() {
        Debug.Log("Ability2Clicked");
    }

    public void ability3Clicked() {
        Debug.Log("Ability3Clicked");
    }
    public void ability4Clicked() {
          Debug.Log("Ability4Clicked");
    }

    public void ability5Clicked() {
        Debug.Log("Ability5Clicked");
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
        //pageIndex = 4;
        //if (character.abilities.Count >= MAX_ABILITIES) {
        //    uiManager.tooltip.showMessage("Cannot add ability. Character already has max abilities.");
        //    return;
        //}
        ////since inventoryCharacterScreen and characterScreen where seperate I have to do some spaghetti code
        ////adds the ability to Character
        //character.abilities.Add(uiManager.inventoryScreen.abilitySelected);
        ////sets the ability's character to this character
        //character.initRoundStart();
        ////adds ability to activeAbilities in playermanager
        ////Debug.Log(uiManager.inventoryScreen.playerParty.activeAbilities.name);
        //uiManager.inventoryScreen.abilitySelected.gameObject.transform.parent = uiManager.playerParty.activeAbilities.transform;
        ////if in inventory
        //if (!uiManager.inventoryScreenHidden.hidden) {
        //    //if ability was selected first go back to inventory screen landing page
        //    if (uiManager.inventoryScreen.pageIndex == 2) {
        //        uiManager.inventoryScreen.openLandingPage();
        //    }
        //    else {
        //        //update the character's ability display
        //        displayCharacterAbilities(uiManager.inventoryScreen.characterSelected);
        //    }
        //}
        ////else if regular character screen
        //else
        //    displayCharacterAbilities(character);
        ////saves adding the ability
        //if (SceneManager.GetActiveScene().name == "World") {
        //    uiManager.saveWorldSave();
        //}
        //else
        //    uiManager.saveMapSave();
        //confirmAddAbilityBtn.gameObject.SetActive(false);
    }


    public void displayInventoryAbilities() {
        ////loops thorugh the abilities in abilityInventory
        //Debug.Log(uiManager.inventoryScreen.playerParty.abilityInventory.transform.childCount);
        //foreach (Transform child in uiManager.inventoryScreen.playerParty.abilityInventory.transform) {
        //    Debug.Log(child.name + "ABILIRTY INVENTORY");
        //    Ability ability = child.GetComponent<Ability>();
        //    GameObject temp = Instantiate(inventoryAbilityDisplay);
        //    //sets the instantiated object as child
        //    temp.transform.parent = abilityDisplayPanel.transform;
        //    InventoryAbilityDisplay displayTemp = temp.GetComponent<InventoryAbilityDisplay>();
        //    //sets the displays name and description
        //    displayTemp.abilityName.text = ability.abilityName;
        //    displayTemp.description.text = ability.description;
        //    displayTemp.ability = ability;
        //    displayTemp.glow = true;
        //    //resetting scale to 1 cuz for somereaosn the scale is 167 otherwise
        //    temp.transform.localScale = new Vector3(1, 1, 1);
        //}
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


        if (opening || closing) {
            handlePanels();
        }

        if (time >= transitionTime) {
            //no longer in the process of opening
            opening = false;
        }

        //once closing is done unpause the game
        if (willHandlePause && time <= 0) {
            uiManager.pausePlay(uiManager.wasPause);
            willHandlePause = false;
        }
        if (character != null) {
            if (character.team == (int)Character.teamList.Player) {
                targetSelectionBtn.interactable = true;
            }
            else
                targetSelectionBtn.interactable = false;
        }
    }
}
