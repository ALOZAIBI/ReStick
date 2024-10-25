using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

//This is used to quickly equip item/ability that we just received from reward
public class CharacterDisplayRewardApplication : CharacterDisplay
{

    //To make the btn accessible from reward manager.
    //The btn's listener will be set there
    public Button getBtn() {
        return btn;
    }

    //When the reward isn't applicable to the character, we disable the button and change the color of the character portrait
    public void disable() {
        btn.interactable = false;
        characterPortrait.SetAlpha(0.3f);
    }
}
