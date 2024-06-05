using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    }

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
