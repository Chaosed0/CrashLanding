using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
public class Player : MonoBehaviour {
    private float activateDistance = 5.0f;
    private bool acceptingInput = true;

    public delegate void StartAcceptingInput();
    public event StartAcceptingInput OnStartAcceptingInput;

    public DialogueBox dialogueBox;
    public Gun gun;

    void Start() {
        GetComponent<Character>().OnDied += OnDied;
    }

    public void activate() {
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(transform.position, transform.forward, out hitInfo, activateDistance);
        if (hit) {
            GameObject hitObject = hitInfo.transform.gameObject;
            NPC npc = hitObject.GetComponent<NPC>();
            if (npc != null) {
                npc.talk(dialogueBox);
                dialogueBox.show();
                acceptingInput = false;
                dialogueBox.OnHidden += acceptInput;
            }
        }
    }

    private void acceptInput() {
        acceptingInput = true;
        OnStartAcceptingInput();
        dialogueBox.OnHidden -= acceptInput;
    }

    public bool isAcceptingInput() {
        return acceptingInput;
    }

    public void setFiring(bool firing) {
        gun.setFiring(firing);
    }

    private void OnDied() {
        acceptingInput = false;
    }
}
