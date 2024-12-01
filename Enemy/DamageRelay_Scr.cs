using UnityEngine;

public class DamageRelay_Scr : MonoBehaviour, IDamageable
{
    public Transform damageable;

    //TODO: добавить искры тут и выключить в основном объекте
    public void TakeDamage(int damage, Vector3 dmgTakenFromPos)
    {
        damageable.GetComponent<IDamageable>().TakeDamage(damage, dmgTakenFromPos);
    }
}
