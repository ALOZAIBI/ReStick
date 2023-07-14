using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Attack Target selector Deprecated use this
public class TargetSelector : MonoBehaviour
{
    //Wether this is selecting highest or lowest
    public bool highest;

    public bool isAbilityTargetSelector;

    [SerializeField]private Button toggle;

    [SerializeField]private RectTransform toggleTransform;

    [SerializeField]private TextMeshProUGUI highestText;
    [SerializeField]private TextMeshProUGUI lowestText;

    [SerializeField]private float yPositionHi;
    [SerializeField]private float yPositionLo;

    [SerializeField]private float time;
    [SerializeField]private float transitionTime;

    private const float selectedAlpha = 1;
    private const float deselectedAlpha = 0.4f;

    void Start()
    {
        toggle.onClick.AddListener(toggleClicked);
    }
    private void toggleClicked() {
        highest = !highest;
        updateToggleView();
    }
    public void updateToggleView() {
        if (highest) {
            toggleTransform.localPosition = new Vector3(toggleTransform.localPosition.x, yPositionHi, toggleTransform.localPosition.z);
            highestText.SetAlpha(selectedAlpha);
            lowestText.SetAlpha(deselectedAlpha);
        }
        else {
            toggleTransform.localPosition = new Vector3(toggleTransform.localPosition.x, yPositionLo, toggleTransform.localPosition.z);
            highestText.SetAlpha(deselectedAlpha);
            lowestText.SetAlpha(selectedAlpha);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
