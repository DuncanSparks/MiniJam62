using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPanelDetector : MonoBehaviour
{
    [SerializeField]
    PaintPanel[] targetPanels;

    [Space(20)]
    [SerializeField]
    Event conditionsMetEvent;

    bool conditionsMet = false;

    void Update()
    {
        if (!conditionsMet)
        {
            bool satisfied = true;
            foreach (PaintPanel panel in targetPanels)
            {
                if (panel.CurrentColor != (int)panel.RequiredAnyColor)
                {
                    satisfied = false;
                    break;
                }
            }

            if (satisfied)
            {
                Debug.Log("CONDITIONS MET");
                conditionsMet = true;
                conditionsMetEvent.Invoke();
            }
        }
    }

    public void Test()
    {
        Debug.Log("CONDITIONS MET PART TWO");
    }
}
