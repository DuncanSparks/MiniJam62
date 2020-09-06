using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [SerializeField]
    float deathPlaneY = -20f;

    void Start()
    {
        var collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(50, 2, 50);
        collider.transform.position = new Vector3(0, deathPlaneY, 0);
    }

    void OnTriggerEnter(Collider collider)
    {
        Controller.Singleton.Respawn();
    }
}
