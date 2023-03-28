using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotSelector : MonoBehaviour
{
    public Button startSlot1Btn;
    public Button startSlot2Btn;
    public Button startSlot3Btn;

    // Start is called before the first frame update
    void Start()
    {
        startSlot1Btn.onClick.AddListener(startSlot1);
        startSlot2Btn.onClick.AddListener(startSlot2);
        startSlot3Btn.onClick.AddListener(startSlot3);

    }

    public void startSlot1() {
        UIManager.saveSlot = "slot1";
        SaveSystem.loadGameState();
    }

    public void startSlot2() {
        UIManager.saveSlot = "slot2";
        SaveSystem.loadGameState();
    }

    public void startSlot3() {
        UIManager.saveSlot = "slot3";
        SaveSystem.loadGameState();
    }

}
