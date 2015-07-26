using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour {
    public void StartGame() {
        GameObject.Find("SharedLevelObject").GetComponent<SharedLevelObject>().nextLevel = "Dungeon";
        Application.LoadLevel("LoadingScene");
    }
    public void ExitGame() {
        Application.Quit();
    }
    public void InvertMouseLook(Toggle toggle) {
        GameObject.Find("SharedLevelObject").GetComponent<SharedLevelObject>().invertMouse = toggle.isOn;
    }
}
