using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovementStrategySelector : MonoBehaviour {
    public TextMeshProUGUI movementText;
    public Character character;

    public Button defaultBtn;
    public Button stayNearAllyBtn;
    public Button dontMoveBtn;
    public Button runAwayFromEnemyBtn;

    public Button closeBtn;
    private void Start() {
        closeBtn.onClick.AddListener(close);
        defaultBtn.onClick.AddListener(selectDefault);
        stayNearAllyBtn.onClick.AddListener(selectStayNearAlly);
        dontMoveBtn.onClick.AddListener (selectDontMove);
        runAwayFromEnemyBtn.onClick.AddListener(selectRunAway);
    }

    private void selectDefault() {
        character.movementStrategy = (int)Character.MovementStrategies.Default;
        updateText();
    }

    private void selectStayNearAlly() {
        character.movementStrategy = (int)Character.MovementStrategies.StayNearAlly; 
        updateText();
    }

    private void selectDontMove() {
        character.movementStrategy = (int)Character.MovementStrategies.DontMove;
        updateText();
    }

    private void selectRunAway() {
        character.movementStrategy = (int)Character.MovementStrategies.RunAwayFromNearestEnemy;
        updateText();
    }
    public void updateText() {
        switch (character.movementStrategy) {
            case (int)Character.MovementStrategies.Default:
                movementText.text = "I keep my attack target in range";
                break;
            case (int)Character.MovementStrategies.StayNearAlly:
                movementText.text = "I stay near my closest ally";
                break;
            case (int)Character.MovementStrategies.DontMove:
                movementText.text = "I stay at my position";
                break;
            case (int)Character.MovementStrategies.RunAwayFromNearestEnemy:
                movementText.text = "I move away from enemies";
                break;
        }
    }

    public void close() {
        //updates the characterInfoScreenview
        //do for regular charInfoScreen
        UIManager.singleton.characterInfoScreen.close();
        UIManager.singleton.viewCharacter(character);

    }
}
