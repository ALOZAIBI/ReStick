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
    public Character character;
    public GameObject targetSelection;

    public Button closestEnemyBtn;
    public Button closestAllyBtn;

    public Button highestDmgEnemyBtn;
    public Button lowestDmgEnemyBtn;
    public Button highestHPEnemyBtn;
    public Button lowestHPEnemyBtn;

    public Button highestDmgAllyBtn;
    public Button lowestDmgAllyBtn;
    public Button highestHPAllyBtn;
    public Button lowestHPAllyBtn;


    //the ability to have it's target change.
    public Ability ability;
    //if true then the selector selects for ability target otherwise it selects for regular attack
    public bool isAbilityTargetSelector;

    private void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        closestEnemyBtn.onClick.AddListener(selectClosestEnemy);
        closestAllyBtn.onClick.AddListener(selectClosestAlly);

        highestDmgEnemyBtn.onClick.AddListener(selectHighestDmgEnemy);
        lowestDmgEnemyBtn.onClick.AddListener(selectLowestDmgEnemy);
        highestHPEnemyBtn.onClick.AddListener(selectHighestHPEnemy);
        lowestHPEnemyBtn.onClick.AddListener(selectLowestHPEnemy);

        highestDmgAllyBtn.onClick.AddListener(selectHighestDmgAlly);
        lowestDmgAllyBtn.onClick.AddListener(selectLowestDmgAlly);
        highestHPAllyBtn.onClick.AddListener(selectHighestHPAlly);
        lowestHPAllyBtn.onClick.AddListener(selectLowestHPAlly);

    }

    private void closeAndUpdateCharScreen() {
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
    //the buttons change the targetting then closes the selection screen
    private void selectClosestEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.ClosestEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.ClosestEnemy;
            character.movementTargetStrategy = (int)Character.targetList.ClosestEnemy;
        }
        closeAndUpdateCharScreen();
    }
    private void selectHighestDmgEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestDMGEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestDMGEnemy;
            character.movementTargetStrategy = (int)Character.targetList.HighestDMGEnemy;
        }
        closeAndUpdateCharScreen();

    }

    private void selectLowestDmgEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestDMGEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestDMGEnemy;
            character.movementTargetStrategy = (int)Character.targetList.LowestDMGEnemy;
        }
        closeAndUpdateCharScreen();

    }

    private void selectHighestHPEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestHPEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestHPEnemy;
            character.movementTargetStrategy = (int)Character.targetList.HighestHPEnemy;
        }
        closeAndUpdateCharScreen();

    }
    private void selectLowestHPEnemy() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestHPEnemy;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestHPEnemy;
            character.movementTargetStrategy = (int)Character.targetList.LowestHPEnemy;
        }
        closeAndUpdateCharScreen();

    }

    private void selectHighestDmgAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestDMGAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestDMGAlly;
            character.movementTargetStrategy = (int)Character.targetList.HighestDMGAlly;
        }
        closeAndUpdateCharScreen();

    }

    private void selectLowestDmgAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestDMGAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestDMGAlly;
            character.movementTargetStrategy = (int)Character.targetList.LowestDMGAlly;
        }
        closeAndUpdateCharScreen();

    }

    private void selectHighestHPAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.HighestHPAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.HighestHPAlly;
            character.movementTargetStrategy = (int)Character.targetList.HighestHPAlly;
        }
        closeAndUpdateCharScreen();

    }

    private void selectLowestHPAlly() {
        if (isAbilityTargetSelector) {
            ability.targetStrategy = (int)Character.targetList.LowestHPAlly;
        }
        else {
            character.attackTargetStrategy = (int)Character.targetList.LowestHPAlly;
            character.movementTargetStrategy = (int)Character.targetList.LowestHPAlly;
        }
        closeAndUpdateCharScreen();

    }
    private void selectClosestAlly() {
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
