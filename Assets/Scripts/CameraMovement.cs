using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Vector2 dragOrigin;
    [SerializeField] private Vector2 dragDifference;

    //pannalbe is set to off when dragging a character from the characterPlacingScreen so it is set to off in the CharacterDisplay Script
    public bool pannable=true;

    //tutorial used https://www.youtube.com/watch?v=R6scxu1BHhs
    private void panCamera() {
        //prevent clicking through UI
        if (IsPointerOverGameObject()) {
            Debug.Log("Camera clickign through");
            return;
        }
        //gets position of initial click
        if (Input.GetMouseButtonDown(0)) {
            dragOrigin = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0)) {
            dragDifference = dragOrigin - (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += (Vector3)dragDifference;
        }
    }
    private void Update() {
        if (pannable) {
            panCamera();
        }
    }

    //used to prevent clicking through UI
    //https://answers.unity.com/questions/1115464/ispointerovergameobject-not-working-with-touch-inp.html
    public static bool IsPointerOverGameObject() {
        // Check mouse
        if (EventSystem.current.IsPointerOverGameObject()) {
            return true;
        }

        //// Check touches
        //for (int i = 0; i < Input.touchCount; i++) {
        //    var touch = Input.GetTouch(i);
        //    if (touch.phase == TouchPhase.Began) {
        //        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) {
        //            return true;
        //        }
        //    }
        //}

        return false;
    }

    
}
