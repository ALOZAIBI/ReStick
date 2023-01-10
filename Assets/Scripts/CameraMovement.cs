using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Vector2 dragOrigin;
    [SerializeField] private Vector2 dragDifference;

    //tutorial used https://www.youtube.com/watch?v=R6scxu1BHhs
    private void panCamera() {
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
        panCamera();
    }
}
