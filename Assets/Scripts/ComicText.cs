using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComicText : MonoBehaviour
{
    public void DisplayText(string text)
    {
        GetComponentInChildren<TextMeshPro>().text = text;
    }
}
