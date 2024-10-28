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

    public bool automaticUpdate = true;
    //Values used to display the progression of HP

    //To initially display the old HP but just once
    private bool manualDisplayOnce = false;

    private float lerpT = 0f;
    [SerializeField] private float lerpSpeed = 0.8f;
    //So that it doesn't animate immediately
    [SerializeField] private float timeBeforeAnimating = 0.7f;
    private float currentTimeBeforeAnimating = 0;
    private void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        //since on start healthdisplay is updated manually this means taht when we create instances of charDisplay the HP will be upToDate
        if (automaticUpdate)
            manualDisplayHealth(character.HP, character.HPMax);
        else
            manualDisplayHealth(character.zsHP, character.zsHPMax);

        if (character != null) {
            //colors the healthbar according to team
            switch (character.team) {
                case ((int)Character.teamList.Enemy1):
                    health.color = ColorPalette.singleton.enemyHealthBar;
                    break;
                case ((int)Character.teamList.Player):
                    health.color = ColorPalette.singleton.allyHealthBar;
                    break;
                default:
                    break;
            }
        }
    }
    public void handleHealthBar(float HP,float HPMax) {
        try {
            health.fillAmount = ((float)HP / (float)HPMax);
        }
        catch { /*just to avoid errors in console*/}
    }
    private void handleHealthText(float HP, float HPMax) {
        //trycatch used since we won't always be displaying text so HPtext would be empty etc...
        try {
            HPtext.text = (HP.ToString("F1") + "/" + HPMax.ToString("F1"));
        }
        catch {
            return;
        }
    }
    public void manualDisplayHealth(float HP, float HPMax) {
        handleHealthText(HP,HPMax);
        handleHealthBar(HP,HPMax);
    }

    public void displayProgression(float HPPrev, float HPMaxPrev) {
        if (!manualDisplayOnce) {
            manualDisplayHealth(HPPrev, HPMaxPrev);
            manualDisplayOnce = true;
        }

        if (currentTimeBeforeAnimating < timeBeforeAnimating) {
            currentTimeBeforeAnimating += Time.unscaledDeltaTime;
            return;
        }

        //Lerp the healthbar fill amount to the new HP
        health.fillAmount = Mathf.Lerp(health.fillAmount, character.HP / character.HPMax, lerpT);
        lerpT += lerpSpeed * Time.unscaledDeltaTime;
    }
    void Update()
    {
        if(automaticUpdate)
            manualDisplayHealth(character.HP,character.HPMax);
        else
            displayProgression(character.zsHP, character.zsHPMax);


    }
}
