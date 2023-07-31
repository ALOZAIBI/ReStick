using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Attack Target selector Deprecated use this
public class TargetSelector : MonoBehaviour
{
    //Wether this is selecting highest or lowest
    public bool highest;

    public bool isAbilityTargetSelector;

    CharacterInfoScreen characterInfoScreen;

    [SerializeField]private Ability ability;

    [SerializeField]private Button toggle;

    [SerializeField] private Button removeAbilityBtn;


    [SerializeField]private RectTransform toggleTransform;

    [SerializeField]private TextMeshProUGUI highestText;
    [SerializeField]private TextMeshProUGUI lowestText;

    [SerializeField]private float yPositionHi;
    [SerializeField]private float yPositionLo;

    [SerializeField]private float time;
    [SerializeField]private float transitionTime;

    [SerializeField] private Button PDEnemy;
    [SerializeField] private Button MDEnemy;
    [SerializeField] private Button INFEnemy;
    [SerializeField] private Button HPEnemy;
    [SerializeField] private Button ClosestEnemy;
    //Ally options should be hidden for primarytargeting
    [SerializeField] private Button PDAlly;
    [SerializeField] private Button MDAlly;
    [SerializeField] private Button INFAlly;
    [SerializeField] private Button HPAlly;
    [SerializeField] private Button ClosestAlly;

    [SerializeField] private GameObject icons;

    private const float selectedAlpha = 1;
    private const float deselectedAlpha = 0.4f;

