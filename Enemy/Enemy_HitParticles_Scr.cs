using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HitParticles_Scr : MonoBehaviour
{
    //[SerializeField] private List<Sprite> particleVariations = new List<Sprite>();
    private float minSpeed = 1f; private float maxSpeed = 2f; private float curSpeed;
    private float maxScale = 3f;
    private float lifetime = 1f;
    public Vector3 directionToMove;

    //private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Initialize();
    }
    private void Update()
    {
        transform.position += directionToMove * curSpeed * Time.deltaTime;
        GetComponent<Renderer>().material.color -= new Color(0, 0, 0, 1) * Time.deltaTime / lifetime;
        //spriteRenderer.color -= new Color (0,0,0,1) * Time.deltaTime / lifetime;
    }


    private void Initialize()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //gameObject.GetComponent<SpriteRenderer>().sprite = particleVariations[UnityEngine.Random.Range(0, particleVariations.Count)];
        transform.localScale = transform.localScale * UnityEngine.Random.Range(1, maxScale);
        curSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        lifetime = UnityEngine.Random.Range(0.5f, 1f);
        Destroy(gameObject, lifetime);
    }
}
