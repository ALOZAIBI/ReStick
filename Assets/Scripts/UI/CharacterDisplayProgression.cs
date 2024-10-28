using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplayProgression : CharacterDisplay
{
    //If the character has leveled up, we display its previous level
    [SerializeField] private TextMeshProUGUI prevLevelText;
    private new void Start() {
        base.Start();
        xpBar.automaticUpdate = false;
        healthBar.automaticUpdate = false;

        prevLevelText.text = XPBar.getLevelText(character.zsLevel);
        prevLevelText.gameObject.SetActive(character.zsLevel != character.level);
    }


}
