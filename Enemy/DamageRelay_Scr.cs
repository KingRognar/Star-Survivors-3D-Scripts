using UnityEngine;

public class DamageRelay_Scr : MonoBehaviour, IDamageable
{
    public Transform damageable;

    //TODO: �������� ����� ��� � ��������� � �������� �������
    public void TakeDamage(int damage, Vector3 dmgTakenFromPos)
    {
        damageable.GetComponent<IDamageable>().TakeDamage(damage, dmgTakenFromPos);
        Collider collider = GetComponent<Collider>();
        Vector3 collisionPoint = collider.ClosestPoint(dmgTakenFromPos);
        DebriesMaker_Scr.instance.HitFromPosAndDir(collisionPoint, dmgTakenFromPos - collisionPoint);
    }
}
