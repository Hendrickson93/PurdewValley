using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenu;
    private bool isPaused = false;

    void Update () {
        if(Input.GetKeyDown(KeyCode.Escape)){
            isPaused = !isPaused;
        }
        if(isPaused){
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else{
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
