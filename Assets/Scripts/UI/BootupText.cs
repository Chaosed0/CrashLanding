using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BootupText : MonoBehaviour {
    public TextAsset textFile;
    public bool active = false;
    public float lineTime = 0.1f;
    public int maxLines = 200;

    private Text text;
    private string[] lines;
    private float lineTimer = 0.0f;
    private int current = 0;

	void Start () {
        lines = textFile.text.Split('\n');
        text = GetComponent<Text>();
	}

	void Update () {
        if (!active) {
            return;
        }

        lineTimer += Time.deltaTime;
        if (lineTimer >= lineTime) {
            if (current > maxLines) {
                int lineEnder = text.text.IndexOf('\n');
                if (lineEnder >= 0) {
                    text.text = text.text.Substring(text.text.IndexOf('\n') + 1);
                }
            }

            if (current < lines.Length) {
                text.text += lines[current] + '\n';
                current++;
            }

            if (text.text.Length == 0) {
                active = false;
            }

            lineTimer = 0;
        }
	}
}
