using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
//The values of how I calculated the initAnchors is in the notebook page 76
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

    //Used to instantiate AbilityDisplayAddAbility prefab
    public AbilityDisplayAddAbility abilityDisplayAddingAbility;

    //Selecting target
    public TargetSelector targetSelector;

    public Button targetSelectionBtn;
    public TextMeshProUGUI targetSelectionTxt;
    [SerializeField] private Transform targettingIconParent;

    public Button abilityScreenBtn;
    public Button itemScreenBtn;

    //Selecting target for attacking and also moving for now.
    public MovementStrategySelector movementSelector;
    
    public Button openMovementSelectorBtn;

    public TextMeshProUGUI openMovementSelectorTxt;


    //character that is currently being viewed
    public Character character;


    //Stat point stuff
    [SerializeField]public StatUpgrading statUpgrading;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelProgress;
    public SlicedFilledImage levelBar;


    //this will be glowing to display that there are stat points available
    public Image statsBorder;
    public RectTransform statsBorderRectTransform;
    public Color upgradeStatsColorPingPong1;
    public Color upgradeStatsColorPingPong2;

    public int pageIndex = -1;
    //-1 is topstatdisplay
    //0 landing page
    //1 target selection

    //to be instantiated this is a different gameobject than the regular abilityDisplay because this one will have diff color
    public GameObject inventoryAbilityDisplay;

    
    public Button addAbilityBtn;
    public RectTransform addAbilityPanel;
    public RectTransform addAbilityDisplaysArea;
    public Button cancelAddingAbilityBtn;

    public Button confirmAddAbilityBtn;
    public Image confirmAddAbilityBtnImage;

    public const int MAX_ABILITIES = 5;
    //pageindex 3 = prompt to add ability
    //pageindex 4 = confirm ability adding
    //base page wehn opening charinfoscreen

    private HideUI hideUI;
    //To be Ablew to force full screen, since hideUI's init values will be constantly changing we can look back to the Vector2 that holds the initinitValues
    [SerializeField]private Vector2 hideUIInitValues;
    //used for initial full screen opening
    [SerializeField] private bool opening;
    [SerializeField] private bool closing;

    [SerializeField] private bool opened;

    //this is used to pause(waspause) once closing is done
    private bool willHandlePause;

    [SerializeField] private bool focusing;
    [SerializeField] public bool unFocusing;
    private bool focused;
    //The element to be focused 
    //0 1 2 3 4 = abilities. 5 = base targetting. 6 = Adding ability screen, 7 = upgrading stats,8 = selectArchetype
    private int focusElement;
    //This is used to set active to false when unfocusing is done
    private bool willHandleDeActivatingFocus;
    ////Once it is fullscreen it will open the other panels such as abilities/ targetting / upgrading
    //private bool opening2;
    //private bool closing2;

    private bool opened2;
    //this is the main panel itself and maybe I will add another button for clarity later
    [SerializeField] public Button openFullScreenBtn;
    [SerializeField] public Button closeFullScreenBtn;

    [SerializeField] private Button confirmTargettingBtn;

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

    [SerializeField]public RectTransform xpPanel;
    public Button xpPanelBtn;
    private float xpPanelAnchorL;
    private float xpPanelAnchorR;
    private float xpPanelAnchorT;
    private float xpPanelAnchorB;

    private float xpPanelPositionL;
    private float xpPanelPositionR;
    private float xpPanelPositionT;
    private float xpPanelPositionB;


    [SerializeField]public RectTransform statsPanel;
    [SerializeField]public Button statsPanelBtn;
    private float statsPanelAnchorL;
    [SerializeField]private float statsPanelAnchorR;
    private float statsPanelAnchorT;
    [SerializeField]private float statsPanelAnchorB;
                  
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

    [SerializeField] public RectTransform abilitiesPanel;

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
    [SerializeField] private float healthBarPanelAnchorT;
    [SerializeField] private float healthBarPanelAnchorB;
    private float healthBarPanelPositionL;
    private float healthBarPanelPositionR;
    private float healthBarPanelPositionT;
    private float healthBarPanelPositionB;

    private RectTransform targetSelectBtnPanel;

    private RectTransform abilityScreenBtnPanel;
    private RectTransform itemScreenBtnPanel;

    [SerializeField] public float transitionTime;
    //Used for opening and closing fullscreen.
    [SerializeField] public float time;
    //Used for focusing elements
    [SerializeField] public float time2;

    
    //to be able to place the ability displays here
    [SerializeField] public List<RectTransform> abilityPlaceholders= new List<RectTransform>();
    private List<float> abilityPlaceHoldersAnchorL = new List<float>(new float[MAX_ABILITIES]);
    private List<float> abilityPlaceHoldersAnchorR = new List<float>(new float[MAX_ABILITIES]);
    private List<float> abilityPlaceHoldersAnchorT = new List<float>(new float[MAX_ABILITIES]);
    private List<float> abilityPlaceHoldersAnchorB = new List<float>(new float[MAX_ABILITIES]);

    //to be able to delete all abilityDisplays
    [SerializeField] public List<RectTransform> abilityDisplays = new List<RectTransform>();
    public List<Ability> abilities = new List<Ability>();

    [SerializeField] private List<RectTransform> abilityTargetting = new List<RectTransform>();
    private List<float> abilityTargettingAnchorL = new List<float>(new float[MAX_ABILITIES]);
    private List<float> abilityTargettingAnchorR = new List<float>(new float[MAX_ABILITIES]);
    private List<float> abilityTargettingAnchorT = new List<float>(new float[MAX_ABILITIES]);
    private List<float> abilityTargettingAnchorB = new List<float>(new float[MAX_ABILITIES]);

    [SerializeField] private List<TextMeshProUGUI> abilityTargettingText = new List<TextMeshProUGUI>();
    //The children of this will be the stat Icons
    [SerializeField] private List<Transform> abilityTargettingIconParent = new List<Transform>();

    //If this Character Info Screen is opened from the inventory screen
    public bool inventoryScreen;

    [SerializeField]private List<SlicedFilledImage> topstatAbilityDisplays = new List<SlicedFilledImage>(new SlicedFilledImage[MAX_ABILITIES]);
    [SerializeField]private List<Image> topstatAbilityDisplaysFill = new List<Image>(new Image[MAX_ABILITIES]);
    [SerializeField]private List<Image> topstatAbilityDisplaysBorder = new List<Image>(new Image[MAX_ABILITIES]);

    const int ARCHETYPESELECTLEVEL = 20;

    [SerializeField]private SelectArchetype selectArchetype;


    public void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        //just so that it doesn't transition on start.
        time = transitionTime;
        time2 = transitionTime;
        hideUI = GetComponent<HideUI>();
        hideUIInitValues.x = hideUI.initPos.x;
        hideUIInitValues.y = hideUI.initPos.y;
        saveInitPanels();
        
        targetSelectBtnPanel = targetSelectionBtn.GetComponent<RectTransform>();
        targetSelectionBtn.onClick.AddListener(openPrimaryTargetSelector);

        abilityScreenBtnPanel = abilityScreenBtn.GetComponent<RectTransform>();
        itemScreenBtnPanel = itemScreenBtn.GetComponent<RectTransform>();



        openFullScreenBtn.onClick.AddListener(viewCharacterFullScreen);
        closeFullScreenBtn.onClick.AddListener(startClosing);

        confirmTargettingBtn.onClick.AddListener(startUnfocusing);
        addAbilityBtn.onClick.AddListener(addAbility);
        cancelAddingAbilityBtn.onClick.AddListener(cancelAddingAbility);
        xpPanelBtn = xpPanel.GetComponent<Button>();

        statsPanelBtn.onClick.AddListener(displayUpgradeStatsOrPickArchetype);
        xpPanelBtn.onClick.AddListener(displayUpgradeStatsOrPickArchetype);

        statsBorderRectTransform = statsBorder.GetComponent<RectTransform>();

        //targetSelectionBtn.onClick.AddListener(openTargetSelectorNormal);
        //openMovementSelectorBtn.onClick.AddListener(openMovementSelectorPage);
        //upgradeStatsColorPingPong1 = new Color(upgradeStats.color.r * .5f, upgradeStats.color.g * .5f, upgradeStats.color.b * .5f, .8f);
        //upgradeStatsColorPingPong2 = new Color(upgradeStats.color.r * 1.2f, upgradeStats.color.g * 1.2f, upgradeStats.color.b * 1.2f, 1);
        //confirmAddAbilityBtn.onClick.AddListener(confirmAddAbility);
        //confirmAddAbilityBtnImage = confirmAddAbilityBtn.GetComponent<Image>();
    }

    private void saveInitPanels() {
        for(int i = 0;i<5;i++) {
            abilityPlaceHoldersAnchorL[i] = abilityPlaceholders[i].GetAnchorLeft();
            abilityPlaceHoldersAnchorR[i] = abilityPlaceholders[i].GetAnchorRight();
            abilityPlaceHoldersAnchorT[i] = abilityPlaceholders[i].GetAnchorTop();
            abilityPlaceHoldersAnchorB[i] = abilityPlaceholders[i].GetAnchorBottom();

            abilityTargettingAnchorL[i] = abilityTargetting[i].GetAnchorLeft();
            abilityTargettingAnchorR[i] = abilityTargetting[i].GetAnchorRight();
            abilityTargettingAnchorT[i] = abilityTargetting[i].GetAnchorTop();
            abilityTargettingAnchorB[i] = abilityTargetting[i].GetAnchorBottom();
        }

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
    public void startOpening() {
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

    private void startClosing() {
        if (opened) {
            setPanelStuffActive(false);
            opened = false;
            time = transitionTime;
            opening = false;
            closing = true;
            willHandlePause = true;
            if(inventoryScreen)
                gameObject.SetActive(false);
        }
    }
    private void setPanelStuffActive(bool bol) {
        closeFullScreenBtn.gameObject.SetActive(bol);
        levelProgress.gameObject.SetActive(bol);
        targetSelectionTxt.gameObject.SetActive(bol);

        if (character.statPoints > 0 && bol && !uiManager.zoneStarted()) {
            statsPanelBtn.enabled = true;
            xpPanelBtn.enabled = true;
        }
        else {
            statsPanelBtn.enabled = false;
            xpPanelBtn.enabled = false;
        }
    }

    private void handleMainPanel() {
        //stretches right anchor to be as far from edge as left anchor is from edge same with bottom
        mainPanel.SetAnchorRight(Mathf.Lerp(mainPanelAnchorR, 1 - mainPanelAnchorL, time/transitionTime));
        mainPanel.SetAnchorBottom(Mathf.Lerp(mainPanelAnchorB, 1 - mainPanelAnchorT, time/transitionTime));
    }

    private float portraitScaleAmount=2;
    private void handlePortraitPanel() {
        //Gets the initial size of the portrait panel
        float initSize = portraitPanelAnchorT - portraitPanelAnchorB;
        //Gets the character info screen scale amount as it expands
        float scaleAmount = (mainPanelAnchorT - mainPanel.GetAnchorBottom()) / ((mainPanelAnchorT)-mainPanelAnchorB) ;
        //Sets the bottom anchor to be the same as as it was before the transition started by dividing it by the scale amount, but also double it's size as it was before thre transition
        portraitPanel.SetAnchorBottom(Mathf.Lerp(portraitPanelAnchorB, (portraitPanelAnchorT - (initSize*1.5f / scaleAmount)), time/transitionTime));
        portraitPanel.SetAnchorTop(Mathf.Lerp(portraitPanelAnchorT,0.99f, time / transitionTime));
        
        //Scale it to the right a bit too
        portraitPanel.SetAnchorRight(Mathf.Lerp(portraitPanelAnchorR, (portraitPanelAnchorR*1.5f), time/transitionTime));
    }
    
    //private void scalePortraitPanel(float amount) {
    //    portraitPanel.SetRight(Mathf.Lerp(portraitPanelPositionR, portraitPanelPositionR * amount, time/transitionTime));
    //    portraitPanel.SetBottom(Mathf.Lerp(portraitPanelPositionB, portraitPanelPositionB * amount, time/transitionTime));
    //}

    private void handleXpPanel() {
        //The comments are in the case of expanding the panel but they are also applicable in reverse I guess
        //Puts it below Portrait Panel (Where stats panel was initially)
        //puts xpPanel under the portrait panel

        xpPanel.SetAnchorTop(Mathf.Lerp(xpPanelAnchorT, portraitPanel.GetAnchorBottom() - 0.005f, time / transitionTime));

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
        statsPanel.SetAnchorBottom(Mathf.Lerp(statsPanelAnchorB, (mainPanel.GetAnchorTop() - mainPanel.GetAnchorBottom())/1.6f, time / transitionTime));

        statsPanel.SetStretchToAnchors();
    }

    private void handleHealthBarPanel() {
        //Keep left anchor and right anchor equal to statsPanel
        healthBarPanel.SetAnchorLeft(Mathf.Lerp(healthBarPanelAnchorL, statsPanel.GetAnchorLeft(),time/transitionTime));
        healthBarPanel.SetAnchorRight(Mathf.Lerp(healthBarPanelAnchorR, statsPanel.GetAnchorRight(),time/transitionTime));
        //Keep top anchor and bottom anchor on stats panel's bottom anchor with some padding
        healthBarPanel.SetAnchorTop(Mathf.Lerp(healthBarPanelAnchorT, statsPanel.GetAnchorBottom()-0.007f,time/transitionTime));
        //To keep the bottom anchor from going all the way to the bottom
        healthBarPanel.SetAnchorBottom(Mathf.Lerp(healthBarPanelAnchorB, statsPanel.GetAnchorBottom()-0.03f,time/transitionTime));

        healthBarPanel.SetStretchToAnchors();

    }

    private void handleAbilitiesPanel() {
        //Sets the top to be the bottom of healthBarPanel with some padding
        abilitiesPanel.SetAnchorTop(Mathf.Lerp(abilitiesPanelAnchorT, healthBarPanel.GetAnchorBottom()-0.007f, time / transitionTime));
        //Sets the bottom to be slightly above the bottom 
        abilitiesPanel.SetAnchorBottom(Mathf.Lerp(abilitiesPanelAnchorB, 0+0.005f, time / transitionTime));

        //stretches it to the left
        abilitiesPanel.SetAnchorLeft(Mathf.Lerp(abilitiesPanelAnchorL, 1 - abilitiesPanelAnchorR, time / transitionTime));
    }
    private void handlePanels() {
        //this is needed to update stats text position and size as we expand and shrink the panel
        handleMainPanel();
        handlePortraitPanel();
        handleXpPanel();
        handleStatsPanel();
        handleOpenScreenBtnsPanel();
        handleTargetSelectorBtnPanel();
        handleHealthBarPanel();
        handleAbilitiesPanel();
        hideUI.setInitPos();
        mainPanel.gameObject.RefreshLayoutGroupsImmediateAndRecursive();
    }
    //Handles the open item and ability screen
    private void handleOpenScreenBtnsPanel() {
        //Sets the bottom to be the same as portrait Panel. And the top to be in the middle of portrait panel
        abilityScreenBtnPanel.SetAnchorBottom(portraitPanel.GetAnchorBottom());
        itemScreenBtnPanel.SetAnchorBottom(portraitPanel.GetAnchorBottom());
        float portraitPanelHeight = portraitPanel.GetAnchorTop() - portraitPanel.GetAnchorBottom();
        abilityScreenBtnPanel.SetAnchorTop(portraitPanel.GetAnchorTop() - (portraitPanelHeight / 2));
        itemScreenBtnPanel.SetAnchorTop(portraitPanel.GetAnchorTop() - (portraitPanelHeight / 2));
        //Sets the abilityBtn to be on Left and itemBtn on right
        //So we need to get the distance from the right of the portraitPanel to the right of the statPanel
        float distance =  statsPanelAnchorR - portraitPanel.GetAnchorRight();

        abilityScreenBtnPanel.SetAnchorLeft(portraitPanel.GetAnchorRight() + 0.01f);
        abilityScreenBtnPanel.SetAnchorRight(portraitPanel.GetAnchorRight() + (distance / 2) - 0.01f);

        itemScreenBtnPanel.SetAnchorLeft(portraitPanel.GetAnchorRight() + (distance / 2) + 0.01f);
        itemScreenBtnPanel.SetAnchorRight(statsPanelAnchorR);

    }
    private void handleTargetSelectorBtnPanel() {
        //Sets the bottom to be the same as portrait Panel. And the top to be in the middle of portrait panel
        targetSelectBtnPanel.SetAnchorBottom(portraitPanel.GetAnchorBottom());
        float portraitPanelHeight = portraitPanel.GetAnchorTop() - portraitPanel.GetAnchorBottom();
        targetSelectBtnPanel.SetAnchorTop(portraitPanel.GetAnchorTop() - (portraitPanelHeight / 2));
        //targetSelectBtnPanel.SetAnchorTop();
        //sets the left anchor to be to the right of the portrait panel.
        targetSelectBtnPanel.SetAnchorLeft(portraitPanel.GetAnchorRight()+0.01f);
        //Grows the right anchor to the right
        targetSelectBtnPanel.SetAnchorRight(Mathf.Lerp(targetSelectBtnPanel.GetAnchorLeft(), statsPanelAnchorR,time/transitionTime));
    }
    private void startFocusing() {
        if (!focused) {
            uiManager.focus.gameObject.SetActive(true);
            
            //Ability target selector
            if (focusElement >= 0 && focusElement <= 4) {
                targetSelector.gameObject.SetActive(true);
                targetSelector.transform.SetParent(uiManager.focus.transform);
                targetSelector.setupTargetSelector(abilityDisplays[focusElement].GetComponent<AbilityDisplay>().ability,this);

                abilityDisplays[focusElement].transform.SetParent(uiManager.focus.transform);
                abilityTargetting[focusElement].transform.SetParent(uiManager.focus.transform);
            }
            //If primary target selector
            if (focusElement == 5) {
                targetSelector.gameObject.SetActive(true);
                targetSelector.transform.SetParent(uiManager.focus.transform);
                targetSelector.setupTargetSelector(this);

                targetSelectionBtn.transform.SetParent(uiManager.focus.transform);
            }

            //If add ability 
            if(focusElement == 6) {
                addAbilityPanel.gameObject.SetActive(true);
                addAbilityPanel.transform.SetParent(uiManager.focus.transform);
                setupAddAbility();
            }
            //If Upgrading stats
            if(focusElement == 7) {
                statUpgrading.showUpgrades();
                statUpgrading.upgradeOptions.gameObject.SetActive(true);
                statUpgrading.upgradeOptions.transform.SetParent(uiManager.focus.transform);
                statUpgrading.characterInfoScreen = this;
                statsPanel.transform.SetParent(uiManager.focus.transform);
                healthBarPanel.transform.SetParent(uiManager.focus.transform);
                statUpgrading.resetChangesBtn.transform.SetParent(uiManager.focus.transform);
                statUpgrading.applyChangesBtn.transform.SetParent(uiManager.focus.transform);
                xpPanelBtn.enabled = false;
                statsPanelBtn.enabled = false;
            }
            if(focusElement == 8) {
                selectArchetype.characterInfoScreen = this;
                selectArchetype.gameObject.SetActive(true);
                selectArchetype.setupAndView(character);
                //Focus it
                selectArchetype.transform.SetParent(uiManager.focus.transform);
            }
            focused = true;
            //resets time2 to start the transition
            time2 = 0;
            focusing = true;
            unFocusing = false;
        }
    }
    public void startUnfocusing() {
        if(focused) {
            //setting the focus image to be inactive done in the update method
            targetSelector.gameObject.SetActive(false);
            targetSelector.transform.SetParent(this.transform);
            willHandleDeActivatingFocus = true;
            focused = false;
            //sets time2 to start the transition
            time2 = transitionTime;
            unFocusing = true;
            focusing = false;
        }
    }
    //handles focusing and unfocusing siomilar to handlePanels
    private void handleFocusing() {
        
        //Lerps the alpha of the focus image to uiManager.focusOpacity
        uiManager.focus.SetAlpha(Mathf.Lerp(0, uiManager.focusOpacity, time2 / transitionTime));
        //Get the ratio of the size of the abilities panel relative to char info screen. This will be used to put the anchors of the abilityDisplays in the correct place
        //since when we change it's parent, the anchors will be relative to the parent and not to how it was initially
        //We're just getteing the top anchor since this is basically the ratio
        float sizeRatio = abilitiesPanel.GetAnchorTop();

        //where to place the focusElement anchors
        float mainArea=0.6f;
        switch (focusElement) {
            //Lerps the positions of the respective ability and targetting to center of screen
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                abilityDisplays[focusElement].SetAnchorTop(Mathf.Lerp(abilityPlaceHoldersAnchorT[focusElement]*sizeRatio, (mainArea+0.175f*sizeRatio), time2 / transitionTime));
                abilityDisplays[focusElement].SetAnchorBottom(Mathf.Lerp(abilityPlaceHoldersAnchorB[focusElement]*sizeRatio, (mainArea - 0.175f*sizeRatio), time2 / transitionTime));
                abilityDisplays[focusElement].SetAnchorLeft(Mathf.Lerp(abilityPlaceHoldersAnchorL[focusElement], 0.5f - 0.15f, time2 / transitionTime));
                abilityDisplays[focusElement].SetAnchorRight(Mathf.Lerp(abilityPlaceHoldersAnchorR[focusElement], 0.5f + 0.15f, time2 / transitionTime));
                abilityDisplays[focusElement].SetTop(0);
                abilityDisplays[focusElement].SetBottom(0);
                abilityDisplays[focusElement].SetLeft(0);
                abilityDisplays[focusElement].SetRight(0);

                abilityTargetting[focusElement].SetAnchorTop(Mathf.Lerp(abilityTargettingAnchorT[focusElement]*sizeRatio, (mainArea - (0.175f+0.033f)*sizeRatio), time2 / transitionTime));
                abilityTargetting[focusElement].SetAnchorBottom(Mathf.Lerp(abilityTargettingAnchorB[focusElement]*sizeRatio, (mainArea - (0.175f + 3f*0.033f) * sizeRatio), time2 / transitionTime));
                abilityTargetting[focusElement].SetAnchorLeft(Mathf.Lerp(abilityTargettingAnchorL[focusElement], 0.5f - 0.15f, time2 / transitionTime));
                abilityTargetting[focusElement].SetAnchorRight(Mathf.Lerp(abilityTargettingAnchorR[focusElement], 0.5f + 0.15f, time2 / transitionTime));
                abilityTargetting[focusElement].SetTop(0);
                abilityTargetting[focusElement].SetBottom(0);
                abilityTargetting[focusElement].SetLeft(0);
                abilityTargetting[focusElement].SetRight(0);
                break;
            case 7:
                //Put the stat upgrading options display's top to be at the top of the portrait
                //And the bottom at the bottom of the XP bar
                statUpgrading.upgradeOptions.SetAnchorTop(portraitPanel.GetAnchorTop());
                statUpgrading.upgradeOptions.SetAnchorBottom(Mathf.Lerp(portraitPanel.GetAnchorTop(),xpPanel.GetAnchorBottom(),time2/transitionTime));
                //Left and right to be same as xp panel
                statUpgrading.upgradeOptions.SetAnchorLeft(xpPanel.GetAnchorLeft());
                statUpgrading.upgradeOptions.SetAnchorRight(xpPanel.GetAnchorRight());
                statUpgrading.upgradeOptions.SetTop(0);
                statUpgrading.upgradeOptions.SetBottom(0);
                statUpgrading.upgradeOptions.SetLeft(0);
                statUpgrading.upgradeOptions.SetRight(0);
                uiManager.focus.gameObject.RefreshLayoutGroupsImmediateAndRecursive();
                break;
        }
        
    }

    private void setupAddAbility() {
        closeInventoryAbilities();
        //Setting the cell size of the grid layout group
        float width = addAbilityDisplaysArea.rect.width;
        float height = addAbilityDisplaysArea.rect.height/2;
        addAbilityDisplaysArea.GetComponent<GridLayoutGroup>().cellSize = new Vector2(abilityPlaceholders[0].rect.width, abilityPlaceholders[0].rect.height);
        addAbilityDisplaysArea.GetComponent<GridLayoutGroup>().spacing = new Vector2(0.025f*width, 0.05f*height);
        displayInventoryAbilities();
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
        updatePrimaryTargettingView();
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
        characterPortrait.preserveAspect = true;
        characterPortrait.sprite = currChar.GetComponent<SpriteRenderer>().sprite;
        characterPortrait.color = currChar.GetComponent<SpriteRenderer>().color;
    }
    public void updateStats(Character currChar) {

    }
    public void displayCharacterAbilities(Character currChar) {
        close();
        //tells the abilities that this owns them so that they correctly display the description
        currChar.ownTheAbility(false);
        int count = 0;
        foreach (Ability ability in currChar.abilities) {
            //updates description
            ability.updateDescription();
            //Instantiates it as child of the abilityDisplayPanel
            GameObject displayObject = Instantiate(this.abilityDisplay, abilitiesPanel.transform);
            //Adds to the list
            abilityDisplays.Add(displayObject.GetComponent<RectTransform>());
            abilities.Add(ability);
            //copies the anchors of the placeholder
            RectTransform displayRect = displayObject.GetComponent<RectTransform>();
            displayRect.SetAnchorBottom(abilityPlaceholders[count].GetAnchorBottom());
            displayRect.SetAnchorTop(abilityPlaceholders[count].GetAnchorTop());
            displayRect.SetAnchorLeft(abilityPlaceholders[count].GetAnchorLeft());
            displayRect.SetAnchorRight(abilityPlaceholders[count].GetAnchorRight());

            
            AbilityDisplay abilityDisplay = displayObject.GetComponent<AbilityDisplay>();
            //sets the displays name and description
            abilityDisplay.setupAbilityDisplay(ability);
            //if ability has target display the targetting and the respective text and icon
            if (ability.hasTarget) {
                updateAbilityTargettingView(ability, count);
            }
            else {
                abilityTargetting[count].gameObject.SetActive(false);
            }

            //Makes the ability display clickable
            switch (count) {
                case 0:
                    abilityDisplay.btn.onClick.AddListener(ability1Clicked);
                    break;
                case 1:
                    abilityDisplay.btn.onClick.AddListener(ability2Clicked);
                    break;
                case 2:
                    abilityDisplay.btn.onClick.AddListener(ability3Clicked);
                    break;
                case 3:
                    abilityDisplay.btn.onClick.AddListener(ability4Clicked);
                    break;
                case 4:
                    abilityDisplay.btn.onClick.AddListener(ability5Clicked);
                    break;
            }
            //resetting scale to 1 cuz for somereaosn the scale is 167 otherwise
            displayObject.transform.localScale = new Vector3(1, 1, 1);


            count++;
        }
        //if the character is player and has less than 5 abilities and zone not started and there are abilities to add available
        if (character.team == (int)Character.teamList.Player && character.abilities.Count < 5 && !uiManager.zoneStarted() && uiManager.playerParty.abilityInventory.transform.childCount > 0) {
            addAbilityBtn.gameObject.SetActive(true);
            //puts the add ability on the first free slot
            addAbilityBtn.transform.parent = abilityPlaceholders[character.abilities.Count];
            addAbilityBtn.GetComponent<RectTransform>().SetStretchToAnchors();
        }
        else
            addAbilityBtn.gameObject.SetActive(false);
    }
    //displays the stats and cool stats of the character and character screen

    private void displayInventoryAbilities() {
        //Adds the inventory Abilities
        foreach (Transform child in uiManager.playerParty.abilityInventory.transform) {
            Debug.Log("Should display inventory ability");
            Ability ability = child.GetComponent<Ability>();
            ability.updateDescription();
            AbilityDisplayAddAbility abilityDisplayAdding = Instantiate(abilityDisplayAddingAbility, addAbilityDisplaysArea.transform);
            abilityDisplayAdding.setupAbilityDisplay(ability);
            abilityDisplayAdding.characterInfoScreen = this;
            Debug.Log("Ability name"+ability.name);
        }
    }
    private void cancelAddingAbility() {
        startUnfocusing();
        //Moves the addAbilityPanel back to CharacterInfoScreen
        addAbilityPanel.transform.parent = uiManager.characterInfoScreen.transform;
        //Sets the addAbilityPanel to inactive
        addAbilityPanel.gameObject.SetActive(false);
    }
    private void closeInventoryAbilities() {
        //Closes all ability displays
        foreach (Transform child in addAbilityDisplaysArea.transform) {
            Destroy(child.gameObject);
        }
    }
    public void updateAbilityTargettingView(Ability ability,int count) {
        abilityTargetting[count].gameObject.SetActive(true);
        //Deletes the old icon by deleting children of abilityTargettingIconParent
        foreach (Transform child in abilityTargettingIconParent[count]) {
            Destroy(child.gameObject);
        }

        switch (ability.targetStrategy) {
            case (int)Character.TargetList.HighestPDAlly:
            case (int)Character.TargetList.LowestPDAlly:
                //Sets Color
                abilityTargetting[count].GetComponent<Button>().image.color = ColorPalette.singleton.allyHealthBar;
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(PDIcon, abilityTargettingIconParent[count]);
                break;
            case (int)Character.TargetList.HighestPDEnemy:
            case (int)Character.TargetList.LowestPDEnemy:
                //Sets Color
                abilityTargetting[count].GetComponent<Button>().image.color = ColorPalette.singleton.enemyHealthBar;
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(PDIcon, abilityTargettingIconParent[count]);
                break;

            case (int)Character.TargetList.HighestMDAlly:
            case (int)Character.TargetList.LowestMDAlly:
                //Sets Color
                abilityTargetting[count].GetComponent<Button>().image.color = ColorPalette.singleton.allyHealthBar;
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(MDIcon, abilityTargettingIconParent[count]);
                break;
            case (int)Character.TargetList.HighestMDEnemy:
            case (int)Character.TargetList.LowestMDEnemy:
                //Sets Color
                abilityTargetting[count].GetComponent<Button>().image.color = ColorPalette.singleton.enemyHealthBar;
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(MDIcon, abilityTargettingIconParent[count]);
                break;

            case (int)Character.TargetList.HighestHPAlly:
            case (int)Character.TargetList.LowestHPAlly:
                //Sets Color
                abilityTargetting[count].GetComponent<Button>().image.color = ColorPalette.singleton.allyHealthBar;
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(HPIcon, abilityTargettingIconParent[count]);
                break;

            case (int)Character.TargetList.HighestHPEnemy:
            case (int)Character.TargetList.LowestHPEnemy:
                //Sets Color
                abilityTargetting[count].GetComponent<Button>().image.color = ColorPalette.singleton.enemyHealthBar;
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(HPIcon, abilityTargettingIconParent[count]);
                break;

            case (int)Character.TargetList.HighestINFAlly:
            case (int)Character.TargetList.LowestINFAlly:
                //Sets Color
                abilityTargetting[count].GetComponent<Button>().image.color = ColorPalette.singleton.allyHealthBar;
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(INFIcon, abilityTargettingIconParent[count]);
                break;

            case (int)Character.TargetList.HighestINFEnemy:
            case (int)Character.TargetList.LowestINFEnemy:
                //Sets Color
                abilityTargetting[count].GetComponent<Button>().image.color = ColorPalette.singleton.enemyHealthBar;
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(INFIcon, abilityTargettingIconParent[count]);
                break;

            case (int)Character.TargetList.ClosestAlly:
                //Sets Color
                abilityTargetting[count].GetComponent<Button>().image.color = ColorPalette.singleton.allyHealthBar;
                break;
            case (int)Character.TargetList.ClosestEnemy:
                //Sets Color
                abilityTargetting[count].GetComponent<Button>().image.color = ColorPalette.singleton.enemyHealthBar;
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

    public void updatePrimaryTargettingView() {

        //Deletes the old icon by deleting children of targettingIconParent
        foreach (Transform child in targettingIconParent) {
                Destroy(child.gameObject);
        }

        switch (character.attackTargetStrategy) {
            case (int)Character.TargetList.HighestPDAlly:
            case (int)Character.TargetList.LowestPDAlly:
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(PDIcon, targettingIconParent);
                break;
            case (int)Character.TargetList.HighestPDEnemy:
            case (int)Character.TargetList.LowestPDEnemy:
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(PDIcon, targettingIconParent);
                break;

            case (int)Character.TargetList.HighestMDAlly:
            case (int)Character.TargetList.LowestMDAlly:
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(MDIcon, targettingIconParent);
                break;
            case (int)Character.TargetList.HighestMDEnemy:
            case (int)Character.TargetList.LowestMDEnemy:
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(MDIcon, targettingIconParent);
                break;

            case (int)Character.TargetList.HighestHPAlly:
            case (int)Character.TargetList.LowestHPAlly:
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(HPIcon, targettingIconParent);
                break;

            case (int)Character.TargetList.HighestHPEnemy:
            case (int)Character.TargetList.LowestHPEnemy:
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(HPIcon, targettingIconParent);
                break;

            case (int)Character.TargetList.HighestINFAlly:
            case (int)Character.TargetList.LowestINFAlly:
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(INFIcon, targettingIconParent);
                break;

            case (int)Character.TargetList.HighestINFEnemy:
            case (int)Character.TargetList.LowestINFEnemy:
                //instantiate staticon as child of abilityTargettingIcon
                Instantiate(INFIcon, targettingIconParent);
                break;

            case (int)Character.TargetList.ClosestAlly:
                break;
            case (int)Character.TargetList.ClosestEnemy:
                break;

            default:
                Debug.Log("No appropriate strategy or closest");
                break;
        }
        targetSelectionTxt.gameObject.SetActive(true);
        targetSelectionTxt.text= TargetNames.getName((character.attackTargetStrategy));

        //Stretches the icon by setting the anchors of the children of targettingIconParent to stretch
        foreach (Transform child in targettingIconParent) {
            RectTransform iconRect = child.GetComponent<RectTransform>();
            iconRect.SetAnchorsStretch();
        }
    }
    private void handleColor(Character currChar) {
        //Sets the color to buff or debuff if there has been a significant change
        if (Mathf.Approximately(currChar.PD, currChar.zsPD))
            PD.color = ColorPalette.singleton.defaultColor;
        else if (currChar.PD > currChar.zsPD)
            PD.color = ColorPalette.singleton.buff;
        else
            PD.color = ColorPalette.singleton.debuff;

        if (Mathf.Approximately(currChar.MD, currChar.zsMD))
            MD.color = ColorPalette.singleton.defaultColor;
        else if (currChar.MD > currChar.zsMD)
            MD.color = ColorPalette.singleton.buff;
        else
            MD.color = ColorPalette.singleton.debuff;

        if (Mathf.Approximately(currChar.INF, currChar.zsINF))
            INF.color = ColorPalette.singleton.defaultColor;
        else if (currChar.INF > currChar.zsINF)
            INF.color = ColorPalette.singleton.buff;
        else
            INF.color = ColorPalette.singleton.debuff;

        if (Mathf.Approximately(currChar.AS, currChar.zsAS))
            AS.color = ColorPalette.singleton.defaultColor;
        else if (currChar.AS > currChar.zsAS)
            AS.color = ColorPalette.singleton.buff;
        else
            AS.color = ColorPalette.singleton.debuff;

        if (Mathf.Approximately(currChar.CDR, currChar.zsCDR))
            CDR.color = ColorPalette.singleton.defaultColor;
        else if (currChar.CDR > currChar.zsCDR)
            CDR.color = ColorPalette.singleton.buff;
        else
            CDR.color = ColorPalette.singleton.debuff;

        if (Mathf.Approximately(currChar.MS, currChar.zsMS))
            MS.color = ColorPalette.singleton.defaultColor;
        else if (currChar.MS > currChar.zsMS)
            MS.color = ColorPalette.singleton.buff;
        else
            MS.color = ColorPalette.singleton.debuff;

        if (Mathf.Approximately(currChar.Range, currChar.zsRange))
            RNG.color = ColorPalette.singleton.defaultColor;
        else if (currChar.Range > currChar.zsRange)
            RNG.color = ColorPalette.singleton.buff;
        else
            RNG.color = ColorPalette.singleton.debuff;

        if (Mathf.Approximately(currChar.LS, currChar.zsLS))
            LS.color = ColorPalette.singleton.defaultColor;
        else if (currChar.LS > currChar.zsLS)
            LS.color = ColorPalette.singleton.buff;
        else
            LS.color = ColorPalette.singleton.debuff;


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
        
        displayXPProgress(currChar);
        
    }

    public void displayXPProgress(Character currChar) {
        levelBar.fillAmount = (float)currChar.xpProgress / currChar.xpCap;
        //If no archetype was selected yet and reached Archetype Level, display "Select Archetype"
        if(!currChar.hasArchetype && currChar.level >= ARCHETYPESELECTLEVEL && !uiManager.zoneStarted()) {
            levelProgress.text = "Select Archetype";
            levelProgress.enableAutoSizing = false;
            levelProgress.fontSize = 60;
            return;
        }
        //If there are stat points available and zone hasn't started display "Upgrades Available"
        if (currChar.statPoints > 0 && !uiManager.zoneStarted()) {
            levelProgress.text = currChar.statPoints+" Upgrades Available";
            levelProgress.enableAutoSizing = false;
            levelProgress.fontSize = 60;
        }
        else {
            levelProgress.text = currChar.xpProgress + "/" + currChar.xpCap;
            levelProgress.enableAutoSizing = true;
        }
    }
    private void displayUpgradeStatsOrPickArchetype() {

        if (character.level >= ARCHETYPESELECTLEVEL && !character.hasArchetype && !uiManager.zoneStarted()) {
            focusElement = 8;
            startFocusing();

            
        }
        else { 
        focusElement = 7;
        startFocusing();
        }


    }
    private void displayInterestingStats(Character currChar) {
        totalKills.text = currChar.totalKills + "";
        totalDamage.text = currChar.totalDamage.ToString("F0");
    }
    public void close() {
        //destroys all ability displays
        for(int i = 0;i<abilityDisplays.Count;i++) {
            //Destroys the ability display then refills the placeholder with a background
            Destroy(abilityDisplays[i].gameObject);
            foreach(Transform child in abilityPlaceholders[i].transform) {
                child.gameObject.SetActive(true);
            }
        }
        abilityDisplays.Clear();
        abilities.Clear();
        //Hides all targetting
        foreach(RectTransform temp in abilityTargetting) {
              temp.gameObject.SetActive(false);
        }
        //targetSelector.targetSelection.SetActive(false);
        //movementSelector.gameObject.SetActive(false);
    }

    private void openPrimaryTargetSelector() {
        //clickable if the character is a player and the zone hasnt started yet
        if (character.team == (int)Character.teamList.Player && !uiManager.zoneStarted()) {
            focusElement = 5;
            startFocusing();
        }
    }

    //The onclick is set in the editor
    public void ability1Clicked() {
        //clickable if the character is a player and the zone hasnt started yet
        if (character.team == (int)Character.teamList.Player && !uiManager.zoneStarted()) { 
            focusElement = 0;
            startFocusing();
        }
    }

    public void ability2Clicked() {
        //clickable if the character is a player and the zone hasnt started yet.
        if (character.team == (int)Character.teamList.Player && !uiManager.zoneStarted()) { 
            focusElement = 1;
            startFocusing();
        }
    }

    public void ability3Clicked() {
        if (character.team == (int)Character.teamList.Player && !uiManager.zoneStarted()) {
            focusElement = 2;
            startFocusing();
        }
    }
    public void ability4Clicked() {
        if (character.team == (int)Character.teamList.Player && !uiManager.zoneStarted()) {
            focusElement = 3;
            startFocusing();
        }
    }

    public void ability5Clicked() {
        if (character.team == (int)Character.teamList.Player && !uiManager.zoneStarted()) {
            focusElement = 4;
            startFocusing();
        }
    }

    //displays the abilities in inventory when clicked
    private void addAbility() {
        focusElement = 6;
        startFocusing();
    }

    //Displays CDs in top left corner under topstatdisplay
    public void displayTopStatAbilities() {
        for (int i = 0; i < character.abilities.Count; i++) {
            topstatAbilityDisplays[i].gameObject.SetActive(true);
            topstatAbilityDisplaysBorder[i].gameObject.SetActive(true);
            topstatAbilityDisplaysFill[i].gameObject.SetActive(true);

            topstatAbilityDisplays[i].fillAmount = (character.abilities[i].getCDAfterChange() - character.abilities[i].abilityNext) / character.abilities[i].getCDAfterChange();
            topstatAbilityDisplays[i].color = ColorPalette.singleton.getIndicatorColor(character.abilities[i].abilityType);
            topstatAbilityDisplays[i].SetAlpha(0.8f);

            if (character.abilities[i].available) {
                topstatAbilityDisplaysBorder[i].color = Color.white;
                topstatAbilityDisplaysBorder[i].SetAlpha(.4f);
            }
            else {
                topstatAbilityDisplaysBorder[i].color = Color.grey;
                topstatAbilityDisplaysBorder[i].SetAlpha(0.4f);
            }

            topstatAbilityDisplaysFill[i].color = ColorPalette.singleton.getIndicatorColor(character.abilities[i].abilityType);
            topstatAbilityDisplaysFill[i].SetAlpha(0.4f);
        }
        //Sets the remaining to inactive
        for(int i = MAX_ABILITIES; i > character.abilities.Count; i--) {
            topstatAbilityDisplays[i-1].gameObject.SetActive(false);
            topstatAbilityDisplaysBorder[i-1].gameObject.SetActive(false);
            topstatAbilityDisplaysFill[i-1].gameObject.SetActive(false);
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
        if (uiManager.charInfoScreenHidden.hidden == false && !inventoryScreen)
            viewCharacterTopStatDisplay(character);

        if (!inventoryScreen && character!=null)
            displayTopStatAbilities();

        if (opening)
            time += Time.unscaledDeltaTime;
        if (closing)
            time -= Time.unscaledDeltaTime;


        if (opening || closing) {
            handlePanels();
        }

        if (time >= transitionTime) {
            //no longer in the process of opening(opening should be done)
            //Forcing the position since if we click on a character that is already apparent in top stat display before the screen is fully unhidden it will be in the wrong position(you can test it out by removing the 2 bottom lines then double clicking a character)
            if (opening) {
                hideUI.initPos.x = 0;
                hideUI.initPos.y = 0;
                opening = false;
            }
            //Tutorial stuff (Prompt to add ability)
            if (!uiManager.tutorial.addingAbilityTutorialDone && uiManager.tutorial.addingAbilityTutorialStep == 3 && opened) {
                uiManager.tutorial.continueAddingAbilityClickAddButton();
            }
            if(!uiManager.tutorial.upgradingStatsTutorialDone && uiManager.tutorial.upgradingStatsTutorialStep == 3 && opened) {
                uiManager.tutorial.continueUpgradingStatsClickOnStats();
            }
        }
        //no longer in process of closing
        if(time<=0)
            closing = false;

        //once closing is done unpause the game
        if (willHandlePause && time <= 0) {
            uiManager.pausePlay(uiManager.wasPause);
            //and remove the ability displays;
            close();
            willHandlePause = false;
        }

        if(focusing)
            time2 += Time.unscaledDeltaTime;
        if(unFocusing)
            time2 -= Time.unscaledDeltaTime;

        if(focusing || unFocusing) {
            handleFocusing();
        }

        if(time2 >= transitionTime) {
            //no longer in the process of focusing(Done focusing)
            focusing = false;
            //if the focus element is statUpgrading then create the abilityDisplays if it wasnt already created
            if(focusElement == 7 && !statUpgrading.createdAbilityDisplays) {
                statUpgrading.createAbilityDisplayStatDifferences();
                statUpgrading.createdAbilityDisplays = true;
                statUpgrading.focusAbilityIconHolder();
                if (!uiManager.tutorial.upgradingStatsTutorialDone && uiManager.tutorial.upgradingStatsTutorialStep == 4) {
                    uiManager.tutorial.continueUpgradingStatsExplainBriefly();
                }
            }
        }
        //no longer in process of unfocusing
        if (time2 <= 0)
            unFocusing = false;
        //once unfocusing is done deactivate the focus
        if(willHandleDeActivatingFocus && time2 <= 0) {
            uiManager.focus.transform.gameObject.SetActive(false);
            //reSets parent from focus to charInfoScreen
            int count = 0;
            foreach(RectTransform temp in abilityTargetting) {
                if (temp.transform.parent == uiManager.focus.transform) {
                    temp.transform.SetParent(this.abilitiesPanel.transform);
                    temp.SetAnchorBottom(abilityTargettingAnchorB[count]);
                    temp.SetAnchorTop(abilityTargettingAnchorT[count]);
                    temp.SetAnchorLeft(abilityTargettingAnchorL[count]);
                    temp.SetAnchorRight(abilityTargettingAnchorR[count]);

                    temp.SetLeft(0);
                    temp.SetRight(0);
                    temp.SetTop(0);
                    temp.SetBottom(0);
                }
                count++;
            }

            targetSelectionBtn.transform.SetParent(transform);
            xpPanel.transform.SetParent(transform);
            statsPanel.transform.SetParent(transform);

            statUpgrading.resetChangesBtn.transform.SetParent(transform);
            statUpgrading.applyChangesBtn.transform.SetParent(transform);

            statUpgrading.applyChangesBtn.gameObject.SetActive(false);
            statUpgrading.resetChangesBtn.gameObject.SetActive(false);

            statUpgrading.upgradeOptions.transform.SetParent(transform);
            statUpgrading.upgradeOptions.gameObject.SetActive(false);
            selectArchetype.transform.SetParent(transform);
            selectArchetype.gameObject.SetActive(false);
            healthBar.transform.SetParent(transform);
            setPanelStuffActive(true);
            //redisplays abilities
            displayCharacterAbilities(character);
            willHandleDeActivatingFocus = false;

            //Tutorial stuff
            if (!uiManager.tutorial.upgradingStatsTutorialDone && uiManager.tutorial.upgradingStatsTutorialStep>=15)
            {
                uiManager.tutorial.continueUpgradingStatsLastMessage();
            }
        }
        if (character != null) {
            if (character.team == (int)Character.teamList.Player) {
                targetSelectionBtn.interactable = true;
            }
            else
                targetSelectionBtn.interactable = false;
        }

        //Emphasizes the upgrade stats border when points are available or archetype select is available and the player has been taught how to upgrade stats
        if (character!=null && (character.statPoints > 0 || (character.level >= ARCHETYPESELECTLEVEL && !character.hasArchetype)) && uiManager.tutorial.upgradingStatsTutorialDone && opened) {
            float lerpFactor = Mathf.PingPong(Time.unscaledTime*1.5f, 1);
            statsBorder.color = Color.Lerp(upgradeStatsColorPingPong1, upgradeStatsColorPingPong2, lerpFactor);

            statsBorderRectTransform.SetLeft(Mathf.Lerp(-2,-15,lerpFactor));
            statsBorderRectTransform.SetRight(Mathf.Lerp(-2, -15, lerpFactor));
            statsBorderRectTransform.SetTop(Mathf.Lerp(-2, -15, lerpFactor));
            statsBorderRectTransform.SetBottom(Mathf.Lerp(-2, -15, lerpFactor));

            statsBorder.pixelsPerUnitMultiplier = Mathf.Lerp(1, 0.5f, lerpFactor);
        }
        else {
            statsBorder.color = upgradeStatsColorPingPong1;
            statsBorderRectTransform.SetLeft(-2);
            statsBorderRectTransform.SetRight(-2);
            statsBorderRectTransform.SetTop(-2);
            statsBorderRectTransform.SetBottom(-2);

            statsBorder.pixelsPerUnitMultiplier = 1;
        }

    }
}
