using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TimeControl : MonoBehaviour
{

    public float currTimeScale=1;
    public Button scaleUpBtn;
    public Button scaleDownBtn;

    public TextMeshProUGUI displayCurrScale;

    public float maxScale;
    public float minScale;
    private void Start() {
        scaleUpBtn.onClick.AddListener(scaleUp);
        scaleDownBtn.onClick.AddListener(scaleDown);
        displayCurrScale.text = currTimeScale + "x";
    }

    private void scaleUp() {
        //can change scale if game not pauysed
        if (Time.timeScale !=0) {
            if (currTimeScale < maxScale) {
                if (currTimeScale == 0.25f)
                    currTimeScale += 0.25f;
                else
                    currTimeScale += 0.5f;
            }
            displayCurrScale.text = currTimeScale + "x";
            Time.timeScale = currTimeScale;
        }
    }

    private void scaleDown() {
        //can change scale if game not pauysed
        if (Time.timeScale != 0) {
            if (currTimeScale > minScale) {
                if (currTimeScale > 0.25f)
                    currTimeScale -= 0.5f;
                else
                    currTimeScale -= 0.25f;
            }
            displayCurrScale.text = currTimeScale + "x";
            Time.timeScale = currTimeScale;
        }
    }

}
