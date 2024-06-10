using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour {
    public float duration;

    [SerializeField] private Sprite[] sprites;

    [SerializeField] private Image image;
    [SerializeField] private int index = 0;
    [SerializeField] private float timer = 0;

    private bool animating = false;

    private void Start() {
        image = GetComponent<Image>();
    }
    public void startAnimation() {
        gameObject.SetActive(true);
        index = 0; 
        image.sprite = sprites[index];
        //timer = 0;
        Debug.Log("Animation started "+gameObject.activeSelf);
    }
    private void Update() {
        if (((timer += Time.unscaledDeltaTime) >= (duration / sprites.Length)) && index<sprites.Length) {
            Debug.Log("Amni,matiuon;l");
            timer = 0;
            image.sprite = sprites[index];
            index++;
        }
        if (index > sprites.Length - 1) {
            gameObject.SetActive(false);
        }
    }
}