using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityDisplay : MonoBehaviour
{
    public UIManager uiManager;
    public Image icon;
    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI description;
    public Ability ability;
    public Button btn;
    public TextMeshProUGUI targettingStrategyText;

    private void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        btn.onClick.AddListener(openTargetSelectorAbility);
    }

    //maybe make the current ability that will have it's target changed in ability header
    public void openTargetSelectorAbility() {
        //if inventoryScreen
        if (!uiManager.inventoryScreenHidden.hidden) {
            //opens the screen and saits ability to true
            uiManager.inventoryScreen.inventoryCharacterScreen.openTargetSelectorAbility();
            //sets the ability to be modified
            uiManager.inventoryScreen.inventoryCharacterScreen.targetSelector.ability = ability;
        }
        //if regular char screen
        else {
            uiManager.characterInfoScreen.openTargetSelectorAbility();
            uiManager.characterInfoScreen.targetSelector.ability = ability;
        }

    }

}
