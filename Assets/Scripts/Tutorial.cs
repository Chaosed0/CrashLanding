using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum TutorialState {
    INIT = 0,
    REBOOTING,
    HEALTH,
    MOVEMENT,
    JUMP,
    FIRE,
    ALL_DONE,
    HOSTILES,
};

public class Tutorial : MonoBehaviour {
    private delegate bool CheckSwitch();

    public AudioClip[] clips;
    public AudioSource source;
    public float switchTime = 3.0f;
    public bool active = false;

    public BootupText bootupText;
    public GameObject healthText;
    public Text tutorialText;

    private TutorialState state = TutorialState.INIT;
    private float switchTimer = 0.0f;
    private bool doSwitch = true;
    private CheckSwitch switchCondition;

    public delegate void TutorialOver();
    public event TutorialOver OnTutorialOver;

    void Start() {
    }

	void Update() {
        if (switchCondition != null && switchCondition()) {
            doSwitch = true;
        }

        if (doSwitch) {
            switchTimer += Time.deltaTime;
        }

        if (switchTimer < switchTime) {
            return;
        }
        
        switch(state) {
            case TutorialState.INIT:
                state = TutorialState.REBOOTING;
                PlayNextClip();
                bootupText.StartScroll();
                break;
            case TutorialState.REBOOTING:
                state = TutorialState.HEALTH;
                PlayNextClip();
                healthText.SetActive(true);
                break;
            case TutorialState.HEALTH:
                state = TutorialState.MOVEMENT;
                PlayNextClip();
                tutorialText.text = "WASD To Move, Mouse to look";
                doSwitch = false;
                switchCondition = switchOnMovement;
                break;
            case TutorialState.MOVEMENT:
                state = TutorialState.JUMP;
                PlayNextClip();
                tutorialText.text = "Space to jump, Alt to dodge";
                doSwitch = false;
                switchCondition = switchOnJumpOrDodge;
                break;
            case TutorialState.JUMP:
                state = TutorialState.FIRE;
                PlayNextClip();
                tutorialText.text = "Left Mouse to fire";
                doSwitch = false;
                switchCondition = switchOnFire;
                break;
            case TutorialState.FIRE:
                state = TutorialState.ALL_DONE;
                PlayNextClip();
                tutorialText.text = "";
                doSwitch = true;
                break;
            case TutorialState.ALL_DONE:
                state = TutorialState.HOSTILES;
                bootupText.EndScroll();
                PlayNextClip();
                break;
            case TutorialState.HOSTILES:
                state = TutorialState.INIT;
                if (OnTutorialOver != null) {
                    OnTutorialOver();
                    gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }

        switchTimer = 0.0f;
	}
    
    bool switchOnMovement() {
        float hmove = Input.GetAxis("Horizontal");
        float vmove = Input.GetAxis("Vertical");
        return hmove > 0.1 || hmove < -0.1 || vmove > 0.1 || vmove < -0.1;
    }
    
    bool switchOnJumpOrDodge() {
        return Input.GetButtonDown("Jump") || Input.GetButtonDown("Dodge");
    }
    
    bool switchOnFire() {
        return Input.GetButtonDown("Fire1");
    }

    void PlayNextClip() {
        source.clip = clips[(int)(state)-1];
        source.Play();
    }
}
