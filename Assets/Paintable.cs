using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintable : MonoBehaviour
{
    [SerializeField]
    bool playerColors = true;
    [SerializeField]
    bool enemyColors = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other) {
        if ((playerColors && other.gameObject.tag == "PlayerProjectile") ||
            (enemyColors && other.gameObject.tag == "EnemyHitbox"))
        {
            GetComponent<Renderer>().material = other.gameObject.GetComponentInChildren<Renderer>().material;  
            Controller.Singleton.ShowComicText("Splat", other.gameObject.transform.position, Camera.main);
        }
    }
}
