using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryCharacterDisplay : MonoBehaviour
{
    public Character character;
    //the btn is the icon itself 
    [SerializeField] private Button btn;
    [SerializeField] public InventoryScreen inventoryScreen;
    [SerializeField] private Image characerPortrait;
    [SerializeField] private CharacterHealthBar healthBar;
    [SerializeField] private TextMeshProUGUI name;

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
        if (inventoryScreen.abilitySelected == null) {
            inventoryScreen.characterSelected = character;
            inventoryScreen.viewCharacter();
            inventoryScreen.inventoryCharacterScreen.addAbilityBtn.gameObject.SetActive(true);
            inventoryScreen.inventoryCharacterScreen.confirmAddAbilityBtn.gameObject.SetActive(false);
        }
        //if ability selected first
        if(inventoryScreen.abilitySelected != null) {
            inventoryScreen.characterSelected = character;
            inventoryScreen.Body.SetActive(true);
            inventoryScreen.viewCharacter();
            //then enables confirm button only and not add ability button since ability already selected
            inventoryScreen.inventoryCharacterScreen.addAbilityBtn.gameObject.SetActive(false);
            inventoryScreen.inventoryCharacterScreen.confirmAddAbilityBtn.gameObject.SetActive(true);
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
    }

}
