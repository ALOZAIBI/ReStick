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
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        sceneSelectors = FindObjectsOfType<SceneSelect>();
        uiManager.menuUIHidden.hidden = false;
        //Displays Map Won Screen
        if (allComplete()) {
            //revives and heals all characters to full
            foreach(Transform child in uiManager.playerParty.transform) {
                if(child.tag == "Character") {
                    Character temp = child.GetComponent<Character>();
                    temp.alive = true;
                    temp.HP = temp.HPMax;
                }
            }
            uiManager.sceneToLoad = belongsToWorld;
            uiManager.mapWonScreenHidden.hidden = false;
        }

        uiManager.retryBtn.gameObject.SetActive(false);
        uiManager.exitBtn.gameObject.SetActive(false);

        //Focuses camera on the next level
        Transform nextLevel = getNextLevel();
        Camera.main.transform.position = new Vector3(0, nextLevel.position.y, Camera.main.transform.position.z);

    }

    private bool allComplete() {
        foreach (SceneSelect sceneSelector in sceneSelectors) {
            if (sceneSelector.completed == false)
                return false;
        }
        return true;
    }

    //Returns the transform of the level after the highest completed level
    private Transform getNextLevel() {
        SceneSelect highestCompleted = sceneSelectors[0];
        int maxCompleted = 0;
        foreach (SceneSelect tempSelect in sceneSelectors) {
            if (tempSelect.completed) {
                //Get the level number by extracting the number after the - from the sceneToLoad
                int levelNumber = int.Parse(tempSelect.sceneToLoad.Split('-')[1]);
                if (levelNumber > int.Parse(highestCompleted.sceneToLoad.Split('-')[1])) {
                    maxCompleted = levelNumber;
                    highestCompleted = tempSelect;
                }
            }
                
        }
        Debug.Log("Max completed" + maxCompleted);
        //Finds the sceneSelector with level number 1 higher than the highest completed level
        foreach (SceneSelect tempSelect in sceneSelectors) {
            if (tempSelect.sceneToLoad.Split('-')[1] == (maxCompleted + 1).ToString()) {
                Debug.Log("Next Level isss " + tempSelect.sceneToLoad);
                return tempSelect.transform;
            }
        }
        Debug.Log("Next Level is " + maxCompleted);
        return highestCompleted.transform;
    }
}
