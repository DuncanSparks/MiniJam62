using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    [SerializeField]
	AudioClip music;

    void Start()
    {
		GameUI.Singleton.EnableUI(true);
		Invoke(nameof(PlayMusic), 1f);
    }

	void PlayMusic()
	{
		Controller.Singleton.PlayMusic(music);
	}
}
