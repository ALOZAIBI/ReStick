using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicators : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject range;

    [SerializeField] private LineRenderer targetRenderer;
    [SerializeField] private LineRenderer rangeRenderer;

    [SerializeField] private int circleQuality;//how many steps
    // Start is called before the first frame update
    void Start()
    {
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
    public void drawRangeCircle(Vector3 position,float radius) {
        rangeRenderer.enabled = true;
        float angle = 0f;
        float angleIncrement = (2f * Mathf.PI) / (circleQuality-1);

        for (int i = 0; i < circleQuality; i++) {
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;

            Vector3 point = new Vector3(x, y, 0f);
            point += position;
            rangeRenderer.SetPosition(i, point);

            angle += angleIncrement;
        }
    }

    public void eraseLines() {
        targetRenderer.enabled = false;
        rangeRenderer.enabled = false;
    }
}
