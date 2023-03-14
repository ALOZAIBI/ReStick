using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour {
    //zone the character is currently In;
    [SerializeField] private Zone zone;
    [SerializeField] private Camera cam;
    [SerializeField] private CameraMovement camMov;

    //Current stats
    public float DMG;
    public float HP;
    public float HPMax;
    public float AS;
    public float MS;
    public float Range;
    public float LS;

    public bool alive = true;

    //Interesting Stats
    public int totalKills = 0;

    //on Zone start stats. (Used to emphaseize buffs and debuffs in the UI and will be used to reset interesting stats on loss_
    public float zsDMG;
    public float zsHP;
    public float zsHPMax;
    public float zsAS;
    public float zsMS;
    public float zsRange;
    public float zsLS;

    public int zsTotalKills;

    //used for stuns/debuffs etc..
    public bool canMove = true;
    public bool canAttack = true;

    public bool targetable = true;


    //used for cooldowns
    private bool AtkAvailable = true;
    private float AtkNext = 0;
    private float MovNext = 0;

    //projectile stuff
    public bool usesProjectile;
    public GameObject projectile;

    //level stuff
    public int level=1;
    //how much xp in current level
    public int xpProgress=0;
    //how much xp needed to level up
    public int xpCap;
    //points that can be used on stats (gained wen leveling up)
    public int statPoints;
    #region
    //Character's team
    public int team;

    //Current targeting strategy
    public int attackTargetStrategy = (int)targetList.DefaultEnemy;   //who to attack
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

        HighestDMGEnemy,
        LowestDMGEnemy,
        
        HighestHPEnemy,
        LowestHPEnemy,



        //selects a character in same team
        DefaultAlly,        
        //selects closest ally 
        ClosestAlly,
        LowestDMGAlly,
        HighestDMGAlly,
        HighestHPAlly,
        LowestHPAlly,

        //I will add highestMaxHP variants
        //and lowest maxHP ...          still working on what to call them. Highest total HP? Highest Full Health?


        //maybe also add highest/lowest AS

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
    //Buffs/Debuffs
    public List<Buff> buffs = new List<Buff>();

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
        //setup level cap
        xpCap = level + (level * ((level - 1) / 2));

        //Connect to camera
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camMov = cam.GetComponent<CameraMovement>();
    }

    //used on every round start(Start button pressed in Character Placing Screen) to prepare the character for round start
    //sets all ablities' cahracter to this character.
    public void initRoundStart() {
        //Tells the abilities that this owns them
        foreach(Ability temp in abilities) {
            temp.character = this;
        }
        //applies the stats
        foreach(BonusStats temp in bonusStats) {
            temp.character = this;
            temp.applyStats();
        }
        //gets the stats on round start
        zsDMG  = DMG;  
        zsHP   = HP;
        zsHPMax= HPMax;
        zsAS   = AS;   
        zsMS   = MS;
        zsRange= Range;
        zsLS   = LS;

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
            transform.position = (Vector2)transform.position + (direction * (MS * 0.5f * Time.fixedDeltaTime));
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

            case (int)targetList.HighestDMGEnemy:
                //initially assume that this is the MaxDmg Character
                Character maxDmg = zone.charactersInside[0];
                foreach(Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if(temp.team != team) {
                        //maxDmg.team == team is done in case the MaxDmg init was actually an ally
                        if (maxDmg.team == team|| temp.DMG > maxDmg.DMG) {
                            maxDmg = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (maxDmg.team == team)
                    maxDmg = null;
                target = maxDmg;
                break;

            case (int)targetList.LowestDMGEnemy:
                //initially assume that this is the MaxDmg Character
                Character minDmg = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        //minDmg.team == team is done in case the MaxDmg init was actually an ally
                        if (minDmg.team == team || temp.DMG < minDmg.DMG) {
                            minDmg = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (minDmg.team == team)
                    minDmg = null;
                target = minDmg;
                break;

            case (int)targetList.HighestHPEnemy:
                //initially assume that this is the MaxHP Character
                Character maxHP = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        //maxHP.team == team is done in case the MaxHP init was actually an ally
                        if (maxHP.team == team || temp.HP > maxHP.HP) {
                            maxHP = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (maxHP.team == team)
                    maxHP = null;
                target = maxHP;
                break;

            case (int)targetList.LowestHPEnemy:
                //initially assume that this is the minHP Character
                Character minHP = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        //minHP.team == team is done in case the minHP init was actually an ally
                        if (minHP.team == team || temp.HP < minHP.HP) {
                            minHP = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (minHP.team == team)
                    minHP = null;
                target = minHP;
                break;


            case (int)targetList.ClosestAlly:
                //initially assume that this is the closest ally
                Character closestAlly = zone.charactersInside[0];
                //loops through all characters
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and is not itself
                    if (temp.team == team&& temp!=this) {
                        //closest.team !=team is done in case closest was actually an enemy
                        if (closestAlly.team != team || Vector2.Distance(temp.transform.position, transform.position) < Vector2.Distance(closestAlly.transform.position, transform.position)) {
                            closestAlly = temp;
                        }
                    }
                }
                //if the only remaining characters are enemies don't select anyone
                if(closestAlly.team != team) {
                    closestAlly = null;
                }
                target = closestAlly;
                break;

            case (int)targetList.HighestDMGAlly:
                //initially assume that this is the MaxDmg Character
                Character maxDmgAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and is not itself
                    if (temp.team == team && temp!=this) {
                        //maxDmg.team == team is done in case the MaxDmg init was actually an enemy
                        if (maxDmgAlly.team != team || temp.DMG > maxDmgAlly.DMG) {
                            maxDmgAlly = temp;
                        }
                    }
                }
                //if there's only enemies remaining target nothing
                if (maxDmgAlly.team != team)
                    maxDmgAlly = null;
                target = maxDmgAlly;
                break;

            case (int)targetList.LowestDMGAlly:
                //initially assume that this is the MaxDmg Character
                Character minDmgAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and not itself
                    if (temp.team == team && temp!=this) {
                        //minDmgAlly.team != team is done in case the MaxDmg init was actually an enemy
                        if (minDmgAlly.team != team || temp.DMG < minDmgAlly.DMG) {
                            minDmgAlly = temp;
                        }
                    }
                }
                //if there's only enemies remaining target nothing
                if (minDmgAlly.team != team)
                    minDmgAlly = null;
                target = minDmgAlly;
                break;

            case (int)targetList.HighestHPAlly:
                //initially assume that this is the maxHPAlly Character
                Character maxHPAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and not self
                    if (temp.team == team && temp!=this) {
                        //maxHPAlly.team != team is done in case the maxHPAlly init was actually an enemy
                        if (maxHPAlly.team != team || temp.HP > maxHPAlly.HP) {
                            maxHPAlly = temp;
                        }
                    }
                }
                //if there's only enemy remaining target nothing
                if (maxHPAlly.team != team)
                    maxHPAlly = null;
                target = maxHPAlly;
                break;

            case (int)targetList.LowestHPAlly:
                //initially assume that this is the minHPAlly Character
                Character minHPAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and not self
                    if (temp.team == team && temp != this) {
                        //minHPAlly.team != team is done in case the minHPAlly init was actually an enemy
                        if (minHPAlly.team != team || temp.HP < minHPAlly.HP) {
                            minHPAlly = temp;
                        }
                    }
                }
                //if there's only enemy remaining target nothing
                if (minHPAlly.team != team)
                    minHPAlly = null;
                target = minHPAlly;
                break;

            case (int)targetList.None:
                target = null;
                break;


            default:
                break;
        }
    }

    //idea to incorporate animation
    //when attack is supposed to happen instead of doing the damage and stuff call another function that 
    //runs the attack animation then on the specified frame deal the damage and stuff.
    //look into animation events
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
                    kill(target);
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
    //if held on character
    public bool held;
    private void mouseClickedNotHeld() {
        //if this function is called by OnMouseDown
        if (click) {
            //if click is still held increment time
            if (Input.GetMouseButton(0)) {
                //using unscaled time since it should work even when timescale is 0 i.e when game is paused.
                mouseHoldDuration += Time.unscaledDeltaTime;

                //Every thing commented out is to be able to reposition character after it has been placed before game starts.

                ////zone is usually detected in the ontrigger however when loading a new zone the game is initially paused so ontrigger won't work
                //if(zone == null) {
                //    zone = GameObject.FindGameObjectWithTag("Zone").GetComponent<Zone>();
                //}
                ////if drag and zone didn't start and is playercharacter then move character
                //if (!zone.started && team == (int)teamList.Player && mouseHoldDuration > 0.2f) {
                //    //hide the placing screen and be able to move the character
                //    //uiManager.placingScreenHidden.hidden = true;
                //    held = true;
                //    transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
                //    camMov.pannable = false;
                //}

            }
            //else(mouse is not clicked)
            else {
                //if is a click and not hold
                if (mouseHoldDuration < 0.2f) {
                    uiManager.viewCharacter(this);

                }
                //reset values
                mouseHoldDuration = 0;
                click = false;
                //uiManager.placingScreenHidden.hidden = false;
                //camMov.pannable = true;
                //held = false;
            }
        }
    }
    #endregion

    //increase killer's kill stats and xp
    public void kill(Character victim) {
        totalKills++;
        killsLastFrame++;
        //level progress will depend on victim's level the equation is open to changing
        xpProgress += victim.level;
    }
    private void levelUp() {
        xpProgress -= xpCap;
        level++;
        //maybe give more stats every 10 levels or smthn level cap setup is done in start method as well.
        statPoints++;
        xpCap = level + (level * ((level - 1) / 2));
        //update xpCap depending on level
    }
    void FixedUpdate()
    {
        
        handleDeath();
        attack();
        cooldown();
        movement();
        doAbilities();
        capHP();
        if (xpProgress >= xpCap)
            levelUp();

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
