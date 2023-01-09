using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Zone zone; //zone the character is currently In;
    public int DMG;              
    public int HP;               
    public int AS;               
    public int MS;
    public int Range;

    public bool canMove=true;
    public bool canAttack=true;

    public float AtkNext = 0;
    public float AtkCD;

    //Character's team
    public int team;

    //Current targeting strategy
    public int targetStrategy=(int)targetList.DefaultEnemy;

    public Character target;
    public enum teamList {
        Player,
        Enemy1,
        Enemy2,
        Enemy3,
        Other
    }

    public enum targetList {
        DefaultEnemy,    //simply selects first Character from List that isn't on same team
        ClosestEnemy,
        LowestHPEnemy,
        HighestDMGEnemy,
        HighestHPEnemy,

        DefaultAlly,        //makes target a character in same team
        ClosestAlly,
        LowestHPAlly,
        HighestDMGAlly,
        HighestHPAlly
    }
    //for the character to detect which zone it's in
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Zone") {
            zone = collision.GetComponent<Zone>();
        }
    }

    void Start() {
        
    }
    private void movement() {
        //kiting
        //IF ATTACK NOT READY AND TARGET IS WITHIN RANGE by a margin|| ATTACK NOT READY AND won't be anytime soon   && canMove
        if ((AtkNext > 0 && Vector2.Distance(transform.position, target.transform.position) < Range - (Range * 0.15)) && canMove) {
            //Debug.Log(Range - (Range * 0.3));
            transform.position = (Vector2.MoveTowards(transform.position, target.transform.position, -MS * Time.fixedDeltaTime));    //move away from target
        }
        else
        //if distance more than range walk towards target && canMove
        if (Vector2.Distance(transform.position, target.transform.position) > Range && canMove) {
            transform.position = (Vector2.MoveTowards(transform.position, target.transform.position, MS * Time.fixedDeltaTime));
            //Debug.Log("work");
        }
        //Debug.Log("test");
    }

    private void attack() {
        //selectTarget();
    }

    private void selectTarget() {
        switch (targetStrategy) {
            case (int)targetList.DefaultEnemy:
                //loops through all characters
                foreach (Character temp in zone.charactersInside) {  
                    //if temp is in a different team make it the target and exit loop
                    if (temp.team != team) {
                        target = temp;
                        break;
                    }
                }
                break;

            case (int)targetList.ClosestEnemy:
                //initially assume that this is the closest Character
                Character closest = zone.charactersInside[0];
                //loops through all characters
                foreach(Character temp in zone.charactersInside) {
                    //if temp in different team
                    if (temp.team != team) {
                        //if distance between temp and this is less than distance between closest and this make closest = temp
                        if (Vector2.Distance(temp.transform.position, transform.position) < Vector2.Distance(closest.transform.position, transform.position)) {
                            closest = temp;
                        }
                    }
                }
                target = closest;
                break;

        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        selectTarget();
        attack();
        movement();
    }
}
