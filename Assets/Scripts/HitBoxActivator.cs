using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxActivator : MonoBehaviour
{
	// BRIAN WHY IS YOUR SCRIPT SO DAMN CLAUSTROPHOBIA INDUCING AAAAGH
	[SerializeField]
	AudioClip attackSound;

	[SerializeField]
	float attackSoundVolume = 1f;

    [SerializeField]
    GameObject hitBox;
    Animator animator;
    public void Activate()
    {
		Controller.Singleton.PlaySoundOneShot(attackSound, Random.Range(0.95f, 1.05f), attackSoundVolume);
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
