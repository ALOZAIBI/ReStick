using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TopStatDisplay : MonoBehaviour
{
    //stats texts
    public UIManager uiManager;


    public TextMeshProUGUI PD, MD, AS, CDR, MS, RNG, LS;
    public TextMeshProUGUI charName;


    public Character character;
    public CharacterHealthBar healthBar;

    public Button moreInfoBtn;

    public TextMeshProUGUI levelText;
    public Image levelBar;
    //the character will be assigned when the character is clicked
    //on zone's start hide me
    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        moreInfoBtn.onClick.AddListener(displayMoreInfo);
    }

    private void handleColor() {
        if (character.PD > character.zsPD)
            PD.color = ColorPalette.buff;
        else
        if (character.PD < character.zsPD)
            PD.color = ColorPalette.debuff;
        else
            PD.color = ColorPalette.defaultColor;

        if (character.MD > character.zsMD)
            MD.color = ColorPalette.buff;
        else
        if (character.MD < character.zsMD)
            MD.color = ColorPalette.debuff;
        else
            MD.color = ColorPalette.defaultColor;

        if (character.AS > character.zsAS)
            AS.color = ColorPalette.buff;
        else
        if (character.AS < character.zsAS)
            AS.color = ColorPalette.debuff;
        else
            AS.color = ColorPalette.defaultColor;

        if (character.CDR > character.zsCDR)
            CDR.color = ColorPalette.buff;
        else
        if (character.CDR < character.zsCDR)
            CDR.color = ColorPalette.debuff;
        else
            CDR.color = ColorPalette.defaultColor;

        if (character.MS > character.zsMS)
            MS.color = ColorPalette.buff;
        else
        if (character.MS < character.zsMS)
            MS.color = ColorPalette.debuff;
        else
            MS.color = ColorPalette.defaultColor;

        if (character.Range > character.zsRange)
            RNG.color = ColorPalette.buff;
        else
        if (character.Range < character.zsRange)
            RNG.color = ColorPalette.debuff;
        else
            RNG.color = ColorPalette.defaultColor;

        if (character.Range > character.zsRange)
            RNG.color = ColorPalette.buff;
        else
        if (character.Range < character.zsRange)
            RNG.color = ColorPalette.debuff;
        else
            RNG.color = ColorPalette.defaultColor;

        if (character.LS > character.zsLS)
            LS.color = ColorPalette.buff;
        else
        if (character.LS < character.zsLS)
            LS.color = ColorPalette.debuff;
        else
            LS.color = ColorPalette.defaultColor;

        //colors the healthbar according to team
        switch (character.team) {
            case ((int)Character.teamList.Enemy1):
                healthBar.health.color = ColorPalette.enemyHealthBar;
                break;
            case ((int)Character.teamList.Player):
                healthBar.health.color = ColorPalette.allyHealthBar;
                break;
            default:
                break;
        }
    }

    //opens charInfoScreen of character
    public void displayMoreInfo() {
        uiManager.viewCharacterInfo(character);
    }
    private void displayStats() {
        //f1 formats it to 1 decimal point
        PD.text = character.PD.ToString("F1");
        MD.text = character.MD.ToString("F1");
        AS.text = character.AS.ToString("F1");
        CDR.text = (character.CDR*100).ToString("F1");
        MS.text = character.MS.ToString("F1");
        RNG.text = character.Range.ToString("F1");
        LS.text = (character.LS*100).ToString("F1");
        healthBar.manualDisplayHealth();
    }
    // Update is called once per frame
    void Update()
    {
        try {
            healthBar.character = character;
            charName.text = character.name;

            levelText.text = "LVL: " + character.level;
            levelBar.fillAmount = (float)character.xpProgress / character.xpCap;

            //if character dies hide topstat display
            if (!character.alive)
                uiManager.topStatDisplayHidden.hidden = true;
        }
        catch { }
        
        handleColor();
        displayStats();
    }
}
