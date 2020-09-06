using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnActivate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {
      Invoke(nameof(Disable), 0.1f);
    }

    void Disable() {
      gameObject.SetActive(false);
    }
}
