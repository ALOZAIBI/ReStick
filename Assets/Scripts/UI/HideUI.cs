using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    [SerializeField] private GameObject btn;
    [SerializeField] private Vector3 initPos;
    [SerializeField] private Vector3 btnInitPos;
    //position difference from initPos
    [SerializeField] private Vector3 targetPosDelta;
    public bool hidden;
    public int speed=500;


    private void Start() {
        initPos = transform.localPosition;
        btnInitPos = btn.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //local position since child of cmaera
        //if this is to be hidden move it towards targetPosDelta;
        if (hidden) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosDelta, speed * Time.unscaledDeltaTime);
            btn.transform.localPosition = Vector3.MoveTowards(btn.transform.localPosition, targetPosDelta, speed * Time.unscaledDeltaTime);
        }
        else {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, initPos, speed * Time.unscaledDeltaTime);
            btn.transform.localPosition = Vector3.MoveTowards(btn.transform.localPosition, btnInitPos, speed * Time.unscaledDeltaTime);

        }
    }
}
