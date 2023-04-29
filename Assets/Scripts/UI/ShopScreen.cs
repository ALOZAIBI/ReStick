using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopScreen : MonoBehaviour
{
    public GameObject abilityDisplayObj;
    public GameObject characterDisplayObj;
    public GameObject characterPlayerPartyDisplayObj;


    public GameObject abilityArea;
    public GameObject characterArea;
    public GameObject characterPlayerPartyArea;

    public Button backBtn;

    public TextMeshProUGUI goldText;

    //cost of purchase of abilities
    public int commonCost;
    public int rareCost;
    public int epicCost;
    public int legendaryCost;

    public Shop shop;

    public HospitalTrainingScreen hospitalTrainingScreen;

    //0 is landing apge
    //1 is Hospital/Training Area
    public int pageIndex;

    //so that AbilityDisplayShop can deselect everything else when it is selected
    public List<AbilityDisplayShop> listAbilities = new List<AbilityDisplayShop>();
    //Haven't done yet but should do same as listAbilities
    public List<CharacterDisplayShop> listCharacters = new List<CharacterDisplayShop>();

    private void Start() {
        backBtn.onClick.AddListener(back);
    }

    //goes from hospital screen to regular shop screen
    private void back() {
        if(pageIndex == 1) {
            openLandingPage();
        }
    }

    public void setupShopScreen() {
        close();
        openLandingPage();
        displayAbilities();
        displayCharacters();
        displayPlayerParty();
    }


    //returns cost of ability This is based on rarity
    private int costOfAbility(Ability ability) {
        switch (ability.rarity) {
            case (int)Ability.raritiesList.Common:
                return commonCost;
            case (int)Ability.raritiesList.Rare:
                return rareCost;
            case (int)Ability.raritiesList.Epic:
                return epicCost;
            case (int)Ability.raritiesList.Legendary:
                return legendaryCost;
            default:
                throw new Exception("RarityUnkown");
        }
    }
    //closes the abilityDisplays
    private void closeAbilities() {
        foreach(Transform child in abilityArea.transform) {
                Destroy(child.gameObject);
        }
    }
    public void closeCharacters() {
        foreach (Transform child in characterArea.transform) {
            Destroy(child.gameObject);
        }
    }

    public void closeCharactersPlayerParty() {
        foreach (Transform child in characterPlayerPartyArea.transform) {
            Destroy(child.gameObject);
        }
    }
    public void close() {
        closeAbilities();
        closeCharacters();
        closeCharactersPlayerParty();
    }
    public void displayAbilities() {
        //creates ability Displays
        for (int i = 0; i < shop.abilityHolder.transform.childCount; i++) {
            //creates the display and makes it a child of abilityArea
            AbilityDisplayShop abilityDisplay = Instantiate(abilityDisplayObj,abilityArea.transform).GetComponent<AbilityDisplayShop>();
            listAbilities.Add(abilityDisplay);
            //gets the ability from shop
            Ability temp = shop.abilityHolder.transform.GetChild(i).GetComponent<Ability>();
            abilityDisplay.price = costOfAbility(temp);
            abilityDisplay.ability = temp;
            abilityDisplay.abilityName.text = temp.abilityName;
            abilityDisplay.description.text = temp.description;
            //marks if it was purchased
            abilityDisplay.purchased = shop.abilitiyPurchased[i];
            //sets the scale for some reason if I dont do this the scale is set to 167
            abilityDisplay.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void displayCharacters() {
        //creates character Displays
        for(int i = 0;i<shop.characterHolder.transform.childCount;i++) {
            Debug.Log("Character holder cjild amount" + shop.characterHolder.transform.childCount);
            CharacterDisplayShop characterDisplay = Instantiate(characterDisplayObj,characterArea.transform).GetComponent<CharacterDisplayShop>() ;
            listCharacters.Add(characterDisplay);
            Character temp = shop.characterHolder.transform.GetChild(i).GetComponent<Character>();
            characterDisplay.character = temp;
            //then marks if it was purchased
            characterDisplay.purchased = shop.characterPurchased[i];
        }
    }

    public void displayPlayerParty() {
        //deletes all created instances before recreating to account for dead characters etc..
        closeCharactersPlayerParty();
        //loops through children of playerParty
        foreach (Transform child in UIManager.singleton.playerParty.transform) {
            //Debug.Log(child.name);
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();

                //instantiates a charcaterDisplay
                CharacterDisplayShopHospitalTraining display = Instantiate(characterPlayerPartyDisplayObj).GetComponent<CharacterDisplayShopHospitalTraining>();
                display.character = temp;
                //sets this display as a child 
                display.transform.parent = characterPlayerPartyArea.transform;
                //sets the scale for some reason if I dont do this the scale is set to 167
                display.gameObject.transform.localScale = new Vector3(1, 1, 1);

            }
        }
    }

    public void openLandingPage() {
        abilityArea.SetActive(true);
        characterArea.SetActive(true);
        characterPlayerPartyArea.SetActive(true);
        backBtn.gameObject.SetActive(false);
        UIManager.singleton.closeUIBtn.gameObject.SetActive(true);
        hospitalTrainingScreen.gameObject.SetActive(false);
        pageIndex = 0;
    }

    public void openHospitalTrainingPage() {
        abilityArea.SetActive(false);
        characterArea.SetActive(false);
        characterPlayerPartyArea.SetActive(true);
        backBtn.gameObject.SetActive(true);
        UIManager.singleton.closeUIBtn.gameObject.SetActive(false);
        hospitalTrainingScreen.gameObject.SetActive(true);
        pageIndex = 1;
    }
    private void Update() {
        goldText.text = "G:"+UIManager.singleton.playerParty.gold;
    }
}
