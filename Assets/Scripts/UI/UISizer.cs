using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

//to be executed before HideUI so that init pos is after being anchored
[DefaultExecutionOrder(-100)]
public class UISizer : MonoBehaviour
{
    private RectTransform rectTransform;
    private RectTransform canvas;
    public float widthPercent;
    public float heightPercent;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = transform.parent.GetComponent<RectTransform>();
        resize();
    }

    public void resize() {
        //The anchor and position is initially set in the editor

        //then we get the size before resizing
        Vector2 initSize = rectTransform.sizeDelta;
        Debug.Log(initSize);
        Debug.Log(rectTransform.anchoredPosition);
        //we resize
        rectTransform.sizeDelta = new Vector2(Screen.width * widthPercent / 100, Screen.height * heightPercent / 100);
        float widthDelta = rectTransform.sizeDelta.x - initSize.x;
        float heightDelta = rectTransform.sizeDelta.y - initSize.y;
        Debug.Log(widthDelta);
        Debug.Log(heightDelta);
        //then we move by difference of size to keep it at the same anchored position
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + (widthDelta / 2),rectTransform.anchoredPosition.y-(heightDelta/2)); 
        

    }
    void Update()
    {

    }
}
