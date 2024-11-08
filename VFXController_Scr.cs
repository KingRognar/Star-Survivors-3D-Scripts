using UnityEngine;
using UnityEngine.VFX;

public class VFXController_Scr : MonoBehaviour
{
    private float lifeTIme = 1.5f; private float destroyTime;

    private void Awake()
    {
        destroyTime = Time.time + lifeTIme;
    }

    private void Update()
    {
        if (destroyTime < Time.time)
            Destroy(gameObject);
    }
}
