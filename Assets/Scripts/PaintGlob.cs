﻿using System.Collections;
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
    int damage = 1;

    public int Damage { get => damage; }

    [SerializeField]
    GameObject[] particles;

    [SerializeField]
    GameObject parent;

	[SerializeField]
	AudioClip[] landSounds;

    Player.PaintColor color;
    public Player.PaintColor Color { get => color; set => color = value; }

    void Start()
    {
        Destroy(gameObject, 3.0f);

        transform.localRotation = Quaternion.AngleAxis(startAngle, Vector3.up);
        if (color == Player.PaintColor.Yellow)
        {
            transform.localRotation = Quaternion.Euler(90f, transform.localRotation.y, transform.localRotation.z);
        }

        var rb = GetComponent<Rigidbody>();
        if (color != Player.PaintColor.Yellow)
        {
            rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(-(parent.transform.position - transform.position).normalized * 3f, ForceMode.Impulse);
        }

        if (color != Player.PaintColor.Blue)
        {
            rb.AddForce(Vector3.up * (speed / (color == Player.PaintColor.Yellow ? Random.Range(4f, 5f) : Random.Range(1.5f, 2.5f))), ForceMode.Impulse);
        }
        
        transform.localRotation *= Quaternion.AngleAxis(180f, Vector3.up);
    }

    void OnCollisionEnter(Collision coll)
    {
        if (!destroyed)
        {
            GetComponentInChildren<MeshCollider>().enabled = false;
            DestroyAnimation();
        }
    }

    void DestroyAnimation()
    {
		Controller.Singleton.PlaySoundOneShot(landSounds[Random.Range(0, landSounds.Length)], Random.Range(0.95f, 1.05f), 0.25f);
        var parts = Instantiate(particles[(int)color], transform.position + new Vector3(0, 0.6f, 0), Quaternion.AngleAxis(-90f, new Vector3(1, 0, 0)));
        Destroy(parts, 2.0f);
        GetComponentInChildren<Animator>().Play("PaintGlob_Destroy");
        destroyed = true;
        Destroy(gameObject, 1.0f);
    }
}
