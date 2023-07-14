using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AbilityDisplay : MonoBehaviour
{
    //main image
    public Image self;
    //these are modified in characterInfoScreen
    public UIManager uiManager;
    public Image cooldownBar;
    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI cooldownText;
    public Ability ability;
    public Button btn;
    //public TextMeshProUGUI targettingStrategyText;
    public Transform iconHolder;

    //we just have the holder so we can better place it visually some horizontal layout group gimic stuff
    public GameObject removeButtonHolder;
    public Button removeButton;
    [SerializeField] public StatIcon HP;
    [SerializeField] public StatIcon PD;
    [SerializeField] public StatIcon MD;
    [SerializeField] public StatIcon INF;
    [SerializeField] public StatIcon AS;
    [SerializeField] public StatIcon MS;
    [SerializeField] public StatIcon RNG;
    [SerializeField] public StatIcon LS;
    [SerializeField] public StatIcon CD;
    [SerializeField] public StatIcon LVL;

    public void Start() {
        HP.ratio = ability.HPMaxRatio*1.5f;
        PD.ratio = ability.PDRatio;
        MD.ratio = ability.MDRatio;
        INF.ratio = ability.INFRatio;
        AS.ratio = ability.ASRatio;
        MS.ratio = ability.MSRatio;
        LVL.ratio = ability.LVLRatio;

        //cooldownBar.color = ColorPalette.singleton.getRarityColor(ability.rarity);

        //delete whatever isn't applicable
        foreach (Transform child in iconHolder) {
            StatIcon temp = child.GetComponent<StatIcon>();
            if (temp.ratio == 0) {
                Destroy(temp.gameObject);
            }
        }
        showScaling();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        self.color = ColorPalette.singleton.getIndicatorColor(ability.abilityType);

        //btn.onClick.AddListener(openTargetSelectorAbility);
        ////if inventory Screen display the remove button
        //if (uiManager.inventoryScreenHidden.hidden == false && uiManager.inventoryScreen.inventoryCharacterScreen.isActiveAndEnabled) {
        //    removeButtonHolder.SetActive(true);
        //    removeButton.onClick.AddListener(removeAbility);
        //}
        //else {
        //    removeButtonHolder.SetActive(false);
        //}
    }
    //this function only happens in inventory screen since the remove button is only visible in the inventorry screen
    private void removeAbility() {
        //sets the parent to be ability inventory
        ability.transform.parent = uiManager.playerParty.abilityInventory.transform;
        //removes ability from character
        uiManager.inventoryScreen.characterSelected.abilities.Remove(ability);
        //updates the character info screen view
        uiManager.inventoryScreen.inventoryCharacterScreen.viewCharacterFullScreen(uiManager.inventoryScreen.characterSelected);
        //saves removing the ability
        if (SceneManager.GetActiveScene().name == "World") {
            uiManager.saveWorldSave();
        }
        else
            uiManager.saveMapSave();
    }
    private void showScaling() {
        //sorts them in descending order
        for (int i = 0; i < iconHolder.childCount-1; i++) {
            //assume first is max
            //Debug.Log(i);
            //StatIcon max = transform.GetChild(i).GetComponent<StatIcon>();
            for (int j = i+1; j < iconHolder.childCount; j++) {
                //StatIcon curr = transform.GetChild(j).GetComponent<StatIcon>();
                ////if (curr.ratio > max.ratio) {
                ////    max = curr;
                ////}
            }
            //Debug.Log(max);
            //max.transform.SetSiblingIndex(i);
        }
        
    }

    //maybe make the current ability that will have it's target changed in ability header
    public void openTargetSelectorAbility() {
        ////This should open the characterInfoScreen's target selection
        //Debug.Log("Clicked");
        ////if inventoryScreen
        //if (!uiManager.inventoryScreenHidden.hidden) {
        //    //sets the ability to be modified
        //    uiManager.inventoryScreen.inventoryCharacterScreen.targetSelector.ability = ability;
        //    //opens the screen and saits ability to true
        //    uiManager.inventoryScreen.inventoryCharacterScreen.openTargetSelectorAbility();
        //    Debug.Log("I am sending this ability" + ability.name);
        //}
        ////if regular char screen
        //else {
        //    uiManager.characterInfoScreen.targetSelector.ability = ability;
        //    uiManager.characterInfoScreen.openTargetSelectorAbility();
        //}

    }

}
