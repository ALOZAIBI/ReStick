using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public Character character;
    [SerializeField] private Image xpBar;

    [SerializeField] private TextMeshProUGUI levelText;

    private void Start() {
        manualDisplayXP();
    }
    public void manualDisplayXP() {
        xpBar.fillAmount = character.xpProgress / character.xpCap;

        levelText.text = character.level.ToString();
    }

    private void Update() {
        manualDisplayXP();
    }
}
