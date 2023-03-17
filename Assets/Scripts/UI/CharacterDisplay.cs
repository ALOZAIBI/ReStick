using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterDisplay : MonoBehaviour {
    public Character character;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private Image characerPortrait;
    [SerializeField] private CharacterHealthBar healthBar;
    [SerializeField] private TextMeshProUGUI name;


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
    private bool click = false;
    //when the characterDisplay is clicked
    //drag
    private void OnMouseDown() {
        Debug.Log("BEING CLICKED");
        click = true;

    }


    private void mouseClickedNotHeld() {
        if (click) {
            if (Input.GetMouseButton(0)) {
                camMov.pannable = false;
                mouseHoldDuration += Time.unscaledDeltaTime;
                //if held drag character to mouse Position
                if (mouseHoldDuration > 0.2f) {
                    //to maybe optimise in future only keep the asterisk'd function here and move the others to be executed once instead of in Update
                    camMov.pannable = false;
                    character.gameObject.SetActive(true);
                    character.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);//asterisk'd
                    Image image = this.gameObject.GetComponent<Image>();
                    Color temp = image.color;
                    temp.a = 0.1f;
                    image.color = temp;
                }
            }
           
            else{
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
    }
    private void Update() {
        //if placing screen is not hidden check if clicked and not held
        //this is important since pannable will be changed in Character script
        //without this conditional pannable will always be set by this function
        if (!uiManager.placingScreenHidden.hidden) {
            mouseClickedNotHeld();
        }
    }

}
