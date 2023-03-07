using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TopStatDisplay : MonoBehaviour
{
    //stats texts
    public UIManager uiManager;

    

    public TextMeshProUGUI DMG, AS, MS, RNG, LS;

    public Character character;

    public Button moreInfoBtn;
    //the character will be assigned when the character is clicked
    //on zone's start hide me
    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        moreInfoBtn.onClick.AddListener(displayMoreInfo);
    }

    //opens charInfoScreen of character
    public void displayMoreInfo() {
        uiManager.viewCharacterInfo(character);
    }
    private void displayStats() {
        DMG.text = "DMG "+character.DMG + "";
        AS.text = "AS "+character.AS + "";
        MS.text = "MS "+character.MS + "";
        RNG.text = "RNG "+character.Range + "";
        LS.text = "LS "+character.LS + "";
    }
    // Update is called once per frame
    void Update()
    {
        displayStats();
    }
}
