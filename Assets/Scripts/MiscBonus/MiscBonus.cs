using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscBonus : MonoBehaviour
{
    public float bonusAmt;

	//Bonus can be either HP or XP
	public enum myType {
        HP,
        XP,
        Gold,
        Life
    }

    public myType type;

    public string getType() {
        return type.ToString();
    }
}
