using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public delegate void ButtonSetListener();
    public Image textBox;
    public TextMeshProUGUI text;
    //The 2 below buttons are used to move through the tutorial
    public Button nextBtn;
    public Button textBoxBtn;

    //Times used to focus and unfocus
    private float time;
    [SerializeField]private float transitionTime;
    private bool focusing;
    private bool unfocusing;

    //Holds the objects that will be focused on
    public List<Transform> objectsToBeFocused = new List<Transform>();
    //Saves the parent's of the objects that will be focused on so that they can be returned to their parent
    public List<Transform> objectsParents = new List<Transform>();

    //Explains How to Drag Character into map
    public bool draggingCharactersTutorialDone;

    private void Start() {
        //nextBtn.onClick.AddListener(returnToParents);
        //textBoxBtn.onClick.AddListener(returnToParents);
    }
    public void draggingCharactersTutorial() {
        if (draggingCharactersTutorialDone)
            return;
        gameObject.SetActive(true);
        focus();

        positionTextBox(0.6f,0.7f,0.2f,0.8f);
        text.text = "Drag the character into the dotted area on the map";

        //Clicking will simply Unfocus
        SetListener(draggingCharactersDone);
    }
    private void draggingCharactersDone() {
        unfocusing = true;
        draggingCharactersTutorialDone = true;
    }
    private void positionTextBox(float bottomAnchor, float topAnchor, float leftAnchor, float rightAnchor) {
        textBox.rectTransform.SetAnchorBottom(bottomAnchor);
        textBox.rectTransform.SetAnchorTop(topAnchor);
        textBox.rectTransform.SetAnchorLeft(leftAnchor);
        textBox.rectTransform.SetAnchorRight(rightAnchor);
    }
    //Sets the buttos' listeners
    private void SetListener(ButtonSetListener listener) {
        nextBtn.onClick.RemoveAllListeners();
        textBoxBtn.onClick.RemoveAllListeners();

        nextBtn.onClick.AddListener(() => listener());
        textBoxBtn.onClick.AddListener(() => listener());
    }
    private void focus() {
        UIManager.singleton.focus.gameObject.SetActive(true);
        foreach(Transform t in objectsToBeFocused) {
            objectsParents.Add(t.parent);
            t.SetParent(UIManager.singleton.focus.transform);
        }
        focusing = true;

        textBox.transform.SetParent(UIManager.singleton.focus.transform);
        nextBtn.transform.SetParent(UIManager.singleton.focus.transform);
    }
    private void returnToParents() {
        foreach(Transform t in objectsToBeFocused) {
            t.SetParent(objectsParents[objectsToBeFocused.IndexOf(t)]);
        }
        objectsToBeFocused.Clear();
        objectsParents.Clear();
        //Reset's it's parent
        textBox.transform.SetParent(transform);
        nextBtn.transform.SetParent(transform);

    }
    private void unfocus() {
        UIManager.singleton.focus.gameObject.SetActive(false);
        returnToParents();
        gameObject.SetActive(false);
    }

    private void Update() {
            

        if (focusing) {
            time += Time.unscaledDeltaTime;
            UIManager.singleton.focus.SetAlpha(Mathf.Lerp(0, UIManager.singleton.focusOpacity, time / transitionTime));
            //Once focusing is done
            if (time >= transitionTime) {
                focusing = false;
                time = transitionTime;
            }
        }

        if (unfocusing) {
            time -= Time.unscaledDeltaTime;
            UIManager.singleton.focus.SetAlpha(Mathf.Lerp(0, UIManager.singleton.focusOpacity, time / transitionTime));
            //Once unfocusing is done
            if (time <= 0) {
                unfocusing = false;
                time = 0;
                unfocus();
            }
        }

    }
}
