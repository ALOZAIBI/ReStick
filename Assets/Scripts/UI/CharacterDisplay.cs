using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterDisplay : MonoBehaviour, IPointerDownHandler {
    public Character character;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private Image characerPortrait;
    [SerializeField] private CharacterHealthBar healthBar;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private Button btn;
    [SerializeField] private GameObject notification;
    [SerializeField] private SlicedFilledImage manualTargettingCDDisplay;
    public GameObject deathSkull;
    [SerializeField] private GameObject cameraOnIcon;

    //to get position of mouse to be used in MOuseUp
    public Camera cam;
    public CameraMovement camMov;
    private void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camMov = cam.GetComponent<CameraMovement>();
        //sets the image
        characerPortrait.sprite = character.GetComponent<SpriteRenderer>().sprite;
        characerPortrait.color = character.GetComponent<SpriteRenderer>().color;
        //sets the HPbar
        healthBar.character = character;
        //sets the name
        name.text = character.name;

    }

    private float mouseHoldDuration = 0;
    public bool click = false;

    //Used to check for double click
    [SerializeField] private float timeSinceFirstClick = 0;
    [SerializeField] private bool firstClickHappened = false;
    //If second click comes within doubleClicktime, open charScreen
    [SerializeField] private float doubleClickTime = 0.25f;

    //when the characterDisplay is clicked
    //drag
    private void dragToZone() {
        if (click) {
            
            camMov.pannable = false;
            //Debug.Log("Mouse clicking?" + Input.GetMouseButton(0));
            if (Input.GetMouseButton(0)) {

                //If held longer than 0.2f seconds
                if (mouseHoldDuration > 0.2f) {
                    //drag character to mouse Position (this only works if the zone hasn't started or character hasn't been dropped already)
                    if ((!character.dropped)||!uiManager.zoneStarted()) {
                        character.dropped = false;

                        //Tutorial Stuff (triggered when dragging character)
                        if (!uiManager.tutorial.draggingCharactersTutorialDone)
                            uiManager.tutorial.endDraggingCharactersTutorial();

                        //If the zone has started display the placeable overlay since it won't be displayed after the zone has started
                        if (uiManager.zoneStarted()) {
                            uiManager.zone.placeableOverlay.gameObject.SetActive(true);
                        }



                        camMov.pannable = false;
                        character.gameObject.SetActive(true);
                        character.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
                        uiManager.viewTopstatDisplay(character);
                        try {
                            //We are disabling the agent here since if it enabled and while im dragging a chracter into the scene if it bumps an obstacle the agent will be stopped by the obstacle the but visually it will look fine until I start the game wher what happens is the character teleports to where the agent is
                            character.agent.enabled = false;
                        }
                        catch { /*Maybe fixes stuff?*/}

                        Image image = gameObject.GetComponent<Image>();
                        Color temp = image.color;
                        temp.a = 0.1f;
                        image.color = temp;
                        //this is done to update collider position when timescale is set to 0 so that the character dragged in is correctly clickable.
                        Physics.SyncTransforms();
                        //Debug.Log("PHYSICS STYNCED");
                    }
                    //If held on a characterDisplay that has already been dropped and that the zone has started
                    else if (character.dropped && uiManager.zoneStarted()) {
                        if(character.manualTargettingCDRemaining <= 0)
                            uiManager.manualTargetting.characterToControl = character;
                        else
                            uiManager.tooltip.showMessage("Commanding this character is on cooldown");
                    }
                }

                mouseHoldDuration += Time.unscaledDeltaTime;
            }
           
            else{
                //on release
                //if held(when cahracter is dropped)
              
                if (mouseHoldDuration >= 0.2f && (!character.dropped)) {
                    character.dropped = true;

                    //re-enabling it
                    character.agent.enabled = true;


                    //Do a raycast to see if this hit's a layer called placeable
                    RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,Mathf.Infinity,LayerMask.GetMask("Placeable"));
                    //if it doesn't hit a placeable layer remove the character
                    if(hit.collider == null) {
                        //reset the characterDisplay color
                        Image image = gameObject.GetComponent<Image>();
                        Color temp = image.color;
                        temp.a = 1f;
                        image.color = temp;
                        //removes character
                        character.gameObject.SetActive(false);
                        character.dropped = false;
                        character.zone = null;
                        uiManager.zone.charactersInside.Remove(character);
                        //tooltip can't place character here
                        uiManager.tooltip.showMessage("Can't place character here");
                    }
                    //Camera focuses character that has just dropped
                    camMov.characterToFocusOn = character;

                }
                //if just a click
                else if(mouseHoldDuration <= 0.2f) {
                    uiManager.viewTopstatDisplay(character);
                    //Camera focuses character that was just clicked
                    camMov.characterToFocusOn = character;
                    mouseHoldDuration = 0;
                    //Double Click stuff
                    //If this is the first click(so firstClick didnt happen)
                    if (!firstClickHappened) {
                        firstClickHappened = true;
                    }
                    //If this isn't the first click(Double click)
                    else if(timeSinceFirstClick < doubleClickTime) {
                        firstClickHappened = false;
                        timeSinceFirstClick = 0;

                        uiManager.viewCharacterInfo(character);
                    }
                }

                

                //resets values
                click = false;
                camMov.pannable = true;
                //And if zone has started hide the placeable overlay
                if (uiManager.zoneStarted()) {
                    uiManager.zone.placeableOverlay.gameObject.SetActive(false);
                }
            }

            //Tutorial stuff triggered when character is tapped
            if(!uiManager.tutorial.addingAbilityTutorialDone && uiManager.tutorial.addingAbilityTutorialStep == 2) {
                uiManager.tutorial.conitnueAddingAbilityClickTopStatDisplay();
            }

            if(!uiManager.tutorial.upgradingStatsTutorialDone && uiManager.tutorial.upgradingStatsTutorialStep == 2 && character.statPoints>=6) {
                uiManager.tutorial.continueUpgradingStatsClickTopStatDisplay();
            }
        }
        else
            mouseHoldDuration = 0;
    }
    private void Update() {
        //if placing screen is not hidden check if clicked and not held
        //this is important since pannable will be changed in Character script
        //without this conditional pannable will always be set by this function
        if (!uiManager.placingScreenHidden.hidden) {
            dragToZone();
        }
        //Displays notifications when stat points are available and when zone not started
        if (character.statPoints > 0 && !uiManager.zoneStarted()) {
            notification.SetActive(true);
        }
        else
            notification.SetActive(false);

        level.text = character.level.ToString();

        //Display the manual targetting cooldown by filling the image accordingly
        if (character.alive) { 
            manualTargettingCDDisplay.fillAmount = (ManualTargetting.manualTargettingCD - character.manualTargettingCDRemaining) / ManualTargetting.manualTargettingCD; 
        }
        else
            manualTargettingCDDisplay.fillAmount = 0;
            
        
        //Continously update image
        characerPortrait.sprite = character.GetComponent<SpriteRenderer>().sprite;

        deathSkull.SetActive(!character.alive);

        if (firstClickHappened) {
            timeSinceFirstClick += Time.unscaledDeltaTime;
            if(timeSinceFirstClick > doubleClickTime) {
                firstClickHappened = false;
                timeSinceFirstClick = 0;
            }
        }

        //Camera icon when this dude is the focus and the dude is on the map
        cameraOnIcon.SetActive(camMov.characterToFocusOn == character
            &&
            character.dropped);

    }

    public void OnPointerDown(PointerEventData eventData) {
        //Debug.Log("Pointer down test" + character.name);
        click = true;
    }
}
