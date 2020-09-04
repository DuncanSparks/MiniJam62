using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGlob : MonoBehaviour
{
    bool destroyed = false;

    [SerializeField]
    float startAngle = 0f;

    [SerializeField]
    float speed = 10f;

    [SerializeField]
    GameObject particles = null;

    void Start()
    {
        Invoke(nameof(DestroySelf), 3.0f);

        transform.localRotation = Quaternion.AngleAxis(startAngle, Vector3.up);
        var rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        rb.AddForce(Vector3.up * (speed / 2f), ForceMode.Impulse);
        transform.localRotation *= Quaternion.AngleAxis(180f, Vector3.up);
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag != "Player" && !destroyed)
        {
            DestroyAnimation();
        }
    }

    void DestroyAnimation()
    {
        var parts = Instantiate(particles, transform.position + new Vector3(0, 0.6f, 0), Quaternion.AngleAxis(-90f, new Vector3(1, 0, 0)));
        GetComponent<Animator>().Play("PaintGlob_Destroy");
        Invoke(nameof(DestroySelf), 1.0f);
        destroyed = true;
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
