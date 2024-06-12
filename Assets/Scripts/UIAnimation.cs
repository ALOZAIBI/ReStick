using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour {
    public float duration;

    [SerializeField] private Sprite[] sprites;

    [SerializeField] private Image image;
    [SerializeField] private int index = 0;
    [SerializeField] private float timer = 0;

    [SerializeField] private bool loop = false;

    private void Start() {
        image = GetComponent<Image>();
    }
    //Manually restart animation.
    public void startAnimation() {
        gameObject.SetActive(true);
        index = 0; 
        image.sprite = sprites[index];
        //timer = 0;
    }

    private void Update() {
        if (((timer += Time.unscaledDeltaTime) >= (duration / sprites.Length)) && index<sprites.Length) {
            timer = 0;
            image.sprite = sprites[index];
            if (loop) {
                index = (index + 1) % sprites.Length;
            }
            else
                index++;
        }
        if (index > sprites.Length - 1) {
            gameObject.SetActive(false);
        }
    }
}