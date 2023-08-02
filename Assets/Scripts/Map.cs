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

    }

    private bool allComplete() {
        foreach (SceneSelect sceneSelector in sceneSelectors) {
            if (sceneSelector.completed == false)
                return false;
        }
        return true;
    }
}
