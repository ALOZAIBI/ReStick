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
    // Start is called before the first frame update
    void Start()
    {
        
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
    private void selectTarget() {
        if (characterToControl != null) {
            Debug.Log("Selecting target");
             if (Input.GetMouseButtonDown(0) && !IsPointerOverGameObject()) {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D[] characters = Physics2D.OverlapCircleAll(mousePos, radius, LayerMask.GetMask("Characters"));

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
                    //Set the target of the characterToControl
                    target = closestCharacter.GetComponent<Character>();
                    characterToControl.selectManualTarget(target);
                    //Unselect the characterToControl
                    characterToControl = null;
                }
            }
        }
            
    }
    // Update is called once per frame
    void Update()
    {
        customMouseDown();
        selectTarget();
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
}
