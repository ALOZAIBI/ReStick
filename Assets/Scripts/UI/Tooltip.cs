using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI tooltipText;
    public Image self;
    public HideUI hideUI;

    public float durationUp;
    public float durationTarget;
    // Start is called before the first frame update
    void Start()
    {
        hideUI = GetComponent<HideUI>();
        self = GetComponent<Image>();
        tooltipText = GetComponentInChildren<TextMeshProUGUI>();
        //make size of self half the screen size
        self.rectTransform.sizeDelta = new Vector2(Screen.width / 1.4f, Screen.height /5 );
    }

    public void showMessage(string message,float duration) {
        durationUp = 0;
        durationTarget = duration;
        tooltipText.text = message;
    }

    public void showMessage(string message) {
        showMessage(message, 1.5f);
    }

    private void Update() {
        if(durationUp<durationTarget) {
            durationUp += Time.unscaledDeltaTime;
            hideUI.hidden = false;
        }
        else {
            hideUI.hidden = true;
        }
    }
}
