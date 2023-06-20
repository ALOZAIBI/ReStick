using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNames : MonoBehaviour
{
    public static string getName(int target) {
        switch (target) {
            case (int)Character.TargetList.DefaultEnemy:
                return "Default Enemy";

            case (int)Character.TargetList.ClosestEnemy:
                return "Closest Enemy";

            case (int)Character.TargetList.HighestPDEnemy:
                return "Highest PWR Enemy";

            case (int)Character.TargetList.LowestPDEnemy:
                return "Lowest PWR Enemy";

            case (int)Character.TargetList.HighestMDEnemy:
                return "Highest MGC Enemy";

            case (int)Character.TargetList.LowestMDEnemy:
                return "Lowest MGC Enemy";

            case (int)Character.TargetList.HighestINFEnemy:
                return "Highest INF Enemy";

            case (int)Character.TargetList.LowestINFEnemy:
                return "Lowest INF Enemy";

            case (int)Character.TargetList.HighestASEnemy:
                return "Highest AS Enemy";

            case (int)Character.TargetList.LowestASEnemy:
                return "Lowest AS Enemy";

            case (int)Character.TargetList.HighestMSEnemy:
                return "Highest SPD Enemy";

            case (int)Character.TargetList.LowestMSEnemy:
                return "Lowest SPD Enemy";

            case (int)Character.TargetList.HighestRangeEnemy:
                return "Highest Range Enemy";

            case (int)Character.TargetList.LowestRangeEnemy:
                return "Lowest Range Enemy";

            case (int)Character.TargetList.HighestHPEnemy:
                return "Highest HP Enemy";

            case (int)Character.TargetList.LowestHPEnemy:
                return "Lowest HP Enemy";

            case (int)Character.TargetList.ClosestAlly:
                return "Closest Ally";

            case (int)Character.TargetList.HighestPDAlly:
                return "Highest PWR Ally";

            case (int)Character.TargetList.LowestPDAlly:
                return "Lowest PWR Ally";

            case (int)Character.TargetList.HighestMDAlly:
                return "Highest MGC Ally";

            case (int)Character.TargetList.LowestMDAlly:
                return "Lowest MGC Ally";

            case (int)Character.TargetList.HighestINFAlly:
                return "Highest INF Ally";

            case (int)Character.TargetList.LowestINFAlly:
                return "Lowest INF Ally";

            case (int)Character.TargetList.HighestASAlly:
                return "Highest AS Ally";

            case (int)Character.TargetList.LowestASAlly:
                return "Lowest AS Ally";

            case (int)Character.TargetList.HighestMSAlly:
                return "Highest SPD Ally";

            case (int)Character.TargetList.LowestMSAlly:
                return "Lowest SPD Ally";

            case (int)Character.TargetList.HighestRangeAlly:
                return "Highest Range Ally";

            case (int)Character.TargetList.LowestRangeAlly:
                return "Lowest Range Ally";

            case (int)Character.TargetList.HighestHPAlly:
                return "Highest HP Ally";

            case (int)Character.TargetList.LowestHPAlly:
                return "Lowest HP Ally";

        }
        return "";
    }
}
