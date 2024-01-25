using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManualTargetting : MonoBehaviour
{
    [SerializeField] private float radius;

    //This holds the character that will have it's targetting modified
    public Character characterToControl;

    public Character target;
    //Used to rotate the indicator in character.cs
    public float indicatorRotation=0;

    //To make the selecting target work by dragging/holding from the characterToControl to the target
    private bool holding=false;

    //Used to draw circle on the character to be targetted
    [SerializeField] private int circleQuality;//how many steps

    [SerializeField] private LineRenderer toBeTargettedRenderer;
    //Line to the target to be, this is good for clarity since the finger can block the toBeTargettedRenderer so a line to the target is needed
    [SerializeField] private LineRenderer lineToTargetToBe;

    // Start is called before the first frame update
    void Start()
    {
        lineToTargetToBe.positionCount = 2;
    }
    //Select the character that is closest to the mouse
    private void customMouseDown() {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverGameObject()) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] characters = Physics2D.OverlapCircleAll(mousePos, radius, LayerMask.GetMask("Characters"));

            GameObject closestCharacter = null;
            float minDistance = float.MaxValue;

            foreach (Collider2D character in characters) {
                if (character.CompareTag("Character")) {
                    float distance = Vector2.Distance(mousePos, character.transform.position);
                    if (distance < minDistance) {
                        minDistance = distance;
                        closestCharacter = character.gameObject;
                    }
                }
            }

            if (closestCharacter != null) {
                closestCharacter.GetComponent<Character>().click = true;
            }
        }
    }

    //Select the target that is closest to the mouse(This will be used to select the target of the characterToControl)
    public void selectTarget() {
        if (characterToControl != null) {
            //While held
             if (Input.GetMouseButton(0) && !IsPointerOverGameObject()) {
                //holding = true;
                Debug.Log("Selecting target");
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D[] characters = Physics2D.OverlapCircleAll(mousePos, radius*2, LayerMask.GetMask("Characters"));

                GameObject closestCharacter = null;
                float minDistance = float.MaxValue;

                foreach (Collider2D character in characters) {
                    if (character.CompareTag("Character")) {
                        Character temp = character.GetComponent<Character>();
                        //Selects only if it's an enemy
                        if(temp.team != (int)Character.teamList.Player) {
                            float distance = Vector2.Distance(mousePos, character.transform.position);
                            if (distance < minDistance) {
                                minDistance = distance;
                                closestCharacter = character.gameObject;
                            }
                        }
                    }
                }

                if (closestCharacter != null) {
                    //Saves the potential target
                    target = closestCharacter.GetComponent<Character>();
                    toBeTargettedRenderer.enabled = true;
                    lineToTargetToBe.enabled = true;
                    drawCircle(target.transform.position, 0.8f, toBeTargettedRenderer, 80, indicatorRotation+60);
                    drawTargetLine(characterToControl.transform.position, target.transform.position);
                }
                else {
                    target = null;
                    toBeTargettedRenderer.enabled = false;
                    lineToTargetToBe.enabled = false;
                }

            }
             //When button released
             //Sets the target of the characterToControl
             if(Input.GetMouseButtonUp(0) && !IsPointerOverGameObject()) {
                if (target != null) {
                    characterToControl.selectManualTarget(target);
                }
                else
                    characterToControl.endManualTarget();
                //Unselect the characterToControl
                characterToControl = null;
                toBeTargettedRenderer.enabled = false;
                lineToTargetToBe.enabled = false;
            }
        }
            
    }
    // Update is called once per frame
    void Update()
    {
        customMouseDown();
        selectTarget();

        //if (Input.GetMouseButtonDown(0)) {
        //    Debug.Log("Manual targetting Clickiiinig");
        //}
    }

    //to prevent clicking thorugh UI
    //https://answers.unity.com/questions/1115464/ispointerovergameobject-not-working-with-touch-inp.html
    public static bool IsPointerOverGameObject() {
        // Check mouse
        if (EventSystem.current.IsPointerOverGameObject()) {
            return true;
        }

        // Check touches
        for (int i = 0; i < Input.touchCount; i++) {
            var touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began) {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) {
                    return true;
                }
            }
        }

        return false;
    }

    //precondition fillAmount could at most be = circlequality
    public void drawCircle(Vector3 position, float radius, LineRenderer lr, float fillAmount, float offset = 0) {
        if (fillAmount == 100)
            lr.loop = true;//to close hte circle
        else
            lr.loop = false;
        lr.enabled = true;
        float angle = 0f + offset;
        float angleIncrement = (2f * Mathf.PI) / (circleQuality);

        int numPoints = Mathf.CeilToInt(circleQuality * fillAmount / 100); // Calculate the number of points based on the fill amount
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

    public void drawTargetLine(Vector3 startPos, Vector3 endPos) {
        lineToTargetToBe.enabled = true;
        lineToTargetToBe.SetPosition(0, startPos);
        lineToTargetToBe.SetPosition(1, endPos);
    }
}
