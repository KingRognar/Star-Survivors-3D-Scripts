using UnityEngine;

interface IDamageable
{
    public void TakeDamage(int damage, Vector3 dmgTakenFromPos); //TODO: проверить чтоб все использовали этот интерфейс
}
