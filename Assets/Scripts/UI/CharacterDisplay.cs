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
    [SerializeField] private Button btn;


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

    //when the characterDisplay is clicked
    //drag
    private void mouseClickedNotHeld() {
        if (click) {
            camMov.pannable = false;
            Debug.Log("Mouse clicking?" + Input.GetMouseButton(0));
            if (Input.GetMouseButton(0)) {
                mouseHoldDuration += Time.unscaledDeltaTime;
                //if held drag character to mouse Position
                if (mouseHoldDuration > 0.2f) {
                    camMov.pannable = false;
                    character.gameObject.SetActive(true);
                    character.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
                    //We are disabling the agent here since if it enabled and while im dragging a chracter into the scene if it bumps an obstacle the agent will be stopped by the obstacle the but visually it will look fine until I start the game wher what happens is the character teleports to where the agent is
                    character.agent.enabled = false;
                    Image image = this.gameObject.GetComponent<Image>();
                    Color temp = image.color;
                    temp.a = 0.1f;
                    image.color = temp;
                }
            }
           
            else{//on release (when cahracter is dropped)
                if(mouseHoldDuration > 0.2f) {
                    //re-enabling it
                    character.agent.enabled = true;
                }
                //if just a click display characterInfoScreen
                if (mouseHoldDuration < 0.2f) {
                    uiManager.viewCharacter(character);
                    mouseHoldDuration = 0;
                }
                //resets values
                click = false;
                camMov.pannable = true;
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
            mouseClickedNotHeld();
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("Pointer down test" + character.name);
        click = true;
    }
}
