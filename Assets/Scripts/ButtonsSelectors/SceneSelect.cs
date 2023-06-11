using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//To make this Start function execute Before Map's Start function
[DefaultExecutionOrder(-100)]

public class SceneSelect : MonoBehaviour
{
    //holds empty parent object that contains children objects that are to be kept between scenes
    public GameObject dontDestroys;
    public string sceneToLoad;

    [SerializeField] private Image goToScene;
    [SerializeField] private Button startBtn;
    [SerializeField] private Button details;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private Button closeUI;
    [SerializeField] private UIManager uiManager;


    public bool map;
    public bool zone;

    public bool completed;
    public class CannotBeMapAndZone : Exception {
        public CannotBeMapAndZone(string message) : base(message) {

        }
    }
    private void Start() {
        if(map == zone) {
            throw new CannotBeMapAndZone("can't be Map AND zone");
        }
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        startBtn.onClick.AddListener(goTo);
        closeUI.onClick.AddListener(close);
        dontDestroys = GameObject.FindGameObjectWithTag("dontDestroys");
        DisplayName();

        SaveSystem.loadCompletionSceneSelect(this);

        //Debug.Log(sceneToLoad + "=zoneName is it completed?" + completed);


        if (completed) {
            GetComponent<SpriteRenderer>().color = new Color(0,1,0);
        }
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
            //counts mouse down to determine if click or hold
            if (Input.GetMouseButton(0)) {
                mouseHoldDuration += Time.unscaledDeltaTime;
            }
            //if click 
            else if (mouseHoldDuration < 0.2f) {
               
                goToScene.gameObject.SetActive(true);
                //reset values
                mouseHoldDuration = 0;
                click = false;
            }
            //if HOLD
            else {
                //reset values
                mouseHoldDuration = 0;
                click = false;
            }
        }
    }

    //jumps to scene and sets all characters to inactive
    private void goTo() {
        if (map) {
            //Save GamestateData to be in this map
            SaveSystem.saveGameState(sceneToLoad, true);
            //deletes shop since everytime you visit a new map a new shop shouild be initialized
            SaveSystem.deleteShop();
            //save WorldSaves
            uiManager.saveWorldSave();
            //and also save MapSave so that I have a mapSave to base stuff off of
            uiManager.saveMapSave();
            uiManager.inZone = false;
        }
        if (zone) {
            //save MapSave
            uiManager.saveMapSave();
            //and marks inZone
            uiManager.inZone = true;
        }

        foreach (Transform child in uiManager.playerParty.transform) {
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
    private void Update() {

        mouseClickedNotHeld();
    }

    //thanks chat gpt
    public void DisplayName() {
        string result = sceneToLoad.Replace("Map", "").Replace("Zone", "");
        nameTxt.text = result;
    }
}
