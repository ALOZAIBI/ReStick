using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacter : MonoBehaviour
{
    //to access teh character's Stats
    [SerializeField] Character character;
    //to change the fill of healthbar
    [SerializeField] Image health;
    public void handleHealthBar() {
        health.fillAmount = ((float)character.HP / (float)character.HPMax);
    }
    void Update()
    {
        handleHealthBar();
    }
}
