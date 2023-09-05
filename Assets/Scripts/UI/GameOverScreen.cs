using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI waitBuyLifeShardText;
    [SerializeField] private Button buyLifeShardBtn;
    [SerializeField] private Button watchAdBtn;
    [SerializeField] private Button giveUpBtn;

    private void Start() {
        giveUpBtn.onClick.AddListener(giveUP);
        buyLifeShardBtn.onClick.AddListener(buyLifeShard);
        watchAdBtn.onClick.AddListener(watchADAddLifeShard);
    }
    //For now this just deletes the save. Later it will take you to the main menu
    private void giveUP() {
        SaveSystem.deleteSave(UIManager.singleton.saveSlot);
    }
    public void setup() { 
        //If the player has enough money then display the buy button and the relevant text
        if (UIManager.singleton.playerParty.gold >= UIManager.singleton.shopScreen.lifeShardCost+50) {
            waitBuyLifeShardText.gameObject.SetActive(true);
            buyLifeShardBtn.interactable = true;
            buyLifeShardBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Buy an Emergency Life Shard " + UIManager.singleton.shopScreen.lifeShardCost+50;
        }
        else {
            waitBuyLifeShardText.gameObject.SetActive(false);
            buyLifeShardBtn.interactable = false;
            buyLifeShardBtn.GetComponentInChildren<TextMeshProUGUI>().text = "No Moneis for Life Shard ";
        }
    }

    public void buyLifeShard() {
        //If the player has enough money to buy shards and has less than maximum shards then buy it
        if (UIManager.singleton.playerParty.gold >= UIManager.singleton.shopScreen.lifeShardCost+50 && UIManager.singleton.playerParty.lifeShards < UIManager.singleton.playerParty.maxLifeShards) {
            UIManager.singleton.playerParty.gold -= UIManager.singleton.shopScreen.lifeShardCost+50;
            UIManager.singleton.playerParty.lifeShards++;
            SaveSystem.updateLifeShardsInMap();
            SaveSystem.reduceGoldInMap(UIManager.singleton.shopScreen.lifeShardCost);
            //Reloads the level and hides the game over screen
            UIManager.singleton.restartZone(false);
            UIManager.singleton.gameOverScreenHidden.hidden = true;
        }
    }

    //Adds 2 life shards
    private void watchADAddLifeShard() {
        UIManager.singleton.playerParty.lifeShards += 2;
        SaveSystem.updateLifeShardsInMap();
        //Hides the game over screen
        UIManager.singleton.gameOverScreenHidden.hidden = true;
        //Reloads the level
        UIManager.singleton.restartZone(false);
    }
}
