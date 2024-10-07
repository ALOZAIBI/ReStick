using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

//This is used to quickly equip item/ability that we just received from reward
public class CharacterDisplayRewardApplication : MonoBehaviour
{
    public Character character;

    [SerializeField] public Image characerPortrait;
    [SerializeField] private CharacterHealthBar healthBar;

    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI level;

    [SerializeField] private Image xpBar;

    // Start is called before the first frame update
    void Start()
    {
        //sets the image
        characerPortrait.sprite = character.GetComponent<SpriteRenderer>().sprite;
        characerPortrait.color = character.GetComponent<SpriteRenderer>().color;
        //sets the HPbar
        healthBar.character = character;
        //sets the name
        name.text = character.name;

        level.text = character.level.ToString();

        xpBar.fillAmount = character.xpProgress / character.xpCap;
    }
    //The onclick listener will be added by the rewardManager


}
