using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    Object startScene;

    [SerializeField]
    AudioClip introSound;

    [SerializeField]
    AudioClip hoverSound;

    [SerializeField]
    AudioClip clickSound;

    [SerializeField]
    GameObject[] buttons;

    void Start()
    {
        Invoke(nameof(Fadein), 1f);
    }

    void Fadein()
    {
        Controller.Singleton.PlaySoundOneShot(introSound, volume: 0.6f);
        GetComponent<Animator>().Play("Fadein");
    }

    public void Hover()
    {
        Controller.Singleton.PlaySoundOneShot(hoverSound, Random.Range(0.9f, 1.1f), 0.7f);
    }

    public void ClickStart()
    {
        Controller.Singleton.PlaySoundOneShot(clickSound, Random.Range(0.95f, 1.05f));
        foreach (GameObject but in buttons)
        {
            but.GetComponent<Button>().enabled = false;
        }

        Invoke(nameof(ClickStart2), 0.5f);
    }

    void ClickStart2()
    {
        Controller.Singleton.ChangeScene(startScene.name, "Start");
    }
}
