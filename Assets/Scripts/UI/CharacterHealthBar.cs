using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterHealthBar : MonoBehaviour
{
    //to access zoneStarted
    public UIManager uiManager;
    //to access teh character's Stats
    public Character character;
    //to change the fill of healthbar
    public Image health;
    //to display hp numerically
    public TextMeshProUGUI HPtext;

    private void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        //since on start healthdisplay is updated manually this means taht when we create instances of charDisplay the HP will be upToDate
        manualDisplayHealth();
    }
    public void handleHealthBar() {
        health.fillAmount = ((float)character.HP / (float)character.HPMax);
    }
    private void handleHealthText() {
        //trycatch used since we won't always be displaying text so HPtext would be empty etc...
        try {
            HPtext.text = (character.HP.ToString("F1") + "/" + character.HPMax.ToString("F1"));
        }
        catch {
            return;
        }
    }
    public void manualDisplayHealth() {
        handleHealthText();
        handleHealthBar();
    }
    void Update()
    {
        if (uiManager.zoneStarted()) {
            handleHealthText();
            handleHealthBar();
        }
    }
}
