using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public Image icon;
    public Item item;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;

    public Button wholeThingBtn;
    public Button removeBtn;

    //If adding is true, clicking the display will add the item to the character
    public bool adding = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
