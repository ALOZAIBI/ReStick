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
    [SerializeField] private GameObject playerParty;

    private void Start() {
        playerParty = GameObject.FindGameObjectWithTag("PlayerParty");
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

    //checks wether the click is held or not
    private void mouseClickedNotHeld() {
        
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

    //jumps to scene and sets all characters to inactive
    private void goTo() {
        foreach (Transform child in playerParty.transform) {
            if (child.tag == "Character") {
                //Debug.Log(child.name + "Disabled fuckl");
                child.gameObject.SetActive(false);
            }
        }
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
