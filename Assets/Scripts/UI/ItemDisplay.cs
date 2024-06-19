using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public Image icon;
    public Item item;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;

    public Image wholeThingImage;
    public Image rarityGem;
    //When adding this button is used to add item, otherwise this item displays the remove btn
    public Button wholeThingBtn;
    public Button removeBtn;

    public CharacterInfoScreen characterInfoScreen;

    //If adding is true, clicking the display will add the item to the character
    public bool adding = false;

    //Reward stuff
    public bool reward = false;
    public RewardSelectItem rewardSelect;
    public bool selected = false;

    //If this is true the display will be clickable to buy the item
    public bool shop = false;
    public bool purchased = false;
    //Used to know which item this is to save if it's purchased etc..
    public int shopIndex;
    public GameObject priceObj;
    public TextMeshProUGUI priceTxt;
    public GameObject showThatNextClickWillPurchase;
    //The image to display when thign is sold
    public GameObject soldImage;

    [SerializeField] private GameObject PWR;
    [SerializeField] private TextMeshProUGUI PWRText;
    [SerializeField] private GameObject MGC;
    [SerializeField] private TextMeshProUGUI MGCText;
    [SerializeField] private GameObject INF;
    [SerializeField] private TextMeshProUGUI INFText;
    [SerializeField] private GameObject HP;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private GameObject AS;
    [SerializeField] private TextMeshProUGUI ASText;
    [SerializeField] private GameObject CDR;
    [SerializeField] private TextMeshProUGUI CDRText;
    [SerializeField] private GameObject MS;
    [SerializeField] private TextMeshProUGUI MSText;
    [SerializeField] private GameObject Range;
    [SerializeField] private TextMeshProUGUI RangeText;
    [SerializeField] private GameObject LS;
    [SerializeField] private TextMeshProUGUI LSText;

    void Start() {
        itemName.text = item.itemName;
        itemDescription.text = item.description;
        wholeThingImage = wholeThingBtn.GetComponent<Image>();

        if (reward) {
            wholeThingBtn.onClick.AddListener(selectReward);
        }
        else if(shop) {
            priceObj.SetActive(true);
            priceTxt.text = getPrice().ToString();
            wholeThingBtn.onClick.AddListener(clickShop);
        }
        else
            wholeThingBtn.onClick.AddListener(click);

        removeBtn.onClick.AddListener(removeItem);
        if (item.PD != 0 ){
            PWR.SetActive(true);
            PWRText.text = item.PD.ToString();
        }
        if (item.MD != 0) {
            MGC.SetActive(true);
            MGCText.text = item.MD.ToString();
        }
        if (item.INF != 0) {
            INF.SetActive(true);
            INFText.text = item.INF.ToString();
        }
        if (item.HP != 0) {
            HP.SetActive(true);
            HPText.text = item.HP.ToString();
        }
        if (item.AS != 0) {
            AS.SetActive(true);
            ASText.text = item.AS.ToString();
        }
        if (item.CDR != 0) {
            CDR.SetActive(true);
            CDRText.text = item.CDR.ToString();
        }
        if (item.MS != 0) {
            MS.SetActive(true);
            MSText.text = item.MS.ToString();
        }
        if (item.Range != 0) {
            Range.SetActive(true);
            RangeText.text = item.Range.ToString();
        }
        if (item.LS != 0) {
            LS.SetActive(true);
            LSText.text = item.LS.ToString();
        }

        //Set rarity gem color
        switch (item.rarity) {
            case (int)Item.RaritiesList.Common:
                rarityGem.color = ColorPalette.singleton.commonRarity;
                break;

            case (int)Item.RaritiesList.Rare:
                rarityGem.color = ColorPalette.singleton.rareRarity;
                break;

            case (int)Item.RaritiesList.Epic:
                rarityGem.color = ColorPalette.singleton.epicRarity;
                break;

            case (int)Item.RaritiesList.Legendary:
                rarityGem.color = ColorPalette.singleton.legendaryRarity;
                break;
        }


    }

    #region shop
    private int getPrice() {
        switch (item.rarity) {
            case (int)Item.RaritiesList.Common:
                return UIManager.singleton.shopScreen.commonCost;
            case (int)Item.RaritiesList.Rare:
                return UIManager.singleton.shopScreen.rareCost;
            case (int)Item.RaritiesList.Epic:
                return UIManager.singleton.shopScreen.epicCost;
            case (int)Item.RaritiesList.Legendary:
                return UIManager.singleton.shopScreen.legendaryCost;
            default:
                return 0;
        }
    }
    private void clickShop() {
        //If this is clicked show that next click will buy and remove that from all others
        if (!selected) {
            showThatNextClickWillPurchase.SetActive(true);
            selected = true;
            //Remove from all others
            foreach (ItemDisplay itemDisplay in UIManager.singleton.shopScreen.listItems) {
                if (itemDisplay != this) {
                    itemDisplay.selected = false;
                    itemDisplay.showThatNextClickWillPurchase.SetActive(false);
                }
            }
            //deselects alll others
            foreach (AbilityDisplayShop deSelect in UIManager.singleton.shopScreen.listAbilities) {
                if (deSelect != this) {
                    deSelect.selected = false;
                    deSelect.unHighlight();
                }
            }
            foreach (CharacterDisplayShop deSelect in UIManager.singleton.shopScreen.listCharacters) {
                if (deSelect != this) {
                    deSelect.selected = false;
                    deSelect.unHighlight();
                }
            }
        }
        //If it was already selected clicking it will purchase
        else if(!purchased){
            //If can afford
            if(UIManager.singleton.playerParty.gold>= getPrice()) {
                markPurchased();
                //Deduct the gold
                UIManager.singleton.playerParty.gold -= getPrice();
                //Add to inventory
                UIManager.singleton.itemFactory.addRequestedItemToInventory(item.itemName);
                //We save in map since shop is only in map
                UIManager.singleton.saveMapSave();
                //Save shop purchaseInfo
                SaveSystem.saveShopAbilitiesItemsAndPurchaseInfo(UIManager.singleton.shopScreen.shop);
            }
        }
    }

    private void markPurchased() {
        purchased = true;
        UIManager.singleton.shopScreen.shop.itemPurchased[shopIndex]=true;
        displaySold();
    }
    public void displaySold() {
        wholeThingImage.SetAlpha(0.1f);
        soldImage.SetActive(true);
        //Rotate the sold image randomly along the z axis
        soldImage.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-40,40));
    }
    #endregion shop


    //If we're in adding mode, add this item to the character
    private void click() {
        if (adding) {
            //Adds the item
            characterInfoScreen.character.items.Add(item);
            //Sets parent of item to playerParty's active items
            item.transform.SetParent(UIManager.singleton.playerParty.activeItems.transform);

            //Applies the stats
            item.applyStats(characterInfoScreen.character);

            characterInfoScreen.startUnfocusing();

            //saves adding the item
            if (SceneManager.GetActiveScene().name == "World") {
                UIManager.singleton.saveWorldSave();
            }
            else
                UIManager.singleton.saveMapSave();
        }
        else {
            removeBtn.gameObject.SetActive(true);
        }
    }

    private void removeItem() {
        //If zone hasn't started
        if (!UIManager.singleton.zoneStarted()) {
            //Remove item from the list and place it in the item inventory
            item.transform.parent = UIManager.singleton.playerParty.itemInventory.transform;
            characterInfoScreen.character.items.Remove(item);
            item.removeStats();

            characterInfoScreen.displayCharacterItems(characterInfoScreen.character);
            if (SceneManager.GetActiveScene().name == "World") {
                UIManager.singleton.saveWorldSave();
            }
            else
                UIManager.singleton.saveMapSave();
        }
    }


    //Reward stuff
    public void highlight() {
        wholeThingImage.SetAlpha(1);
    }
    public void unhighlight() {
        wholeThingImage.SetAlpha(0.3f);
    }
    private void selectReward() {
        selected = true;
        highlight();
        //Deselect all others
        foreach (ItemDisplay itemDisplay in rewardSelect.listItemReward) {
            if (itemDisplay != this) {
                itemDisplay.selected = false;
                itemDisplay.unhighlight();
            }
        }
        rewardSelect.unGreyOutItemBtn();
    }
}
