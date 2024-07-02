using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Buffs you depending on the number of characters around you
public class BuffOnCharactersAround : Item
{
    //If it checks for ally
    [SerializeField] private bool ally = true;
    //If it checks for enemy
    [SerializeField] private bool enemy = true;

    //The range in which it checks
    [SerializeField] private int withinRanged = 5;

    [SerializeField] private Buff buffObject;

    public float buffPD;
    public float buffMD;
    public float buffINF;
    public float buffHP;
    public float buffAS;
    public float buffCDR;
    public float buffMS;
    public float buffRange;
    public float buffLS;
    public float buffsize;

    [SerializeField] private float timeSinceLastBuff;
    [SerializeField] private float timeBetweenBuffs;

    public LayerMask mask;

    private void Start() {
        mask = LayerMask.GetMask("Characters");
    }
    public override void reset() {
        timeSinceLastBuff = 0;
    }
    public override void continuous() {
        timeSinceLastBuff += Time.fixedDeltaTime;
        if (timeSinceLastBuff >= timeBetweenBuffs) {
            timeSinceLastBuff = 0;

            //Checks how many characters around are in range
            int count = 0;
            int allyCount = 0;
            int enemyCount = 0;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(character.transform.position, withinRanged, mask);
            foreach(Collider2D col in colliders) {
                Character c = col.GetComponent<Character>();
                //If if is self it doesn't count
                if(c == character) {
                    continue;
                }
                if(c.team == character.team) {
                    allyCount++;
                }
                else {
                    enemyCount++;
                }
            }
            if(ally) {
                count += allyCount;
            }
            if(enemy) {
                count += enemyCount;
            }

            //TODO: currently having duplicates of this object doesn't stack

            string code = itemName + character.name;


            //We are chjecking if the buff already exists, this way we keep the buff going withou creatintg a new instance(which can lead to having duplicates of the buff for a few frames)
            Buff buff = null;
            //Find buff in character with the same code
            foreach(Buff b in character.buffs) {
                if(b.code == code) {
                    buff = b;
                    buff.removeBuffAppliedStats(character,false);
                    Debug.Log("Found buff");
                    //We break since we don't want to continuously remove the same buff
                    break;
                }

            }

            //if the buff is not found, it creates a new one
            if(buff == null) {
                buff = Instantiate(buffObject).GetComponent<Buff>();
                Debug.Log("Created new buff");
            }

            //Sets the stats of the buff
            buff.PD = buffPD * count;
            buff.MD = buffMD * count;
            buff.INF = buffINF * count;
            //If the amount of HP given now is more than before we should increase the HP (Not always (we don't want the item to be regenning health every proc))
            //TODO: it still heals more than it should(We have 10 characters arond, then we have 11 characters around, it should only heal the difference but it isn't currently)
            bool shouldIncreaseHP = false;
            if(buffHP * count > buff.HP) {
                buff.HP = buffHP * count;
                shouldIncreaseHP = true;
            }
            buff.AS = buffAS * count;
            buff.CDR = buffCDR * count;
            buff.MS = buffMS * count;
            buff.Range = buffRange * count;
            buff.LS = buffLS * count;
            buff.size = buffsize * count;

            buff.caster = character;
            buff.target = character;
            //code used to identify duplicate buffs to refresh duration when a new stack is added
            buff.code = itemName + character.name;


            buff.duration = timeBetweenBuffs+2;

            buff.applyBuff(shouldIncreaseHP);

            if(count >0)
                startItemActivation();
        }
    }
}
