using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

public class InventoryCharacterDisplay : MonoBehaviour
{
    public Character character;
    //the btn is the cooldownBar itself 
    [SerializeField] private Button btn;
    [SerializeField] public InventoryScreen inventoryScreen;
    [SerializeField] private Image characerPortrait;
    [SerializeField] private CharacterHealthBar healthBar;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private GameObject notification;


    //to make the button glow
    public bool glow;
    public ColorBlock defaultColor;
    void Start()
    {
        inventoryScreen = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().inventoryScreen;
        btn.onClick.AddListener(viewCharacter);
        //sets the image
        characerPortrait.sprite = character.GetComponent<SpriteRenderer>().sprite;
        characerPortrait.color = character.GetComponent<SpriteRenderer>().color;
        //sets the HPbar
        healthBar.character = character;
        //sets the name
        name.text = character.name;
        defaultColor = btn.colors;
    }

    public void viewCharacter() {
        //if character selectetd first
        //(Still in landingPage
        if (inventoryScreen.pageIndex == 0) {
            inventoryScreen.characterSelected = character;
            inventoryScreen.viewCharacter();
        }
        //if ability selected first then character selected
        if(inventoryScreen.pageIndex == 1) {
            inventoryScreen.characterSelected = character;
            inventoryScreen.viewCharacter();
            inventoryScreen.openAbilityCharacterPage();
            inventoryScreen.inventoryCharacterScreen.confirmAddAbilityPage();
        }
        //and also open CharacterScreen with character
    }
    private void Update() {
        if (glow) {
            float x = Mathf.PingPong(Time.unscaledTime * 0.3f, 0.5f);
            new Color(x, x, x);
            ColorBlock cb = btn.colors;
            cb.normalColor = defaultColor.normalColor + new Color(x, x, x);
            btn.colors = cb;
        }
        else {
            btn.colors = defaultColor;
        }
        if (character.statPoints > 0) {
            notification.SetActive(true);
        }
        else
            notification.SetActive(false);
    }

}
