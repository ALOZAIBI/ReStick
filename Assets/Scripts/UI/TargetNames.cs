using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNames : MonoBehaviour
{
    public static string getName(int target) {
        switch (target) {
            case (int)Character.TargetList.DefaultEnemy:
                return "Default";

            case (int)Character.TargetList.ClosestEnemy:
                return "Closest";

            case (int)Character.TargetList.HighestPDEnemy:
                return "Highest PWR";

            case (int)Character.TargetList.LowestPDEnemy:
                return "Lowest PWR";

            case (int)Character.TargetList.HighestMDEnemy:
                return "Highest MGC";

            case (int)Character.TargetList.LowestMDEnemy:
                return "Lowest MGC";

            case (int)Character.TargetList.HighestINFEnemy:
                return "Highest INF";

            case (int)Character.TargetList.LowestINFEnemy:
                return "Lowest INF";

            case (int)Character.TargetList.HighestASEnemy:
                return "Highest AS";

            case (int)Character.TargetList.LowestASEnemy:
                return "Lowest AS";

            case (int)Character.TargetList.HighestMSEnemy:
                return "Highest SPD";

            case (int)Character.TargetList.LowestMSEnemy:
                return "Lowest SPD";

            case (int)Character.TargetList.HighestRangeEnemy:
                return "Highest Range";

            case (int)Character.TargetList.LowestRangeEnemy:
                return "Lowest Range";

            case (int)Character.TargetList.HighestHPEnemy:
                return "Highest HP";

            case (int)Character.TargetList.LowestHPEnemy:
                return "Lowest HP";

            case (int)Character.TargetList.ClosestAlly:
                return "Closest";

            case (int)Character.TargetList.HighestPDAlly:
                return "Highest PWR";

            case (int)Character.TargetList.LowestPDAlly:
                return "Lowest PWR";

            case (int)Character.TargetList.HighestMDAlly:
                return "Highest MGC";

            case (int)Character.TargetList.LowestMDAlly:
                return "Lowest MGC";

            case (int)Character.TargetList.HighestINFAlly:
                return "Highest INF";

            case (int)Character.TargetList.LowestINFAlly:
                return "Lowest INF";

            case (int)Character.TargetList.HighestASAlly:
                return "Highest AS";

            case (int)Character.TargetList.LowestASAlly:
                return "Lowest AS";

            case (int)Character.TargetList.HighestMSAlly:
                return "Highest SPD";

            case (int)Character.TargetList.LowestMSAlly:
                return "Lowest SPD";

            case (int)Character.TargetList.HighestRangeAlly:
                return "Highest Range";

            case (int)Character.TargetList.LowestRangeAlly:
                return "Lowest Range";

            case (int)Character.TargetList.HighestHPAlly:
                return "Highest HP";

            case (int)Character.TargetList.LowestHPAlly:
                return "Lowest HP";

        }
        return "";
    }

    public static bool isEnemy(int target) {
        switch (target) {
            case (int)Character.TargetList.DefaultEnemy:
            case (int)Character.TargetList.ClosestEnemy:
            case (int)Character.TargetList.HighestPDEnemy:
            case (int)Character.TargetList.LowestPDEnemy:
            case (int)Character.TargetList.HighestMDEnemy:
            case (int)Character.TargetList.LowestMDEnemy:
            case (int)Character.TargetList.HighestINFEnemy:
            case (int)Character.TargetList.LowestINFEnemy:
            case (int)Character.TargetList.HighestASEnemy:
            case (int)Character.TargetList.LowestASEnemy:
            case (int)Character.TargetList.HighestMSEnemy:
            case (int)Character.TargetList.LowestMSEnemy:
            case (int)Character.TargetList.HighestRangeEnemy:
            case (int)Character.TargetList.LowestRangeEnemy:
            case (int)Character.TargetList.HighestHPEnemy:
            case (int)Character.TargetList.LowestHPEnemy:
                return true;

            case (int)Character.TargetList.ClosestAlly:
            case (int)Character.TargetList.HighestPDAlly:
            case (int)Character.TargetList.LowestPDAlly:
            case (int)Character.TargetList.HighestMDAlly:
            case (int)Character.TargetList.LowestMDAlly:
            case (int)Character.TargetList.HighestINFAlly:
            case (int)Character.TargetList.LowestINFAlly:
            case (int)Character.TargetList.HighestASAlly:
            case (int)Character.TargetList.LowestASAlly:
            case (int)Character.TargetList.HighestMSAlly:
            case (int)Character.TargetList.LowestMSAlly:
            default:
                return false;

        }
    }
}
