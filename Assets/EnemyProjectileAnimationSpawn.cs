using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAnimationSpawn : MonoBehaviour
{
    [SerializeField]
    Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Activate() {
      enemy.SpawnAttack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
