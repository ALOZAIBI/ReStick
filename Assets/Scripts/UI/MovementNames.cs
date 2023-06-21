using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNames : MonoBehaviour
{
    public static string getName(int target) {
        switch (target) {
            case (int)Character.MovementStrategies.Default:
                return "Walk to target";
            case (int)Character.MovementStrategies.StayNearAlly:
                return "Stay Near Ally";
            case (int)Character.MovementStrategies.DontMove:
                return "Don't move";
            case (int)Character.MovementStrategies.RunAwayFromNearestEnemy:
                return "Run away";

        }
        return "";
    }
}
