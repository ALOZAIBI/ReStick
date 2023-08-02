using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//To make this Start function execute Before Map's Start function
[DefaultExecutionOrder(-100)]

public class SceneSelect : MonoBehaviour
{
    //holds empty parent object that contains children objects that are to be kept between scenes
    public GameObject dontDestroys;
    public string sceneToLoad;

    [SerializeField] private TextMeshProUGUI nameTxt;
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
        dontDestroys = GameObject.FindGameObjectWithTag("dontDestroys");
        DisplayName();

        SaveSystem.loadCompletionSceneSelect(this);

        //Debug.Log(sceneToLoad + "=zoneName is it completed?" + completed);


        if (completed) {
            GetComponent<SpriteRenderer>().color = new Color(0,1,0);
        }
    }

    //clicky stuff
    public float mouseHoldDuration = 0;
    public bool click = false;

    //onclick load specified scene
    private void customMouseDown() {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverGameObject()) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("SceneSelect"));
            //Debug.Log("This thing is getting touched"+hit.collider.name);
            if (hit.collider != null && hit.collider.tag == "SceneSelect") {
                hit.collider.GetComponent<SceneSelect>().click = true;
            }
            else {
                //Debug.Log("Blocking raycast" + hit.collider.name);
            }
        }
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
                //reset values
                mouseHoldDuration = 0;
                click = false;
                goTo();
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
            Camera.main.transform.position = new Vector3(0, 0, -10);
            //Save GamestateData to be in this map
            SaveSystem.saveGameState(sceneToLoad, true);
            //deletes shop since everytime you visit a new map a new shop shouild be initialized
            SaveSystem.deleteShop();
            //save WorldSaves
            uiManager.saveWorldSave();
            //and also save MapSave so that I have a mapSave to base stuff off of
            uiManager.saveMapSave();
            uiManager.inZone = false;
            //Initializes rewardProgress
            SaveSystem.setRewardProgress();
        }
        if (zone) {
            //save MapSave
            uiManager.saveMapSave();
            //and marks inZone
            uiManager.inZone = true;
        }

        foreach (Transform child in uiManager.playerParty.transform) {
            if (child.tag == "Character") {
                child.gameObject.SetActive(false);
            }
        }
        DontDestroyOnLoad(dontDestroys);
        SceneManager.LoadScene(sceneToLoad);
    }

    private void Update() {
        customMouseDown();
        mouseClickedNotHeld();
    }

    //thanks chat gpt
    public void DisplayName() {
        string result = sceneToLoad.Replace("Map", "").Replace("Zone", "");
        nameTxt.text = result;
    }
    public static bool IsPointerOverGameObject() {
        // Check mouse
        if (EventSystem.current.IsPointerOverGameObject()) {
            return true;
        }

        // Check touches
        for (int i = 0; i < Input.touchCount; i++) {
            var touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began) {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) {
                    return true;
                }
            }
        }

        return false;
    }
}
