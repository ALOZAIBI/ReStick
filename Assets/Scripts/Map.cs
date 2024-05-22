using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Since the value is 0 (more than SceneSelect's Value which is -100) this start function will execute after the SceneSelect's stasrt function
[DefaultExecutionOrder(0)]
public class Map : MonoBehaviour
{
    public UIManager uiManager;
    //the world this map belongs to
    public string belongsToWorld;

    public SceneSelect[] sceneSelectors;
    private void Start() {
        uiManager = UIManager.singleton;
        sceneSelectors = FindObjectsOfType<SceneSelect>();
        uiManager.menuUIHidden.hidden = false;

        uiManager.retryBtn.gameObject.SetActive(false);
        uiManager.exitBtn.gameObject.SetActive(false);

        //Focuses camera on the next level
        Transform nextLevel = getNextLevel();
        Camera.main.transform.position = new Vector3(0, nextLevel.position.y, Camera.main.transform.position.z);

        //Makes camera pannable (Sometimes it's not pannable since the camera isnot done doing the focus thing)
        uiManager.camMov.pannable = true;

        checkIfWon();

    }

    private void Update() {
        uiManager.updateGoldtextToCurrent();
    }
    private void displayMapWon() {
        uiManager.sceneToLoad = belongsToWorld;
        uiManager.mapWonScreenHidden.hidden = false;
        //revives and heals all characters to full
        foreach (Transform child in uiManager.playerParty.transform) {
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                temp.alive = true;
                temp.HP = temp.HPMax;
            }
        }
    }
    //Checks if won by checking the highest sceneSelector number then checks if that one is completed

    public void checkIfWon() {
        //Debug.Log("DBG Checking if won");
        //Finds the highest sceneSelector number
        int max = 0;
        foreach (SceneSelect tempSelect in sceneSelectors) {
            int levelNumber = int.Parse(tempSelect.sceneToLoad.Split('-')[1]);
            if (levelNumber > max) {
                max = levelNumber;
            }
        }
        //Debug.Log("DBG Max is " + max);
        //Finds the sceneSelector with the highest number
        foreach (SceneSelect tempSelect in sceneSelectors) {
            if (tempSelect.sceneToLoad.Split('-')[1] == max.ToString()) {
                //Debug.Log("DBG Max is " + max + " and is completed?" + tempSelect.completed);
                if (tempSelect.completed) {
                    displayMapWon();
                }
            }
        }
    }


    //Returns the transform of the level after the highest completed level
    private Transform getNextLevel() {
        SceneSelect highestCompleted = sceneSelectors[0];
        int maxCompleted = 0;
        foreach (SceneSelect tempSelect in sceneSelectors) {
            if (tempSelect.completed) {
                //Get the level number by extracting the number after the - from the sceneToLoad
                int levelNumber = int.Parse(tempSelect.sceneToLoad.Split('-')[1]);
                //Debug.Log("DBG " + tempSelect.sceneToLoad + " is completed?" + tempSelect.completed + " LevelNumber =" +levelNumber);
                if (levelNumber > maxCompleted) {
                    maxCompleted = levelNumber;
                    highestCompleted = tempSelect;
                    //Debug.Log("DBG Max completed" + maxCompleted);
                }
            }
                
        }
        //Debug.Log("DBG Max completed" + maxCompleted);
        //Finds the sceneSelector with level number 1 higher than the highest completed level
        foreach (SceneSelect tempSelect in sceneSelectors) {
            if (tempSelect.sceneToLoad.Split('-')[1] == (maxCompleted + 1).ToString()) {
                //Debug.Log("DBG Next Level isss " + tempSelect.sceneToLoad);
                return tempSelect.transform;
            }
        }
        //This return shouldnt be used it's just there to make the compiler happy
        //Debug.Log("DBG Next Level is " + maxCompleted);
        return highestCompleted.transform;
    }
}
