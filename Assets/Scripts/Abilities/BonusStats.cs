using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusStats : MonoBehaviour
{
    public float DMG=0;
    public float HP=0;
    public float AS=0;
    public float MS=0;
    public float Range=0;
    public float LS=0;

    public Character character;
    public bool applied = false;

    //applies stats once ( if applystats is called again applied would be true so nothing would happen)
    public void applyStats() {
        if (!applied) {
            character.DMG += DMG;
            character.HP += HP;
            character.HPMax += HP;
            character.AS += AS;
            character.MS += MS;
            character.Range += Range;
            character.LS += LS;
            applied = true;
        }

    }
}
