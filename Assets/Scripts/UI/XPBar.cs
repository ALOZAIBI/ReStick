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

    public bool automaticUpdate = true;

    //Values used to display the progression of XP

    //To keep count of how many times to fill out xpBar
    private int levelDiff;
    //To initially display the old xpProgress but just once
    private bool manualDisplayOnce = false;

    private float lerpT = 0f;
    [SerializeField]private float lerpSpeed = 0.8f;
    //So that it doesn't animate immediately
    [SerializeField]private float timeBeforeAnimating = 0.7f;
    private float currentTimeBeforeAnimating = 0;




    private void Start() {
        if (automaticUpdate)
            manualDisplayXP(character.xpProgress,character.xpCap,character.level);
        else {
            manualDisplayXP(character.zsXPProgress, Character.getXPCap(character.zsLevel), character.zsLevel);
        }
    }
    public void manualDisplayXP(float xpProgress, float xpCap, int level) {
        xpBar.fillAmount = xpProgress / xpCap;

        levelText.text = level.ToString();
    }

    public void displayProgression(float xpProgressPrev, int levelPrev) {
        if (!manualDisplayOnce) {
            levelDiff = character.level - levelPrev;
            manualDisplayXP(xpProgressPrev, Character.getXPCap(levelPrev), levelPrev);
            manualDisplayOnce = true;
        }

        if(currentTimeBeforeAnimating < timeBeforeAnimating) {
            currentTimeBeforeAnimating += Time.unscaledDeltaTime;
            return;
        }

        //For each level gained, fill out the xpBar
        if (levelDiff > 0) {
            xpBar.fillAmount = Mathf.Lerp(xpBar.fillAmount, 1, lerpT);
            lerpT += lerpSpeed * Time.unscaledDeltaTime;

            if(xpBar.fillAmount >= 0.99f) {
                levelDiff--;
                levelText.text = (character.level - levelDiff).ToString();
                xpBar.fillAmount = 0;
                lerpT = 0;
            }
        }

        //Once all levels are filled out, display the progression within the current level
        else {
            xpBar.fillAmount = Mathf.Lerp(xpBar.fillAmount, character.xpProgress / character.xpCap, lerpT);
            lerpT += lerpSpeed * Time.unscaledDeltaTime;
        }
    }



    private void Update() {
        if (automaticUpdate)
            manualDisplayXP(character.xpProgress,character.xpCap,character.level);
        else {
            displayProgression(character.zsXPProgress, character.zsLevel);
        }

    }
}
