using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour
{
    public Character character;
    [SerializeField] private Image characerPortrait;
    [SerializeField] private CharacterHealthBar healthBar;

    private void Start() {
        //sets the image
        characerPortrait.sprite = character.GetComponent<SpriteRenderer>().sprite;
        characerPortrait.color = character.GetComponent<SpriteRenderer>().color;
        
    }
    private void Update() {
        //sets the HPbar
        healthBar.character = character;
    }

}
