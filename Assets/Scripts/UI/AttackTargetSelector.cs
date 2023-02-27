using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackTargetSelector : MonoBehaviour {
    public UIManager uiManager;
    public TextMeshProUGUI target;
    public Character character;
    public Button openTargetSelectionBtn;
    public GameObject targetSelection;

    public Button closestEnemyBtn;
    public Button closestAllyBtn;
    public Button highestDmgEnemyBtn;

    //the ability to have it's target change.
    public Ability ability;
    //if true then the selector selects for ability target otherwise it selects for regular attack
    public bool abilityTarget;

    private void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        openTargetSelectionBtn.onClick.AddListener(openTargetSelection);
        closestEnemyBtn.onClick.AddListener(selectClosestEnemy);
        closestAllyBtn.onClick.AddListener(selectClosestAlly);
        highestDmgEnemyBtn.onClick.AddListener(selectHighestDmgEnemy);
    }

    private void closeAndUpdateCharScreen() {
        //closes the windows
        targetSelection.SetActive(false);
        //updates the characterInfoScreenview
        uiManager.viewCharacter(character);
        uiManager.inventoryScreen.inventoryCharacterScreen.viewCharacter(character);
    }
    private void selectHighestDmgEnemy() {
        character.attackTargetStrategy = (int)Character.targetList.HighestDMGEnemy;
        character.movementTargetStrategy = (int)Character.targetList.HighestDMGEnemy;
        closeAndUpdateCharScreen();

    }
    //the buttons change the targetting then closes the selection screen
    private void selectClosestEnemy() {
        character.attackTargetStrategy = (int)Character.targetList.ClosestEnemy;
        character.movementTargetStrategy = (int)Character.targetList.ClosestEnemy;
        closeAndUpdateCharScreen();
    }

    private void selectClosestAlly() {
        character.attackTargetStrategy = (int)Character.targetList.ClosestAlly;
        character.movementTargetStrategy = (int)Character.targetList.ClosestAlly;
        closeAndUpdateCharScreen();
    }
    public void openTargetSelection() {
        //the conditions are true on zone start 
        if(uiManager.zone == null || uiManager.zone.started ==false && character.team == (int)Character.teamList.Player)
            targetSelection.SetActive(true);
    }
    
}
