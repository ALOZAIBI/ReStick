using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TargettingText : KeepOnTarget {
    
    public TextMeshProUGUI text;

    //Distance from the target it will slowly be going up
    [SerializeField]private float distance=0;
    [SerializeField] private float speed = 1f;

    public bool debugging = false;

    private void Start() {
        //Change the text color to green if ally, red if enemy
        if(debugging && target.GetComponent<Character>().team == (int)Character.teamList.Player)
            text.color = Color.green;
    }
    // Update is called once per frame
    void Update() {
        if (debugging) {
            distance = 0.5f;
            transform.position = target.transform.position + new Vector3(0, distance, 0);
        }
        else { 
            //Slowly move up
            distance += speed * Time.fixedUnscaledDeltaTime;
            speed += 5f * Time.fixedUnscaledDeltaTime;
            //Set the transform
            transform.position = target.transform.position + new Vector3(0, distance, 0);
        }
        
    }
}
