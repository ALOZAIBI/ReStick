using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

//Creates an FX on all targets within the collider and keeps them on the target

public class SimpleCreateFXOnTargetsWithin : MonoBehaviour
{

    //Holds all the HitFXs created
    [SerializeField] private List<SimpleFX> simpleFX = new List<SimpleFX>();

    //Tells HitFXs to keep going
    private float refreshFrequency = 0.05f;
    private float lastRefresh = 0;

    [SerializeField] private SimpleFX simpleFXPrefab;
    //To know who is considered an enemy or an ally
    public Character caster;

    public bool enemy;
    public bool ally;

    private void FixedUpdate() {

        //Removes All HitFXs from dead Characters
        foreach (SimpleFX simFX in simpleFX) {
            if (!simFX.keepOnTarget.target.activeSelf) {
                Destroy(simFX.gameObject);
                simpleFX.Remove(simFX);
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
                SimpleFX simFX = Instantiate(simpleFXPrefab, character.transform.position, Quaternion.identity);
                simFX.destroyOnLastFrame = false;
                simFX.keepOnTarget.target = character.gameObject;
                simpleFX.Add(simFX);
            }
            if(ally && character.team == caster.team && !characterHasFX(character)) {
                SimpleFX simFX = Instantiate(simpleFXPrefab, character.transform.position, Quaternion.identity);
                simFX.destroyOnLastFrame = false;
                simFX.keepOnTarget.target = character.gameObject;
                simpleFX.Add(simFX);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Character") {
            Character character = collision.gameObject.GetComponent<Character>();
            //If the character has FX on them remove it from the list and endAnimation
            if (characterHasFX(character)) {
                foreach (SimpleFX simFX in simpleFX) {
                    if (simFX.keepOnTarget.target == character.gameObject) {
                        Destroy(simFX.gameObject);
                        simpleFX.Remove(simFX);
                        break;
                    }
                }
            }
        }
    }

    //Checks if character has this FX on them
    private bool characterHasFX(Character character) {
        foreach (SimpleFX hitFX in simpleFX) {
            if (hitFX.keepOnTarget.target == character.gameObject) {
                return true;
            }
        }
        return false;
    }
}
 