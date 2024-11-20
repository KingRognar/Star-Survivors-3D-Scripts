using UnityEngine;
using UnityEngine.VFX;

public class Boss_1_BflLazer_Scr : MonoBehaviour
{
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private float size;
    [SerializeField] private float vfxTime; private float startTime = 0;
    private CapsuleCollider capsuleCollider;

    private void OnEnable() //TODO: передвинуть неоторые вещи в awake или start
    {
        size = GetComponent<VisualEffect>().GetFloat("Size");
        vfxTime = GetComponent<VisualEffect>().GetFloat("Time");
        capsuleCollider = GetComponent<CapsuleCollider>();
        startTime = Time.time + vfxTime / 3;
        capsuleCollider.enabled = false;
    }

    void Update()
    {
        if (!capsuleCollider.enabled && startTime < Time.time)
        {
            capsuleCollider.enabled = true;
        }
        capsuleCollider.radius = size * scaleCurve.Evaluate((Time.time - startTime) / (vfxTime * 2 / 3));
    }
}
