using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxActivator : MonoBehaviour
{
    [SerializeField]
    GameObject hitBox;
    Animator animator;
    public void Activate()
    {
        hitBox.SetActive(true);
    }
    public void Deactivate()
    {
        hitBox.SetActive(false);
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update() {
        if(!animator.GetBool("InAttackState"))
        {
            Deactivate();
        }
    }
}
