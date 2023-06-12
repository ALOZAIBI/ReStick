using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public TextMeshProUGUI targettingStrategyText;
    public Transform iconHolder;

    [SerializeField] private StatIcon HP;
    [SerializeField] private StatIcon PD;
    [SerializeField] private StatIcon MD;
    [SerializeField] private StatIcon INF;
    [SerializeField] private StatIcon AS;
    [SerializeField] private StatIcon MS;
    [SerializeField] private StatIcon RNG;
    [SerializeField] private StatIcon LS;
    [SerializeField] private StatIcon CD;
    [SerializeField] private StatIcon LVL;

    public void Start() {
        HP.ratio = ability.HPMaxRatio*1.5f;
        PD.ratio = ability.PDRatio;
        MD.ratio = ability.MDRatio;
        INF.ratio = ability.INFRatio;
        AS.ratio = ability.ASRatio;
        MS.ratio = ability.MSRatio;
        LVL.ratio = ability.LVLRatio;


        //delete whatever isn't applicable
        foreach (Transform child in iconHolder) {
            StatIcon temp = child.GetComponent<StatIcon>();
            if (temp.ratio == 0) {
                Destroy(temp.gameObject);
            }
        }
        showScaling();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        btn.onClick.AddListener(openTargetSelectorAbility);
        self.color = ColorPalette.singleton.getIndicatorColor(ability.abilityType);
    }

    private void showScaling() {
        //sorts them in descending order
        for (int i = 0; i < iconHolder.childCount-1; i++) {
            //assume first is max
            Debug.Log(i);
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
        Debug.Log("Clicked");
        //if inventoryScreen
        if (!uiManager.inventoryScreenHidden.hidden) {
            //sets the ability to be modified
            uiManager.inventoryScreen.inventoryCharacterScreen.targetSelector.ability = ability;
            //opens the screen and saits ability to true
            uiManager.inventoryScreen.inventoryCharacterScreen.openTargetSelectorAbility();
            Debug.Log("I am sending this ability" + ability.name);
        }
        //if regular char screen
        else {
            uiManager.characterInfoScreen.targetSelector.ability = ability;
            uiManager.characterInfoScreen.openTargetSelectorAbility();
        }

    }

}
