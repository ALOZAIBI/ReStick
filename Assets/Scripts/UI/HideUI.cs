using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    //just to be able to hide multiple objects in 1 script this is rarely used currently it is used for CharacterPlacingscreen and it's button is other
    [SerializeField] private GameObject other;
    [SerializeField] public Vector3 initPos;
    [SerializeField] private Vector3 otherInitPos;
    //position difference from initPos
    [SerializeField] private Vector3 targetPosDelta;
    //if this is true then initPos will be manually set otherwise it is calculated on start
    [SerializeField] private bool manualInit;
    private bool initPosSet;
    public bool hidden;
    public int speed=500;


    private void Start() {
        if (!manualInit) { 
            setInitPos();
        }
    }
    public void setInitPos() {
        initPos = transform.localPosition;
        try { otherInitPos = other.transform.localPosition; } catch { }

        //if it's hidden immediately move this to the target so that once game starts there is immediately nothing on screen
        if (hidden) {
            transform.localPosition = initPos + targetPosDelta;
            try { other.transform.localPosition = otherInitPos + targetPosDelta; } catch { }
        }
        initPosSet = true;
    }
    // Update is called once per frame
    void Update() {
        if (initPosSet) { 
            //local position since child of cmaera
            //if this is to be hidden move it towards init - targetPosDelta;
            if (hidden) {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, initPos + targetPosDelta, speed * Time.unscaledDeltaTime);
                try { other.transform.localPosition = Vector3.MoveTowards(other.transform.localPosition, otherInitPos + targetPosDelta, speed * Time.unscaledDeltaTime); } catch { }
            }
            //else return to init
            else {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, initPos, speed * Time.unscaledDeltaTime);
                try { other.transform.localPosition = Vector3.MoveTowards(other.transform.localPosition, otherInitPos, speed * Time.unscaledDeltaTime); } catch { }

            }
        }
    }
}
