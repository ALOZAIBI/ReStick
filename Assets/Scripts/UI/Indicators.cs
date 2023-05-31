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
    //this will be displayed on top of target renderer and will be filled when cooldown is ready
    public List<LineRenderer> abilitiesCooldownRenderer = new List<LineRenderer>();
    //to be instantiated
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private GameObject outlinedIndicatorPrefab;

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
    //precondition fillAmount could at most be = circlequality
    public void drawCircle(Vector3 position,float radius,LineRenderer lr,float fillAmount) {
        if(fillAmount == 100)
            lr.loop = true;//to close hte circle
        else
            lr.loop = false;
        lr.enabled = true;
        float angle = 0f;
        float angleIncrement = (2f * Mathf.PI) / (circleQuality);

        int numPoints = Mathf.CeilToInt(circleQuality * fillAmount/100); // Calculate the number of points based on the fill amount
        lr.positionCount = numPoints;

        for (int i = 0; i < numPoints; i++) {
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;

            Vector3 point = new Vector3(x, y, 0f);
            point += position;
            lr.SetPosition(i, point);

            angle += angleIncrement;
        }
    }
    //if an ability uses range draw range indicator. If ability has target draw target Indicator.
    public void drawAbilitiesCircles(Vector3 position) {
        int index = 0;
        foreach(Ability ability in character.abilities) {
                if(ability.rangeAbility > 0) {
                    drawCircle(position, ability.rangeAbility, abilitiesRangeRenderer[index],100);
                }
            //draw circle on target if possible
            if (ability.hasTarget) {
                //if target within range
                if (character.selectTarget(ability.targetStrategy, ability.rangeAbility)) {
                    //draw target circle
                    drawCircle(character.target.transform.position, character.target.transform.lossyScale.x / 1.5f, abilitiesTargetRenderer[index], 100);
                    float fillAmount = (ability.CD - ability.abilityNext) / ability.CD;fillAmount *= 100;
                    //draw cooldown circle on targetCircle
                    drawCircle(character.target.transform.position, character.target.transform.lossyScale.x / 1.5f, abilitiesCooldownRenderer[index], fillAmount);

                }
                else {
                    abilitiesTargetRenderer[index].enabled = false;
                    abilitiesCooldownRenderer[index].enabled = false;
                }
            }
            if (ability.hasTarget || ability.rangeAbility > 0) {
                index++;
            }
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

                LineRenderer temp3 = Instantiate(outlinedIndicatorPrefab, transform).GetComponent<LineRenderer>();
                temp3.positionCount = circleQuality;
                temp3.name = "abilityCooldown" + index;
                temp3.startColor = color;
                temp3.endColor = color;
                abilitiesCooldownRenderer.Add(temp3);
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
        abilitiesCooldownRenderer.Clear();
    }
    public void eraseLines() {
        targetRenderer.enabled = false;
        rangeRenderer.enabled = false;
        foreach (LineRenderer temp in abilitiesRangeRenderer) {
            temp.enabled = false;
        }
        foreach (LineRenderer temp in abilitiesTargetRenderer) {
            temp.enabled = false;
        }
        foreach (LineRenderer temp in abilitiesCooldownRenderer) {
            temp.enabled = false;
        }
    }
}
