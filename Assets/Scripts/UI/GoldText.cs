using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldText : MonoBehaviour
{
    private RectTransform rectTransform;
    public TextMeshProUGUI text;
    //This isn't total gold, this is the gold to display. It is different from the actual gold amount, since hte display will increment once a coin hits the display

    public int goldToDisplay;

    private Vector2 initPos;

    [SerializeField]private float totalMoveTime = 2f;
    private float moveTimeRemaining;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        // To get the position of the element on the screen
        //initPos = RectTransformUtility.WorldToScreenPoint(Camera.main, rectTransform.position);
        initPos = rectTransform.localPosition;
    }
    //Move this object up a bit to simulate coin hitting
    public void moveDisplayUp() {
        //transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        rectTransform.localPosition = new Vector2(rectTransform.localPosition.x, rectTransform.localPosition.y + 10);
        moveTimeRemaining = totalMoveTime;
    }

    public void updateView() {
        text.text = goldToDisplay.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        //If the object isn't at it's initPos slowly move it back to it
        moveTimeRemaining -= Time.unscaledDeltaTime;

        if (moveTimeRemaining > 0) {
            //Vector3.Lerp(transform.position, initPos, (totalMoveTime - moveTimeRemaining) / totalMoveTime);
            rectTransform.localPosition = Vector2.Lerp(rectTransform.localPosition, initPos, (totalMoveTime - moveTimeRemaining) / totalMoveTime);
        }
        else {
            rectTransform.localPosition = initPos;
        }

    }
}
