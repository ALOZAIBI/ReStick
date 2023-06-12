using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryAbilityDisplay : AbilityDisplay
{
    public Button button;
    public InventoryScreen inventoryScreen;

    //to make the button glow
    public bool glow;
    public ColorBlock defaultColor;
    private void Start() {
        base.Start();
        inventoryScreen = GameObject.FindGameObjectWithTag("InventoryScreen").GetComponent<InventoryScreen>();
        button.onClick.AddListener(selectAbility);
        defaultColor = button.colors;
    }

    private void selectAbility() {
        //if character had already been selected I.E character is selected first
        //when clicked set the inventorySCreen's ability selected to this. and prompt to confirm
        if(inventoryScreen.pageIndex == 3) {
            inventoryScreen.abilitySelected = ability;
            //prompt to confirm
            inventoryScreen.inventoryCharacterScreen.confirmAddAbilityPage();
        }
        //if character hadn't been selected. I.E ability is selected first(still in landingPage)
        else if(inventoryScreen.pageIndex == 0) {
            //deletes other displays
            inventoryScreen.closeAbilityHeader();
            //moves the display to ability header
            transform.parent = inventoryScreen.AbilityHeader.transform;
            ////ability header has to manuallly be set to active so that I can move the transform
            //inventoryScreen.AbilityHeader.SetActive(true);
            //selects the ability
            inventoryScreen.abilitySelected = ability;
            inventoryScreen.openAbilityPickedPage();
        }
            
    }
    private void showScaling() {
        //sorts them in descending order
        for (int i = 0; i < iconHolder.childCount - 1; i++) {
            //assume first is max
            Debug.Log(i);
            //StatIcon max = transform.GetChild(i).GetComponent<StatIcon>();
            for (int j = i + 1; j < iconHolder.childCount; j++) {
                //StatIcon curr = transform.GetChild(j).GetComponent<StatIcon>();
                ////if (curr.ratio > max.ratio) {
                ////    max = curr;
                ////}
            }
            //Debug.Log(max);
            //max.transform.SetSiblingIndex(i);
        }

    }
    private void Update() {
        if (glow) {
            float x =  Mathf.PingPong(Time.unscaledTime * 0.3f, 0.2f);
            new Color(x, x, x);
            ColorBlock cb = button.colors;
            cb.normalColor = defaultColor.normalColor + new Color(x, x, x);
            button.colors = cb;
        }
        else {
            button.colors = defaultColor;
        }
    }
}
