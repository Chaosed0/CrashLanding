using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {
    public TextAsset dialogueFile;

    private void Start() {
    }

    public void talk(DialogueBox box) {
        box.display(dialogueFile.text);
    }
}
