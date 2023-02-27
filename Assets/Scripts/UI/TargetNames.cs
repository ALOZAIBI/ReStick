using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNames : MonoBehaviour
{
    public static string getName(int target) {
        switch (target) {
            case (int)Character.targetList.ClosestEnemy:
                return "Closest Enemy";

            case (int)Character.targetList.ClosestAlly:
                return "Closest Ally";

            case (int)Character.targetList.DefaultEnemy:
                return "Default Enemy";

            case (int)Character.targetList.HighestDMGEnemy:
                return "Highest DMG Enemy";
        }
        return "";
    }
}
