using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSelect : MonoBehaviour
{
    //holds empty parent object that contains children objects that are to be kept between scenes
    public GameObject dontDestroys;
    public string sceneToLoad;

    [SerializeField] private Image goToScene;
    [SerializeField] private Button startBtn;
    [SerializeField] private Button details;
    [SerializeField] private Button closeUI;

    private void Start() {
        startBtn.onClick.AddListener(goTo);
        closeUI.onClick.AddListener(close);
        dontDestroys = GameObject.FindGameObjectWithTag("dontDestroys");
    }

    //clicky stuff
    private float mouseHoldDuration = 0;
    private bool click = false;

    //onclick load specified scene
    private void OnMouseDown() {
        click = true;
    }

    private void mouseClickedNotHeld() {
        //if this function is called by OnMouseDown
        if (click) {
            //if click is still held increment time
            if (Input.GetMouseButton(0)) {
                mouseHoldDuration += Time.fixedDeltaTime;
            }
            //if click is not held check how long it was held for. If it was held for less than 0.2 seconds show goToScene screen
            else if (mouseHoldDuration < 0.2f) {
                goToScene.gameObject.SetActive(true);
                //reset values
                mouseHoldDuration = 0;
                click = false;
            }
            //if click is held too long
            else {
                //reset values
                mouseHoldDuration = 0;
                click = false;
            }
        }
    }

    //jumps to scene and keeps the dontDestroys objects
    private void goTo() {
        DontDestroyOnLoad(dontDestroys);
        SceneManager.LoadScene(sceneToLoad);
    }

    private void close() {
        goToScene.gameObject.SetActive(false);
    }
    private void FixedUpdate() {

        mouseClickedNotHeld();
    }
}
