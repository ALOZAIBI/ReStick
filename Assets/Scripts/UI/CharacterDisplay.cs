using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterDisplay : MonoBehaviour
{
    public Character character;

    [SerializeField] protected Image characterPortrait;
    [SerializeField] protected Image characterPortraitBox;
    [SerializeField] protected CharacterHealthBar healthBar;
    [SerializeField] protected XPBar xpBar;

    [SerializeField] protected TextMeshProUGUI nameTxt;
    [SerializeField] protected TextMeshProUGUI levelTxt;

    [SerializeField] protected Button btn;

    protected void Start() {
        //sets the image
        characterPortrait.sprite = character.GetComponent<SpriteRenderer>().sprite;
        characterPortrait.color = character.GetComponent<SpriteRenderer>().color;

        //Get characterPortraitBox (Parent of characterPortrait)
        characterPortraitBox = characterPortrait.transform.parent.GetComponent<Image>();

        Debug.Log("Debwug0"+character.name);
        //sets the HPbar
        healthBar.character = character;
        xpBar.character = character;
        //sets the name
        nameTxt.text = character.name;
        //levelTxt.text = character.level.ToString();
    }


}
