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
    SHIP_POWER,
    HOSTILES,
};

public class Tutorial : MonoBehaviour {
    private delegate bool CheckSwitch();

    public AudioClip[] clips;
    public AudioSource audioSource;
    public float switchTime = 1.0f;

    public BootupText bootupText;
    public Text tutorialText;
    public HealthText healthText;
    public ShipPowerText shipPowerText;

    public GameRules gameRules;

    private TutorialState state = TutorialState.INIT;
    private float switchTimer = 0.0f;
    private bool doSwitch = true;
    private CheckSwitch switchCondition;

    public delegate void TutorialEnd();
    public event TutorialEnd OnTutorialEnd;

    public delegate void EarlyTutorialEnd();
    public event EarlyTutorialEnd OnEarlyTutorialEnd;

    public bool skipTutorial;

    void Start() {
        GameObject sharedLevelObject = GameObject.Find("SharedLevelObject");
        bool skipTutorial = false;
        if (sharedLevelObject != null) {
            skipTutorial = sharedLevelObject.GetComponent<SharedLevelObject>().skipTutorial;
        }

        if (skipTutorial || this.skipTutorial) {
            if (OnEarlyTutorialEnd != null) {
                OnEarlyTutorialEnd();
            }
            healthText.gameObject.SetActive(true);
            shipPowerText.gameObject.SetActive(true);
            state = TutorialState.SHIP_POWER;
        }
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
                switchTime = 3.0f;
                break;
            case TutorialState.REBOOTING:
                state = TutorialState.HEALTH;
                PlayNextClip();
                healthText.gameObject.SetActive(true);
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
                if (OnEarlyTutorialEnd != null) {
                    OnEarlyTutorialEnd();
                }
                PlayNextClip();
                tutorialText.text = "";
                doSwitch = true;
                break;
            case TutorialState.ALL_DONE:
                state = TutorialState.SHIP_POWER;
                switchTime = 4.0f;
                shipPowerText.gameObject.SetActive(true);
                bootupText.EndScroll();
                PlayNextClip();
                break;
            case TutorialState.SHIP_POWER:
                state = TutorialState.HOSTILES;
                switchTime = 3.0f;
                PlayNextClip();
                break;
            case TutorialState.HOSTILES:
                state = TutorialState.INIT;
                gameRules.enabled = true;
                this.enabled = false;
                if (OnTutorialEnd != null) {
                    OnTutorialEnd();
                }
                GameObject sharedObject = GameObject.Find("SharedLevelObject");
                if (sharedObject != null) {
                    sharedObject.GetComponent<SharedLevelObject>().skipTutorial = true;
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
        audioSource.clip = clips[(int)(state)-1];
        audioSource.Play();
    }
}
