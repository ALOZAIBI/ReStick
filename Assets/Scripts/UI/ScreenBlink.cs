using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBlink : MonoBehaviour
{
    [SerializeField]private GameObject panel1, panel2;
    [SerializeField]private Vector2 panel1RelativeInitPos, panel2RelativeInitPos;
    [SerializeField]private float blinkSpeed;

    [SerializeField]private bool blinkTrigger;

    public bool blinkClose;
    private bool blinkOpen;

    [SerializeField]private float blinkDuration = 0.5f; // duration for blink in seconds

    private float blinkCloseStartTime;
    private float blinkOpenStartTime;

    //Used to make lerp work with deltaTime
    private float timeSinceBlinkStart;

    // Start is called before the first frame update
    void Start()
    {
        panel1RelativeInitPos = panel1.transform.position - Camera.main.transform.position;
        panel2RelativeInitPos = panel2.transform.position - Camera.main.transform.position;
    }

    
    private Vector2 panel1OutOfViewPos() {
        Vector2 topRightScreenPos = new Vector2(Screen.width, Screen.height);
        Vector2 topRightWorldPos = Camera.main.ScreenToWorldPoint(topRightScreenPos);

        return new Vector2(panel1.transform.position.x, topRightWorldPos.y);
    }

    private Vector2 panel2OutOfViewPos() {
        Vector2 bottomLeftScreenPos = new Vector2(0, 0);
        Vector2 bottomLeftWorldPos = Camera.main.ScreenToWorldPoint(bottomLeftScreenPos);

        return new Vector2(panel2.transform.position.x, bottomLeftWorldPos.y);
    }
    private void putPanelsOutOfView() {
        panel1.transform.position = panel1OutOfViewPos();
        panel2.transform.position = panel2OutOfViewPos();

    }

    public void startBlinkClose() {
        //Replaces panels in place
        //panel1.transform.position = (Vector2)Camera.main.transform.position + panel1RelativeInitPos;
        //panel2.transform.position = (Vector2)Camera.main.transform.position + panel2RelativeInitPos;

        putPanelsOutOfView();

        blinkClose = true;
        blinkCloseStartTime = Time.unscaledTime;
        timeSinceBlinkStart = blinkCloseStartTime;
    }

    public void startBlinkOpen() {
        blinkOpen = true;

        blinkOpenStartTime = Time.unscaledTime;
        timeSinceBlinkStart = blinkOpenStartTime;
    }
    // Update is called once per frame
    void Update() {
        if (blinkClose) {
            timeSinceBlinkStart += Time.unscaledDeltaTime;

            float t = (timeSinceBlinkStart - blinkCloseStartTime) / blinkDuration;
            panel1.transform.position = Vector2.Lerp(panel1.transform.position, Camera.main.transform.position, t);
            panel2.transform.position = Vector2.Lerp(panel2.transform.position, Camera.main.transform.position, t);

            if (t >= 1) {
                blinkClose = false;
                panel1.transform.position = Camera.main.transform.position; // Ensure final position
                panel2.transform.position = Camera.main.transform.position; // Ensure final position
            }
        }

        if (blinkOpen && !blinkClose) {
            timeSinceBlinkStart += Time.unscaledDeltaTime;

            float t = (timeSinceBlinkStart - blinkOpenStartTime) / blinkDuration;
            panel1.transform.position = Vector2.Lerp(panel1.transform.position, panel1OutOfViewPos(), t);
            panel2.transform.position = Vector2.Lerp(panel2.transform.position, panel2OutOfViewPos(), t);

            if (t >= 1) {
                blinkOpen = false;
                panel1.transform.position = panel1OutOfViewPos(); // Ensure final position
                panel2.transform.position = panel2OutOfViewPos(); // Ensure final position
            }
        }
    }
}
