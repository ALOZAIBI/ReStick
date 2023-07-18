using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AbilityDisplayAddAbility : AbilityDisplay
{
    private void Start() {
        base.Start();
        btn.onClick.AddListener(addToCharacter);
    }

    private void addToCharacter() {
        //Adds the clicked ability to the selected character(selected in charInfoScreen)
        uiManager.characterInfoScreen.character.abilities.Add(ability);
        //Sets parent of ability to playerParty's active abilities
        ability.transform.parent = uiManager.playerParty.activeAbilities.transform;
        //Starts unfocusing
        uiManager.characterInfoScreen.startUnfocusing();

        //saves adding the ability
        if (SceneManager.GetActiveScene().name == "World") {
            uiManager.saveWorldSave();
        }
        else
            uiManager.saveMapSave();

        //Moves the addAbilityPanel back to CharacterInfoScreen
        uiManager.characterInfoScreen.addAbilityPanel.transform.parent = uiManager.characterInfoScreen.transform;
        //Sets the addAbilityPanel to inactive
        uiManager.characterInfoScreen.addAbilityPanel.gameObject.SetActive(false);
    }
}
