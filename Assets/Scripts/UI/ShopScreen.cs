using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopScreen : MonoBehaviour {

    public GameObject itemDisplayObj;

    public GameObject abilityDisplayObj;
    public GameObject sellAbilityDisplayObj;

    public GameObject characterDisplayObj;
    public GameObject characterPlayerPartyDisplayObj;

    public GameObject itemArea;
    public GameObject abilityArea;
    public GameObject characterArea;
    public GameObject characterPlayerPartyArea;

    public GameObject sellAbilityArea;


    public Button closeBtn;

    public Button buyLifeShardBtn;

    public GameObject buyScreen;
    public GameObject sellScreen;

    public Button goToBuyScreenBtn;
    public Button goToSellScreenBtn;

    public List<Image> buyScreenBtnImages = new List<Image>();
    public List<Image> sellScreenBtnImages = new List<Image>();

    //cost of purchase of abilities
    public int commonCost;
    public int rareCost;
    public int epicCost;
    public int legendaryCost;
    public int lifeShardCost;

    public Shop shop;

    public HospitalScreen hospitalTrainingScreen;

    //0 is landing apge
    //1 is Hospital/Training Area
    public int pageIndex;

    public List<ItemDisplay> listItems = new List<ItemDisplay>();
    //so that AbilityDisplayShop can deselect everything else when it is selected
    public List<AbilityDisplayShop> listAbilities = new List<AbilityDisplayShop>();
    //Haven't done yet but should do same as listAbilities
    public List<CharacterDisplayShop> listCharacters = new List<CharacterDisplayShop>();

    //so that AbilityDisplaySell can deselect everything else when it is selected
    public List<AbilityDisplaySell> listSellableAbilities = new List<AbilityDisplaySell>();


    private void Start() {
        closeBtn.onClick.AddListener(closeScreen);
        buyLifeShardBtn.onClick.AddListener(buyLifeShard);

        goToBuyScreenBtn.onClick.AddListener(displayBuyScreen);
        goToSellScreenBtn.onClick.AddListener(displaySellScreen);
    }


    public void buyLifeShard() {
        //If the player has enough money to buy shards and has less than maximum shards then buy it
        if (UIManager.singleton.playerParty.gold >= lifeShardCost && UIManager.singleton.playerParty.lifeShards < UIManager.singleton.playerParty.maxLifeShards) {
            UIManager.singleton.playerParty.gold -= lifeShardCost;
            UIManager.singleton.playerParty.lifeShards++;
            UIManager.singleton.saveMapSave();
        }
    }
    public void setupShopScreen() {
        displayBuyScreen();
        //buyLifeShardBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Buy a Life Shard " + lifeShardCost;
    }


    //returns cost of ability This is based on rarity
    private int costOfAbility(Ability ability) {
        switch (ability.rarity) {
            case (int)Ability.RaritiesList.Common:
                return commonCost;
            case (int)Ability.RaritiesList.Rare:
                return rareCost;
            case (int)Ability.RaritiesList.Epic:
                return epicCost;
            case (int)Ability.RaritiesList.Legendary:
                return legendaryCost;
            default:
                throw new Exception("RarityUnkown");
        }
    }
    //closes the abilityDisplays
    private void closeAbilities() {
        foreach (Transform child in abilityArea.transform) {
            if (child.tag != "DontDelete")
                Destroy(child.gameObject);
        }
    }

    private void closeItems() {
        foreach(Transform child in itemArea.transform) {
            if (child.tag != "DontDelete")
                Destroy(child.gameObject);
        }
    }
    
    public void closeCharacters() {
        foreach (Transform child in characterArea.transform) {
            if (child.tag != "DontDelete")
                Destroy(child.gameObject);
        }
    }

    //public void closeCharactersPlayerParty() {
    //    foreach (Transform child in characterPlayerPartyArea.transform) {
    //        if (child.tag != "DontDelete")
    //            Destroy(child.gameObject);
    //    }
    //}
    public void closeBuyScreen() {
        closeAbilities();
        closeCharacters();
        closeItems();
        //closeCharactersPlayerParty();
        listAbilities.Clear();
        listCharacters.Clear();

        shop.clean();
    }
    private void closeSellAbilities() {
        foreach (Transform child in sellAbilityArea.transform) {
            if (child.tag != "DontDelete")
                Destroy(child.gameObject);
        }
    }
    public void closeSellScreen() {
        closeSellAbilities();
        listSellableAbilities.Clear();
    }

    public void closeScreen() {
        closeBuyScreen();
        closeSellScreen();
        UIManager.singleton.shopScreenHidden.hidden = true;
    }

    public void displayItems() {
        //Creates itemDisplays
        for(int i = 0; i < shop.itemHolder.transform.childCount; i++) {
            //Creates the display and makes it a child of itemArea
            ItemDisplay itemDisplay = Instantiate(itemDisplayObj, itemArea.transform).GetComponent<ItemDisplay>();
            itemDisplay.shop = true;
            itemDisplay.shopIndex = i;

            Item temp = shop.itemHolder.transform.GetChild(i).GetComponent<Item>();
            itemDisplay.item = temp;
            //Marks if it was purchased
            itemDisplay.purchased = shop.itemPurchased[i];
            //Display if it was sold
            if (itemDisplay.purchased) {
                itemDisplay.displaySold();
            }
            listItems.Add(itemDisplay);
        }
    }
    public void displayAbilities() {
        //creates ability Displays
        for (int i = 0; i < shop.abilityHolder.transform.childCount; i++) {
            //creates the display and makes it a child of abilityArea
            AbilityDisplayShop abilityDisplay = Instantiate(abilityDisplayObj, abilityArea.transform).GetComponent<AbilityDisplayShop>();
            listAbilities.Add(abilityDisplay);
            abilityDisplay.index = i;
            //gets the ability from shop
            Ability temp = shop.abilityHolder.transform.GetChild(i).GetComponent<Ability>();
            abilityDisplay.price = costOfAbility(temp);
            abilityDisplay.ability = temp;
            abilityDisplay.abilityName.text = temp.abilityName;
            abilityDisplay.description.text = temp.description;
            //marks if it was purchased
            abilityDisplay.purchased = shop.abilitiyPurchased[i];
            //change alpha to 0.1 if purchased
            if (abilityDisplay.purchased) {
                abilityDisplay.displaySold();
            }
            //sets the scale for some reason if I dont do this the scale is set to 167
            abilityDisplay.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void displayCharacters() {
        //creates character Displays
        for (int i = 0; i < shop.characterHolder.transform.childCount; i++) {
            Debug.Log("Character holder cjild amount" + shop.characterHolder.transform.childCount);
            CharacterDisplayShop characterDisplay = Instantiate(characterDisplayObj, characterArea.transform).GetComponent<CharacterDisplayShop>();
            listCharacters.Add(characterDisplay);
            characterDisplay.index = i;
            Character temp = shop.characterHolder.transform.GetChild(i).GetComponent<Character>();
            characterDisplay.character = temp;
            //then marks if it was purchased
            characterDisplay.purchased = shop.characterPurchased[i];
        }
    }

    //Display buy screen
    private void displayBuyScreen() {
        closeBuyScreen();
        displayAbilities();
        displayItems();
        displayCharacters();

        buyScreen.SetActive(true);
        sellScreen.SetActive(false);
        //grey out the sellScreenBtn images
        foreach (Image image in sellScreenBtnImages) {
            image.SetAlpha(0.5f);
        }

        //Ungrey the buyScreenBtn images
        foreach (Image image in buyScreenBtnImages) {
            image.SetAlpha(1);
        }

    }
    public void displaySellScreen() {
        closeSellScreen();
        buyScreen.SetActive(false);
        sellScreen.SetActive(true);
        //grey out the buyScreenBtn images
        foreach (Image image in buyScreenBtnImages) {
            image.SetAlpha(0.5f);
        }

        //Ungrey the sellScreenBtn images
        foreach (Image image in sellScreenBtnImages) {
            image.SetAlpha(1);
        }

        displaySellableAbilities();
    }
    private void displaySellableAbilities() {
        //creates ability Displays
        for (int i = 0; i < UIManager.singleton.playerParty.abilityInventory.transform.childCount; i++) {
            //creates the display and makes it a child of abilityArea
            AbilityDisplaySell abilityDisplay = Instantiate(sellAbilityDisplayObj, sellAbilityArea.transform).GetComponent<AbilityDisplaySell>();
            listSellableAbilities.Add(abilityDisplay);
            abilityDisplay.index = i;
            //gets the ability from inventory
            Ability temp = UIManager.singleton.playerParty.abilityInventory.transform.GetChild(i).GetComponent<Ability>();
            //Sell price is half of buy price
            abilityDisplay.price = costOfAbility(temp)/2;
            abilityDisplay.ability = temp;
            abilityDisplay.abilityName.text = temp.abilityName;
            abilityDisplay.description.text = temp.description;
            //sets the scale for some reason if I dont do this the scale is set to 167
            abilityDisplay.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }

    }
    //public void displayPlayerParty() {
    //    //deletes all created instances before recreating to account for dead characters etc..
    //    closeCharactersPlayerParty();
    //    //loops through children of playerParty
    //    foreach (Transform child in UIManager.singleton.playerParty.transform) {
    //        //Debug.Log(child.name);
    //        if (child.tag == "Character") {
    //            Character temp = child.GetComponent<Character>();

    //            //instantiates a charcaterDisplay
    //            CharacterDisplayShopHospitalTraining display = Instantiate(characterPlayerPartyDisplayObj).GetComponent<CharacterDisplayShopHospitalTraining>();
    //            display.character = temp;
    //            //sets this display as a child 
    //            display.transform.parent = characterPlayerPartyArea.transform;
    //            //sets the scale for some reason if I dont do this the scale is set to 167
    //            display.gameObject.transform.localScale = new Vector3(1, 1, 1);

    //        }
    //    }
    //}

    //public void openLandingPage() {
    //    abilityArea.SetActive(true);
    //    characterArea.SetActive(true);
    //    characterPlayerPartyArea.SetActive(true);
    //    backBtn.gameObject.SetActive(false);
    //    //UIManager.singleton.closeUIBtn.gameObject.SetActive(true);
    //    hospitalTrainingScreen.gameObject.SetActive(false);
    //    pageIndex = 0;
    //}

    //public void openHospitalTrainingPage() {
    //    abilityArea.SetActive(false);
    //    characterArea.SetActive(false);
    //    characterPlayerPartyArea.SetActive(true);
    //    backBtn.gameObject.SetActive(true);
    //    //UIManager.singleton.closeUIBtn.gameObject.SetActive(false);
    //    hospitalTrainingScreen.gameObject.SetActive(true);
    //    hospitalTrainingScreen.updateButtons();
    //    pageIndex = 1;
    //}
    //private void Update() {
    //    goldText.text = "G:"+UIManager.singleton.playerParty.gold;
    //}


}
