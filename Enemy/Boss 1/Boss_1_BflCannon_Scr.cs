using UnityEngine;

public class Boss_1_BflCannon_Scr : MonoBehaviour, IDamageable
{
    private int curHp; [SerializeField] private int maxHp = 1000;
    [SerializeField] private UI_BossHP_Bar_Scr uiBossHp;


    public void Start()
    {
        curHp = maxHp;
    }

    public void TakeDamage(int damage, Vector3 dmgTakenFromPos)
    {
        curHp -= damage;

        Enemy_Flash_Scr[] flashes = GetComponentsInChildren<Enemy_Flash_Scr>();
        foreach (Enemy_Flash_Scr flash in flashes)
            flash.StartFlash();
        //GetComponent<Enemy_HitEffect_Scr>().SpawnParticles(dmgTakenFromPos);
        uiBossHp.UpdateHPBar(curHp, maxHp);     //TODO: instantiate HP bar by yourself

        if (curHp <= 0)
            Explode();
    }

    //TODO: �������� ���������� ������ � ������� 2 ���� (����� ����� ����������)

    private void Explode()
    {
        Destroy(gameObject);
    }
}
