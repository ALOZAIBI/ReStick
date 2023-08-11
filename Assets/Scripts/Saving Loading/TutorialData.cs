using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class TutorialData
{
    public bool draggingCharactersTutorialDone;
    public bool chooseRewardTutorialDone;
    public bool addingAbilityTutorialDone;
    public bool upgradingStatsTutorialDone;

    public TutorialData() { }

    public TutorialData(Tutorial tutorial) {
        draggingCharactersTutorialDone = tutorial.draggingCharactersTutorialDone;
        chooseRewardTutorialDone = tutorial.chooseRewardTutorialDone;
        addingAbilityTutorialDone = tutorial.addingAbilityTutorialDone;
        upgradingStatsTutorialDone = tutorial.upgradingStatsTutorialDone;
    }
}
