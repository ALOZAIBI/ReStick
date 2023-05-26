using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicators : MonoBehaviour
{
    [SerializeField]public Character character;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject range;

    [SerializeField] private LineRenderer targetRenderer;
    [SerializeField] public LineRenderer rangeRenderer;

    public List<LineRenderer> abilitiesTargetRenderer = new List<LineRenderer>();
    public List<LineRenderer> abilitiesRangeRenderer = new List<LineRenderer>();
    //to be instantiated
    [SerializeField] private GameObject indicatorPrefab;

    [SerializeField] private int circleQuality;//how many steps
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponentInParent<Character>();
        targetRenderer = target.GetComponent<LineRenderer>();
        // 2 points for a straight line
        targetRenderer.positionCount = 2;
        rangeRenderer = range.GetComponent<LineRenderer>();
        rangeRenderer.positionCount = circleQuality;
    }

    public void drawTargetLine(Vector3 startPos,Vector3 endPos) {
        targetRenderer.enabled = true;
        targetRenderer.SetPosition(0, startPos);
        targetRenderer.SetPosition(1, endPos);
    }
    public void drawRangeCircle(Vector3 position,float radius,LineRenderer lr) {
        lr.enabled = true;
        float angle = 0f;
        float angleIncrement = (2f * Mathf.PI) / (circleQuality-1);

        for (int i = 0; i < circleQuality; i++) {
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;

            Vector3 point = new Vector3(x, y, 0f);
            point += position;
            lr.SetPosition(i, point);

            angle += angleIncrement;
        }
    }

    public void drawAbilitiesCircles(Vector3 position) {
        int index = 0;
        foreach(Ability ability in character.abilities) {
                if(ability.rangeAbility > 0) {
                    drawRangeCircle(position, ability.rangeAbility, abilitiesRangeRenderer[index]);
                }
                //draw circle on target if possible
                if(ability.hasTarget&&character.selectTarget(ability.targetStrategy, ability.rangeAbility)) {
                    drawRangeCircle(character.target.transform.position, .7f, abilitiesTargetRenderer[index]);
                }
                else
                    abilitiesTargetRenderer[index].enabled = false;
                index++;
        }
    }
    //creates indicatorPrefab for each ability that has range or has a target
    public void setupAbilitiesIndicators() {
        closeAbilityIndicators();
        int index = 0;
        foreach(Ability ability in character.abilities) {
            if (ability.hasTarget || ability.rangeAbility > 0) {
                Color color = ColorPalette.singleton.getIndicatorColor(ability.abilityType);
                //instantiate as child and save the corresponding renderers
                LineRenderer temp1 = Instantiate(indicatorPrefab, transform).GetComponent<LineRenderer>();
                temp1.positionCount = circleQuality;
                temp1.name = "abilityTarget" + index;
                temp1.startColor = color;
                temp1.endColor = color;
                abilitiesTargetRenderer.Add(temp1);

                LineRenderer temp2 = Instantiate(indicatorPrefab, transform).GetComponent<LineRenderer>();
                temp2.positionCount = circleQuality;
                temp2.name = "abilityRange" + index;
                temp2.startColor = color;
                temp2.endColor = color;
                abilitiesRangeRenderer.Add(temp2);
                index++;
            }
        }
    }
    //deletes abilityIndicatorObjects
    public void closeAbilityIndicators() {
        foreach(Transform child in transform) {
            //deletes all abilioty indicators
            if (child.name != "Range" && child.name != "Target") {
                Destroy(child.gameObject);
            }
        }
        abilitiesRangeRenderer.Clear();
        abilitiesTargetRenderer.Clear();
    }
    public void eraseLines() {
        targetRenderer.enabled = false;
        rangeRenderer.enabled = false;
    }
}
