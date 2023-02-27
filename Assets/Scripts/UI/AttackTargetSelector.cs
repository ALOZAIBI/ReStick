using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackTargetSelector : MonoBehaviour {
    //for somereason this component is placed on the attackTargetting btn
    public UIManager uiManager;
    public TextMeshProUGUI target;
    public Character character;
    public GameObject targetSelection;

    public Button closestEnemyBtn;
    public Button closestAllyBtn;
    public Button highestDmgEnemyBtn;

    //the ability to have it's target change.
    public Ability ability;
    //if true then the selector selects for ability target otherwise it selects for regular attack
    public bool isAbilityTargetSelector;

    private void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        closestEnemyBtn.onClick.AddListener(selectClosestEnemy);
        closestAllyBtn.onClick.AddListener(selectClosestAlly);
        highestDmgEnemyBtn.onClick.AddListener(selectHighestDmgEnemy);
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
