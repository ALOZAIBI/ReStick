using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//so that abilities in ability factory can be colored on start
[DefaultExecutionOrder(-100)]
public class ColorPalette : MonoBehaviour
{
    //this script is on an empty in the do not destroys
    public static ColorPalette singleton;
    //default color of text
    public Color defaultColor;
    //color of text when gaining a buff (Green)
    public Color buff;
    //color of text when gaining a debuff(red)
    public Color debuff;
    
    public Color allyHealthBar;
    public Color enemyHealthBar;

    public Color commonRarity;
    public Color rareRarity;
    public Color epicRarity;
    public Color legendaryRarity;

    public Color autoAttackIndicator;
    public Color magicAbilityIndicator;
    public Color buffAbilityIndicator;
    public Color debuffAbilityIndicator;
    public Color selfBuffAbilityIndicator;
    public Color physicalAbilityIndicator;
    public Color healAbilityIndicator;
    public Color healthDamageIndicator;//stuff that deals damage based on teh caster's health (for tanks)
    public Color crowdControlAbilityIndicator;
    public Color specialAbilityIndicator;
    public Color otherAbilityIndicator;


    public Color xpMisc;
    public Color goldMisc;
    public Color hpMisc;
    public Color lifeMisc;

    //public static Color neutralHealthBar;


    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        //Debug.Log(allyHealthBar);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Color getRarityColor(int rarity) {
        switch (rarity) {
            case 0:
                return commonRarity;
            case 1:
                return rareRarity;
            case 2:
                return epicRarity;
            case 3:
                return legendaryRarity;
            default:
                return defaultColor;
        }
    }

    public Color getTypeColor(int val) {
        switch (val) {
            case (int)Ability.AbilityTypeList.MagicDamage:
                return magicAbilityIndicator;
            case (int)Ability.AbilityTypeList.Buff:
                return buffAbilityIndicator;
            case (int)Ability.AbilityTypeList.Debuff:
                return debuffAbilityIndicator;
            case (int)Ability.AbilityTypeList.SelfBuff:
                return selfBuffAbilityIndicator;
            case (int)Ability.AbilityTypeList.PhysicalDamage:
                return physicalAbilityIndicator;
            case (int)Ability.AbilityTypeList.Heal:
                return healAbilityIndicator;
            case (int)Ability.AbilityTypeList.HealthDamage:
                return healthDamageIndicator;
            case (int)Ability.AbilityTypeList.CrowdControl:
                return crowdControlAbilityIndicator;
            case (int)Ability.AbilityTypeList.Special:
                return specialAbilityIndicator;
            case (int)Ability.AbilityTypeList.Other:
                return otherAbilityIndicator;
            default:
                return defaultColor;
        }
    }

}
