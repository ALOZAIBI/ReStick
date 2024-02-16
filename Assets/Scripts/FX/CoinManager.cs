using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public List<Coin> coins = new List<Coin>();
    public bool zoneEnded = false;

    public void setUp() {
        zoneEnded = false;
        coins.Clear();
    }
    //On zone end, make all coins move towards the gold gain thing
    private void Update() {
        if (zoneEnded) {
            foreach (Coin coin in coins) {
                coin.FlyToInventory();
            }
        }
    }
}
