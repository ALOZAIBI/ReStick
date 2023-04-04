using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetOptionButton : MonoBehaviour {
    //we are getting the ImageComponents to Disable/Enable them to emphasize what is currently selected
    public Button enemyBtn;
    public Component enemyImageComponent;
    public Button allyBtn;
    public Component allyImageComponent;

    public Component anothaImageComponent;

    public Image EnemyHighlight;   //highlights that enemy is selected
    public Image AllyHighlight;   //highlights that ally is selected

    public AttackTargetSelector targetSelector;

    public bool currentlySelectedBtn = false;

    //the targetting strategy's stat
    public string stat;



    private void Start() {
        Debug.Log("BUtton");
        enemyBtn.onClick.AddListener(selectEnemy);
        allyBtn.onClick.AddListener(selectAlly);
    }

    public void highlightEnemy() {
        //maybe animate the filling
        EnemyHighlight.fillAmount = 1;
        if (targetSelector.highest)
            EnemyHighlight.transform.localScale = new Vector3(1, -1, 1); //Maximum
        else
            EnemyHighlight.transform.localScale = new Vector3(1, 1, 1); //Minimum
    }
    public void selectEnemy() {
        currentlySelectedBtn = true;
        removeHighlights("ally");
        //removes highlight from all other Buttons in TargetSelector
        targetSelector.removeHighlights(this);

        highlightEnemy();

        switch (stat) {
            case "PD":
                if (targetSelector.highest)
                    targetSelector.selectHighestPDEnemy();
                else
                    targetSelector.selectLowestPDEnemy();
                break;
            case "MD":
                if (targetSelector.highest)
                    targetSelector.selectHighestMDEnemy();
                else
                    targetSelector.selectLowestMDEnemy();
                break;
            case "AS":
                if (targetSelector.highest)
                    targetSelector.selectHighestASEnemy();
                else
                    targetSelector.selectLowestASEnemy();
                break;
            case "MS":
                if (targetSelector.highest)
                    targetSelector.selectHighestMSEnemy();
                else
                    targetSelector.selectLowestMSEnemy();
                break;
            case "Range":
                if (targetSelector.highest)
                    targetSelector.selectHighestRangeEnemy();
                else
                    targetSelector.selectLowestRangeEnemy();
                break;
            case "HP":
                if (targetSelector.highest)
                    targetSelector.selectHighestHPEnemy();
                else
                    targetSelector.selectLowestHPEnemy();
                break;

            case "Closest":
                targetSelector.selectClosestEnemy();
                break;
            default:
                Debug.LogError("Invalid stat: " + stat);
                break;
        }

    }

    public void highlightAlly() {
        //maybe animate the filling
        AllyHighlight.fillAmount = 1;
        if (targetSelector.highest)
            AllyHighlight.transform.localScale = new Vector3(1, 1, 1); //Maximum
        else
            AllyHighlight.transform.localScale = new Vector3(1, -1, 1); //Minimum
    }
    public void selectAlly() {
        currentlySelectedBtn = true;
        removeHighlights("enemy");
        //removes highlight from all other Buttons in TargetSelector
        targetSelector.removeHighlights(this);

        highlightAlly();

        switch (stat) {
            case "PD":
                if (targetSelector.highest)
                    targetSelector.selectHighestPDAlly();
                else
                    targetSelector.selectLowestPDAlly();
                break;
            case "MD":
                if (targetSelector.highest)
                    targetSelector.selectHighestMDAlly();
                else
                    targetSelector.selectLowestMDAlly();
                break;
            case "AS":
                if (targetSelector.highest)
                    targetSelector.selectHighestASAlly();
                else
                    targetSelector.selectLowestASAlly();
                break;
            case "MS":
                if (targetSelector.highest)
                    targetSelector.selectHighestMSAlly();
                else
                    targetSelector.selectLowestMSAlly();
                break;
            case "Range":
                if (targetSelector.highest)
                    targetSelector.selectHighestRangeAlly();
                else
                    targetSelector.selectLowestRangeAlly();
                break;
            case "HP":
                if (targetSelector.highest)
                    targetSelector.selectHighestHPAlly();
                else
                    targetSelector.selectLowestHPAlly();
                break;

            case "Closest":
                targetSelector.selectClosestAlly();
                break;
            default:
                Debug.LogError("Invalid stat: " + stat);
                break;
        }
        Debug.Log("A;lly targetting clickedf");
    }

    public void removeHighlights(string toBeRemoved) {
        //removes highlight from current Button
        switch (toBeRemoved) {
            case "both":
                AllyHighlight.fillAmount = 0;
                EnemyHighlight.fillAmount = 0;
                break;
            case "enemy":
                EnemyHighlight.fillAmount = 0;
                break;
            case "ally":
                AllyHighlight.fillAmount = 0;
                break;

            default: break;
        }
    }
}
