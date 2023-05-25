using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicators : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject range;

    [SerializeField] private LineRenderer targetRenderer;
    [SerializeField] private LineRenderer rangeRenderer;

    // Start is called before the first frame update
    void Start()
    {
        targetRenderer = target.GetComponent<LineRenderer>();
        // 2 points for a straight line
        targetRenderer.positionCount = 2;
        rangeRenderer = range.GetComponent<LineRenderer>();
    }

    public void drawTargetLine(Vector3 startPos,Vector3 endPos) {
        targetRenderer.enabled = true;
        targetRenderer.SetPosition(0, startPos);
        targetRenderer.SetPosition(1, endPos);
    }

    public void eraseLines() {
        targetRenderer.enabled = false;
    }
}
