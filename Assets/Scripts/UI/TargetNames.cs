using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNames : MonoBehaviour
{
    public static string getName(int target) {
        switch (target) {
            case (int)Character.targetList.DefaultEnemy:
                return "Default Enemy";

            case (int)Character.targetList.ClosestEnemy:
                return "Closest Enemy";

            case (int)Character.targetList.HighestDMGEnemy:
                return "Highest DMG Enemy";

            case (int)Character.targetList.LowestDMGEnemy:
                return "Lowest DMG Enemy";

            case (int)Character.targetList.HighestHPEnemy:
                return "Highest HP Enemy";

            case (int)Character.targetList.LowestHPEnemy:
                return "Lowest HP Enemy";

            case (int)Character.targetList.ClosestAlly:
                return "Closest Ally";

            case (int)Character.targetList.HighestDMGAlly:
                return "Highest DMG Ally";

            case (int)Character.targetList.LowestDMGAlly:
                return "Lowest DMG Ally";

            case (int)Character.targetList.HighestHPAlly:
                return "Highest HP Ally";

            case (int)Character.targetList.LowestHPAlly:
                return "Lowest HP Ally";

        }
        return "";
    }
}
