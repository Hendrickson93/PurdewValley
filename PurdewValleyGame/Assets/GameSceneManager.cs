using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;  
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;

    private void Awake()
    {
        instance = this;
    }

    string currentScene;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void SwitchScene(string to)
    {
        SceneManager.LoadScene(to, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(currentScene);
        currentScene = to;
    }



}
