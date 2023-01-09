using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //zone the character is currently In;
    [SerializeField] private Zone zone; 

    public int DMG;              
    public int HP;
    public int HPMax;
    public float AS;               
    public int MS;
    public int Range;

    //used for stuns/debuffs etc..
    public bool canMove=true;
    public bool canAttack=true;

    
    private bool AtkAvailable = true;

    //projectile stuff
    public bool usesProjectile;
    public GameObject projectile;
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
        //simply selects first Character from List that isn't on same team
        DefaultEnemy,    
        //selects the closest enemy by distance
        ClosestEnemy,
        LowestHPEnemy,
        HighestDMGEnemy,
        HighestHPEnemy,

        //selects a character in same team
        DefaultAlly,        
        ClosestAlly,
        LowestHPAlly,
        HighestDMGAlly,
        HighestHPAlly
    }
    //A function passes what action it wants a cooldown on then the cooldown function using a switch case does the appropriate thing
    public enum actionAvailable {
        Attack,
        Ability1
    }
    //for the character to detect which zone it's in
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Zone") {
            zone = collision.GetComponent<Zone>();
        }
    }

    void Start() {
        //Initialising HPMax on start
        HPMax = HP;
    }
    private void movement() {
        //kiting
        //IF ATTACK NOT READY AND TARGET IS WITHIN RANGE by a margin|| ATTACK NOT READY AND won't be anytime soon   && canMove
        if ((!AtkAvailable && Vector2.Distance(transform.position, target.transform.position) < Range - (Range * 0.15)) && canMove) {
            //move away from target at a slighlty slower speed
            transform.position = (Vector2.MoveTowards(transform.position, target.transform.position, (-MS*0.9f) * Time.fixedDeltaTime));    
        }
        else
        //if canMove && distance more than range walk towards target 
        if (Vector2.Distance(transform.position, target.transform.position) > Range && canMove) {
            transform.position = (Vector2.MoveTowards(transform.position, target.transform.position, MS * Time.fixedDeltaTime));
        }
    }

   

    //does targetting logic to select target
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

    private void attack() {
        selectTarget();
        //deal Damage when target is within range and Attack is available and player can Attack
        if (AtkAvailable && canAttack && Vector2.Distance(target.transform.position, transform.position) <= Range ) {
            //if character uses projectile launch the projectile
            if (usesProjectile) {
                GameObject temp = Instantiate(projectile,transform.position,transform.rotation);
                Projectile instantiatedProjectile = temp.GetComponent<Projectile>();
                instantiatedProjectile.shooter = this;
                instantiatedProjectile.dmg = DMG;
                instantiatedProjectile.speed = 4;
                instantiatedProjectile.lifetime = 10;
                instantiatedProjectile.target = target;
            }
            else {
                //deal damage to target
                target.HP -= DMG;
            }
            //start cooldown
            StartCoroutine(StartCooldown(1/AS, (int)actionAvailable.Attack));
        }
    }
    //Function Starts Coroutine of cooldown, Cooldown will render the appropriate available variable to false until cooldown duration is over
    public IEnumerator StartCooldown(float CooldownDuration,int actionIsAvailable) {
        switch (actionIsAvailable) {
            case (int)actionAvailable.Attack:
                AtkAvailable = false;
                yield return new WaitForSeconds(CooldownDuration);
                AtkAvailable = true;
                break;
        }
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        attack();
        movement();
    }
}
