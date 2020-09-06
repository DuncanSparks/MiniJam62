using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingZone : MonoBehaviour
{
    [SerializeField]
    Object targetScene;

    [SerializeField]
    Vector3 targetPosition;

    [SerializeField]
    Vector3 targetRotation;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Controller.Singleton.ChangeScene(targetScene.name, targetPosition, targetRotation);
        }
    }
}
