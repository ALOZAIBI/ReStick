using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNames : MonoBehaviour
{
    public static string getName(int target) {
        switch (target) {
            case (int)Character.movementStrategies.Default:
                return "Default";
            case (int)Character.movementStrategies.StayNearAlly:
                return "Stay Near Ally";
            case (int)Character.movementStrategies.DontMove:
                return "Don't move";
            case (int)Character.movementStrategies.RunAwayFromNearestEnemy:
                return "Run away";

        }
        return "";
    }
}
