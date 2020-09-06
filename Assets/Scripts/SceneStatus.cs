using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStatus : MonoBehaviour
{
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
		Controller.Singleton.AddClearedScene(SceneManager.GetActiveScene().name);
	}
}
