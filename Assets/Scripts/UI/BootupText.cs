using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BootupText : MonoBehaviour {
    public TextAsset textFile;
    public float lineTime = 0.1f;
    public int maxLines = 200;

    private Text text;
    private string[] lines;
    private float lineTimer = 0.0f;
    private int current = 0;
    private int numLines = 0;
    private bool done = true;
    private bool active = false;

	void Start () {
        lines = textFile.text.Split('\n');
        text = GetComponent<Text>();
	}

    public void StartScroll() {
        active = true;
        done = false;
    }

    public void EndScroll() {
        active = false;
    }

	void Update () {
        if (done) {
            return;
        }

        lineTimer += Time.deltaTime;
        if (lineTimer >= lineTime) {
            if (numLines > maxLines || !active) {
                int lineEnder = text.text.IndexOf('\n');
                if (lineEnder >= 0) {
                    text.text = text.text.Substring(text.text.IndexOf('\n') + 1);
                    numLines--;
                }
            }

            if (active) {
                text.text += lines[current] + '\n';
                current = (current+1)%lines.Length;
                numLines++;
            }

            if (numLines <= 0) {
                done = true;
            }

            lineTimer = 0;
        }
	}
}
