using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerNextScenePosition;
    public VectorValue playerStorage;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            playerStorage.initialValue = playerNextScenePosition;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
