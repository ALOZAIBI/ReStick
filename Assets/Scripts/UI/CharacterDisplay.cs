using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterDisplay : MonoBehaviour
{
    public Character character;
    
    [SerializeField] private Image characerPortrait;
    [SerializeField] private CharacterHealthBar healthBar;
    [SerializeField] private TextMeshProUGUI name;

    public bool selected = false;

    //to get position of mouse to be used in MOuseUp
    public Camera cam;
    public CameraMovement camMov;
    private void Start() {
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

    //when the characterDisplay is clicked
    //drag
    private void OnMouseDown() {
        camMov.pannable = false;
        selected = true;
    }
    //drop
    private void OnMouseUp() {
        camMov.pannable = true;
        if (selected) {
            character.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
            selected = false;
        }
    }
    private void Update() {
        
    }

}
