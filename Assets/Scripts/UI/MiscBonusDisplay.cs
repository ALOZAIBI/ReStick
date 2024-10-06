using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiscBonusDisplay : MonoBehaviour {
    [SerializeField] public MiscBonus bonus;

    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI amtText;

    [SerializeField] private Image background;

    public bool selected;

    [SerializeField] private Button self;

    private void Start() {
        init(bonus);
    }

    public void init(MiscBonus bonus) {
        this.bonus = bonus;
        typeText.text = bonus.type.ToString();
        amtText.text = bonus.bonusAmt.ToString();

        self.onClick.AddListener(select);

        //Set color using color palette
        switch ((int)bonus.type) {
            case (int)MiscBonus.myType.HP:
                background.color = ColorPalette.singleton.hpMisc;
                break;
            case (int)MiscBonus.myType.XP:
                background.color = ColorPalette.singleton.xpMisc;
                break;
            case (int)MiscBonus.myType.Gold:
                background.color = ColorPalette.singleton.goldMisc;
                break;
            case (int)MiscBonus.myType.Life:
                background.color = ColorPalette.singleton.lifeMisc;
                break;
        }
    }

    public void highlight() {
        background.SetAlpha(1);
    }

    public void unHighlight() {
        background.SetAlpha(0.3f);
    }

    public void select() {
        selected = true;
        highlight();
        //deselects all other miscBonusDisplays
        foreach (MiscBonusDisplay display in RewardManager.singleton.miscBonusRewarder.displays) {
            if (display != this) {
                display.unHighlight();
                display.selected = false;
            }
        }
        RewardManager.singleton.unGreyOutConfirmBtn();
    }
}
