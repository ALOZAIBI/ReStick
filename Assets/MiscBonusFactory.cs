using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscBonusFactory : MonoBehaviour
{
    [SerializeField] private MiscBonus emptyBonus;

    [SerializeField] private int goldMin;
    [SerializeField] private int goldMax;

    [SerializeField] private int xPMin;
    [SerializeField] private int xPMax;

    [SerializeField] private int healthMin;
    [SerializeField] private int healthMax;

    [SerializeField] private int lifeMin;
    [SerializeField] private int lifeMax;

    //Currently this instantiates a miscBonus, we'll need to do garbage collection on this
    public MiscBonus randomMiscBonus() {
        MiscBonus bonus = Instantiate(emptyBonus);

        int randomType = Random.Range(0, 4);

        switch (randomType) {
            case 0:
                bonus.type = MiscBonus.myType.HP;
                bonus.bonusAmt = Random.Range(healthMin, healthMax);
                break;
            case 1:
                bonus.type = MiscBonus.myType.XP;
                bonus.bonusAmt = Random.Range(xPMin, xPMax);
                break;
            case 2:
                bonus.type = MiscBonus.myType.Gold;
                bonus.bonusAmt = Random.Range(goldMin, goldMax);
                break;
            case 3:
                bonus.type = MiscBonus.myType.Life;
                bonus.bonusAmt = Random.Range(lifeMin, lifeMax);
                break;
            default:
                break;
        }


        return bonus;

    }
}
