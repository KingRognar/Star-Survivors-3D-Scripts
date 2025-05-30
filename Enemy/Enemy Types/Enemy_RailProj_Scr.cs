using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy_RailProj_Scr : Enemy_BaseProj_Scr
{
    private Renderer rend;
    private Material shaderMaterial;

    private bool isGlowStarted = false;
    private bool isMovementStarted = false;
    private float currentGlowIntensity = 0f;

    //TEST

    private void Awake()
    {
        Destroy(gameObject, 4f);
        rend = GetComponent<Renderer>();
        shaderMaterial = rend.material;
        shaderMaterial.SetFloat("_GlowIntensity", 0f); 
    }

    protected override void Update()
    {
        if (isGlowStarted && !isMovementStarted)
            RiseGlowIntensity();
        if (isMovementStarted)
            Movement();
    }

    public void startGlow()
    {
        isGlowStarted = true;
    }
    private void RiseGlowIntensity()
    {
        currentGlowIntensity = Mathf.MoveTowards(currentGlowIntensity, 1, 1 * Time.deltaTime);
        shaderMaterial.SetFloat("_GlowIntensity", currentGlowIntensity);
        if (currentGlowIntensity == 1)
        {
            isMovementStarted = true;
            transform.parent = null;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
