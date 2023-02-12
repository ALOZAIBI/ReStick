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
    //used to color what is selected
    public Image forColorPurposes;

    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI description;

    public Button self;

    private void Start() {
        self.onClick.AddListener(select);
        forColorPurposes = GetComponent<Image>();
        forColorPurposes.color = new Color(0.7f, 0.7f, 0.7f);
    }

    public void highlight() {
        forColorPurposes.color = new Color(0.7f, 1, 0.7f);
    }
    public void unHighlight() {
        forColorPurposes.color = new Color(0.7f, 0.7f, 0.7f);
    }
    private void select() {
        Debug.Log("Selected");
        selected = true;
        highlight();
        //deselects alll others
        foreach(AbilityDisplayReward deSelect in rewardSelect.listReward) {
            if(deSelect != this) {
                deSelect.selected = false;
                deSelect.unHighlight();
            }
        }
        //and ungreys out the confirmselection button
        rewardSelect.unGreyOutBtn();
    }
}
