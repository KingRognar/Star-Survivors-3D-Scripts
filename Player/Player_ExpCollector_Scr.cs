using UnityEngine;

public class Player_ExpCollector_Scr : MonoBehaviour
{
    SphereCollider sCollider;

    void Start()
    {
        sCollider = GetComponent<SphereCollider>();
        sCollider.radius = Player_Stats_Scr.Ship.ExpCollectorRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<ExpShard_Scr>().GetCaught();
    }
}
