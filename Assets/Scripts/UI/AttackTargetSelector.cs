using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Deprecated use TargetSelector.cs
/// </summary>
public class AttackTargetSelector : MonoBehaviour {
    public UIManager uiManager;
    //just text 
    public TextMeshProUGUI target;
    public TextMeshProUGUI phrase;
    public Character character;
    public GameObject targetSelection;

    public TargetOptionButton[] buttons;


    //the ability to have it's target change.
    public Ability ability;
    //if true then the selector selects for ability target otherwise it selects for regular attack
    public bool isAbilityTargetSelector;

    public bool highest;//if true highest if false lowest

    public Button closeBtn;
    public void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        closeBtn.onClick.AddListener(close);
    }
    public void close() {
        //updates the characterInfoScreenview
        //do for regular charInfoScreen
        if (uiManager.inventoryScreenHidden.hidden) {
            uiManager.characterInfoScreen.close();
            uiManager.viewCharacter(character);
        }
        //do for inventory charInfoScreen
        else {
            uiManager.inventoryScreen.inventoryCharacterScreen.close();
            uiManager.inventoryScreen.inventoryCharacterScreen.viewCharacterFullScreen(character);
        }
    }

    public void updateView() {
        //gets wether this is highest or not from the character or ability
        if (isAbilityTargetSelector) {
            switch (ability.targetStrategy) {
                case (int)Character.TargetList.HighestASAlly:
                case (int)Character.TargetList.HighestASEnemy:
                case (int)Character.TargetList.HighestHPAlly:
                case (int)Character.TargetList.HighestHPEnemy:
                case (int)Character.TargetList.HighestMDAlly:
                case (int)Character.TargetList.HighestMDEnemy:
                case (int)Character.TargetList.HighestINFAlly:
                case (int)Character.TargetList.HighestINFEnemy:
                case (int)Character.TargetList.HighestMSAlly:
                case (int)Character.TargetList.HighestMSEnemy:
                case (int)Character.TargetList.HighestPDAlly:
                case (int)Character.TargetList.HighestPDEnemy:
                case (int)Character.TargetList.HighestRangeAlly:
                case (int)Character.TargetList.HighestRangeEnemy:

                    highest = true;
                    break;
                default:
                    highest = false;
                    break;
            }
        }
        else {
            switch (character.attackTargetStrategy) {
                case (int)Character.TargetList.HighestASAlly:
                case (int)Character.TargetList.HighestASEnemy:
                case (int)Character.TargetList.HighestHPAlly:
                case (int)Character.TargetList.HighestHPEnemy:
                case (int)Character.TargetList.HighestMDAlly:
                case (int)Character.TargetList.HighestMDEnemy:
                case (int)Character.TargetList.HighestINFAlly:
                case (int)Character.TargetList.HighestINFEnemy:
                case (int)Character.TargetList.HighestMSAlly:
                case (int)Character.TargetList.HighestMSEnemy:
                case (int)Character.TargetList.HighestPDAlly:
                case (int)Character.TargetList.HighestPDEnemy:
                case (int)Character.TargetList.HighestRangeAlly:
                case (int)Character.TargetList.HighestRangeEnemy:

                    highest = true;
                    break;
                default:
                    highest = false;
                    break;
            }
        }
        
        removeHighlights();
        //if the current target to be modified is for ability then use ability.TargetStrategy for display otherwise use character.attacktargetstrategy
        int targetStrategy;
        if (!isAbilityTargetSelector)
            targetStrategy = character.attackTargetStrategy;
        else
            targetStrategy = ability.targetStrategy;
        updatePhrase(targetStrategy);
        switch (targetStrategy) {
            case (int)Character.TargetList.ClosestEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "Closest") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                    break;
            case (int)Character.TargetList.HighestPDEnemy:
            case (int)Character.TargetList.LowestPDEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "PD") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                break;
            case (int)Character.TargetList.HighestASEnemy:
            case (int)Character.TargetList.LowestASEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "AS") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                break;
            case (int)Character.TargetList.HighestMDEnemy:
            case (int)Character.TargetList.LowestMDEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "MD") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                break;
            case (int)Character.TargetList.HighestINFEnemy:
            case (int)Character.TargetList.LowestINFEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "INF") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                break;
            case (int)Character.TargetList.HighestMSEnemy:
            case (int)Character.TargetList.LowestMSEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "MS") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                break;
            case (int)Character.TargetList.HighestRangeEnemy:
            case (int)Character.TargetList.LowestRangeEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "Range") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                break;
            case (int)Character.TargetList.HighestHPEnemy:
            case (int)Character.TargetList.LowestHPEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "HP") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                break;
            case (int)Character.TargetList.ClosestAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "Closest") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }
                }
                break;
            case (int)Character.TargetList.HighestPDAlly:
            case (int)Character.TargetList.LowestPDAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "PD") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }
                }
                break;
            case (int)Character.TargetList.HighestMDAlly:
            case (int)Character.TargetList.LowestMDAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "MD") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }
                }
                break;
            case (int)Character.TargetList.HighestINFAlly:
            case (int)Character.TargetList.LowestINFAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "INF") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }
                }
                break;
            case (int)Character.TargetList.HighestASAlly:
            case (int)Character.TargetList.LowestASAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "AS") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }
                }
                break;
            case (int)Character.TargetList.HighestMSAlly:
            case (int)Character.TargetList.LowestMSAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "MS") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }
                }
                break;
            case (int)Character.TargetList.HighestRangeAlly:
            case (int)Character.TargetList.LowestRangeAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "Range") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }
                }
                break;
            case (int)Character.TargetList.HighestHPAlly:
            case (int)Character.TargetList.LowestHPAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "HP") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }

                }
                break;
        }
    }


    private void updatePhrase(int targetStrategy) {
        phrase.text = "My target is the";
        switch (targetStrategy) {
            case (int)Character.TargetList.ClosestEnemy:
                phrase.text += " closest enemy";
                break;
            case (int)Character.TargetList.HighestPDEnemy:
                phrase.text += " highest Power enemy";
                break;
            case (int)Character.TargetList.LowestPDEnemy:
                phrase.text += " lowest Power enemy";
                break;
            case (int)Character.TargetList.HighestMDEnemy:
                phrase.text += " highest Magic enemy";
                break;
            case (int)Character.TargetList.LowestMDEnemy:
                phrase.text += " lowest Magic enemy";
                break;
            case (int)Character.TargetList.HighestINFEnemy:
                phrase.text += " highest Influence enemy";
                break;
            case (int)Character.TargetList.LowestINFEnemy:
                phrase.text += " lowest Influence enemy";
                break;
            case (int)Character.TargetList.HighestASEnemy:
                phrase.text += " highest Atk Speed enemy";
                break;
            case (int)Character.TargetList.LowestASEnemy:
                phrase.text += " lowest Atk Speed enemy";
                break;
            case (int)Character.TargetList.HighestMSEnemy:
                phrase.text += " highest Speed enemy";
                break;
            case (int)Character.TargetList.LowestMSEnemy:
                phrase.text += " lowest Speed enemy";
                break;
            case (int)Character.TargetList.HighestRangeEnemy:
                phrase.text += " highest Range enemy";
                break;
            case (int)Character.TargetList.LowestRangeEnemy:
                phrase.text += " lowest Range enemy";
                break;
            case (int)Character.TargetList.HighestHPEnemy:
                phrase.text += " highest Health enemy";
                break;
            case (int)Character.TargetList.LowestHPEnemy:
                phrase.text += " lowest Health enemy";
                break;
            case (int)Character.TargetList.ClosestAlly:
                phrase.text += " closest ally";
                break;
            case (int)Character.TargetList.LowestPDAlly:
                phrase.text += " lowest Power ally";
                break;
            case (int)Character.TargetList.HighestPDAlly:
                phrase.text += " highest Power ally";
                break;
            case (int)Character.TargetList.HighestMDAlly:
                phrase.text += " highest Magic ally";
                break;
            case (int)Character.TargetList.LowestMDAlly:
                phrase.text += " lowest Magic ally";
                break;
            case (int)Character.TargetList.HighestINFAlly:
                phrase.text += " highest Influence ally";
                break;
            case (int)Character.TargetList.LowestINFAlly:
                phrase.text += " lowest Influence ally";
                break;
            case (int)Character.TargetList.HighestASAlly:
                phrase.text += " highest Atk Speed ally";
                break;
            case (int)Character.TargetList.LowestASAlly:
                phrase.text += " lowest Atk Speed ally";
                break;
            case (int)Character.TargetList.HighestMSAlly:
                phrase.text += " highest Speed ally";
                break;
            case (int)Character.TargetList.LowestMSAlly:
                phrase.text += " lowest Speed ally";
                break;
            case (int)Character.TargetList.HighestRangeAlly:
                phrase.text += " highest Range ally";
                break;
            case (int)Character.TargetList.LowestRangeAlly:
                phrase.text += " lowest Range ally";
                break;
            case (int)Character.TargetList.HighestHPAlly:
                phrase.text += " highest Health ally";
                break;
            case (int)Character.TargetList.LowestHPAlly:
                phrase.text += " lowest Health ally";
                break;
        }

    }


    //remove highlights from all buttons except the selected one
    public void removeHighlights(TargetOptionButton button) {
        foreach (TargetOptionButton temp in buttons) {
            if (temp != button) {
                temp.removeHighlights("both");
                temp.currentlySelectedBtn = false;
            }
        }
    }

    //remove highlights from all buttons 
    public void removeHighlights() {
        foreach (TargetOptionButton temp in buttons) {
                temp.removeHighlights("both");
                temp.currentlySelectedBtn = false;
            }
    }
    //the buttons change the targetting then closes the selection screen
    public void selectClosestEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.ClosestEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.ClosestEnemy;
        }
        updateView();
    }
    public void selectHighestPDEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.HighestPDEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.HighestPDEnemy;
        }
        updateView();

    }

    public void selectLowestPDEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.LowestPDEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.LowestPDEnemy;
        }
        updateView();

    }

    public void selectHighestMDEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.HighestMDEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.HighestMDEnemy;
        }
        updateView();

    }

    public void selectLowestMDEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.LowestMDEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.LowestMDEnemy;
        }
        updateView();

    }
    public void selectHighestINFEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.HighestINFEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.HighestINFEnemy;
        }
        updateView();

    }

    public void selectLowestINFEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.LowestINFEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.LowestINFEnemy;
        }
        updateView();

    }
    public void selectHighestASEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.HighestASEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.HighestASEnemy;
        }
        updateView();

    }

    public void selectLowestASEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.LowestASEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.LowestASEnemy;
        }
        updateView();

    }

    public void selectHighestMSEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.HighestMSEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.HighestMSEnemy;
        }
        updateView();

    }

    public void selectLowestMSEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.LowestMSEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.LowestMSEnemy;
        }
        updateView();

    }

    public void selectHighestRangeEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.HighestRangeEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.HighestRangeEnemy;
        }
        updateView();

    }

    public void selectLowestRangeEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.LowestRangeEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.LowestRangeEnemy;
        }
        updateView();

    }

    public void selectHighestHPEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.HighestHPEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.HighestHPEnemy;
        }
        updateView();

    }

    public void selectLowestHPEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.LowestHPEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.LowestHPEnemy;
        }
        updateView();

    }

    public void selectHighestPDAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.HighestPDAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.HighestPDAlly;
        }
        updateView();

    }

    public void selectLowestPDAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.LowestPDAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.LowestPDAlly;
        }
        updateView();

    }
    public void selectHighestMDAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.HighestMDAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.HighestMDAlly;
        }
        updateView();

    }

    public void selectLowestMDAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.LowestMDAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.LowestMDAlly;
        }
        updateView();

    }
    public void selectHighestINFAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.HighestINFAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.HighestINFAlly;
        }
        updateView();

    }

    public void selectLowestINFAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.LowestINFAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.LowestINFAlly;
        }
        updateView();

    }
    public void selectHighestASAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.HighestASAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.HighestASAlly;
        }
        updateView();

    }

    public void selectLowestASAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.LowestASAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.LowestASAlly;
        }
        updateView();

    }

    public void selectHighestMSAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.HighestMSAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.HighestMSAlly;
        }
        updateView();

    }

    public void selectLowestMSAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.LowestMSAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.LowestMSAlly;
        }
        updateView();

    }

    public void selectHighestRangeAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.HighestRangeAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.HighestRangeAlly;
        }
        updateView();

    }

    public void selectLowestRangeAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.LowestRangeAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.LowestRangeAlly;
        }
        updateView();

    }

    public void selectHighestHPAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.HighestHPAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.HighestHPAlly;
        }
        updateView();

    }

    public void selectLowestHPAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.LowestHPAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.LowestHPAlly;
        }
        updateView();

    }
    public void selectClosestAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.TargetList.ClosestAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.TargetList.ClosestAlly;
        }
        updateView();
    }


}
