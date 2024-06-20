using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

//Creates an FX on all targets within the collider and keeps them on the target
public class CreateFXOnTargetsWithin : MonoBehaviour
{

    //Holds all the HitFXs created
    [SerializeField] private List<HitFX> HitFXs = new List<HitFX>();

    //Tells HitFXs to keep going
    private float refreshFrequency = 0.05f;
    private float lastRefresh = 0;

    [SerializeField] private HitFX hitFXPrefab;
    //To know who is considered an enemy or an ally
    public Character caster;

    public bool enemy;
    public bool ally;

    private void FixedUpdate() {
        if (lastRefresh < refreshFrequency) {
            lastRefresh += Time.fixedDeltaTime;
        }
        if (lastRefresh >= refreshFrequency) {
            //Refresh all HitFXs
            foreach (HitFX hitFX in HitFXs) {
                hitFX.refreshDuration();
            }
            lastRefresh = 0;
        }
        //Ends All HitFXs from dead Characters
        foreach (HitFX hitFX in HitFXs) {
            if (!hitFX.keepOnTarget.target.activeSelf) {
                hitFX.playEndAnimation();
                HitFXs.Remove(hitFX);
                break;
            }
        }
    }
    //im doiong this instead of ontrigger enter because on trigger enter doesn't work on very first frame. read api
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Character") {
            Character character = collision.gameObject.GetComponent<Character>();
            //If the character doesn't have FX on them create one and add it to the list
            if(enemy && character.team!=caster.team && !characterHasFX(character)) {
                HitFX hitFX = Instantiate(hitFXPrefab, character.transform.position, Quaternion.identity);
                hitFX.keepOnTarget.target = character.gameObject;
                HitFXs.Add(hitFX);
            }
            if(ally && character.team == caster.team && !characterHasFX(character)) {
                HitFX hitFX = Instantiate(hitFXPrefab, character.transform.position, Quaternion.identity);
                hitFX.keepOnTarget.target = character.gameObject;
                HitFXs.Add(hitFX);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Character") {
            Character character = collision.gameObject.GetComponent<Character>();
            //If the character has FX on them remove it from the list and endAnimation
            if (characterHasFX(character)) {
                foreach (HitFX hitFX in HitFXs) {
                    if (hitFX.keepOnTarget.target == character.gameObject) {
                        hitFX.playEndAnimation();
                        HitFXs.Remove(hitFX);
                        break;
                    }
                }
            }
        }
    }

    //Checks if character has this FX on them
    private bool characterHasFX(Character character) {
        foreach (HitFX hitFX in HitFXs) {
            if (hitFX.keepOnTarget.target == character.gameObject) {
                return true;
            }
        }
        return false;
    }
}
