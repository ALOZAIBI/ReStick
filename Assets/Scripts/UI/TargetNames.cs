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

            case (int)Character.targetList.HighestMDEnemy:
                return "Highest MD Enemy";

            case (int)Character.targetList.LowestMDEnemy:
                return "Lowest MD Enemy";

            case (int)Character.targetList.HighestINFEnemy:
                return "Highest INF Enemy";

            case (int)Character.targetList.LowestINFEnemy:
                return "Lowest INF Enemy";

            case (int)Character.targetList.HighestASEnemy:
                return "Highest AS Enemy";

            case (int)Character.targetList.LowestASEnemy:
                return "Lowest AS Enemy";

            case (int)Character.targetList.HighestMSEnemy:
                return "Highest MS Enemy";

            case (int)Character.targetList.LowestMSEnemy:
                return "Lowest MS Enemy";

            case (int)Character.targetList.HighestRangeEnemy:
                return "Highest Range Enemy";

            case (int)Character.targetList.LowestRangeEnemy:
                return "Lowest Range Enemy";

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

            case (int)Character.targetList.HighestMDAlly:
                return "Highest MD Ally";

            case (int)Character.targetList.LowestMDAlly:
                return "Lowest MD Ally";

            case (int)Character.targetList.HighestINFAlly:
                return "Highest INF Ally";

            case (int)Character.targetList.LowestINFAlly:
                return "Lowest INF Ally";

            case (int)Character.targetList.HighestASAlly:
                return "Highest AS Ally";

            case (int)Character.targetList.LowestASAlly:
                return "Lowest AS Ally";

            case (int)Character.targetList.HighestMSAlly:
                return "Highest MS Ally";

            case (int)Character.targetList.LowestMSAlly:
                return "Lowest MS Ally";

            case (int)Character.targetList.HighestRangeAlly:
                return "Highest Range Ally";

            case (int)Character.targetList.LowestRangeAlly:
                return "Lowest Range Ally";

            case (int)Character.targetList.HighestHPAlly:
                return "Highest HP Ally";

            case (int)Character.targetList.LowestHPAlly:
                return "Lowest HP Ally";

        }
        return "";
    }
}
