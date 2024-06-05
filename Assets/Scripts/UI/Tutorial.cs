using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public delegate void ButtonSetListener();
    public Image textBox;
    public TextMeshProUGUI text;
    //The 2 below buttons are used to move through the tutorial
    public Button nextBtn;
    public Button textBoxBtn;

    //Times used to focus and unfocus
    private float time;
    [SerializeField]private float transitionTime;
    private bool focusing;
    private bool unfocusing;

    //Holds the objects that will be focused on
    public List<Transform> objectsToBeFocused = new List<Transform>();
    //Saves the parent's of the objects that will be focused on so that they can be returned to their parent
    public List<Transform> objectsParents = new List<Transform>();
    //Saves the Object's index in it's parent
    public List<int> objectsIndex = new List<int>();

    //Explains How to Drag Character into map
    public bool draggingCharactersTutorialDone;
    [SerializeField] private GameObject characterPlacingScreen;
    
    public bool chooseRewardTutorialDone;
    [SerializeField] private GameObject chooseRewardScreen;

    public bool addingAbilityTutorialDone;
    //Which step in the tutorial we're in. 
    public int addingAbilityTutorialStep;
    [SerializeField] private CharacterInfoScreen characterInfoScreen;

    public bool upgradingStatsTutorialDone;

    public int upgradingStatsTutorialStep;
    //Explains how to drag characters into the map

    //Just to avoid going through tutorial for now
    private void Start() {
        //Make all bools true
        draggingCharactersTutorialDone = true;
        chooseRewardTutorialDone = true;
        addingAbilityTutorialDone = true;
        upgradingStatsTutorialDone = true;
        SaveSystem.saveTutorialProgress(this);

    }
    public void beginDraggingCharactersTutorial() {
        if (draggingCharactersTutorialDone)
            return;

        gameObject.SetActive(true);

        nextBtn.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);

        objectsToBeFocused.Add(characterPlacingScreen.transform);
        focus();

        positionTextBox(0.6f,0.7f,0.2f,0.8f);
        text.text = "Tap and hold your character, then drag it into the dotted area on the map";

        //To Unfocus Have to drag characterDisplay
    }
    public void endDraggingCharactersTutorial() {
        unfocusing = true;
        draggingCharactersTutorialDone = true;

        SaveSystem.saveTutorialProgress(this);
    }

    public void beginChooseRewardTutorial() {
        if (chooseRewardTutorialDone)
            return;

        gameObject.SetActive(true);

        nextBtn.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);

        objectsToBeFocused.Add(chooseRewardScreen.transform);
        focus();

        //Positions it above reward screen
        RectTransform rt = chooseRewardScreen.GetComponent<RectTransform>();
        positionTextBox(rt.GetAnchorTop(),rt.GetAnchorTop()*1.2f, 0.2f, 0.8f);
        text.text = "Pick a reward";

        RewardSelectAbility rewardSelect = chooseRewardScreen.GetComponent<RewardSelectAbility>();
        foreach(AbilityDisplayReward reward in rewardSelect.listAbilityReward) {
            AddListener(reward.self, endChooseRewardTutorial);
        }
    }

    public void endChooseRewardTutorial() {
        unfocusing = true;
        chooseRewardTutorialDone = true;

        SaveSystem.saveTutorialProgress(this);

    }

    public void beginAddingAbilityTutorial() {
        //If there are no abilities to add then don't show tutorial
        if (addingAbilityTutorialDone || UIManager.singleton.playerParty.abilityInventory.transform.childCount == 0)
            return;

        addingAbilityTutorialStep = 1;
        gameObject.SetActive(true);

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);
        positionTextBox(0.6f, 0.7f, 0.2f, 0.8f);
        text.text = "Now, I will teach you how to add abilities to your characters";

        focus();

        //Jumps to next part of tutorial
        SetListener(continueAddingAbilityClickPortrait);

        
    }
    //The continuation of the adding ability tutorial. (Focuses character portrait and prompts to click it)
    private void continueAddingAbilityClickPortrait() {
        addingAbilityTutorialStep = 2;
        nextBtn.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);
        positionTextBox(0.5f, 0.6f, 0.2f, 0.8f);
        text.text = "Select a character to add an ability to";
        //Focuses the character placing screen
        objectsToBeFocused.Add(characterPlacingScreen.transform);
        focus();
        //Character display jumps to next part of tutorial
    }
    public void conitnueAddingAbilityClickTopStatDisplay() {
        addingAbilityTutorialStep = 3;
        returnToParents();

        nextBtn.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);
        positionTextBox(0.7f, 0.8f, 0.2f, 0.8f);
        text.text = "Click on the character information display to open the character screen";

        nextBtn.onClick.RemoveAllListeners();
        textBoxBtn.onClick.RemoveAllListeners();

        objectsToBeFocused.Add(characterInfoScreen.transform);

        focus();

    }
    public void continueAddingAbilityClickAddButton() {
        addingAbilityTutorialStep = 4;
        returnToParents();

        nextBtn.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);

        text.text = "Click on the add button";
        //Focuses the availabile ability display placeholder and the add Button
        objectsToBeFocused.Add(characterInfoScreen.abilityPlaceholders[characterInfoScreen.character.abilities.Count]);
        objectsToBeFocused.Add(characterInfoScreen.addAbilityBtn.transform);

        focus();

        //Adds a listener to the add button to proceed with tutorial
        characterInfoScreen.addAbilityBtn.onClick.AddListener(continueAddingAbilityClickAbilityDisplay);
    }

    public void continueAddingAbilityClickAbilityDisplay() {
        //Removes the listener that was added
        characterInfoScreen.addAbilityBtn.onClick.RemoveListener(continueAddingAbilityClickAbilityDisplay);
        addingAbilityTutorialStep = 5;
        returnToParents();

        nextBtn.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);

        text.text = "Click on the ability you want to add to the character";
        focus();
        //the ability display to be added is already focused from another function

    }

    public void continueAddingAbilityCloseCharacterInfoScreen() {
        addingAbilityTutorialStep = 6;
        returnToParents();

        nextBtn.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);

        text.text = "Now you can close the character screen and return to combat";
        objectsToBeFocused.Add(characterInfoScreen.closeFullScreenBtn.transform);
        focus();

        characterInfoScreen.closeFullScreenBtn.onClick.AddListener(endAddingAbilityTutorial);
        characterInfoScreen.unFocusing = false;
    }
    private void endAddingAbilityTutorial() {
        //remove listener from close button
        characterInfoScreen.closeFullScreenBtn.onClick.RemoveListener(endAddingAbilityTutorial);
        returnToParents();

        addingAbilityTutorialDone = true;
        unfocusing = true;

        SaveSystem.saveTutorialProgress(this);

    }
    #region upgradingStatsTutorial
    //Decides wether to show the upgrading stats tutorial
    //It Will show if the player has one character with 6 or more stat points
    public bool showUpgradingStatTutorial() {
        foreach(Transform child in UIManager.singleton.playerParty.transform) {
            if(child.tag == "Character") {
                Character character = child.GetComponent<Character>();
                if(character.statPoints >= 6) {
                    return true;
                }
            }
        }
        return false;

    }
    public void beginUpgradingStatsTutorial() {
        if (upgradingStatsTutorialDone || !showUpgradingStatTutorial())
            return;

        upgradingStatsTutorialStep = 1;
        gameObject.SetActive(true);

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.6f, 0.7f, 0.2f, 0.8f);
        text.text = "Did you know as a character levels up they gain upgrade points?";

        focus();

        SetListener(continueUpgradingStatsTellAboutNotification);
    }

    private void continueUpgradingStatsTellAboutNotification() {
        upgradingStatsTutorialStep = 2;

        nextBtn.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.4f, 0.6f, 0.2f, 0.8f);
        text.text = "When a character has upgrade points to spend there will be an indicator over their portrait.\n Click a character with upgrade points";

        objectsToBeFocused.Add(characterPlacingScreen.transform);
        focus();

        //Clicking a characterDisplay with more than 6 stat points will jump to next part of tutorial
    }

    public void continueUpgradingStatsClickTopStatDisplay() {
        upgradingStatsTutorialStep = 3;
        returnToParents();

        nextBtn.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.7f, 0.8f, 0.2f, 0.8f);
        text.text = "Click on the character information display to open the character screen";

        RemoveListeners();

        objectsToBeFocused.Add(characterInfoScreen.transform);

        focus();

    }
    public void continueUpgradingStatsClickOnStats() {
        upgradingStatsTutorialStep = 4;
        returnToParents();

        nextBtn.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.4f, 0.5f, 0.2f, 0.8f);
        text.text = "Click on your stats to start upgrading";

        objectsToBeFocused.Add(characterInfoScreen.statsPanel);
        objectsToBeFocused.Add(characterInfoScreen.xpPanel);
        objectsToBeFocused.Add(characterInfoScreen.healthBar.transform);

        focus();

        //This is called when stats are focused

    }
    public void continueUpgradingStatsExplainBriefly() {
        upgradingStatsTutorialStep = -1;
        returnToParents();

        ////Turns off all layoutgroups
        //characterInfoScreen.statUpgrading.iconLayoutGroup1.enabled = false;
        //characterInfoScreen.statUpgrading.iconLayoutGroup2.enabled = false;
        //characterInfoScreen.statUpgrading.numberLayoutGroup1.enabled = false;
        //characterInfoScreen.statUpgrading.numberLayoutGroup2.enabled = false;
        //characterInfoScreen.statUpgrading.textLayoutGroup1.enabled = false;
        //characterInfoScreen.statUpgrading.textLayoutGroup2.enabled = false;
        //characterInfoScreen.statUpgrading.column1.enabled = false;
        //characterInfoScreen.statUpgrading.column2.enabled = false;

        //characterInfoScreen.statUpgrading.applyChangesBtn.interactable = false;

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.4f, 0.5f, 0.2f, 0.8f);
        text.text = "I will now briefly explain every stat's main function";


        focus();

        //This is called when stats are focused
        SetListener(continueUpgradingStatsExplainPwr);
    }
    public void continueUpgradingStatsExplainPwr() {
        

        upgradingStatsTutorialStep = 5;
        returnToParents();

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.4f, 0.5f, 0.2f, 0.8f);
        text.text = "Power: Regular attacks' damage";

        objectsToBeFocused.Add(characterInfoScreen.statUpgrading.PDIcon.transform);
        objectsToBeFocused.Add(characterInfoScreen.PD.transform);

        focus();

        SetListener(continueUpgradingStatsExplainMgc);
    }

    private void continueUpgradingStatsExplainMgc() { 
        upgradingStatsTutorialStep = 6;
        returnToParents();

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.4f, 0.5f, 0.2f, 0.8f);
        text.text = "Magic: Magical capabilities";


        objectsToBeFocused.Add(characterInfoScreen.statUpgrading.MDIcon.transform);
        objectsToBeFocused.Add(characterInfoScreen.MD.transform);

        focus();

        SetListener(continueUpgradingStatsExplainInf);
    }

    private void continueUpgradingStatsExplainInf() {
        upgradingStatsTutorialStep = 7;
        returnToParents();

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.4f, 0.5f, 0.2f, 0.8f);
        text.text = "Influence: Healing, buffs, and debuffs effectiveness";

        objectsToBeFocused.Add(characterInfoScreen.statUpgrading.INFIcon.transform);
        objectsToBeFocused.Add(characterInfoScreen.INF.transform);

        focus();

        SetListener(continueUpgradingStatsExplainAS);
    }
    private void continueUpgradingStatsExplainAS() {
        upgradingStatsTutorialStep = 8;
        returnToParents();

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.4f, 0.5f, 0.2f, 0.8f);
        text.text = "Attack Speed: The frequency of regular attacks";


        objectsToBeFocused.Add(characterInfoScreen.statUpgrading.ASIcon.transform);
        objectsToBeFocused.Add(characterInfoScreen.AS.transform);


        focus();

        SetListener(continueUpgradingStatsExplainCDR);
    }

    private void continueUpgradingStatsExplainCDR() {
        upgradingStatsTutorialStep = 9;
        returnToParents();

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.4f, 0.5f, 0.2f, 0.8f);
        text.text = "Cooldown Reduction: Reduces the cooldown of abilities";

        objectsToBeFocused.Add(characterInfoScreen.statUpgrading.CDRIcon.transform);
        objectsToBeFocused.Add(characterInfoScreen.CDR.transform);

        focus();

        SetListener(continueUpgradingStatsExplainSPD);
    }
    
    private void continueUpgradingStatsExplainSPD() {
        upgradingStatsTutorialStep = 10;
        returnToParents();

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.4f, 0.5f, 0.2f, 0.8f);
        text.text = "Speed: Movement speed";

        objectsToBeFocused.Add(characterInfoScreen.statUpgrading.MSIcon.transform);
        objectsToBeFocused.Add(characterInfoScreen.MS.transform);


        focus();

        SetListener(continueUpgradingStatsExplainRNG);
    }  
    private void continueUpgradingStatsExplainRNG() {
        upgradingStatsTutorialStep = 11;
        returnToParents();

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.4f, 0.5f, 0.2f, 0.8f);
        text.text = "Range: The distance of regular attacks";

        objectsToBeFocused.Add(characterInfoScreen.statUpgrading.RNGIcon.transform);
        objectsToBeFocused.Add(characterInfoScreen.RNG.transform);

        focus();

        SetListener(continueUpgradingStatsExplainLS);
    }

    private void continueUpgradingStatsExplainLS() {
        upgradingStatsTutorialStep = 12;
        returnToParents();

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.4f, 0.5f, 0.2f, 0.8f);
        text.text = "Lifesteal: The percentage of damage dealt that is returned as health (more effective on regular attacks)";

        objectsToBeFocused.Add(characterInfoScreen.statUpgrading.LSIcon.transform);
        objectsToBeFocused.Add(characterInfoScreen.LS.transform);

        focus();

        SetListener(continueUpgradingStatsExplainHP);
    }

    private void continueUpgradingStatsExplainHP() {
        upgradingStatsTutorialStep = 13;
        returnToParents();

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.35f, 0.45f, 0.2f, 0.8f);
        text.text = "Health: The amount of damage you can take before dying";

        objectsToBeFocused.Add(characterInfoScreen.healthBar.transform);
        //characterInfoScreen.statUpgrading.addHP.interactable = false;
        //characterInfoScreen.statUpgrading.subHP.interactable = false;

        focus();

        SetListener(continueUpgradingStatsExplainHowAbilitiesScale);
    }
    
    private void continueUpgradingStatsExplainHowAbilitiesScale() {
        upgradingStatsTutorialStep = 14;
        returnToParents();

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.6f, 0.7f, 0.2f, 0.8f);
        text.text = "These stats also amplify abilities. You can see what stats amplify an ability by looking at the icons on the bottom of the ability";

        focus();

        SetListener(continueUpgradingStatsTellWhatToUpgradeBasedOnAbility);
    }
    //If the player only has one character(or the first character) has one of the first 3 abilities unlocked, tell to add stats based on what that ability requires
    //^ This is what is most likely to always happen if the tutorial is followed correctly. Otherwise just end the tutorial
    private void continueUpgradingStatsTellWhatToUpgradeBasedOnAbility() {
        upgradingStatsTutorialStep = 15;
        returnToParents();

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);


        positionTextBox(0.6f, 0.7f, 0.2f, 0.8f);
        text.text = "Now we will upgrade our stats to amplify our ability";
        //If the first character has as the first ability one of the 3 initial abilities given in tutorial, take the respective tutorial path
        //Otherwise end the upgrading stats tutorial
        foreach(Transform child in UIManager.singleton.playerParty.transform) {
            //Gets first character's first ability
            Character character = child.GetComponent<Character>();
            if (character != null) {
                Ability ability = character.abilities[0];

                if (ability != null) {
                    switch (ability.abilityName) {
                        case "Stab":
                            pwrLeftToAdd = 4;
                            hpLeftToAdd = 2;
                            SetListener(continueUpgradingStatsStabAbility);
                            //AddListener(characterInfoScreen.statUpgrading.addPD, continueUpgradingStatsStabAbility);
                            //AddListener(characterInfoScreen.statUpgrading.addHP, continueUpgradingStatsStabAbility);
                            break;

                        case "Magic Hit":
                            mgcLeftToAdd = 4;
                            hpLeftToAdd = 2;
                            SetListener(continueUpgradingStatsMagicHitAbility);
                            //AddListener(characterInfoScreen.statUpgrading.addMD, continueUpgradingStatsMagicHitAbility);
                            //AddListener(characterInfoScreen.statUpgrading.addHP, continueUpgradingStatsMagicHitAbility);
                            break;

                        case "Healing Aura":
                            mgcLeftToAdd = 2;
                            infLeftToAdd = 2;
                            hpLeftToAdd = 2;
                            SetListener(continueUpgradingStatsHealAbility);
                            //AddListener(characterInfoScreen.statUpgrading.addHP, continueUpgradingStatsHealAbility);
                            //AddListener(characterInfoScreen.statUpgrading.addMD, continueUpgradingStatsHealAbility);
                            //AddListener(characterInfoScreen.statUpgrading.addINF, continueUpgradingStatsHealAbility);
                            break;
                        default:
                            continueUpgradingStatsLastMessage();
                            break;
                    }
                }
                else
                    continueUpgradingStatsLastMessage();

                break;
            }
        }


        focus();

    }
    public int pwrLeftToAdd;
    public int hpLeftToAdd;
    public int mgcLeftToAdd;
    public int infLeftToAdd;
    private void continueUpgradingStatsStabAbility() {

        upgradingStatsTutorialStep = 16;
        returnToParents();

        RemoveListeners();

        nextBtn.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);
        if (pwrLeftToAdd>0) {
            objectsToBeFocused.Add(characterInfoScreen.statUpgrading.PDIcon.transform);
            objectsToBeFocused.Add(characterInfoScreen.PD.transform);
            //objectsToBeFocused.Add(characterInfoScreen.statUpgrading.addPD.transform);
            pwrLeftToAdd--;
            text.text = "Upgrade power for higher Stab and regular attack damage";
        }//Has to be more than one so that it correctly goes to Else.
        else if (hpLeftToAdd>0) {
            //characterInfoScreen.statUpgrading.addHP.interactable = true;
            //characterInfoScreen.statUpgrading.subHP.interactable = true;
            objectsToBeFocused.Add(characterInfoScreen.healthBar.transform);
            hpLeftToAdd--;
            text.text = "Upgrade health for some increased defenses";
        }
        else
            continueUpgradingStatsConfirm();

        positionTextBox(0.15f, 0.25f, 0.2f, 0.8f);

        focus();
    }
    private void continueUpgradingStatsHealAbility() {
        upgradingStatsTutorialStep = 16;
        returnToParents();

        RemoveListeners();

        nextBtn.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);
        if (mgcLeftToAdd>0) {
            objectsToBeFocused.Add(characterInfoScreen.statUpgrading.MDIcon.transform);
            objectsToBeFocused.Add(characterInfoScreen.MD.transform);
            //objectsToBeFocused.Add(characterInfoScreen.statUpgrading.addMD.transform);
            mgcLeftToAdd--;
            text.text = "Upgrade magic for a stronger heal";
        }
        else if (infLeftToAdd>0) {
            objectsToBeFocused.Add(characterInfoScreen.statUpgrading.INFIcon.transform);
            objectsToBeFocused.Add(characterInfoScreen.INF.transform);
            //objectsToBeFocused.Add(characterInfoScreen.statUpgrading.addINF.transform);
            infLeftToAdd--;
            text.text = "Upgrade influence for a stronger heal";
        }
        else if (hpLeftToAdd>0) {
            //characterInfoScreen.statUpgrading.addHP.interactable = true;
            //characterInfoScreen.statUpgrading.subHP.interactable = true;
            objectsToBeFocused.Add(characterInfoScreen.healthBar.transform);
            hpLeftToAdd--;
            text.text = "Upgrade health for some increased defenses and a stronger heal";
        }
        else
            continueUpgradingStatsConfirm();

        positionTextBox(0.15f, 0.25f, 0.2f, 0.8f);

        focus();
    }

    private void continueUpgradingStatsMagicHitAbility() {
        upgradingStatsTutorialStep = 16;
        returnToParents();

        RemoveListeners();

        nextBtn.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);
        if (mgcLeftToAdd>0) {
            objectsToBeFocused.Add(characterInfoScreen.statUpgrading.MDIcon.transform);
            objectsToBeFocused.Add(characterInfoScreen.MD.transform);
            //objectsToBeFocused.Add(characterInfoScreen.statUpgrading.addMD.transform);
            mgcLeftToAdd--;
            text.text = "Upgrade magic for increased Magic damage";
        }
        else if (hpLeftToAdd>0) {
            //characterInfoScreen.statUpgrading.addHP.interactable = true;
            //characterInfoScreen.statUpgrading.subHP.interactable = true;
            objectsToBeFocused.Add(characterInfoScreen.healthBar.transform);
            hpLeftToAdd--;
            text.text = "Upgrade health for some increased defenses";
        }
        else
            continueUpgradingStatsConfirm();

        positionTextBox(0.15f, 0.25f, 0.2f, 0.8f);

        focus();
    }
    //Prompt to click confirm
    private void continueUpgradingStatsConfirm() {
        //characterInfoScreen.statUpgrading.addPD.onClick.RemoveListener(continueUpgradingStatsStabAbility);
        //characterInfoScreen.statUpgrading.addHP.onClick.RemoveListener(continueUpgradingStatsStabAbility);

        //characterInfoScreen.statUpgrading.addHP.onClick.RemoveListener(continueUpgradingStatsHealAbility);
        //characterInfoScreen.statUpgrading.addMD.onClick.RemoveListener(continueUpgradingStatsHealAbility);
        //characterInfoScreen.statUpgrading.addINF.onClick.RemoveListener(continueUpgradingStatsHealAbility);

        //characterInfoScreen.statUpgrading.addMD.onClick.RemoveListener(continueUpgradingStatsMagicHitAbility);
        //characterInfoScreen.statUpgrading.addHP.onClick.RemoveListener(continueUpgradingStatsMagicHitAbility);


        upgradingStatsTutorialStep = 17;
        returnToParents();

        nextBtn.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.4f, 0.5f, 0.2f, 0.8f);
        text.text = "Click confirm to apply the upgrades";

        //objectsToBeFocused.Add(characterInfoScreen.statUpgrading.applyChangesBtn.transform);

        //turns on all layoutgroups
        //characterInfoScreen.statUpgrading.iconLayoutGroup1.enabled = true;
        //characterInfoScreen.statUpgrading.iconLayoutGroup2.enabled = true;
        //characterInfoScreen.statUpgrading.numberLayoutGroup1.enabled = true;
        //characterInfoScreen.statUpgrading.numberLayoutGroup2.enabled = true;
        //characterInfoScreen.statUpgrading.textLayoutGroup1.enabled = true;
        //characterInfoScreen.statUpgrading.textLayoutGroup2.enabled = true;
        //characterInfoScreen.statUpgrading.column1.enabled = true;
        //characterInfoScreen.statUpgrading.column2.enabled = true;

        //characterInfoScreen.statUpgrading.addHP.interactable = true;
        //characterInfoScreen.statUpgrading.subHP.interactable = true;

        //characterInfoScreen.statUpgrading.applyChangesBtn.interactable = true;

        focus();
    }

    public void continueUpgradingStatsLastMessage() {
        //This will be called after characterInfoScreen is unfocused(After confirming the upgrades)
        upgradingStatsTutorialStep = 18;
        returnToParents();

        nextBtn.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);

        positionTextBox(0.4f, 0.5f, 0.2f, 0.8f);
        text.text = "You can always come back to upgrade your characters if they have upgrade points.\nFeel free to experiment with different stats !";

        SetListener(endUpgradingStatsTutorial);
        focus();
    }

    private void endUpgradingStatsTutorial() {
        returnToParents();

        upgradingStatsTutorialDone = true;
        unfocusing = true;

        SaveSystem.saveTutorialProgress(this);
    }
    #endregion
    private void positionTextBox(float bottomAnchor, float topAnchor, float leftAnchor, float rightAnchor) {
        textBox.rectTransform.SetAnchorBottom(bottomAnchor);
        textBox.rectTransform.SetAnchorTop(topAnchor);
        textBox.rectTransform.SetAnchorLeft(leftAnchor);
        textBox.rectTransform.SetAnchorRight(rightAnchor);
    }
    //Sets the buttos' listeners removing all others
    private void SetListener(ButtonSetListener listener) {
        nextBtn.onClick.RemoveAllListeners();
        textBoxBtn.onClick.RemoveAllListeners();

        nextBtn.onClick.AddListener(() => listener());
        textBoxBtn.onClick.AddListener(() => listener());
    }
    private void RemoveListeners() {
        nextBtn.onClick.RemoveAllListeners();
        textBoxBtn.onClick.RemoveAllListeners();
    }
    //Adds a listener without removing all others
    private void AddListener(Button button, ButtonSetListener listener) {
        button.onClick.AddListener(() => listener());
    }
    private void focus() {
        UIManager.singleton.focus.gameObject.SetActive(true);
        //We're doing 2 seperate loops to prevent bugs that happen when we are focusing 2 objects of the same parent (issue with the index)
        foreach(Transform t in objectsToBeFocused) {
            objectsParents.Add(t.parent);
            objectsIndex.Add(t.GetSiblingIndex());
        }
        foreach (Transform t in objectsToBeFocused) {
            t.SetParent(UIManager.singleton.focus.transform);
        }
        focusing = true;

        textBox.transform.SetParent(UIManager.singleton.focus.transform);
        nextBtn.transform.SetParent(UIManager.singleton.focus.transform);
    }
    private void returnToParents() {
        foreach(Transform t in objectsToBeFocused) {
            t.SetParent(objectsParents[objectsToBeFocused.IndexOf(t)]);
            t.SetSiblingIndex(objectsIndex[objectsToBeFocused.IndexOf(t)]);
        }
        objectsToBeFocused.Clear();
        objectsParents.Clear();
        objectsIndex.Clear();
        //Reset's it's parent
        textBox.transform.SetParent(transform);
        nextBtn.transform.SetParent(transform);

    }
    private void unfocus() {
        UIManager.singleton.focus.gameObject.SetActive(false);
        returnToParents();
        gameObject.SetActive(false);
    }

    private void Update() {
            

        if (focusing) {
            time += Time.unscaledDeltaTime;
            UIManager.singleton.focus.SetAlpha(Mathf.Lerp(0, UIManager.singleton.focusOpacity, time / transitionTime));
            //Once focusing is done
            if (time >= transitionTime) {
                focusing = false;
                time = transitionTime;
            }
        }

        if (unfocusing) {
            time -= Time.unscaledDeltaTime;
            UIManager.singleton.focus.SetAlpha(Mathf.Lerp(0, UIManager.singleton.focusOpacity, time / transitionTime));
            //Once unfocusing is done
            if (time <= 0) {
                unfocusing = false;
                time = 0;
                unfocus();
            }
        }

    }
}
