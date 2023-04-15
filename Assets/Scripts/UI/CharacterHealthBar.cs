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
        //colors the healthbar according to team
        switch (character.team) {
            case ((int)Character.teamList.Enemy1):
                health.color = ColorPalette.enemyHealthBar;
                break;
            case ((int)Character.teamList.Player):
                health.color = ColorPalette.allyHealthBar;
                break;
            default:
                break;
        }
    }
    public void handleHealthBar() {
        try {
            health.fillAmount = ((float)character.HP / (float)character.HPMax);
        }
        catch { /*just to avoid errors in console*/}
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
