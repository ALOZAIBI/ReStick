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
    //drops the character in mouse position
    private void OnMouseUp() {
        camMov.pannable = true;
        if (selected) {
            character.gameObject.SetActive(true);
            character.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
            selected = false;
            Image image = this.gameObject.GetComponent<Image>();
            Color temp = image.color;
            temp.a = 0.1f;
            image.color = temp;
        }
    }
    private void Update() {
        
    }

}
