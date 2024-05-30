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

    public Button wholeThingBtn;
    public Button removeBtn;

    public CharacterInfoScreen characterInfoScreen;

    //If adding is true, clicking the display will add the item to the character
    public bool adding = false;
    // Start is called before the first frame update
    void Start()
    {
        itemName.text = item.itemName;
        itemDescription.text = item.description;
        wholeThingBtn.onClick.AddListener(click);
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
