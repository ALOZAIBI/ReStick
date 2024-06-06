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

    private void Start() {
        rarity = (int)Rarities;
    }

    public void OnValidate() {
        rarity = (int)Rarities;
    }

    //Applies stats
    //This is only done once then followed by a save
    public void applyStats(Character c) {
        character = c;
        character.PD += PD;
        character.MD += MD;
        character.HP += HP;
        character.HPMax += HP;
        character.AS += AS;
        character.CDR += CDR;
        character.MS += MS;
        character.Range += Range;
        character.LS += LS;
        character.gameObject.transform.localScale += new Vector3(size, size, size);
    }

    public void removeStats() {
        if(character != null) {
            character.PD -= PD;
            character.MD -= MD;
            character.HP -= HP;
            character.HPMax -= HP;
            character.AS -= AS;
            character.CDR -= CDR;
            character.MS -= MS;
            character.Range -= Range;
            character.LS -= LS;
            character.gameObject.transform.localScale -= new Vector3(size, size, size);

            character = null;
        }
    }


    //To do when zone starts
    public virtual void onZoneStart() {

    }

    public virtual void onDeath() {

    }

    public virtual void onKill() {

    }

    public virtual void afterAbility() {

    }

    public virtual void afterAttack() {

    }

    public virtual void continuous() {

    }
}
