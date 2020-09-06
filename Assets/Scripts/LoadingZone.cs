using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingZone : MonoBehaviour
{
    [SerializeField]
    Object targetScene;

    [SerializeField]
    string targetLocationObject;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Controller.Singleton.ChangeScene(targetScene.name, targetLocationObject);
        }
    }

    public void ChangeScene() {
        Controller.Singleton.ChangeScene(targetScene.name, targetLocationObject);
    }
}
