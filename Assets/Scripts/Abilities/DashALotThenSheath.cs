using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashALotThenSheath : Ability {
    //roots ALL(including allies) characters around the caster, then dash towards all enemies then back to the original position, and deal damage to all enemies hit

    private Aura aura;
    [SerializeField]private List<Character> enemiesHit = new List<Character>();
    [SerializeField]private Character toDashTo;
    public override bool doAbility() {
        bool done = false;
        if (available && step == 0&& character.selectTarget((int)Character.TargetList.ClosestEnemy, rangeAbility) && canUseDash()) {
            Debug.Log("Dashing alot casted");
            calculateAmt();
            character.currentDashingAbility = this;
            playAnimation("castRaise");
            done = true;
        }
        else
        //This step checks if all enies have been hit
        if(available && step == 1 && canUseDash()) {
            calculateAmt();
            //To end the dash animation from step 2 before perhaps redoing it to go to step 3 or 2 again
            character.animationManager.keepAbilityMakeInterupttible();
            executeAbility();
            done = true;
        }
        //Dashes to all enemies that haven't been hit (goes back to step 1 after each completed dash)
        if(available && step == 2 && canUseDash()) {
            calculateAmt();
            playAnimation("castDash");
            executeAbility();
            done = true;
        }//Dashes back to the original position and deals damage and start cooldown
        if(available && step == 3 && canUseDash()) {
            calculateAmt();
            playAnimation("castDash");
            executeAbility();
            done = true;
        }
        return done;
    }

    public override void executeAbility() {
        switch (step) {
            //First step creates the aura and disables the pathfinding agent
            case 0:
            GameObject temp = Instantiate(prefabObject);
                //Makes the aura slightly bigger than the range that triggers the aura to grow. 
            temp.transform.localScale = new Vector3((rangeAbility+1) * 2, (rangeAbility+1) * 2, (rangeAbility + 1) * 2);
            temp.transform.position = character.transform.position;
            aura = temp.GetComponent<Aura>();
            aura.caster = character; 
            if(buffPrefab == null)
                throw new System.Exception("NO BUFF PREFAB");
            Buff buff = createBuff();
                //Since I know this will only be used for rooting I don't need to do all the other BS
                {
                    buff.snare = true;
                    buff.caster = character;
                    buff.target = character.target;
                    //The buff will be reapplied while the aura is on so the number doesn't matter much
                    buff.duration = 0.5f;
                    buff.code = abilityName + character.name;
                }

            aura.buff = buff;
            aura.castingAbilityName = abilityName;
            character.agent.enabled = false;
            step++;
                break;

            //Checking if all enemies have been hit
            case 1:
                
                bool allEnemiesHit = true;
                bool thereIsSomethingInAura=false;
                //Checks if there is an enemy that is still alive that hasn't been hit yet. (Have to check if there are even any enemies in the aura)
                foreach (Character charInAura in aura.charactersInAura) {
                    if(aura.charactersInAura.Count > 0) {
                        thereIsSomethingInAura = true;
                    }
                    if (charInAura.team != character.team && charInAura.alive) {
                        if (!enemiesHit.Contains(charInAura)) {
                            allEnemiesHit = false;
                            //ends the forloop
                            break;
                        }
                    }
                }
                //If there is nothing in the aura remain in this step (Leave the case so that we go back to this step when do ability is called again)
                if(!thereIsSomethingInAura) {
                    break;
                }


                if (!allEnemiesHit) {
                    step = 2;
                    //Selects a random enemy that hasn't been hit yet
                    bool foundEnemy = false;
                    while (!foundEnemy) {
                        int rand = Random.Range(0, aura.charactersInAura.Count);

                        if (aura.charactersInAura[rand].team != character.team && aura.charactersInAura[rand].alive) {
                            if (!enemiesHit.Contains(aura.charactersInAura[rand])) {
                                toDashTo = aura.charactersInAura[rand];
                                foundEnemy = true;
                            }
                        }
                    }
                }
                //if all enemies have been hit start dashing back to the original position
                else {
                    step=3;
                }

                break;
            //Dashes to an enemy that hasn't been hit
            case 2:
                //Dash
                character.transform.position = Vector2.MoveTowards(character.transform.position, toDashTo.transform.position, valueAmt.getAmtValueFromName(this,"DashSpeed") * Time.fixedDeltaTime);
                //Once in Range MarkAsHit
                if (Vector2.Distance(character.transform.position, toDashTo.transform.position) < 0.5f) {
                    enemiesHit.Add(toDashTo);
                    //Goes back to checking if all hit
                    step = 1;
                }
                break;

            //Dashes back to the original position and deals damage to all enemiesHit then starts the cooldown
            case 3:
                //Dash to original position (position of the aura)
                character.transform.position = Vector2.MoveTowards(character.transform.position, aura.transform.position, valueAmt.getAmtValueFromName(this, "DashSpeed")/2 * Time.fixedDeltaTime);
                int numOfEnemies = 0;
                foreach (Character charInAura in aura.charactersInAura) {
                    if (charInAura.team != character.team && charInAura.alive) {
                        numOfEnemies++;
                    }
                }
                //Once in Range Do the Damage and End the Aura and stasrt Cooldown
                if (Vector2.Distance(character.transform.position, aura.transform.position) < 0.1f) {
                    //Deals damage to all Alive Enemies Hit
                    foreach (Character victim in enemiesHit) {
                        if (victim.alive) {
                        character.damage(victim, valueAmt.getAmtValueFromName(this, "Damage")/numOfEnemies, 0.5f);
                        applyHitFX(victim);
                        }
                    }

                    //Once all is done you can set the animation to interupttible again
                    character.animationManager.forceStop();
                    startCooldown();
                    Destroy(aura.gameObject);
                    character.agent.enabled = true;
                }
                
                break;
            default:
                character.animationManager.forceStop();
                break;
        }
    }



    public override void updateDescription() {
        if (character == null) {
            description = "Dash around to all nearby enemies then annihilate them all";
        }
        else {
            calculateAmt();
            description = "Dash towards all nearby enemies dealing" + valueAmt.getAmtValueFromName(this, "Damage") + "divided amongst them";
        }
    }

    private void FixedUpdate() {
        cooldown();
    }
}
