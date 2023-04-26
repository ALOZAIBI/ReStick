using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            uiManager.inventoryScreen.inventoryCharacterScreen.viewCharacter(character);
        }
    }

    public void updateView() {
        //gets wether this is highest or not from the character or ability
        if (isAbilityTargetSelector) {
            switch (ability.targetStrategy) {
                case (int)Character.targetList.HighestASAlly:
                case (int)Character.targetList.HighestASEnemy:
                case (int)Character.targetList.HighestHPAlly:
                case (int)Character.targetList.HighestHPEnemy:
                case (int)Character.targetList.HighestMDAlly:
                case (int)Character.targetList.HighestMDEnemy:
                case (int)Character.targetList.HighestINFAlly:
                case (int)Character.targetList.HighestINFEnemy:
                case (int)Character.targetList.HighestMSAlly:
                case (int)Character.targetList.HighestMSEnemy:
                case (int)Character.targetList.HighestPDAlly:
                case (int)Character.targetList.HighestPDEnemy:
                case (int)Character.targetList.HighestRangeAlly:
                case (int)Character.targetList.HighestRangeEnemy:

                    highest = true;
                    break;
                default:
                    highest = false;
                    break;
            }
        }
        else {
            switch (character.attackTargetStrategy) {
                case (int)Character.targetList.HighestASAlly:
                case (int)Character.targetList.HighestASEnemy:
                case (int)Character.targetList.HighestHPAlly:
                case (int)Character.targetList.HighestHPEnemy:
                case (int)Character.targetList.HighestMDAlly:
                case (int)Character.targetList.HighestMDEnemy:
                case (int)Character.targetList.HighestINFAlly:
                case (int)Character.targetList.HighestINFEnemy:
                case (int)Character.targetList.HighestMSAlly:
                case (int)Character.targetList.HighestMSEnemy:
                case (int)Character.targetList.HighestPDAlly:
                case (int)Character.targetList.HighestPDEnemy:
                case (int)Character.targetList.HighestRangeAlly:
                case (int)Character.targetList.HighestRangeEnemy:

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
            case (int)Character.targetList.ClosestEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "Closest") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                    break;
            case (int)Character.targetList.HighestPDEnemy:
            case (int)Character.targetList.LowestPDEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "PD") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                break;
            case (int)Character.targetList.HighestASEnemy:
            case (int)Character.targetList.LowestASEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "AS") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                break;
            case (int)Character.targetList.HighestMDEnemy:
            case (int)Character.targetList.LowestMDEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "MD") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                break;
            case (int)Character.targetList.HighestINFEnemy:
            case (int)Character.targetList.LowestINFEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "INF") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                break;
            case (int)Character.targetList.HighestMSEnemy:
            case (int)Character.targetList.LowestMSEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "MS") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                break;
            case (int)Character.targetList.HighestRangeEnemy:
            case (int)Character.targetList.LowestRangeEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "Range") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                break;
            case (int)Character.targetList.HighestHPEnemy:
            case (int)Character.targetList.LowestHPEnemy:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "HP") {
                        temp.highlightEnemy();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = true;
                    }
                }
                break;
            case (int)Character.targetList.ClosestAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "Closest") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }
                }
                break;
            case (int)Character.targetList.HighestPDAlly:
            case (int)Character.targetList.LowestPDAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "PD") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }
                }
                break;
            case (int)Character.targetList.HighestMDAlly:
            case (int)Character.targetList.LowestMDAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "MD") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }
                }
                break;
            case (int)Character.targetList.HighestINFAlly:
            case (int)Character.targetList.LowestINFAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "INF") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }
                }
                break;
            case (int)Character.targetList.HighestASAlly:
            case (int)Character.targetList.LowestASAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "AS") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }
                }
                break;
            case (int)Character.targetList.HighestMSAlly:
            case (int)Character.targetList.LowestMSAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "MS") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }
                }
                break;
            case (int)Character.targetList.HighestRangeAlly:
            case (int)Character.targetList.LowestRangeAlly:
                foreach (TargetOptionButton temp in buttons) {
                    if (temp.stat == "Range") {
                        temp.highlightAlly();
                        temp.currentlySelectedBtn = true;
                        temp.enemySelected = false;
                    }
                }
                break;
            case (int)Character.targetList.HighestHPAlly:
            case (int)Character.targetList.LowestHPAlly:
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
            case (int)Character.targetList.ClosestEnemy:
                phrase.text += " closest enemy";
                break;
            case (int)Character.targetList.HighestPDEnemy:
                phrase.text += " highest PD enemy";
                break;
            case (int)Character.targetList.LowestPDEnemy:
                phrase.text += " lowest PD enemy";
                break;
            case (int)Character.targetList.HighestMDEnemy:
                phrase.text += " highest MD enemy";
                break;
            case (int)Character.targetList.LowestMDEnemy:
                phrase.text += " lowest MD enemy";
                break;
            case (int)Character.targetList.HighestINFEnemy:
                phrase.text += " highest INF enemy";
                break;
            case (int)Character.targetList.LowestINFEnemy:
                phrase.text += " lowest INF enemy";
                break;
            case (int)Character.targetList.HighestASEnemy:
                phrase.text += " highest AS enemy";
                break;
            case (int)Character.targetList.LowestASEnemy:
                phrase.text += " lowest AS enemy";
                break;
            case (int)Character.targetList.HighestMSEnemy:
                phrase.text += " highest MS enemy";
                break;
            case (int)Character.targetList.LowestMSEnemy:
                phrase.text += " lowest MS enemy";
                break;
            case (int)Character.targetList.HighestRangeEnemy:
                phrase.text += " highest range enemy";
                break;
            case (int)Character.targetList.LowestRangeEnemy:
                phrase.text += " lowest range enemy";
                break;
            case (int)Character.targetList.HighestHPEnemy:
                phrase.text += " highest HP enemy";
                break;
            case (int)Character.targetList.LowestHPEnemy:
                phrase.text += " lowest HP enemy";
                break;
            case (int)Character.targetList.ClosestAlly:
                phrase.text += " closest ally";
                break;
            case (int)Character.targetList.LowestPDAlly:
                phrase.text += " lowest PD ally";
                break;
            case (int)Character.targetList.HighestPDAlly:
                phrase.text += " highest PD ally";
                break;
            case (int)Character.targetList.HighestMDAlly:
                phrase.text += " highest MD ally";
                break;
            case (int)Character.targetList.LowestMDAlly:
                phrase.text += " lowest MD ally";
                break;
            case (int)Character.targetList.HighestINFAlly:
                phrase.text += " highest INF ally";
                break;
            case (int)Character.targetList.LowestINFAlly:
                phrase.text += " lowest INF ally";
                break;
            case (int)Character.targetList.HighestASAlly:
                phrase.text += " highest AS ally";
                break;
            case (int)Character.targetList.LowestASAlly:
                phrase.text += " lowest AS ally";
                break;
            case (int)Character.targetList.HighestMSAlly:
                phrase.text += " highest MS ally";
                break;
            case (int)Character.targetList.LowestMSAlly:
                phrase.text += " lowest MS ally";
                break;
            case (int)Character.targetList.HighestRangeAlly:
                phrase.text += " highest range ally";
                break;
            case (int)Character.targetList.LowestRangeAlly:
                phrase.text += " lowest range ally";
                break;
            case (int)Character.targetList.HighestHPAlly:
                phrase.text += " highest HP ally";
                break;
            case (int)Character.targetList.LowestHPAlly:
                phrase.text += " lowest HP ally";
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
            ability.targetStrategy = (int)Character.targetList.ClosestEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.ClosestEnemy;
        }
        updateView();
    }
    public void selectHighestPDEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestPDEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestPDEnemy;
        }
        updateView();

    }

    public void selectLowestPDEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestPDEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestPDEnemy;
        }
        updateView();

    }

    public void selectHighestMDEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestMDEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestMDEnemy;
        }
        updateView();

    }

    public void selectLowestMDEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestMDEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestMDEnemy;
        }
        updateView();

    }
    public void selectHighestINFEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestINFEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestINFEnemy;
        }
        updateView();

    }

    public void selectLowestINFEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestINFEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestINFEnemy;
        }
        updateView();

    }
    public void selectHighestASEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestASEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestASEnemy;
        }
        updateView();

    }

    public void selectLowestASEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestASEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestASEnemy;
        }
        updateView();

    }

    public void selectHighestMSEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestMSEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestMSEnemy;
        }
        updateView();

    }

    public void selectLowestMSEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestMSEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestMSEnemy;
        }
        updateView();

    }

    public void selectHighestRangeEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestRangeEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestRangeEnemy;
        }
        updateView();

    }

    public void selectLowestRangeEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestRangeEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestRangeEnemy;
        }
        updateView();

    }

    public void selectHighestHPEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestHPEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestHPEnemy;
        }
        updateView();

    }

    public void selectLowestHPEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestHPEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestHPEnemy;
        }
        updateView();

    }

    public void selectHighestPDAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestPDAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestPDAlly;
        }
        updateView();

    }

    public void selectLowestPDAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestPDAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestPDAlly;
        }
        updateView();

    }
    public void selectHighestMDAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestMDAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestMDAlly;
        }
        updateView();

    }

    public void selectLowestMDAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestMDAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestMDAlly;
        }
        updateView();

    }
    public void selectHighestINFAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestINFAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestINFAlly;
        }
        updateView();

    }

    public void selectLowestINFAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestINFAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestINFAlly;
        }
        updateView();

    }
    public void selectHighestASAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestASAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestASAlly;
        }
        updateView();

    }

    public void selectLowestASAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestASAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestASAlly;
        }
        updateView();

    }

    public void selectHighestMSAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestMSAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestMSAlly;
        }
        updateView();

    }

    public void selectLowestMSAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestMSAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestMSAlly;
        }
        updateView();

    }

    public void selectHighestRangeAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestRangeAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestRangeAlly;
        }
        updateView();

    }

    public void selectLowestRangeAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestRangeAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestRangeAlly;
        }
        updateView();

    }

    public void selectHighestHPAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestHPAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestHPAlly;
        }
        updateView();

    }

    public void selectLowestHPAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestHPAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestHPAlly;
        }
        updateView();

    }
    public void selectClosestAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.ClosestAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.ClosestAlly;
        }
        updateView();
    }


}
