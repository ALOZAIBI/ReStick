using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public bool targetable = true;
    

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
    public int attackTargetStrategy=(int)targetList.DefaultEnemy;   //who to attack
    public int movementTargetStrategy = (int)targetList.DefaultEnemy;   //By default is the same as attackTarget

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
        //selects the closest enemy
        ClosestEnemy,
        LowestHPEnemy,
        HighestDMGEnemy,
        HighestHPEnemy,

        

        //selects a character in same team
        DefaultAlly,        
        //selects closest ally 
        ClosestAlly,
        LowestHPAlly,
        HighestDMGAlly,
        HighestHPAlly,

        //dont select anyting
        None
    }
    //A function passes what action it wants a cooldown on then the cooldown function using a switch case does the appropriate thing
    public enum actionAvailable {
        Attack,
        Moving,
        isIdle,
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

    //isIdle stuff
        //position in last frame used to check isIdle
        public Vector2 lastPosition;
        //wether or not has been idle
        public bool isIdle;
        //wether or not currently moving cause idle
        public bool idleMov;
        //how many seconds have been idle
        public float secondsIdle;
        //A direction used to move when isIdle or when hitting obstacle
        public Vector2 direction;
        //to be used in cooldown to determine how long the direction will be taken when isIdle
        public float moveDuration;

    //for the character to detect which zone it's in
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Zone") {
            zone = collision.GetComponent<Zone>();
        }
    }

    void Start() {
        //Initialising HPMax on start
        HPMax = HP;
        initRoundStart();
        //Connect to UIManager
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        
    }

    //used on every round start to prepare the character for round start
    private void initRoundStart() {
        //Tells the abilities that this owns them
        foreach(Ability temp in abilities) {
            temp.character = this;
        }
        //applies the stats
        foreach(BonusStats temp in bonusStats) {
            temp.character = this;
            temp.applyStats();
        }

    }

    //sends position to next frame to be used to check for isIdle
    private void lastFramePosition() {
        lastPosition = transform.position;
    }

    //compares current position to last frames position to check if idle
    private void checkIdle(float randomMovDuration,float timeToConsiderIdle) {
        //checks if same position
            if (Vector2.SqrMagnitude((Vector2)transform.position - lastPosition) < 0.000001) {
                secondsIdle += Time.deltaTime;  //keep this delta time since checkIdle is in the update and not fixedupdate
            }
            else
                secondsIdle = 0;
            
        if (isIdle == false) {
            //once the character is deemed to be idle make the character move for randomMovDuration in direction direction
            if (secondsIdle >= timeToConsiderIdle) {
                startCooldown(randomMovDuration, (int)actionAvailable.isIdle);
                //generates random direction
                direction = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
                direction.Normalize();
            }
        }
        
    }
    private void movement() {
        //if the character is idle move towards direction that was randomly generated in checkIdle
        if (isIdle) {
            transform.position = (Vector2)transform.position + (direction * (MS*0.5f * Time.fixedDeltaTime));
        }
        
        else {
            selectTarget(movementTargetStrategy);
            //Kiting(Moves away from target when attack not ready)
            if (target.alive && canMove && (!AtkAvailable && Vector2.Distance(transform.position, target.transform.position) < Range - (Range * 0.15))) {
                //move away from target
                transform.position = (Vector2.MoveTowards(transform.position, target.transform.position, -MS * Time.fixedDeltaTime));
            }
            else
            //walks towards target till in range
            if (target.alive && canMove && Vector2.Distance(transform.position, target.transform.position) > Range) {
                transform.position = (Vector2.MoveTowards(transform.position, target.transform.position, MS * Time.fixedDeltaTime));
            }
        }
    }

   

    //does targetting logic to select target
    public void selectTarget(int whatStrategy) {
        switch (whatStrategy) {
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

            case (int)targetList.ClosestAlly:
                //initially assume that this is the closest ally
                Character closestAlly = zone.charactersInside[0];
                //loops through all characters
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and is not itself
                    if (temp.team == team&& temp!=this) {
                        //sets closestAlly
                        if (closestAlly.team == team || Vector2.Distance(temp.transform.position, transform.position) < Vector2.Distance(closestAlly.transform.position, transform.position)) {
                            closestAlly = temp;
                        }
                    }
                }
                target = closestAlly;
                break;

            case (int)targetList.None:
                target = null;
                break;

            default:
                break;
        }
    }

    private void attack() {
        selectTarget(attackTargetStrategy);
        //deal Damage when target is within range and Attack is available and player can Attack and the target is alive
        if (AtkAvailable && canAttack && Vector2.Distance(target.transform.position, transform.position) <= Range && target.alive) {
            //if character uses projectile launch the projectile the projectile will deal the damage and detect if target is killed
            if (usesProjectile) {
                GameObject temp = Instantiate(projectile,transform.position,transform.rotation);
                Projectile instantiatedProjectile = temp.GetComponent<Projectile>();
                instantiatedProjectile.shooter = this;
                instantiatedProjectile.dmg = DMG;
                //THE SPEED IS SET IN THE PROJECTILE OBJECT ITSELF
                //instantiatedProjectile.speed = 4;       //can make this an attribute to character
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
    //used to set the ActionNext value to CD value. It will actually be cooled down in the cooldown function which is called in the update function
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

            case (int)actionAvailable.isIdle:
                moveDuration = cooldownDuration;
                //idleMov = true;
                isIdle = true;
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

        if (moveDuration > 0) {
            moveDuration -= Time.fixedDeltaTime;
        }
        else {
            //resets isIdle to exit idle movement loop and resets idleMov like other cooldowns
            //idleMov = false;
            //secondsIdle = 0;
            isIdle = false;
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
    //When Character is clicked checks if the click is held or if it's just a quick click. If it's a quick click open cahracter screen otherwise do nothing since holding is used for panning camera
    private void OnMouseDown() {
        //prevent clijcking through UI
        //if (IsPointerOverGameObject()) {
        //    return;
        //}
        //to start mouseClickedNotHeld Function
        click = true;
    }
    private float mouseHoldDuration = 0;
    private bool click = false;
    private void mouseClickedNotHeld() {
        //if this function is called by OnMouseDown
        if (click) {
            //if click is still held increment time
            if (Input.GetMouseButton(0)) {
                //using unscaled time since it should work even when timescale is 0 i.e when game is paused.
                mouseHoldDuration += Time.unscaledDeltaTime;
            }
            //if click is not held check how long it was held for. If it was held for less than 0.2 seconds open character screen
            else if (mouseHoldDuration < 0.2f) {
                uiManager.viewCharacter(this);
                //reset values
                mouseHoldDuration = 0;
                click = false;
            }
            //if click is held too long
            else {
                //reset values
                mouseHoldDuration = 0;
                click = false;
            }
        }
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

    private void Update() {
        mouseClickedNotHeld();

        checkIdle(0.2f,2f);//receives last frame position
        lastFramePosition();//sends last frame position
    }

    //to prevent clicking thorugh UI
    //https://answers.unity.com/questions/1115464/ispointerovergameobject-not-working-with-touch-inp.html
    public static bool IsPointerOverGameObject() {
        // Check mouse
        if (EventSystem.current.IsPointerOverGameObject()) {
            return true;
        }

        // Check touches
        for (int i = 0; i < Input.touchCount; i++) {
            var touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began) {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) {
                    return true;
                }
            }
        }

        return false;
    }
}

/***
 * RigidBody's sleeping mode has been set to never sleep. Because otherwise the ontrigger enter of the zone and character 
 * won't work unless 1 of the 2 move. However this might cause some optimization issues. So in the future maybe set it to never sleep on 
 * zone start and switch it back to start awake
 */
