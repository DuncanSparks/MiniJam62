using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
	ParticleSystem particles;
	new BoxCollider collider;
	AudioSource sound;

	void Start()
	{
		particles = GetComponentInChildren<ParticleSystem>();
		collider = GetComponent<BoxCollider>();
		sound = GetComponent<AudioSource>();
	}

    public void DisableForceField()
	{
		particles.Stop();
		collider.enabled = false;
		sound.Play();
	}
}
