using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStatus : MonoBehaviour
{
    [SerializeField]
    bool playSound = true;

    [SerializeField]
    Event sceneClearedEvent;

    void Start()
    {
        if (Controller.Singleton.IsSceneCleared(SceneManager.GetActiveScene().name))
        {
            sceneClearedEvent.Invoke();
        }
    }

    public void ClearScene()
    {
        if (playSound)
        {
            GetComponent<AudioSource>().Play();
        }

        Controller.Singleton.AddClearedScene(SceneManager.GetActiveScene().name);
    }
}
