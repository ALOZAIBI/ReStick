using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Vector2 dragOrigin;
    [SerializeField] private Vector2 dragDifference;

    [SerializeField] private float zoomMin;
    [SerializeField] private float zoomMax;

    //used to get first touch to be used in dragOrigin
    private bool touching = false;

    //pannalbe is set to off when dragging a character from the characterPlacingScreen so it is set to off in the CharacterDisplay Script
    public bool pannable=true;

    //tutorial used https://www.youtube.com/watch?v=R6scxu1BHhs
    private void panCamera() {
        //prevent clicking through UI
        if (IsPointerOverGameObject()) {
            Debug.Log("Camera clickign through");
            return;
        }
        //to prevent weird stuff when two touches happen(to zoom for instance) we seperate the functionality of mouse and touch
        if (Input.touchCount > 0) {
            if(!touching)
                dragOrigin = (Vector2)cam.ScreenToWorldPoint(Input.GetTouch(0).position);
            touching = true;
            if (touching) {
                dragDifference = dragOrigin - (Vector2)cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                cam.transform.position += (Vector3)dragDifference;
            }
        }
        else {
            //resets touching to false ot be able to find first touch
            touching = false;
            //gets position of initial click
            if (Input.GetMouseButtonDown(0)) {
                dragOrigin = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0)) {
                dragDifference = dragOrigin - (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.position += (Vector3)dragDifference;
            }
        }

    }
    private void Update() {
        if (pannable) {
            panCamera();
            
            //zoom using touch https://www.youtube.com/watch?v=K_aAnBn5khA
            if (Input.touchCount == 2) {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currMagnitude - prevMagnitude;

                zoom(difference * 0.01f);
            }
            else
            //multiplied by 2 just to increase sens
            zoom(Input.GetAxis("Mouse ScrollWheel") * 2);
        }
    }

    public void zoom(float amount) {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize-amount, zoomMin, zoomMax);
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
