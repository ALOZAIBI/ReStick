using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityDisplayReward : MonoBehaviour
{
    //need to actuaklly create the prefab for this script

    public Ability ability;

    //to be able to deselect everything else when this is selected
    public RewardSelect rewardSelect;
    //wether this is selected or not
    public bool selected;

    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI description;

    public Button self;

    private void Start() {
        self.onClick.AddListener(select);
    }
    private void select() {
        Debug.Log("Selected");
        selected = true;
        //deselects alll others
        foreach(AbilityDisplayReward deSelect in rewardSelect.listReward) {
            if(deSelect != this) {
                deSelect.selected = false;
            }
        }
    }
}
