using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
	bool disabled = false;

    public void DisableForceField()
	{
		if (!disabled)
		{
			GetComponentInChildren<ParticleSystem>().Stop();
			GetComponent<BoxCollider>().enabled = false;
			disabled = true;
		}
	}
}
