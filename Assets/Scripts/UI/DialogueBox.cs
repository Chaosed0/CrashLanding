using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueBox : MonoBehaviour {
    public Text displayText;
    public Button nextButton;

    public delegate void hidden();
    public event hidden OnHidden;

    private CanvasGroup canvasGroup;

    private void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
        nextButton.onClick.AddListener (hide);
    }

    public void display(string textString) {
        displayText.text = textString;
    }

    public void show() {
        canvasGroup.alpha = 1.0f;
    }

    public void hide() {
        canvasGroup.alpha = 0.0f;
        OnHidden();
    }
}
