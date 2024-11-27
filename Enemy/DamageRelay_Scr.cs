using UnityEngine;

public class DamageRelay_Scr : MonoBehaviour, IDamageable
{
    public Transform damageable;
    public void TakeDamage(int damage, Vector3 dmgTakenFromPos)
    {
        damageable.GetComponent<IDamageable>().TakeDamage(damage, dmgTakenFromPos);
    }
}
