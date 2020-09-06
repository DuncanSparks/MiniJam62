using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
	[SerializeField]
	AudioClip walkSound;

    void Attack()
	{
		GetComponentInParent<Player>().Attack();
	}

	public void PlayWalkSound()
	{
		Controller.Singleton.PlaySoundOneShot(walkSound, Random.Range(0.96f, 1.04f), 0.3f);
	}
}
