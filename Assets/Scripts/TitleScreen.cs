using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    AudioClip introSound;

    void Start()
    {
        Invoke(nameof(Fadein), 1f);
    }

    void Fadein()
    {
        Controller.Singleton.PlaySoundOneShot(introSound, volume: 0.6f);
        GetComponent<Animator>().Play("Fadein");
    }
}
