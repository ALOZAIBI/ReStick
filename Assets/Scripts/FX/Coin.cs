using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float speed = 10f;
    //This will determine the size of the coin (It's scale will be multiplied by this value)
    public int valueInGold;

    [SerializeField]private SpriteRenderer spriteRenderer;
    void Start()
    {
        //Give random speed between 10 and 25
        speed = Random.Range(10, 25);
        //Add this to the list of coins in the CoinManager
        UIManager.singleton.coinManager.coins.Add(this);

        //Multiply the scale of the coin by the valueInGold
        transform.localScale *= valueInGold*0.05f;
    }

    public bool flyToInventory = false;

    public void FlyToInventory() {

        spriteRenderer.sortingLayerName = "Top Most UI";
        spriteRenderer.sortingOrder = 10;
        spriteRenderer.SetAlpha(1);

        transform.position = Vector2.MoveTowards(transform.position, UIManager.singleton.goldtext.transform.position, speed * Time.unscaledDeltaTime);
        //Remove the coin from the scene once it has reached the goldtext
        if (Vector2.Distance(transform.position, UIManager.singleton.goldtext.transform.position) < 0.05f) {
            UIManager.singleton.coinManager.coins.Remove(this);
            //To simulate the coin hitting
            UIManager.singleton.goldtext.moveDisplayUp();
            //Increment the gold to display
            UIManager.singleton.goldtext.goldToDisplay += valueInGold;
            UIManager.singleton.goldtext.updateView();
            gameObject.SetActive(false);
        }
    }
            
        
    
}
