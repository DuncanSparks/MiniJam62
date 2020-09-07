using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingZone : MonoBehaviour
{
    [SerializeField]
    Object targetScene;
    [SerializeField]
    string targetSceneName;

    [SerializeField]
    string targetLocationObject;

    void OnValidate() {
      if (targetScene)
        targetSceneName = targetScene.name;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Controller.Singleton.ChangeScene(targetSceneName, targetLocationObject);
        }
    }

    public void ChangeScene() {
        Controller.Singleton.ChangeScene(targetSceneName, targetLocationObject);
    }
}
