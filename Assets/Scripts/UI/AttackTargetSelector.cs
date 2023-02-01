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
        //updates the characterscreenview
        uiManager.viewCharacter(character);
    }
    private void selectHighestDmgEnemy() {
        character.attackTargetStrategy = (int)Character.targetList.HighestDMGEnemy;
        character.movementTargetStrategy = (int)Character.targetList.HighestDMGEnemy;
        closeAndUpdateCharScreen();

    }
    //the buttons change the targetting then closes the selection screen
    private void selectClosestEnemy() {
        Debug.Log("Closesetnemay");
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
