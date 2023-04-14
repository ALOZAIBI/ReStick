using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterDisplayShop : MonoBehaviour {
    public Character character;

    [SerializeField] private Image characerPortrait;
    [SerializeField] private CharacterHealthBar healthBar;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private Button self;
    private void Start() {
        //sets the image
        characerPortrait.sprite = character.GetComponent<SpriteRenderer>().sprite;
        characerPortrait.color = character.GetComponent<SpriteRenderer>().color;
        //sets the HPbar
        healthBar.character = character;
        //sets the name
        name.text = character.name;

        self.onClick.AddListener(select);
    }

    private void select() {

    }
}
