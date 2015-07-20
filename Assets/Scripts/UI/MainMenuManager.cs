using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour {
    public void StartGame() {
        GameObject.Find("SharedLevelObject").GetComponent<SharedLevelObject>().nextLevel = "Dungeon";
        Application.LoadLevel("LoadingScene");
    }
}
