using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackTargetSelector : MonoBehaviour {
    //for somereason this component is placed on the attackTargetting btn
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
    public void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

    }

    public void closeAndUpdateCharScreen() {
        //closes the windows
        targetSelection.SetActive(false);
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
    //remove highlights from all buttons except the selected one
    public void removeHighlights(TargetOptionButton button) {
        foreach(TargetOptionButton temp in buttons) {
            if (temp != button) {
                temp.removeHighlights("both");
                temp.currentlySelectedBtn = false;
            }
        }
        Debug.Log("Removing Other Highlights");
    }
    //the buttons change the targetting then closes the selection screen
    public void selectClosestEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.ClosestEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.ClosestEnemy;
            character.movementTargetStrategy = (int)Character.targetList.ClosestEnemy;
        }
        closeAndUpdateCharScreen();
    }
    public void selectHighestPDEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestPDEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestPDEnemy;
            character.movementTargetStrategy = (int)Character.targetList.HighestPDEnemy;
        }
        closeAndUpdateCharScreen();

    }

    public void selectLowestPDEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestPDEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestPDEnemy;
            character.movementTargetStrategy = (int)Character.targetList.LowestPDEnemy;
        }
        closeAndUpdateCharScreen();

    }

    public void selectHighestMDEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestMDEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestMDEnemy;
            character.movementTargetStrategy = (int)Character.targetList.HighestMDEnemy;
        }
        closeAndUpdateCharScreen();

    }

    public void selectLowestMDEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestMDEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestMDEnemy;
            character.movementTargetStrategy = (int)Character.targetList.LowestMDEnemy;
        }
        closeAndUpdateCharScreen();

    }
    public void selectHighestASEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestASEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestASEnemy;
            character.movementTargetStrategy = (int)Character.targetList.HighestASEnemy;
        }
        closeAndUpdateCharScreen();

    }

    public void selectLowestASEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestASEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestASEnemy;
            character.movementTargetStrategy = (int)Character.targetList.LowestASEnemy;
        }
        closeAndUpdateCharScreen();

    }

    public void selectHighestMSEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestMSEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestMSEnemy;
            character.movementTargetStrategy = (int)Character.targetList.HighestMSEnemy;
        }
        closeAndUpdateCharScreen();

    }

    public void selectLowestMSEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestMSEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestMSEnemy;
            character.movementTargetStrategy = (int)Character.targetList.LowestMSEnemy;
        }
        closeAndUpdateCharScreen();

    }

    public void selectHighestRangeEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestRangeEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestRangeEnemy;
            character.movementTargetStrategy = (int)Character.targetList.HighestRangeEnemy;
        }
        closeAndUpdateCharScreen();

    }

    public void selectLowestRangeEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestRangeEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestRangeEnemy;
            character.movementTargetStrategy = (int)Character.targetList.LowestRangeEnemy;
        }
        closeAndUpdateCharScreen();

    }

    public void selectHighestHPEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestHPEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestHPEnemy;
            character.movementTargetStrategy = (int)Character.targetList.HighestHPEnemy;
        }
        closeAndUpdateCharScreen();

    }

    public void selectLowestHPEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestHPEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestHPEnemy;
            character.movementTargetStrategy = (int)Character.targetList.LowestHPEnemy;
        }
        closeAndUpdateCharScreen();

    }

    public void selectHighestPDAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestPDAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestPDAlly;
            character.movementTargetStrategy = (int)Character.targetList.HighestPDAlly;
        }
        closeAndUpdateCharScreen();

    }

    public void selectLowestPDAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestPDAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestPDAlly;
            character.movementTargetStrategy = (int)Character.targetList.LowestPDAlly;
        }
        closeAndUpdateCharScreen();

    }
    public void selectHighestMDAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestMDAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestMDAlly;
            character.movementTargetStrategy = (int)Character.targetList.HighestMDAlly;
        }
        closeAndUpdateCharScreen();

    }

    public void selectLowestMDAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestMDAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestMDAlly;
            character.movementTargetStrategy = (int)Character.targetList.LowestMDAlly;
        }
        closeAndUpdateCharScreen();

    }
    public void selectHighestASAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestASAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestASAlly;
            character.movementTargetStrategy = (int)Character.targetList.HighestASAlly;
        }
        closeAndUpdateCharScreen();

    }

    public void selectLowestASAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestASAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestASAlly;
            character.movementTargetStrategy = (int)Character.targetList.LowestASAlly;
        }
        closeAndUpdateCharScreen();

    }

    public void selectHighestMSAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestMSAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestMSAlly;
            character.movementTargetStrategy = (int)Character.targetList.HighestMSAlly;
        }
        closeAndUpdateCharScreen();

    }

    public void selectLowestMSAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestMSAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestMSAlly;
            character.movementTargetStrategy = (int)Character.targetList.LowestMSAlly;
        }
        closeAndUpdateCharScreen();

    }

    public void selectHighestRangeAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestRangeAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestRangeAlly;
            character.movementTargetStrategy = (int)Character.targetList.HighestRangeAlly;
        }
        closeAndUpdateCharScreen();

    }

    public void selectLowestRangeAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestRangeAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestRangeAlly;
            character.movementTargetStrategy = (int)Character.targetList.LowestRangeAlly;
        }
        closeAndUpdateCharScreen();

    }

    public void selectHighestHPAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestHPAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestHPAlly;
            character.movementTargetStrategy = (int)Character.targetList.HighestHPAlly;
        }
        closeAndUpdateCharScreen();

    }

    public void selectLowestHPAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestHPAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestHPAlly;
            character.movementTargetStrategy = (int)Character.targetList.LowestHPAlly;
        }
        closeAndUpdateCharScreen();

    }
    public void selectClosestAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.ClosestAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.ClosestAlly;
            character.movementTargetStrategy = (int)Character.targetList.ClosestAlly;
        }
        closeAndUpdateCharScreen();
    }

    
}
