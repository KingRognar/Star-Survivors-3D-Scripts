using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Flash_Scr : MonoBehaviour
{
    private MeshRenderer meshRendrer;
    private Material shaderMaterial;
    [SerializeField] private float flashTime = 0.2f;
    [SerializeField] private float fullOpacityDuration = 0.05f;
    //[SerializeField] private float flashOpacity = 1f;

    private CancellationTokenSource cts;
    private CancellationToken token;


    //TODO: переименовать и перенести в папку скриптов для шейдеров

    public void Awake()
    {
        meshRendrer = GetComponent<MeshRenderer>();
        foreach (Material mat in meshRendrer.materials)
        {
            if (mat.name.EndsWith("Flash_Mat (Instance)"))
                shaderMaterial = mat;
        }
    }

    public void StartFlash()
    {
        cts = new CancellationTokenSource();
        token = cts.Token;
        if (shaderMaterial == null)
        {
            Debug.Log("Не нашёл Flash_Mat", this);
            return;
        }

        _ = FlashTask(token);
    }
    private async Task FlashTask(CancellationToken token)
    {
        float t = 0;
        float opacityDecreaseTime = flashTime - fullOpacityDuration;

        while (t < flashTime)
        {
            if (destroyCancellationToken.IsCancellationRequested || token.IsCancellationRequested)
            {
                shaderMaterial.SetFloat("_FlashOpacity", 0);

                return;
            }


            if (t > fullOpacityDuration)
                shaderMaterial.SetFloat("_FlashOpacity", Mathf.Lerp(1, 0, (t / opacityDecreaseTime)));

            t += Time.deltaTime;
            await Task.Yield();
        }
    }


    private void OnDestroy()
    {
        if (cts != null)
            cts.Cancel();
    }
}

