using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UISizer : MonoBehaviour
{
    private RectTransform rectTransform;
    private RectTransform parent;

    public float widthPercent;
    public float heightPercent;

    public bool resizeDone;
    //if this is ticked. do the size relative to parent
    [SerializeField]private bool sizeRelativeToParent;

    //on the editor if this is ticked then the width and height will be the same. The one that is at 0% will be like the other
    [SerializeField]private bool keepSquared;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if(sizeRelativeToParent)
            parent = transform.parent.GetComponent<RectTransform>();
    }

    public void resize() {
        //The anchor and position is initially set in the editor

        //then we get the size before resizing
        Vector2 initSize = rectTransform.sizeDelta;

        //we resize
        if (sizeRelativeToParent) {
              rectTransform.sizeDelta = new Vector2(parent.sizeDelta.x * widthPercent / 100, parent.sizeDelta.y * heightPercent / 100);
            if (keepSquared)
                keepSquaredFunc();
            toString();
            resizeDone = true;
            //we end the function here since the anchor has already been set and we don't care about screen out of bounds since this is within the parent
            return;
        }
        else
        rectTransform.sizeDelta = new Vector2(Screen.width * widthPercent / 100, Screen.height * heightPercent / 100);

        float widthDelta = rectTransform.sizeDelta.x - initSize.x;
        float heightDelta = rectTransform.sizeDelta.y - initSize.y;
        
        //then we move by difference of size to keep it at the same anchored position
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + (widthDelta / 2),rectTransform.anchoredPosition.y-(heightDelta/2));
        if (keepSquared)
            keepSquaredFunc();
        toString();
        GetComponent<HideUI>().setInitPos();
        resizeDone = true;

    }

    private void toString() {
        if(sizeRelativeToParent)
            Debug.Log(name + " " + rectTransform.sizeDelta+"Parent size"+parent.sizeDelta);
        else
        Debug.Log(name+" "+rectTransform.sizeDelta);

        Debug.Log(rectTransform.anchoredPosition);
    }
    private void keepSquaredFunc() {
        if (widthPercent == 0) {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.y, rectTransform.sizeDelta.y);
        }
        else if (heightPercent == 0) {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.x);
        }
    }
    void Update()
    {
        //if hasn't been resized yet
        if (!resizeDone) {
            //make sure that the parent, if it has the UI Sizer component that it is done
            if (sizeRelativeToParent) {
                //checks if it has the sizer component
                if (parent.GetComponent<UISizer>() != null) {
                    //if it does then check if it is done resizing
                    if (parent.GetComponent<UISizer>().resizeDone) {
                        resize();
                    }
                }
                //else if parent doesn't have sizer component then resize immediately;
                else
                    resize();
                    
            }
            else { 
                resize();
            }
        }
    }
}
