using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoScreen : MonoBehaviour
{
    public UIManager uiManager;
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
    public AttackTargetSelector targetSelector;

    //character that is currently being viewed
    public Character character;

    public Button openTargetSelectionBtn;


    public void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        openTargetSelectionBtn.onClick.AddListener(openTargetSelectorNormal);
    }
    //this function displays the information in the characterInfoScreen
    public void viewCharacter(Character currChar) {
        //sets the attributes to the character's
        characterName.text = currChar.name;
        //sets the image of character
        characterPortrait.sprite = currChar.GetComponent<SpriteRenderer>().sprite;
        characterPortrait.color = currChar.GetComponent<SpriteRenderer>().color;
        //Debug.Log("Is this causing bug?:" + GetComponent<TargetNames>().getName(currChar.attackTargetStrategy));
        //sets the text of the targetting
        targetSelector.target.text = TargetNames.getName(currChar.attackTargetStrategy);
        targetSelector.character = currChar;

        displayStats(currChar);
        displayCharacterAbilities(currChar);

        character = currChar;
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
            displayTemp.ability = ability;
            displayTemp.targettingStrategyText.text = TargetNames.getName((ability.targetStrategy));
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

    //opens target selector for normal attacks
    public void openTargetSelectorNormal() {
        Debug.Log("OPen target selector");
        if (uiManager.zone == null || uiManager.zone.started == false && character.team == (int)Character.teamList.Player) {
            //to indicate that it isnt for an ability
            targetSelector.isAbilityTargetSelector = false;
            targetSelector.targetSelection.SetActive(true);
        }
    }

    //this function is called in abilityDisplay
    public void openTargetSelectorAbility() {                   //change back to false. only true for testingf purposes
        if (uiManager.zone == null || uiManager.zone.started == false && character.team == (int)Character.teamList.Player) {
            //to indicate that it is for an ability
            targetSelector.isAbilityTargetSelector = true;
            targetSelector.targetSelection.SetActive(true);
        }
    }


}
