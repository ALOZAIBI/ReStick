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

        //Displays Map Won Screen
        if (allComplete()) {
            uiManager.sceneToLoad = belongsToWorld;
            uiManager.mapWonScreenHidden.hidden = false;
        }
        
    }

    private bool allComplete() {
        foreach (SceneSelect sceneSelector in sceneSelectors) {
            if (sceneSelector.completed == false)
                return false;
        }
        return true;
    }
}
