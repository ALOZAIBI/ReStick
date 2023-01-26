using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterScreen : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    //stats texts
    public TextMeshProUGUI DMG, AS, MS, RNG, LS;
    //cool stats texts
    public TextMeshProUGUI totalKills;
    public CharacterHealthBar healthBar;

    public Image characterPortrait;

    //Used to instantiate AbilityDisplay prefab
    public GameObject abilityDisplay;
    //Instantiate abilityDisplay as child of this
    public GameObject abilityDisplayPanel;

    //Selecting target for attacking and also moving for now.
    public AttackTargetSelector attackTargetSelector;
    //this function displays the information in the characterscreen
    public void viewCharacter(Character currChar) {
        //sets the attributes to the character's
        characterName.text = currChar.name;
        //sets the image of character
        characterPortrait.sprite = currChar.GetComponent<SpriteRenderer>().sprite;
        characterPortrait.color = currChar.GetComponent<SpriteRenderer>().color;
        //Debug.Log("Is this causing bug?:" + GetComponent<TargetNames>().getName(currChar.attackTargetStrategy));
        //sets the text of the targetting
        attackTargetSelector.target.text = getTargetStrategyName(currChar.attackTargetStrategy);
        attackTargetSelector.character = currChar;

        displayStats(currChar);
        displayCharacterAbilities(currChar);
    }

    public void displayCharacterAbilities(Character currChar) {
        foreach (Ability ability in currChar.abilities) {
            GameObject temp = Instantiate(abilityDisplay);
            //sets the instantiated object as child
            temp.transform.parent = abilityDisplayPanel.transform;
            AbilityDisplay displayTemp = temp.GetComponent<AbilityDisplay>();
            //sets the displays name and description
            displayTemp.abilityName.text = ability.abilityName;
            displayTemp.description.text = ability.description;
            //resetting scale to 1 cuz for somereaosn the scale is 167 otherwise
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    //displays the stats and cool stats of the character and character screen
    private void displayStats(Character currChar) {
        //the empty quotes is to convert float to str
        DMG.text = currChar.DMG + "";
        AS.text = currChar.AS + "";
        MS.text = currChar.MS + "";
        RNG.text = currChar.Range + "";
        LS.text = currChar.LS + "";
        totalKills.text = currChar.totalKills + "";
        //fills the HP bar correctly
        healthBar.character = currChar;
    }

    public void close() {
        //destroys all ability displays
        foreach (Transform toDestroy in abilityDisplayPanel.transform) {
            GameObject.Destroy(toDestroy.gameObject);
        }
    }

    private string getTargetStrategyName(int target) {
            switch (target) {
                case (int)Character.targetList.ClosestEnemy:
                    return "Closest Enemy";

                case (int)Character.targetList.ClosestAlly:
                    return "Closest Ally";

                case (int)Character.targetList.DefaultEnemy:
                    return "Default Enemy";
            }
            return "";
    }
}
