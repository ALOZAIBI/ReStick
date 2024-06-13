using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//After using an ability your next basic attack launches a projectile that deals damage to the target
public class Sheen : Item
{
    [SerializeField] private int maxStacks;
    [SerializeField] private int currentStacks;
    [SerializeField] private float PDScaling;
    [SerializeField] private float MDScaling;

    [SerializeField] private HitFX sheenFx;
    [SerializeField] private List<HitFX> listOfsheenFXs;

    [SerializeField] private GameObject projectileObject;
    public override void afterAbility() {
        if(currentStacks < maxStacks) {
            Debug.Log("AAAAAAAAAFTEEEEEEER ABILIT");
            currentStacks++;
            //Create a sheen FX
            HitFX fx = Instantiate(sheenFx, character.transform.position, character.transform.rotation);
            fx.keepOnTarget.target = character.gameObject;
            fx.creator = this.gameObject;
            //Add the sheen FX to the list of FXs
            listOfsheenFXs.Add(fx);
        }
    }

    //Do the damage
    public override void afterAttack() {
        //After one attack end the animation of one sheen FX
        if(listOfsheenFXs.Count > 0) {
            Debug.Log("AAAAAAAAAFTEEEEEEER ATTTTTTAAAAACK");
            HitFX fx = listOfsheenFXs[0];
            fx.playEndAnimation();
            listOfsheenFXs.Remove(fx);
        }

    }

    public override void afterAnimation() {
        //After the animation is done create the projectile
        createProjectile();
        //After creating the projectile remove a stack
        currentStacks--;
        Debug.Log("AAAAAAAAAFTEEEEEEER ANIMAT");
    }

    private void createProjectile() {

        startItemActivation();
        //creates the projectile
        GameObject objProjectile = Instantiate(projectileObject, character.transform.position, character.transform.rotation);
        Projectile projectile = objProjectile.GetComponent<Projectile>();
        //sets the shooter to be the caster of this ability
        projectile.shooter = character;
        //sets the damage amount 
        projectile.DMG = character.PD * PDScaling + character.MD * MDScaling;

        character.selectTarget(character.attackTargetStrategy);

        projectile.target = character.target;
        projectile.setAngle();
        //tells it this abilityName
        projectile.castingAbilityName = itemName;
    }

    //Reset
    public override void reset() {
        listOfsheenFXs.Clear();
        currentStacks = 0;

    }

}
