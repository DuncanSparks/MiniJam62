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
    GameObject creditsBackground;

    [SerializeField]
    GameObject creditsText;

    [SerializeField]
    GameObject[] buttons;

    bool creditsOpen = false;

    void Start()
    {
        Invoke(nameof(Fadein), 1f);
    }

    void Update()
    {
        if (creditsOpen && Input.anyKeyDown)
        {
            Controller.Singleton.PlaySoundOneShot(clickSound, Random.Range(0.95f, 1.05f));
            OpenCredits(false);
        }
    }

    void Fadein()
    {
        Controller.Singleton.PlaySoundOneShot(introSound, volume: 0.6f);
        GetComponent<Animator>().Play("Fadein");
        Invoke(nameof(Fadein2), 1f);
    }

    void Fadein2()
    {
        ActivateButtons(true);
    }

    void ActivateButtons(bool activate)
    {
        foreach (GameObject but in buttons)
        {
            but.GetComponent<Button>().interactable = activate;
        }
    }

    public void Hover()
    {
        Controller.Singleton.PlaySoundOneShot(hoverSound, Random.Range(0.9f, 1.1f), 0.7f);
    }

    public void ClickStart()
    {
        Controller.Singleton.PlaySoundOneShot(clickSound, Random.Range(0.95f, 1.05f));
        ActivateButtons(false);
        Invoke(nameof(ClickStart2), 0.5f);
    }

    void ClickStart2()
    {
        Controller.Singleton.ChangeScene(startScene.name, "StartEntrance");
    }

    public void ClickCredits()
    {
        Controller.Singleton.PlaySoundOneShot(clickSound, Random.Range(0.95f, 1.05f));
        OpenCredits(true);
    }

    public void ClickExit()
    {
        Controller.Singleton.PlaySoundOneShot(clickSound, Random.Range(0.95f, 1.05f));
        Controller.Singleton.PlaySoundOneShot(introSound, volume: 0.6f);
        GetComponent<Animator>().Play("Fadeout");
        Invoke(nameof(ClickExit2), 0.8f);
    }

    void ClickExit2()
    {
        #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
         #else
             Application.Quit();
         #endif
    }

    void OpenCredits(bool open)
    {
        creditsBackground.SetActive(open);
        creditsText.SetActive(open);
        creditsOpen = open;
    }
}
