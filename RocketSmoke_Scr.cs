using UnityEngine;
using UnityEngine.VFX;

public class RocketSmoke_Scr : MonoBehaviour
{
    [HideInInspector] public bool rocketIsDestroyed = false;
    private bool setToDestroy = false;
    VisualEffect vs;
    float t = 1;

    private void Start()
    {
        vs = GetComponentInChildren<VisualEffect>();
    }
    private void Update()
    {
        if (!rocketIsDestroyed)
            return;

        if (!setToDestroy)
        {
            Destroy(gameObject, 1f);
            setToDestroy = true;
        }

        t = Mathf.MoveTowards(t, 0, Time.deltaTime);
        vs.SetFloat("Lifetime", t);
    }
}
