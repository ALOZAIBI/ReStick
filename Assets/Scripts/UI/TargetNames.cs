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

            case (int)Character.targetList.HighestPDEnemy:
                return "Highest PD Enemy";

            case (int)Character.targetList.LowestPDEnemy:
                return "Lowest PD Enemy";

            case (int)Character.targetList.HighestHPEnemy:
                return "Highest HP Enemy";

            case (int)Character.targetList.LowestHPEnemy:
                return "Lowest HP Enemy";

            case (int)Character.targetList.ClosestAlly:
                return "Closest Ally";

            case (int)Character.targetList.HighestPDAlly:
                return "Highest PD Ally";

            case (int)Character.targetList.LowestPDAlly:
                return "Lowest PD Ally";

            case (int)Character.targetList.HighestHPAlly:
                return "Highest HP Ally";

            case (int)Character.targetList.LowestHPAlly:
                return "Lowest HP Ally";

        }
        return "";
    }
}
