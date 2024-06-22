using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class AbilityDisplayStatDifference : AbilityDisplay
{
    [SerializeField]private List<float> valueAmt;
    [SerializeField] private float coolDownInit;

    //The GameObject that holds all the arrays below
    [SerializeField] private GameObject[] valueDisplay;
    //The image that holds the text for the value name
    [SerializeField] private Image[] image;
    //The text that holds the value name
    [SerializeField] private TextMeshProUGUI[] valueName;
    //The text that holds the initial value
    [SerializeField] private TextMeshProUGUI[] initialValueTxt;
    //The text that holds the upgraded value
    [SerializeField] private TextMeshProUGUI[] upgradedValueTxt;

    const int indexOfCooldownValue = 5;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //Saves the value amounts of the ability //ToList is used to create a copy of the list otherwise they would be pointing to the same thing
        valueAmt = ability.valueAmt.ToList();

        coolDownInit = ability.getCDAfterChange();

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows or hides this gameObject based on if there is a difference after updating stats
    /// </summary>
    public void showOrHide(List<float>upgradedValueAmt) {
        close();
        if(valueAmt.Count > 4) {
            Debug.LogError("More values than this was programmed to handle");
        }

        if(valueAmt.Count != upgradedValueAmt.Count) {
            Debug.LogError("Lists are not the same size");
            return;
        }
        bool showAbilityDisplay = false;
        //Displays the values that are different
        for(int i = 0;i< valueAmt.Count; i++) {
            if (valueAmt[i] != upgradedValueAmt[i]) {
                showAbilityDisplay = true;
                updateValueDisplay(i);
            }
        }
        //Displays the CD if it's different
        if (coolDownInit != ability.getCDAfterChange()) {
            showAbilityDisplay = true;
            updateCoolDownDisplay();
        }
        gameObject.SetActive(showAbilityDisplay);

    }
    private void updateValueDisplay(int index) {
        //Sets the display to active
        valueDisplay[index].SetActive(true);
        //Gives it color
        image[index].color = ColorPalette.singleton.getTypeColor(ability.abilityType);
        image[index].Darken();
        //Gives it text
        valueName[index].text = ability.valueNames[index];
        //A Lazy bandaid to display int if it's number of balls
        if (valueName[index].text == "NumberOfBalls") {
            initialValueTxt[index].text = valueAmt[index].ToString("F0");
            upgradedValueTxt[index].text = ability.valueAmt[index].ToString("F0");
        }
        else {
            initialValueTxt[index].text = valueAmt[index].ToString("F1");
            upgradedValueTxt[index].text = ability.valueAmt[index].ToString("F1");
        }

    }
    private void updateCoolDownDisplay(int index = indexOfCooldownValue) {
        //Sets the display to active
        valueDisplay[index].SetActive(true);
        //Gives it color
        image[index].color = ColorPalette.singleton.getTypeColor(ability.abilityType);
        image[index].Darken();
        //Text is already set in the editor
        initialValueTxt[index].text = coolDownInit.ToString("F2");
        upgradedValueTxt[index].text = ability.getCDAfterChange().ToString("F2");
    }
    //Closes all the value displays
    private void close() {
        foreach (GameObject obj in valueDisplay) {
            obj.SetActive(false);
        }
    }
    public override void setupAbilityDisplay(Ability abilityToDisplay) {
        showScaling();
        ability = abilityToDisplay;

        self.color = ColorPalette.singleton.getTypeColor(ability.abilityType);
        abilityName.text = ability.abilityName;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
