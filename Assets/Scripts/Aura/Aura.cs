using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Aura : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Character caster;

    public float amt;
    //wether this targets enemy otr ally or both 
    public bool enemy;
    public bool ally;

    public bool damage;
    public bool heal;

    public Buff buff;

    public string castingAbilityName;

    //We will use this list in the DashALotThenSheath
    public bool saveCharacterInAura;
    public List<Character> charactersInAura = new List<Character>();

    [SerializeField]private bool grow = true;
    //Time to grow and/or time to set Alpha to 1
    [SerializeField]private float timeToSetupAura = 0.5f;
    [SerializeField]private float time = 0;
    [SerializeField]private float sizeTarget;
    const int INITSIZE = 5;

    protected void Start() {
        if (grow) {
            //Saves size target
            sizeTarget = transform.localScale.x;
            //Sets the size to INITSIZE
            transform.localScale = new Vector3(INITSIZE, INITSIZE, INITSIZE);
        }
    }

    //returns true if no buff on character and if there is the sameBuff on Character simply refresh it's duration
    public bool buffNotOnCharacter(Character victim) {
        try {
            foreach (Buff temp in victim.buffs) {
                //if buff is already applied refresh it's duration
                if (temp.code == castingAbilityName + caster.name) {
                    temp.durationRemaining = buff.duration;
                    return false;
                }
            }
        }
        catch { return true; };
        return true;
    }
    /// <summary>
    /// Creates an instance of the buff and applies it on the victim
    /// </summary>
    /// <param name="victim"></param>
    public void applyBuff(Character victim) {
        if (buff != null) {
            if (buffNotOnCharacter(victim)) {
                //create an instance of the buff
                Buff temp = Instantiate(buff);
                temp.gameObject.SetActive(true);
                temp.target = victim;
                temp.applyBuff();
                Debug.Log("IOssue");
            }
        }
    }

    private void FixedUpdate() {
        //If the character is silenced or dead then destroy the aura
        if (caster == null || caster.silence>0 || !caster.alive) {
            Destroy(gameObject);
        }

        //Grow the aura to targetsize over timeToGrow seconds
        if (time < timeToSetupAura) {
            time += Time.fixedDeltaTime;
            if (grow) {
                float scaleAmount = Mathf.Lerp(INITSIZE, sizeTarget, time / timeToSetupAura);
                transform.localScale = new Vector3(scaleAmount, scaleAmount, scaleAmount);
            }
            //Set the alpha to 1 over timeToGrow seconds
            if (spriteRenderer != null) {
                spriteRenderer.SetAlpha(Mathf.Lerp(0, 1, time / timeToSetupAura));
            }

        }
        

    }

}
