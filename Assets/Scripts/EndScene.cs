using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    void Start()
    {
        GameUI.Singleton.EnableUI(false);
    }
}
