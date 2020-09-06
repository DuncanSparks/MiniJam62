using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndGate : MonoBehaviour
{
    [SerializeField]
    string[] conditions;

    [Space(20)]
    [SerializeField]
    Event conditionsMetEvent;

    bool[] conditionBools;
    bool activated = false;

    void Start()
    {
        conditionBools = new bool[conditions.Length];
    }

    public void SetBool(int index, bool value)
    {
        if (activated)
        {
            return;
        }

        conditionBools[index] = value;
        if (AllTrue())
        {
            conditionsMetEvent.Invoke();
            activated = true;
        }
    }

    bool AllTrue()
    {
        foreach (bool value in conditionBools)
        {
            if (!value)
            {
                return false;
            }
        }

        return true;
    }
}
