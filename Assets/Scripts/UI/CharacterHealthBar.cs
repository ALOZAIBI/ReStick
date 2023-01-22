using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterHealthBar : MonoBehaviour
{
    //to access teh character's Stats
    public Character character;
    //to change the fill of healthbar
    public Image health;
    //to display hp numerically
    public TextMeshProUGUI HPtext;
    public void handleHealthBar() {
        health.fillAmount = ((float)character.HP / (float)character.HPMax);
    }
    private void handleHealthText() {
        //trycatch used since we won't always be displaying text so HPtext would be empty etc...
        try {
            HPtext.text = (int)character.HP + "/" + (int)character.HPMax;
        }
        catch {
            return;
        }
    }
    void Update()
    {
        handleHealthText();
        handleHealthBar();
    }
}
