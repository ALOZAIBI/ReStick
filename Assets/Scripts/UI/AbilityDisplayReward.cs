using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityDisplayReward : MonoBehaviour
{

    public Ability ability;
    //to be able to deselect everything else when this is selected
    public RewardSelectAbility rewardSelect;
    //wether this is selected or not
    public bool selected;
    //used to color what is selected
    public Image background;

    public Image rarity;

    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI description;

    public Button self;

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
    private void Start() {

        //Summs the raio array to get total ratio, This will be used to display the scaling of the ability(in descending order)
        HP.ratio = ability.HPMaxRatio.getSumOfValues() + ability.HPRatio.getSumOfValues();
        PD.ratio = ability.PDRatio.getSumOfValues();
        MD.ratio = ability.MDRatio.getSumOfValues();
        INF.ratio = ability.INFRatio.getSumOfValues();
        AS.ratio = ability.ASRatio.getSumOfValues();
        MS.ratio = ability.MSRatio.getSumOfValues();
        LVL.ratio = ability.LVLRatio.getSumOfValues();

        //delete whatever isn't applicable
        foreach (Transform child in iconHolder) {
            StatIcon temp = child.GetComponent<StatIcon>();
            if (temp.ratio == 0) {
                Destroy(temp.gameObject);
            }
        }

        self.onClick.AddListener(select);


        if (ability.rarity == (int)Ability.RaritiesList.Common)
            rarity.color = ColorPalette.singleton.commonRarity;
        if (ability.rarity == (int)Ability.RaritiesList.Rare) {
            rarity.color = ColorPalette.singleton.rareRarity;
        }
        if (ability.rarity == (int)Ability.RaritiesList.Epic) {
            rarity.color = ColorPalette.singleton.epicRarity;
        }
        if (ability.rarity == (int)Ability.RaritiesList.Legendary) {
            rarity.color = ColorPalette.singleton.legendaryRarity;
        }
        background.color = ColorPalette.singleton.getIndicatorColor(ability.abilityType);

        //unHighlight();
    }

    public void highlight() {
        Color temp = background.color;
        temp.a = 1f;
        background.color = temp;
    }
    public void unHighlight() {
        Color temp = background.color;
        temp.a = 0.3f;
        background.color = temp;
    }

    private void select() {
        Debug.Log("Selected");
        selected = true;
        highlight();
        //deselects alll others
        foreach(AbilityDisplayReward deSelect in rewardSelect.listAbilityReward) {
            if(deSelect != this) {
                deSelect.selected = false;
                deSelect.unHighlight();
            }
        }
        //and ungreys out the confirmselection button
        rewardSelect.unGreyOutAbilityBtn();
    }
}
