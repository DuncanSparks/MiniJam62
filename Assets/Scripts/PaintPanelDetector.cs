using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPanelDetector : MonoBehaviour
{
    // [SerializeField]
    PaintPanel[] targetPanels;

    [Space(20)]
    [SerializeField]
    Event conditionsMetEvent;

    bool conditionsMet = false;

    void Start() {
        targetPanels = GetComponentsInChildren<PaintPanel>();
    }

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
                conditionsMet = true;
                foreach (PaintPanel panel in targetPanels)
                {
                    panel.CanBePainted = false;
                }

                conditionsMetEvent.Invoke();
            }
        }
    }

	public void SolveAllPanels()
	{
		conditionsMet = true;
		foreach (PaintPanel panel in GetComponentsInChildren<PaintPanel>())
		{
			panel.Solve();
			panel.CanBePainted = false;
		}
	}
}
