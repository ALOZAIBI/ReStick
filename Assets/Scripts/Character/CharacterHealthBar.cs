using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthBar : MonoBehaviour
{
    //to access teh character's Stats
    public Character character;
    //to change the fill of healthbar
    public Image health;
    public void handleHealthBar() {
        health.fillAmount = ((float)character.HP / (float)character.HPMax);
    }
    void Update()
    {
        handleHealthBar();
    }
}