    void Start()
    {
        toggle.onClick.AddListener(toggleClicked);
        PDEnemy.onClick.AddListener(selectPDEnemy);
        MDEnemy.onClick.AddListener(selectMDEnemy);
        INFEnemy.onClick.AddListener(selectINFEnemy);
        HPEnemy.onClick.AddListener(selectHPEnemy);
        ClosestEnemy.onClick.AddListener(selectClosestEnemy);

        PDAlly.onClick.AddListener(selectPDAlly);
        MDAlly.onClick.AddListener(selectMDAlly);
        INFAlly.onClick.AddListener(selectINFAlly);
        HPAlly.onClick.AddListener(selectHPAlly);
        ClosestAlly.onClick.AddListener(selectClosestAlly);

        removeAbilityBtn.onClick.AddListener(removeAbility);

    }
    private void toggleClicked() {
        highest = !highest;
        updateToggleView();
    }
    //if without parameter then this is not ability target selector
    public void setupTargetSelector(CharacterInfoScreen characterInfoScreen) {
        this.characterInfoScreen = characterInfoScreen;
        isAbilityTargetSelector = false;
        this.ability = null;
        removeAbilityBtn.gameObject.SetActive(false);
        if(characterInfoScreen.character.attackTargetStrategy == (int)Character.TargetList.HighestPDEnemy
            || characterInfoScreen.character.attackTargetStrategy == (int)Character.TargetList.HighestMDEnemy
            || characterInfoScreen.character.attackTargetStrategy == (int)Character.TargetList.HighestINFEnemy
            || characterInfoScreen.character.attackTargetStrategy == (int)Character.TargetList.HighestHPEnemy
            || characterInfoScreen.character.attackTargetStrategy == (int)Character.TargetList.HighestPDAlly
            || characterInfoScreen.character.attackTargetStrategy == (int)Character.TargetList.HighestMDAlly
            || characterInfoScreen.character.attackTargetStrategy == (int)Character.TargetList.HighestINFAlly
            || characterInfoScreen.character.attackTargetStrategy == (int)Character.TargetList.HighestHPAlly 
            ) {
            highest = true;
        }
        else {
            highest = false;
        }
        updateToggleView();
    }
    //With parameter so this is ability target selecotr
    public void setupTargetSelector(Ability ability,CharacterInfoScreen characterInfoScreen) {
        this.characterInfoScreen = characterInfoScreen;
        isAbilityTargetSelector = true;
        this.ability = ability;
        removeAbilityBtn.gameObject.SetActive(true);
        if (ability.hasTarget) {
            showTargettingButtons(true);

            if (ability.targetStrategy == (int)Character.TargetList.HighestPDEnemy
                || ability.targetStrategy == (int)Character.TargetList.HighestMDEnemy
                || ability.targetStrategy == (int)Character.TargetList.HighestINFEnemy
                || ability.targetStrategy == (int)Character.TargetList.HighestHPEnemy
                || ability.targetStrategy == (int)Character.TargetList.HighestPDAlly
                || ability.targetStrategy == (int)Character.TargetList.HighestMDAlly
                || ability.targetStrategy == (int)Character.TargetList.HighestINFAlly
                || ability.targetStrategy == (int)Character.TargetList.HighestHPAlly
                ) {
                highest = true;
            }
            else {
                highest = false;
            }
            updateToggleView();
        }
        else {
            showTargettingButtons(false);
        }
    }
    private void showTargettingButtons(bool show) {
        PDEnemy.gameObject.SetActive(show);
        MDEnemy.gameObject.SetActive(show);
        INFEnemy.gameObject.SetActive(show);
        HPEnemy.gameObject.SetActive(show);
        ClosestEnemy.gameObject.SetActive(show);
        PDAlly.gameObject.SetActive(show);
        MDAlly.gameObject.SetActive(show);
        INFAlly.gameObject.SetActive(show);
        HPAlly.gameObject.SetActive(show);
        ClosestAlly.gameObject.SetActive(show);
        icons.SetActive(show);
    }
    private void updateToggleView() {
        if (highest) {
            toggleTransform.localPosition = new Vector3(toggleTransform.localPosition.x, yPositionHi, toggleTransform.localPosition.z);
            highestText.SetAlpha(selectedAlpha);
            lowestText.SetAlpha(deselectedAlpha);
            updateText(true);
        }
        else {
            toggleTransform.localPosition = new Vector3(toggleTransform.localPosition.x, yPositionLo, toggleTransform.localPosition.z);
            highestText.SetAlpha(deselectedAlpha);
            lowestText.SetAlpha(selectedAlpha);
            updateText(false);
        }
    }
    private void updateText(bool highest) {

        if (highest) {
            PDEnemy.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.HighestPDEnemy);
            MDEnemy.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.HighestMDEnemy);
            INFEnemy.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.HighestINFEnemy);
            HPEnemy.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.HighestHPEnemy);
            ClosestEnemy.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.ClosestEnemy);

            PDAlly.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.HighestPDAlly);
            MDAlly.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.HighestMDAlly);
            INFAlly.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.HighestINFAlly);
            HPAlly.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.HighestHPAlly);
            ClosestAlly.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.ClosestAlly);
        }
        else {

            PDEnemy.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.LowestPDEnemy);
            MDEnemy.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.LowestMDEnemy);
            INFEnemy.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.LowestINFEnemy);
            HPEnemy.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.LowestHPEnemy);
            ClosestEnemy.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.ClosestEnemy);

            PDAlly.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.LowestPDAlly);
            MDAlly.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.LowestMDAlly);
            INFAlly.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.LowestINFAlly);
            HPAlly.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.LowestHPAlly);
            ClosestAlly.GetComponentInChildren<TextMeshProUGUI>().text = TargetNames.getName((int)Character.TargetList.ClosestAlly);
        }
    }

    //Updates the focus Element's targetting view
    private void updateTargettingView() {
        if (isAbilityTargetSelector) {
            //Get it's index within charinfoscreen
            int index = characterInfoScreen.abilities.IndexOf(ability);
            //Updates the targetting view
            characterInfoScreen.updateAbilityTargettingView(ability,index);
        }
        else {
            characterInfoScreen.updatePrimaryTargettingView();
        }

    }
    #region buttonFunctions
    private void selectPDEnemy() {
        if (isAbilityTargetSelector) {
            if(highest) {
                ability.targetStrategy = (int)Character.TargetList.HighestPDEnemy;
            }
            else {
                ability.targetStrategy = (int)Character.TargetList.LowestPDEnemy;
            }
        }
        else {

            if (highest) {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.HighestPDEnemy;
            }
            else {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.LowestPDEnemy;
            }
        }
        updateTargettingView();
    }

    private void selectMDEnemy() {
        if (isAbilityTargetSelector) {
            if (highest) {
                ability.targetStrategy = (int)Character.TargetList.HighestMDEnemy;
            }
            else {
                ability.targetStrategy = (int)Character.TargetList.LowestMDEnemy;
            }
        }
        else {
            if(highest) {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.HighestMDEnemy;
            }
            else {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.LowestMDEnemy;
            }
        }
        updateTargettingView();
    }

    private void selectINFEnemy() {
        if (isAbilityTargetSelector) {
            if (highest) {
                ability.targetStrategy = (int)Character.TargetList.HighestINFEnemy;
            }
            else {
                ability.targetStrategy = (int)Character.TargetList.LowestINFEnemy;
            }
        }
        else {
            if (highest) {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.HighestINFEnemy;
            }
            else {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.LowestINFEnemy;
            }
        }
        updateTargettingView();
    }

    private void selectHPEnemy() {
        if (isAbilityTargetSelector) {
            if (highest) {
                ability.targetStrategy = (int)Character.TargetList.HighestHPEnemy;
            }
            else {
                ability.targetStrategy = (int)Character.TargetList.LowestHPEnemy;
            }
        }
        else {
            if(highest) {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.HighestHPEnemy;
            }
            else {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.LowestHPEnemy;
            }
        }
        updateTargettingView();
    }

    private void selectClosestEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.ClosestEnemy;
        }
        else {
            characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.ClosestEnemy;
        }
        updateTargettingView();
    }

    private void selectPDAlly() {
        if (isAbilityTargetSelector) {
            if (highest) {
                ability.targetStrategy = (int)Character.TargetList.HighestPDAlly;
            }
            else {
                ability.targetStrategy = (int)Character.TargetList.LowestPDAlly;
            }
        }
        else {
            if(highest) {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.HighestPDAlly;
            }
            else {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.LowestPDAlly;
            }
        }
        updateTargettingView();
    }
    private void selectMDAlly() {
        if (isAbilityTargetSelector) {
            if (highest) {
                ability.targetStrategy = (int)Character.TargetList.HighestMDAlly;
            }
            else {
                ability.targetStrategy = (int)Character.TargetList.LowestMDAlly;
            }
        }
        else {
            if(highest) {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.HighestMDAlly;
            }
            else {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.LowestMDAlly;
            }
        }
        updateTargettingView();
    }
    private void selectINFAlly() {
        if (isAbilityTargetSelector) {
            if (highest) {
                ability.targetStrategy = (int)Character.TargetList.HighestINFAlly;
            }
            else {
                ability.targetStrategy = (int)Character.TargetList.LowestINFAlly;
            }
        }
        else {
            if(highest) {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.HighestINFAlly;
            }
            else {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.LowestINFAlly;
            }
        }
        updateTargettingView();
    }
    private void selectHPAlly() {
        if (isAbilityTargetSelector) {
            if (highest) {
                ability.targetStrategy = (int)Character.TargetList.HighestHPAlly;
            }
            else {
                ability.targetStrategy = (int)Character.TargetList.LowestHPAlly;
            }
        }
        else {
            if(highest) {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.HighestHPAlly;
            }
            else {
                characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.LowestHPAlly;
            }
        }
        updateTargettingView();
    }
    private void selectClosestAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.ClosestAlly;
        }
        else {
            characterInfoScreen.character.attackTargetStrategy = (int)Character.TargetList.ClosestAlly;
        }
        updateTargettingView();
    }
#endregion buttonFunctions

    private void removeAbility() {
        //sets the parent to be ability inventory
        ability.transform.parent = UIManager.singleton.playerParty.abilityInventory.transform;
        //removes ability from character
        characterInfoScreen.character.abilities.Remove(ability);
        //Removes character from ability
        ability.character = null;
        //Starts unfocusing
        characterInfoScreen.startUnfocusing();
        //saves removing the ability
        if (SceneManager.GetActiveScene().name == "World") {
            UIManager.singleton.saveWorldSave();
        }
        else
            UIManager.singleton.saveMapSave();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
