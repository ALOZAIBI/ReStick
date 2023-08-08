using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class AbilityDisplayAddAbility : AbilityDisplay
{
    public CharacterInfoScreen characterInfoScreen;
    private void Start() {
        base.Start();
        btn.onClick.AddListener(addToCharacter);
    }

    private void addToCharacter() {
        //Adds the clicked ability to the selected character(selected in charInfoScreen)
        characterInfoScreen.character.abilities.Add(ability);
        //Sets parent of ability to playerParty's active abilities
        ability.transform.parent = uiManager.playerParty.activeAbilities.transform;
        //Starts unfocusing
        characterInfoScreen.startUnfocusing();

        if(!uiManager.tutorial.addingAbilityTutorialDone && uiManager.tutorial.addingAbilityTutorialStep == 5) {
            uiManager.tutorial.continueAddingAbilityCloseCharacterInfoScreen();
            uiManager.characterInfoScreen.displayCharacterAbilities(uiManager.characterInfoScreen.character);
        }

        //saves adding the ability
        if (SceneManager.GetActiveScene().name == "World") {
            uiManager.saveWorldSave();
        }
        else
            uiManager.saveMapSave();

        //Moves the addAbilityPanel back to CharacterInfoScreen
        characterInfoScreen.addAbilityPanel.transform.parent = characterInfoScreen.transform;
        //Sets the addAbilityPanel to inactive
        characterInfoScreen.addAbilityPanel.gameObject.SetActive(false);
    }
}
