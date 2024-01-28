using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Vector2 dragOrigin;
    [SerializeField] private Vector2 dragDifference;

    [SerializeField] private float zoomMin;
    [SerializeField] private float zoomMax;

    [SerializeField] private bool zoomingBackIn;
    [SerializeField] private float pauseDuration;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float pauseDurationTarget;
    [SerializeField] public int zoomTarget;
    [SerializeField] public Tilemap tilemapToDisplayFully;
    [SerializeField] public Tilemap tilemapToFocus;
    [SerializeField] private float zoomLevelBeforeDisplayingFully;

    private int touchCount;
    //used to get first touch to be used in dragOrigin
    private bool touching = false;

    //pannalbe is set to off when dragging a character from the characterPlacingScreen so it is set to off in the CharacterDisplay Script
    public bool pannable=true;

    //Camera focusing on a character
    public Character characterToFocusOn;
    //tutorial used https://www.youtube.com/watch?v=R6scxu1BHhs
    private void panCamera() {
        //prevent clicking through UI
        if (IsPointerOverGameObject()) {
            //Debug.Log("Camera clickign through");
            return;
        }
        //to prevent weird stuff when two touches happen(to zoom for instance) we seperate the functionality of mouse and touch
        
        if (Input.touchCount > 0) {
            //If touchcount changed from previous frame recalculate the drag origin
            if(!touching || Input.touchCount != touchCount)
                dragOrigin = (Vector2)cam.ScreenToWorldPoint(avgPosOfTouches());
            touching = true;
            touchCount = Input.touchCount;
            if (touching) {
                dragDifference = dragOrigin - (Vector2)cam.ScreenToWorldPoint(avgPosOfTouches());
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
                //To unlock the camera from character drag difference has to be greater than 0.1
                if (dragDifference.magnitude > 0.2f) {
                    characterToFocusOn = null;
                }
                //Only move camera if no character currently being focused on
                if (characterToFocusOn == null)
                    cam.transform.position += (Vector3)dragDifference;
            }
        }

    }

    private Vector2 avgPosOfTouches() {
        //Gets the average position of all touches
        Vector2 avgPos = new Vector2(0, 0);
        for (int i = 0; i < Input.touchCount; i++) {
            avgPos += Input.GetTouch(i).position;
        }
        avgPos /= Input.touchCount;
        return avgPos;
    }

    //Instantly zooms out and centers camera so that all tiles of the tilemape are visible, then after a delay zoom back into the previous zoom level
    public void showMapIntoZoom() {
        //Debug.Log("Showing map fully");
        if (tilemapToDisplayFully == null)
            return;
        pauseDuration = 0;
        pannable = false;
        zoomingBackIn = true;
        BoundsInt bounds = tilemapToDisplayFully.cellBounds;
        float aspectRatio = (float)Screen.width / Screen.height;

        // Calculate the width and height of the bounds in world space
        float boundsWidth = bounds.size.x * tilemapToDisplayFully.cellSize.x;
        float boundsHeight = bounds.size.y * tilemapToDisplayFully.cellSize.y;

        // Add padding to avoid tiles being too close to the edge of the camera view
        float padding = 1.25f;

        // Calculate the required camera size based on the bounds and padding
        float targetWidth = boundsWidth / aspectRatio * padding;
        float targetHeight = boundsHeight / aspectRatio * padding;

        // Determine the larger dimension and set the targetOrthographicSize accordingly
        cam.orthographicSize = Mathf.Max(targetWidth, targetHeight) * 0.4f;

        //// Ensure the targetOrthographicSize doesn't go below the minimum specified value
        //targetOrthographicSize = Mathf.Max(targetOrthographicSize, minOrthographicSize);

        

        if (tilemapToFocus != null) {
            // Calculate the center position of the Tilemap to focus in world space
            Vector3 tilemapCenter1 = tilemapToFocus.cellBounds.center;
            Vector3 tilemapCenterWorld1 = tilemapToFocus.transform.TransformPoint(tilemapCenter1);

            // Set the camera's position to the center of the Tilemap to focus
            cam.transform.position = new Vector3(tilemapCenterWorld1.x, tilemapCenterWorld1.y, cam.transform.position.z);
        }

        else {
            // Calculate the center position of the Tilemap in world space
            Vector3 tilemapCenter = tilemapToDisplayFully.cellBounds.center;
            Vector3 tilemapCenterWorld = tilemapToDisplayFully.transform.TransformPoint(tilemapCenter);

            cam.transform.position = new Vector3(tilemapCenterWorld.x, tilemapCenterWorld.y, cam.transform.position.z);
        }
        
    
    }
    private void Update() {
        //Panning and zooming
        if (pannable) {
            panCamera();
            
            //Focus camera on character when zone has started
            if (characterToFocusOn != null && UIManager.singleton.zoneStarted()) {
                cam.transform.position = new Vector3(characterToFocusOn.transform.position.x, characterToFocusOn.transform.position.y, cam.transform.position.z);
            }

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
        //Focusing Map
        //Maybe add delay before zooming in
        if (zoomingBackIn) {
            //pause zoom delay
            if (pauseDuration < pauseDurationTarget) {
                pauseDuration += Time.unscaledDeltaTime;
            }
            else {
                if (Mathf.RoundToInt(cam.orthographicSize) != zoomTarget) {
                    cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomTarget, zoomSpeed * Time.unscaledDeltaTime);
                }
                else {
                    zoomingBackIn = false;
                    pannable = true;
                }
            }
        }
        //If not in zone keep camera locked in the x axis and locked at 11 zoom
        if(UIManager.singleton.inZone == false) {
            cam.transform.position = new Vector3(0, cam.transform.position.y, cam.transform.position.z);
            cam.orthographicSize = 11;
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
