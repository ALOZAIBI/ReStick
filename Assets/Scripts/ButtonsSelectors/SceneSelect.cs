using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour
{
    //holds a list of objects that shouldn't be destroyed.
    public List<GameObject> dontDestroys = new List<GameObject>();
    public string sceneToLoad;
    //onclick load specified scene
    private void OnMouseDown() {
        foreach(GameObject temp in dontDestroys) {
            DontDestroyOnLoad(temp);
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
