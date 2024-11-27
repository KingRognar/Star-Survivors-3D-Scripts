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
        DebriesMaker_Scr.instance.HitFromPosAndDir(transform.position, dmgTakenFromPos - transform.position); //TODO: исправить
        //GetComponent<Enemy_HitEffect_Scr>().SpawnParticles(dmgTakenFromPos);
        uiBossHp.UpdateHPBar(curHp, maxHp);

        if (curHp <= 0)
            Explode();
    }

    //TODO: instantiate HP bar by yourself
    //TODO: включать коллайдеры только с началом 2 фазы (когда пушка выдвинется)

    private void Explode()
    {
        Destroy(gameObject);
    }
}
