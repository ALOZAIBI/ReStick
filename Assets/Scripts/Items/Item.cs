using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    //The current holder of this item
    //Set in initroundstart
    public Character character;

    public string itemName;
    public string description;

    //The stats that this item will give
    public float PD;
    public float MD;
    public float INF;
    public float HP;
    public float AS;
    public float CDR;
    public float MS;
    public float Range;
    public float LS;
    public float size;

    public enum RaritiesList {
        Common,
        Rare,
        Epic,
        Legendary
    }
    [SerializeField] public RaritiesList Rarities;
    public int rarity;

    //Determines the color of the item/what the character will look like
    //We are setting this to the ability equivalent so that identical types will have teh same value in order to easily use the ColorPalette function
    public enum ItemTypesList { 
        PhysicalDamage = Ability.AbilityTypeList.PhysicalDamage,
        MagicDamage = Ability.AbilityTypeList.MagicDamage,
        Influence = Ability.AbilityTypeList.Buff,
        Heal = Ability.AbilityTypeList.Heal,//Heal like on kill heal
        Health = Ability.AbilityTypeList.HealthDamage,//Health stat
        SelfBuff = Ability.AbilityTypeList.SelfBuff,
        Special = Ability.AbilityTypeList.Special,//Summons/revives etc..
        Other = Ability.AbilityTypeList.Other//Other stats
    }
    [SerializeField] public ItemTypesList itemTypesList;

    //Used for the color of the item in item Display
    public int type = (int)ItemTypesList.Other;



    //Used to choose the icon of the item
    //Used to choose the look of the character
    public Archetype.ArchetypeList archetypePrimary;

    public Archetype.ArchetypeList archetypeSecondary;

    private void Start() {
        rarity = (int)Rarities;
        type = (int)itemTypesList;
    }

    public void OnValidate() {
        rarity = (int)Rarities;
        type = (int)itemTypesList;
    }

    //Applies stats
    //This is only done once then followed by a save
    public void applyStats(Character c) {
        character = c;
        character.PD += PD;
        character.MD += MD;
        character.INF += INF;
        character.HP += HP;
        character.HPMax += HP;
        character.AS += AS;
        character.CDR += CDR;
        character.MS += MS;
        character.Range += Range;
        character.LS += LS;
        character.gameObject.transform.localScale += new Vector3(size, size, size);

        //applyArchetypeLook();
        character.applyArchetypeLook();
    }

    public void removeStats() {
        if(character != null) {
            character.PD -= PD;
            character.MD -= MD;
            character.INF -= INF;
            character.HP -= HP;
            character.HPMax -= HP;
            character.AS -= AS;
            character.CDR -= CDR;
            character.MS -= MS;
            character.Range -= Range;
            character.LS -= LS;
            character.gameObject.transform.localScale -= new Vector3(size, size, size);

            character.applyArchetypeLook();

            character = null;
        }
    }

    protected void startItemActivation() {
        //Plays the UI Activation animation, if this is the character currently selected
        if (character == UIManager.singleton.characterInfoScreen.character) {
            //Find the index of the ability in the character's ability list
            for (int i = 0; i < character.items.Count; i++) {
                if (character.items[i] == this) {
                    UIManager.singleton.characterInfoScreen.displayItemActivation(i);
                    break;
                }
            }
        }
    }

    //To do when zone starts
    public virtual void onZoneStart() {

    }
    //Returns true if the character holding this item remains alive (Revived)
    public virtual bool onDeath(Character killer=null) {
        return false;
    }

    public virtual void onKill() {

    }

    public virtual void onTakeDamage(Character theDamager,float damageAmount) {

    }
    public virtual void afterAbility() {

    }


    public virtual void afterAttack() {

    }

    public virtual void continuous() {

    }

    //For example in sheen, afterAttack will trigger an animation, after thaat animation is done we'll do the effect of sheen
    public virtual void afterAnimation() {

    }

    //This needs to be done in some cases. Take this for example
    //We are cloning/ so sheen is cloned but we want the clone to have the list of hitFXs reset
    public virtual void reset() {

    }
}
