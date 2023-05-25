using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour {
    //which prefab this character is this is used to save and load character
    public int prefabIndex;
    //zone the character is currently In;
    [SerializeField] public Zone zone;
    [SerializeField] private Camera cam;
    [SerializeField] private CameraMovement camMov;
    public NavMeshAgent agent;
    //Current stats
    public float PD;
    public float MD;
    public float INF;
    public float HP;
    public float HPMax;
    public float AS;
    //CDR is a percentage if it is at 1.00 it iss (100%) 
    public float CDR;
    public float MS;
    public float Range;
    public float LS;

    public bool alive = true;

    //Interesting Stats
    public int totalKills = 0;
    public float totalDamage = 0;

    //on Zone start stats. (Used to emphaseize buffs and debuffs in the UI 
    [HideInInspector]public float zsPD;
    [HideInInspector]public float zsMD;
    [HideInInspector]public float zsINF;
    [HideInInspector]public float zsHP;
    [HideInInspector]public float zsHPMax;
    [HideInInspector]public float zsAS;
    [HideInInspector]public float zsCDR;
    [HideInInspector]public float zsMS;
    [HideInInspector]public float zsRange;
    [HideInInspector]public float zsLS;

    //[HideInInspector]public int zsTotalKills;
    //[HideInInspector]public float zsTotalDamage;

    //used for stuns/debuffs etc..
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canAttack = true;

    [HideInInspector] public bool targetable = true;


    //used for cooldowns
    private bool AtkAvailable = true;
    public float AtkNext = 0;
    public float MovNext = 0;

    //projectile stuff
    public bool usesProjectile;
    public GameObject projectile;

    //level stuff
    public int level=1;
    //how much xp in current level
    public float xpProgress=0;
    //how much xp needed to level up
    public int xpCap=1;
    //points that can be used on stats (gained wen leveling up)
    public int statPoints;
    #region
    //Character's team
    public int team;
    //indicates wether this character has been summoned by another
    public bool summoned;
    public Character summoner;

    //Current targeting strategy
    public int attackTargetStrategy = (int)targetList.ClosestEnemy;   //who to attack
    public int movementStrategy = (int)movementStrategies.Default;   //By default is the same as attackTarget
    public int stayNearAllyTarge=(int)targetList.ClosestAlly;//if movement strategy

    //used to root silence and blind etc..
    public bool snare;
    public bool silence;
    public bool blind;

    public Character target;

    //animation stuff
    public AnimationManager animationManager;

    //indicator stuff
    public Indicators indicators;

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

        HighestPDEnemy,
        LowestPDEnemy,

        HighestMDEnemy,
        LowestMDEnemy,

        HighestASEnemy,
        LowestASEnemy,

        HighestMSEnemy,
        LowestMSEnemy,

        HighestRangeEnemy,
        LowestRangeEnemy,


        HighestHPEnemy,
        LowestHPEnemy,



        //selects a character in same team
        DefaultAlly,        
        //selects closest ally 
        ClosestAlly,

        LowestPDAlly,
        HighestPDAlly,

        HighestMDAlly,
        LowestMDAlly,

        HighestASAlly,
        LowestASAlly,

        HighestMSAlly,
        LowestMSAlly,

        HighestRangeAlly,
        LowestRangeAlly,

        HighestHPAlly,
        LowestHPAlly,

        //I will add highestMaxHP variants
        //and lowest maxHP ...          still working on what to call them. Highest total HP? Highest Full Health?

        HighestINFEnemy,
        LowestINFEnemy,

        HighestINFAlly,
        LowestINFAlly,
        //maybe also add highest/lowest AS

        //dont select anyting
        None
    }
    public enum movementStrategies {
        Default,    //walks to target and kite
        StayNearAlly,   //for now it is stay near closest ally. Update it later to make it so that it stays near stayNearAllyTarget
        DontMove,
        RunAwayFromNearestEnemy
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
        public Vector2 randomDestination;
        //to be used in cooldown to determine how long the direction will be taken when isIdle
        public float moveDuration;

    //for the character to detect which zone it's in
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Zone") {
            zone = collision.GetComponent<Zone>();
        }
    }

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        initRoundStart();
        animationManager = GetComponent<AnimationManager>();
        //Connect to UIManager
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        //setup level cap
        xpCap = level + (level * ((level - 1) / 2));

        indicators = GetComponentInChildren<Indicators>();
        //Connect to camera
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camMov = cam.GetComponent<CameraMovement>();
    }

    //used on every round start(Start button pressed in Character Placing Screen) to prepare the character for round start
    //sets all ablities' cahracter to this character.
    public void initRoundStart() {
        //Tells the abilities that this owns them and resets their cd
        foreach(Ability temp in abilities) {
            temp.character = this;
            temp.available = true;
            temp.abilityNext = 0;
        }
        //applies the stats
        foreach(BonusStats temp in bonusStats) {
            temp.character = this;
            temp.applyStats();
        }
        //to force the character to go through the moveTowardsFunction gets the stats on round start
        timeSinceDestinationUpdate = 100;
        previousMovementState = 99;

        MovNext = 0;
        AtkNext = 0;
        zsPD  = PD;
        zsMD = MD;
        zsINF = INF;
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
            //once the character is deemed to be idle make the character move for randomMovDuration towards the randomDestination
            if (secondsIdle >= timeToConsiderIdle) {
                startCooldown(randomMovDuration, (int)actionAvailable.isIdle);
                //generates random destination
                Vector2 randomAmount =new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
                randomDestination = (Vector2)transform.position + randomAmount;
            }
        }
        
    }

    public float destinationUpdateInterval=0.5f;
    public float timeSinceDestinationUpdate;
    public int movementState;// 0 not moving 1 idle 2 kiting 3 movingToTarget
    public int previousMovementState;
    //call this to update destination. We are using this function so that setDestionation isn't done on every frame to improve performance.
    //instead it will be done in an interval basis or when changing movement states
    private void moveTowards(Vector3 destination) {

        if (timeSinceDestinationUpdate >= destinationUpdateInterval || movementState != previousMovementState) {

            timeSinceDestinationUpdate = 0;
                agent.SetDestination(destination);
        }
        //Debug.Log(destination+ "MoveTwrds");
    }
    private void doMoveStrategy(int strategy) {
        //the range that is used to apply the move strategy
        int seekRange;
        switch (strategy) {
            case (int)movementStrategies.Default:
                selectTarget(attackTargetStrategy);
                //Kiting(Moves away from target when attack not ready)
                if (target.alive && canMove && (!AtkAvailable && Vector2.Distance(transform.position, target.transform.position) < Range - (Range * 0.15))) {
                    movementState = 2;
                    try { animationManager.move(true); }
                    catch { /*IF this character has no animation manager it's okay*/}
                    //move away from target
                    //finds the point opposite the target https://gamedev.stackexchange.com/questions/80277/how-to-find-point-on-a-circle-thats-opposite-another-point
                    Vector2 pointOpposite = new Vector2(transform.position.x - target.transform.position.x, transform.position.y - target.transform.position.y) + (Vector2)transform.position;
                    moveTowards(pointOpposite);
                }
                else
                //walks towards target till in range
                if (target.alive && canMove && Vector2.Distance(transform.position, target.transform.position) > Range) {
                    movementState = 3;
                    try { animationManager.move(true); }
                    catch { /*IF this character has no animation manager it's okay*/}
                    //transform.position = (Vector2.MoveTowards(transform.position, target.transform.position, MS * Time.fixedDeltaTime));
                    moveTowards(target.transform.position);
                }
                else {
                    //once it is in range stop moving
                    agent.isStopped = true;
                    try { animationManager.move(false); }
                    catch { /*IF this character has no animation manager it's okay*/}
                }
                break;

            case (int)movementStrategies.StayNearAlly:
                //move towards closest ally within a radius of 2
                seekRange = 2;
                selectTarget(stayNearAllyTarge);
                if (target.alive && canMove && Vector2.Distance(transform.position, target.transform.position) > seekRange) {
                    moveTowards(target.transform.position);
                    try { animationManager.move(true); }
                    catch { /*IF this character has no animation manager it's okay*/}
                }
                //once in range stop moving
                else {
                    agent.isStopped = true;
                    try { animationManager.move(false); }
                    catch { /*IF this character has no animation manager it's okay*/}
                }
                break;

            case (int)movementStrategies.DontMove:
                agent.isStopped = true;
                break;

            case (int)movementStrategies.RunAwayFromNearestEnemy:
                //run away from enemies as far as 8 range
                seekRange = 8;
                selectTarget((int)targetList.ClosestEnemy);
                if(target.alive && canMove && Vector2.Distance(transform.position, target.transform.position) < seekRange) {
                    //finds the point opposite the target https://gamedev.stackexchange.com/questions/80277/how-to-find-point-on-a-circle-thats-opposite-another-point
                    Vector2 pointOpposite = new Vector2(transform.position.x - target.transform.position.x, transform.position.y - target.transform.position.y) + (Vector2)transform.position;
                    moveTowards(pointOpposite);
                    
                }
                break;
        }
    }
    private void movement() {
        //sets the speed
        agent.speed = MS;
        //if can't move
        if (!canMove) {

            movementState = 0;
            //stops the agent from moving
            agent.isStopped = true;
            try { animationManager.move(false);} 
            catch { /*IF this character has no animation manager it's okay*/}
            
        }
        else {
            agent.isStopped = false;

        }
        //if the character is idle and movestrategy is not set to dont move, move towards direction that was randomly generated in checkIdle
        if (isIdle && movementStrategy !=(int)movementStrategies.DontMove) {
            movementState = 1;
            try { animationManager.move(true); }
            catch { /*IF this character has no animation manager it's okay*/}
            moveTowards(randomDestination);

        }
        //movement
        else {
            
            doMoveStrategy(movementStrategy);

        }
    }

   

    //does targetting logic to select target
    //the return used to be void since we didn't need to return anything but in some cases it is needed to return the character that this targetted for example in the projectiledestinationthentarget script.
    public Character selectTarget(int whatStrategy) {
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
                //if it happens to be self, select another
                if(closest == this)
                    closest = zone.charactersInside[1];
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

            case (int)targetList.HighestPDEnemy:
                //initially assume that this is the MaxPD Character
                Character maxPD = zone.charactersInside[0];
                foreach(Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if(temp.team != team) {
                        //if maxPD init was actually an ally
                        if (maxPD.team == team) {
                            maxPD = temp;
                        }
                        else//if it wasn't an ally
                        if (temp.PD > maxPD.PD)
                            maxPD = temp;
                        else if(temp.PD == maxPD.PD)
                        {
                            //if temp PD is == to max PD then select the closest of the 2
                            if(Vector2.Distance(transform.position,temp.transform.position)<Vector2.Distance(transform.position,maxPD.transform.position))
                            maxPD = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (maxPD.team == team)
                    maxPD = null;
                target = maxPD;
                break;

            case (int)targetList.LowestPDEnemy:
                //initially assume that this is the MinPD Character
                Character minPD = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        if (minPD.team == team) {
                            minPD = temp;
                        }
                        else
                        //minPD.team == team is done in case the MinPD init was actually an ally
                        if (temp.PD < minPD.PD)
                            minPD = temp;
                        else if(temp.PD == minPD.PD)
                        {
                            //if temp PD is == to max PD then select the closest of the 2
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, minPD.transform.position))
                                minPD = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (minPD.team == team)
                    minPD = null;
                target = minPD;
                break;

            case (int)targetList.HighestMDEnemy:
                //initially assume that this is the MaxMD Character
                Character maxMD = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        if (maxMD.team == team) {
                            maxMD = temp;
                        }
                        else
                        if (temp.MD > maxMD.MD)
                            maxMD = temp;
                        else if (maxMD.MD == temp.MD)
                        {
                            //if temp PD is == to max PD then select the closest of the 2
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, maxMD.transform.position))
                                maxMD = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (maxMD.team == team)
                    maxMD = null;
                target = maxMD;
                break;

            case (int)targetList.LowestMDEnemy:
                //initially assume that this is the MaxMD Character
                Character minMD = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        if (minMD.team == team) {
                            minMD = temp;
                        }
                        else
                        //minMD.team == team is done in case the MaxMD init was actually an ally
                        if (temp.MD < minMD.MD)
                            minMD = temp;
                        else if (temp.MD == minMD.MD)
                        {
                            //if temp PD is == to max PD then select the closest of the 2
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, minMD.transform.position))
                                minMD = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (minMD.team == team)
                    minMD = null;
                target = minMD;
                break;

            case (int)targetList.HighestINFEnemy:
                //initially assume that this is the MaxINF Character
                Character maxINF = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        if (maxINF.team == team) {
                            maxINF = temp;
                        }
                        else
                            if (temp.INF > maxINF.INF)
                            maxINF = temp;
                        else if (temp.INF == maxINF.INF) {
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, maxINF.transform.position))
                                maxINF = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (maxINF.team == team)
                    maxINF = null;
                target = maxINF;
                break;

            case (int)targetList.LowestINFEnemy:
                //initially assume that this is the MaxINF Character
                Character minINF = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        if (minINF.team == team) {
                            minINF = temp;
                        }
                        else
                            if (temp.INF < minINF.INF)
                            minINF = temp;
                        else if (temp.INF == minINF.INF) {
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, minINF.transform.position))
                                minINF = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (minINF.team == team)
                    minINF = null;
                target = minINF;
                break;

            case (int)targetList.HighestASEnemy:
                //initially assume that this is the MaxAS Character
                Character maxAS = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        if(maxAS.team == team) {
                            maxAS = temp;
                        }
                        else
                            if (temp.AS > maxAS.AS) 
                            maxAS = temp;
                        else if (temp.AS == maxAS.AS) {
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, maxAS.transform.position))
                                maxAS = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (maxAS.team == team)
                    maxAS = null;
                target = maxAS;
                break;

            case (int)targetList.LowestASEnemy:
                //initially assume that this is the MaxAS Character
                Character minAS = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        if (minAS.team == team) {
                            minAS = temp;
                        }
                        else
                            if (temp.AS < minAS.AS)
                            minAS = temp;
                        else if(temp.AS == minAS.AS)
                        {
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, minAS.transform.position))
                                minAS = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (minAS.team == team)
                    minAS = null;
                target = minAS;
                break;

            case (int)targetList.HighestMSEnemy:
                //initially assume that this is the MaxMS Character
                Character maxMS = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        if (maxMS.team == team) {
                            maxMS = temp;
                        }
                        else
                            //maxMS.team == team is done in case the MaxMS init was actually an ally
                            if (temp.MS > maxMS.MS)
                            maxMS = temp;
                        else if(temp.MS == maxMS.MS)
                        {
                            //if temp PD is == to max PD then select the closest of the 2
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, maxMS.transform.position))
                                maxMS = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (maxMS.team == team)
                    maxMS = null;
                target = maxMS;
                break;

            case (int)targetList.LowestMSEnemy:
                //initially assume that this is the MaxMS Character
                Character minMS = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        if (minMS.team == team) {
                            minMS = temp;
                        }
                        else
                            if (temp.MS < minMS.MS)
                            minMS = temp;
                        else if(temp.MS == minMS.MS)
                        {
                            //if temp PD is == to max PD then select the closest of the 2
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, minMS.transform.position))
                                minMS = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (minMS.team == team)
                    minMS = null;
                target = minMS;
                break;

            case (int)targetList.HighestRangeEnemy:
                //initially assume that this is the MaxRange Character
                Character maxRange = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        //if maxrange init was an ally
                        if (maxRange.team == team) {
                            maxRange = temp;
                        }
                        else//if it wasnt an ally
                            if (temp.Range > maxRange.Range)
                            maxRange = temp;
                        else if(temp.Range == maxRange.Range)
                        {
                            //if temp PD is == to max PD then select the closest of the 2
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, maxRange.transform.position))
                                maxRange = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (maxRange.team == team)
                    maxRange = null;
                target = maxRange;
                break;

            case (int)targetList.LowestRangeEnemy:
                //initially assume that this is the MaxRange Character
                Character minRange = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        //if minrange init was actually an ally
                        if (minRange.team == team) {
                            minRange = temp;
                        }
                        else
                        //if it wasn;t an ally
                        if (temp.Range < minRange.Range)
                            minRange = temp;
                        else if(temp.Range == minRange.Range)
                        {
                            //if temp PD is == to max PD then select the closest of the 2
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, minRange.transform.position))
                                minRange = temp;
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (minRange.team == team)
                    minRange = null;
                target = minRange;
                break;

            case (int)targetList.HighestHPEnemy:
                //initially assume that this is the MaxHP Character
                Character maxHP = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in different team(enemy)
                    if (temp.team != team) {
                        //if maxHP init was actually an ally
                        if (maxHP.team == team) {
                            maxHP = temp;
                        }
                        //if it wasn't an ally 
                        else//select 
                        if (temp.HP > maxHP.HP)
                            maxHP = temp;
                        //if there's a draw select the closest of the 2
                        else if (temp.HP == maxHP.HP) {
                            //if temp HP is == to max HP then select the closest of the 2
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, maxHP.transform.position))
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
                        //if minHP init was actually an ally
                        if (minHP.team == team) {
                            minHP = temp;
                        }
                        //if it wasn't an ally 
                        else {
                            if (temp.HP < minHP.HP) {
                                minHP = temp;
                            }
                            //if there's a draw select the closest of the 2
                            else if (temp.HP == minHP.HP) {
                                //if temp HP is == to min HP then select the closest of the 2
                                if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, minHP.transform.position)) {
                                    minHP = temp;
                                }
                            }
                        }
                    }
                }
                //if there's only allies remaining target nothing
                if (minHP.team == team) {
                    minHP = null;
                }
                target = minHP;
                break;


            case (int)targetList.ClosestAlly:
                //initially assume that this is the closest ally
                Character closestAlly = zone.charactersInside[0];
                //if it happens to be self select another
                if (closestAlly == this)
                    closestAlly = zone.charactersInside[1];
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
                //if the only remaining characters are enemies select self
                if(closestAlly.team != team) {
                    closestAlly = this;
                }
                target = closestAlly;
                break;

            case (int)targetList.HighestPDAlly:
                //initially assume that this is the MaxPD Character
                Character maxPDAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team 
                    if (temp.team == team) {
                        //if maxPD init was an enemy select the first ally
                        if (maxPDAlly.team != team) {
                            maxPDAlly = temp;
                        }
                        //if it wasn't an enemy
                        else    //if temp is strictly superior select it
                            if (temp.PD > maxPDAlly.PD)
                            maxPDAlly = temp;
                        else if(temp.PD == maxPDAlly.PD)
                        {
                            //if temp PD is == to max PD and temp isn't itself select closest of the 2. (If it was itself it would obviously be closer so we don't want that)
                            if(temp !=this)
                                if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, maxPDAlly.transform.position))
                                maxPDAlly = temp;
                            //if temp is itself simply do nothing and keep maxPDAlly
                        }
                    }
                }
                //if there's only enemies remaining target nothing
                if (maxPDAlly.team != team)
                    maxPDAlly = null;
                target = maxPDAlly;
                break;

            case (int)targetList.LowestPDAlly:
                //initially assume that this is the MaxPD Character
                Character minPDAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and not itself
                    if (temp.team == team) {
                        //minPDAlly.team != team is done in case the MaxPD init was actually an enemy
                        if (minPDAlly.team != team) {
                            minPDAlly = temp;
                        }
                        else
                            if (temp.PD < minPDAlly.PD)
                            minPDAlly = temp;
                        else if(temp.PD == minPDAlly.PD)
                        {
                            if(temp!=this)
                                if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, minPDAlly.transform.position))
                                    minPDAlly = temp;
                        }
                    }
                }
                //if there's only enemies remaining target nothing
                if (minPDAlly.team != team)
                    minPDAlly = null;
                target = minPDAlly;
                break;

            case (int)targetList.HighestMDAlly:
                //initially assume that this is the MaxMD Character
                Character maxMDAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and is not itself
                    if (temp.team == team) {
                        //maxMD.team == team is done in case the MaxMD init was actually an enemy
                        if (maxMDAlly.team != team) {
                            maxMDAlly = temp;
                        }
                        else
                            if (temp.MD > maxMDAlly.MD)
                            maxMDAlly = temp;
                        else if(temp.MD == maxMDAlly.MD)
                        {
                            if(temp!=this)
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, maxMDAlly.transform.position))
                                maxMDAlly = temp;
                        }
                    }
                }
                //if there's only enemies remaining target nothing
                if (maxMDAlly.team != team)
                    maxMDAlly = null;
                target = maxMDAlly;
                break;

            case (int)targetList.LowestMDAlly:
                //initially assume that this is the MaxMD Character
                Character minMDAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team 
                    if (temp.team == team) {
                        //minMDAlly.team != team is done in case the MaxMD init was actually an enemy
                        if (minMDAlly.team != team) {
                            minMDAlly = temp;
                        }
                        else
                        if (temp.MD < minMDAlly.MD)
                            minMDAlly = temp;
                        else if(temp.MD == minMDAlly.MD)
                        {
                            if(temp != this)
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, minMDAlly.transform.position))
                                minMDAlly = temp;
                        }
                    }
                }
                //if there's only enemies remaining target nothing
                if (minMDAlly.team != team)
                    minMDAlly = null;
                target = minMDAlly;
                break;

            case (int)targetList.HighestINFAlly:
                //initially assume that this is the MaxINF Character
                Character maxINFAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and is not itself
                    if (temp.team == team) {
                        //maxINF.team == team is done in case the MaxINF init was actually an enemy
                        if (maxINFAlly.team != team) {
                            maxINFAlly = temp;
                        }
                        else
                            if (temp.INF > maxINFAlly.INF)
                            maxINFAlly = temp;
                        else if (temp.INF == maxINFAlly.INF) {
                            if (temp != this)
                                if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, maxINFAlly.transform.position))
                                    maxINFAlly = temp;
                        }
                    }
                }
                //if there's only enemies remaining target nothing
                if (maxINFAlly.team != team)
                    maxINFAlly = null;
                target = maxINFAlly;
                break;

            case (int)targetList.LowestINFAlly:
                //initially assume that this is the MaxINF Character
                Character minINFAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and not itself
                    if (temp.team == team) {
                        if (minINFAlly.team != team) {
                            minINFAlly = temp;
                        }
                        else
                        //minINFAlly.team != team is done in case the MaxINF init was actually an enemy
                        if (temp.INF < minINFAlly.INF)
                            minINFAlly = temp;
                        else if (temp.INF == minINFAlly.INF) {
                            if (temp != this)
                                if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, minINFAlly.transform.position))
                                    minINFAlly = temp;
                        }
                    }
                }
                //if there's only enemies remaining target nothing
                if (minINFAlly.team != team)
                    minINFAlly = null;
                target = minINFAlly;
                break;

            case (int)targetList.HighestASAlly:
                //initially assume that this is the MaxAS Character
                Character maxASAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and is not itself
                    if (temp.team == team) {
                        //maxAS.team == team is done in case the MaxAS init was actually an enemy
                        if (maxASAlly.team != team) {
                            maxASAlly = temp;
                        }
                        else
                        if (temp.AS > maxASAlly.AS)
                            maxASAlly = temp;
                        else if (temp.AS == maxASAlly.AS) 
                        {
                            if(temp !=this)
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, maxASAlly.transform.position))
                                maxASAlly = temp;
                        }
                    }
                }
                //if there's only enemies remaining target nothing
                if (maxASAlly.team != team)
                    maxASAlly = null;
                target = maxASAlly;
                break;

            case (int)targetList.LowestASAlly:
                //initially assume that this is the MaxAS Character
                Character minASAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and not itself
                    if (temp.team == team) {
                        //minASAlly.team != team is done in case the MaxAS init was actually an enemy
                        if (minASAlly.team != team) {
                            minASAlly = temp;
                        }
                        else
                        if (temp.AS < minASAlly.AS)
                            minASAlly = temp;
                        else if(temp.AS == minASAlly.AS)
                        {
                            if(temp != this)
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, minASAlly.transform.position))
                                minASAlly = temp;
                        }
                    }
                }
                //if there's only enemies remaining target nothing
                if (minASAlly.team != team)
                    minASAlly = null;
                target = minASAlly;
                break;

            case (int)targetList.HighestMSAlly:
                //initially assume that this is the MaxMS Character
                Character maxMSAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and is not itself
                    if (temp.team == team) {
                        //maxMS.team == team is done in case the MaxMS init was actually an enemy
                        if (maxMSAlly.team != team) {
                            maxMSAlly = temp;
                        }
                        else
                        if (temp.MS > maxMSAlly.MS)
                            maxMSAlly = temp;
                        else if (temp.MS == maxMSAlly.MS)
                        {
                            if(temp != this)
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, maxMSAlly.transform.position))
                                maxMSAlly = temp;
                        }
                    }
                }
                //if there's only enemies remaining target nothing
                if (maxMSAlly.team != team)
                    maxMSAlly = null;
                target = maxMSAlly;
                break;

            case (int)targetList.LowestMSAlly:
                //initially assume that this is the MaxMS Character
                Character minMSAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and not itself
                    if (temp.team == team) {
                        //minMSAlly.team != team is done in case the MaxMS init was actually an enemy
                        if (minMSAlly.team != team) {
                            minMSAlly = temp;
                        }
                        else
                        if (temp.MS < minMSAlly.MS)
                            minMSAlly = temp;
                        else if(temp.MS == minMSAlly.MS && temp != this)
                        {
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, minMSAlly.transform.position))
                                minMSAlly = temp;
                        }
                    }
                }
                //if there's only enemies remaining target nothing
                if (minMSAlly.team != team)
                    minMSAlly = null;
                target = minMSAlly;
                break;

            case (int)targetList.HighestRangeAlly:
                //initially assume that this is the MaxRange Character
                Character maxRangeAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and is not itself
                    if (temp.team == team) {
                        //maxRange.team == team is done in case the MaxRange init was actually an enemy
                        if (maxRangeAlly.team != team)
                            maxRangeAlly = temp;
                        else
                        if (temp.Range > maxRangeAlly.Range)
                            maxRangeAlly = temp;
                        else if(temp.Range == maxRangeAlly.Range && temp != this)
                        {
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, maxRangeAlly.transform.position))
                                maxRangeAlly = temp;
                        }
                    }
                }
                //if there's only enemies remaining target nothing
                if (maxRangeAlly.team != team)
                    maxRangeAlly = null;
                target = maxRangeAlly;
                break;

            case (int)targetList.LowestRangeAlly:
                //initially assume that this is the MaxRange Character
                Character minRangeAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and not itself
                    if (temp.team == team) {
                        if (minRangeAlly.team != team) {
                            minRangeAlly = temp;
                        }
                        else
                        //minRangeAlly.team != team is done in case the MaxRange init was actually an enemy
                        if (temp.Range < minRangeAlly.Range)
                            minRangeAlly = temp;
                        else if (temp.Range == minRangeAlly.Range && temp != this) 
                        {
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, minRangeAlly.transform.position))
                                minRangeAlly = temp;
                        }
                    }
                }
                //if there's only enemies remaining target nothing
                if (minRangeAlly.team != team)
                    minRangeAlly = null;
                target = minRangeAlly;
                break;

            case (int)targetList.HighestHPAlly:
                //initially assume that this is the maxHPAlly Character
                Character maxHPAlly = zone.charactersInside[0];
                foreach (Character temp in zone.charactersInside) {
                    //if temp in same team and not self
                    if (temp.team == team) {
                        //maxHPAlly.team != team is done in case the maxHPAlly init was actually an enemy
                        if (maxHPAlly.team != team)
                            maxHPAlly = temp;
                        else
                        if (temp.HP > maxHPAlly.HP)
                            maxHPAlly = temp;
                        else if(temp.HP == maxHPAlly.HP && temp !=this)
                        {
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, maxHPAlly.transform.position))
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
                    if (temp.team == team && temp) {
                        //minHPAlly.team != team is done in case the minHPAlly init was actually an enemy
                        if (minHPAlly.team != team)
                            minHPAlly = temp;
                        else
                        if (temp.HP < minHPAlly.HP)
                            minHPAlly = temp;
                        else if(temp.HP == minHPAlly.HP && temp != this)
                        {
                            if (Vector2.Distance(transform.position, temp.transform.position) < Vector2.Distance(transform.position, minHPAlly.transform.position))
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
        return target;
    }

    //idea to incorporate animation
    //when attack is supposed to happen instead of doing the damage and stuff call another function that 
    //runs the attack animation then on the specified frame deal the damage and stuff.
    //look into animation events
    private void attack() {
        selectTarget(attackTargetStrategy);
        //deal Damage when target is within range and Attack is available and player can Attack and the target is alive
        if (AtkAvailable && canAttack && Vector2.Distance(target.transform.position, transform.position) <= Range && target.alive) {
            //if character is ranged use projectile
            if (Range>=1.5f) {
                GameObject temp = Instantiate(projectile,transform.position,transform.rotation);
                Projectile instantiatedProjectile = temp.GetComponent<Projectile>();
                instantiatedProjectile.shooter = this;
                instantiatedProjectile.DMG = PD;
                //THE SPEED IS SET IN THE PROJECTILE OBJECT ITSELF
                //instantiatedProjectile.speed = 4;       //can make this an attribute to character
                instantiatedProjectile.lifetime = 2;    //can make this an attribute to character
                instantiatedProjectile.target = target;
                instantiatedProjectile.LS = LS;
            }
            else {
                damage(target, PD, true);
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
    //private void OnMouseDown() {
    //    Debug.Log(name + "Clicked");
    //    //prevent clijcking through UI
    //    //if (IsPointerOverGameObject()) {
    //    //    return;
    //    //}
    //    //to start mouseClickedNotHeld Function
    //    click = true;
    //}

    private void customMouseDown() {
        int layerMask = LayerMask.GetMask("Characters");
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider != null && hit.collider.tag == "Character") {
                //Debug.Log("Character" + hit.collider.name);
                hit.collider.GetComponent<Character>().click = true;
            }
            else {
                //Debug.Log("Blocking raycast" + hit.collider.name);
            }
        }
    }
    private float mouseHoldDuration = 0;
    public bool click = false;
    //if held on character
    [HideInInspector]public bool held;
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
                    drawIndicators();
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

    public void damage(Character victim,float damageAmount,bool applyLS) {
        victim.HP-=damageAmount;
        if (applyLS)
            HP += damageAmount * LS;
        if (victim.HP <= 0) 
            kill(victim);
        //if summoned also increase the summoner's total damage
        if (summoned) 
            summoner.totalDamage+= damageAmount;
        totalDamage += damageAmount;
    }
    //increase killer's kill stats and xp
    public void kill(Character victim) {
        if (summoned) {
            summoner.totalKills++;
            summoner.killsLastFrame++;
            //level progress will depend on victim's level the equation is open to changing
            increasePartyXP(victim.level);
        }
        else {
            totalKills++;
            killsLastFrame++;
            //level progress will depend on victim's level the equation is open to changing
            increasePartyXP(victim.level);
        }

        //add gold
        if(this.team == (int)teamList.Player) {
            uiManager.playerParty.gold += 13 + victim.level * 2;
        }
    }
    //Shares XP TO ALL ACTIVE PLAYERPARTY MEMBERS
    private void increasePartyXP(int level) {
        //only applies if the caller is a player Character
        if (this.team != (int)teamList.Player)
            return;
        int activeCharacters=0;
        //counts how many active characters
        foreach(Transform child in UIManager.singleton.playerParty.transform) {
            if(child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                if(temp.gameObject.activeSelf && temp.alive)
                    activeCharacters++;
            }
        }
        //give xp evenly split on active characters
        foreach (Transform child in UIManager.singleton.playerParty.transform) {
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                if (temp.gameObject.activeSelf && temp.alive)
                    temp.xpProgress += (float)level / activeCharacters;
            }
        }
    }
    private void levelUp() {
        xpProgress -= xpCap;
        level++;
        //maybe give more stats every 10 levels or smthn level cap setup is done in start method as well.
        statPoints++;
        //update xpCap depending on level
        xpCap = level + (level * ((level - 1) / 2));

        //(this only applies to player characters that are not summoned)
        if(!summoned && team == (int)teamList.Player) {
            //increase stats a bit
            HPMax += 5;
            PD += 0.5f;
            AS += 0.02f;
            //heal character by 10% of max HP on level up 
            HP += 0.1f * HPMax;

        }
    }

    //returns wether the character is selected ornot
    public bool getSelected() {
        return uiManager.topStatDisplay.character == this;
    }

    private void drawTargetIndicator() {
        selectTarget(attackTargetStrategy);
        if (getSelected()) {
            indicators.drawTargetLine(transform.position,target.transform.position);
        }
        else {
            indicators.eraseLines();
        }
    }

    public void drawIndicators() {
        drawTargetIndicator();
        //display Range Indicator and ability indicators....
    }
    void FixedUpdate()
    {
        

        handleDeath();
        if (!blind)
            attack();
        cooldown();
        if (!snare)
            movement();
        else //here we're doing else since it might already have a set route and would conitnue moving so we delete it's path
            agent.ResetPath();
        if(!silence)
            doAbilities();
        capHP();
        if (xpProgress >= xpCap)
            levelUp();
        previousMovementState = movementState; // this will be used to see if the movementState changed or not
        resetKillsLastFrame();//always keep me last in update
    }

    private void Update() {
        if(zone == null) {
            zone = uiManager.zone;
            zone.charactersInside.Add(this);
        }
        drawIndicators();
        customMouseDown();
        //this doesn't have to be done on every frame so having it in update instead of fixedupdate is fine
        timeSinceDestinationUpdate += Time.deltaTime;
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
