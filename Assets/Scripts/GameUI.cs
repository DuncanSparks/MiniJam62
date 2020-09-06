using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    static GameUI singleton = null;
    public static GameUI Singleton { get => singleton; set => singleton = value; }

    [SerializeField]
    AudioClip colorSound;

    [SerializeField]
    AudioClip modeSound;

    [SerializeField]
    AudioClip transitionSoundIn;

    [SerializeField]
    AudioClip transitionSoundOut;

    [SerializeField]
    GameObject healthbar;

    [SerializeField]
    GameObject colorIndicator;

    [SerializeField]
    GameObject colorText;

    [SerializeField]
    GameObject aimModeText;

    [SerializeField]
    GameObject fade;

    [SerializeField]
    GameObject transition1;

    [SerializeField]
    GameObject transition2;

    [SerializeField]
    Color[] colors;

    readonly string[] colorNames = {
        "Red",
        "Blue",
        "Yellow"
    };

    Player.PaintColor currentColor = Player.PaintColor.Red;
    public Player.PaintColor CurrentColor { get => currentColor; }

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetHealth(int health, int maxhealth)
    {
        healthbar.GetComponent<Image>().fillAmount = (float)health / (float)maxhealth;
    }

    public void SetIndicatorColor(Player.PaintColor color)
    {
        Controller.Singleton.PlaySoundOneShot(colorSound, 1f, 0.4f);

        colorIndicator.GetComponent<Image>().color = colors[(int)color];
        var text = colorText.GetComponent<TextMeshProUGUI>();
        text.color = colors[(int)color];
        text.text = colorNames[(int)color];
        currentColor = color;
    }

    public void SetAimMode(bool mouse)
    {
        Controller.Singleton.PlaySoundOneShot(modeSound);
        aimModeText.GetComponent<TextMeshProUGUI>().text = $"Aim mode: {(mouse ? "Mouse" : "Direction")}";
    }

    public void Fade(Color color, float duration, bool fadeout)
    {
        fade.GetComponent<Image>().color = new Color(color.r, color.g, color.b);
        var animator = GetComponent<Animator>();
        animator.speed = 1f / duration;
        animator.Play(fadeout ? "Fadeout" : "Fadein");
    }

    public void Transition(bool fadeout)
    {
        Controller.Singleton.PlaySoundOneShot(fadeout ? transitionSoundOut : transitionSoundIn, volume: 0.38f);
        GetComponent<Animator>().Play(fadeout ? "TransitionOut" : "TransitionIn");
    }
}
