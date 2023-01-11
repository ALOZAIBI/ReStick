using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //zone the character is currently In;
    [SerializeField] private Zone zone; 

    public float DMG;              
    public float HP;
    public float HPMax;
    public float AS;               
    public float MS;
    public float Range;
    public float LS;

    public bool alive = true;

    //used for stuns/debuffs etc..
    public bool canMove=true;
    public bool canAttack=true;
    

    //used for cooldowns
    private bool AtkAvailable = true;
    private float AtkNext = 0;
    private float MovNext = 0;

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
        Moving,
        Ability1
    }

    [SerializeField] private UIManager uiManager;
    //Ability Stuff
        public List<Ability> abilities = new List<Ability>();
        //This is needed for abilities that apply on kill for example heal after a kill
        public int killsLastFrame = 0;
    //Bonus Stats
    public List<BonusStats> bonusStats = new List<BonusStats>();
    //Interesting Stats
    public int totalKills = 0;




    //for the character to detect which zone it's in
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Zone") {
            zone = collision.GetComponent<Zone>();
        }
    }

    void Start() {
        //Initialising HPMax on start
        HPMax = HP;
        //Connect to UIManager
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        initRoundStart();
    }

    //used on every round start to prepare the character for round start
    private void initRoundStart() {
        //Tells the abilities that this owns them
        foreach(Ability temp in abilities) {
            temp.character = this;
        }

        foreach(BonusStats temp in bonusStats) {
            temp.character = this;
            temp.applyStats();
        }

    }
    private void movement() {
        //kiting
        //IF ATTACK NOT READY AND TARGET IS WITHIN RANGE by a margin|| ATTACK NOT READY AND won't be anytime soon   && canMove
        if (canMove&& (!AtkAvailable && Vector2.Distance(transform.position, target.transform.position) < Range - (Range * 0.15))) {
            //move away from target at a slighlty slower speed
            transform.position = (Vector2.MoveTowards(transform.position, target.transform.position, (-MS*0.7f) * Time.fixedDeltaTime));    
        }
        else
        //if canMove && distance more than range walk towards target 
        if (canMove && Vector2.Distance(transform.position, target.transform.position) > Range) {
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
                        //makes the closest be the closest enemy
                        if (closest.team==team || Vector2.Distance(temp.transform.position, transform.position) < Vector2.Distance(closest.transform.position, transform.position)) {
                            closest = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (closest.team == team)
                    closest = null;
                target = closest;
                break;

        }
    }

    private void attack() {
        selectTarget();
        //deal Damage when target is within range and Attack is available and player can Attack and the target is alive
        if (AtkAvailable && canAttack && Vector2.Distance(target.transform.position, transform.position) <= Range && target.alive) {
            //if character uses projectile launch the projectile the projectile will deal the damage and detect if target is killed
            if (usesProjectile) {
                GameObject temp = Instantiate(projectile,transform.position,transform.rotation);
                Projectile instantiatedProjectile = temp.GetComponent<Projectile>();
                instantiatedProjectile.shooter = this;
                instantiatedProjectile.dmg = DMG;
                instantiatedProjectile.speed = 4;       //can make this an attribute to character
                instantiatedProjectile.lifetime = 2;    //can make this an attribute to character
                instantiatedProjectile.target = target;
                instantiatedProjectile.LS = LS;
            }
            else {
                //deal damage to target
                target.HP -= DMG;
                HP += DMG * LS;
                //detect if target is killed to increase totalKills stat
                if (target.HP <= 0) {
                    totalKills++;
                    killsLastFrame++;
                }
            }
            //start cooldown of attack
            startCooldown(1 / AS, (int)actionAvailable.Attack);

            //start cooldown of movement(Character stops moving for a bit after attack)
            //When character has more than 5 AS there is no stopping movement
            if (AS < 5)
                startCooldown(1 / (AS * 2), (int)actionAvailable.Moving);
        }
    }
    //used to set the ActionNext value to CD value. It will actually be cooled down in teh cooldown function
    private void startCooldown(float cooldownDuration,int action) {
        switch (action) {
            case (int)actionAvailable.Attack:
                AtkNext = cooldownDuration;
                AtkAvailable = false;
                break;

            case (int)actionAvailable.Moving:
                MovNext = cooldownDuration;
                canMove = false;
                break;
        }
    }
    //coolsdown everything
    private void cooldown() {
        if (AtkNext > 0) {
            AtkNext -= Time.fixedDeltaTime;
        }
        else {
            AtkAvailable = true;
            AtkNext = 0;
        }

        if (MovNext > 0) {
            MovNext -= Time.fixedDeltaTime;
        }
        else {
            canMove = true;
            MovNext = 0;
        }
    }
    //executes all available abilities
    private void doAbilities() {
        foreach(Ability temp in abilities) {
            temp.doAbility();
        }
    }

    //resets kills last frame at the end of this frame. always keep last in the update function
    private void resetKillsLastFrame() {
        killsLastFrame = 0;
    }
   
    public void handleDeath() {
        if (HP <= 0) {
            //remove character from the zone's character list
            zone.charactersInside.Remove(this);
            gameObject.SetActive(false);
            alive = false;
        }
    }
    //to prevent HP going over the maximum
    private void capHP() {
        if (HP > HPMax)
            HP = HPMax;
    }
    //When Character is clicked opens character Screen with information of this cahracter
    private void OnMouseDown() {
        uiManager.viewCharacter(this);
    }
    void FixedUpdate()
    {
        handleDeath();
        attack();
        cooldown();
        movement();
        doAbilities();
        capHP();
        resetKillsLastFrame();//always keep me last in update
    }
}
