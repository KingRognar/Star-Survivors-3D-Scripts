using Unity.VisualScripting;
using UnityEngine;

public class Boss_1_BflCannon_Scr : MonoBehaviour, IDamageable
{
    private int curHp; [SerializeField] private int maxHp = 1000;
    private UI_BossHP_Bar_Scr uiBossHp;
    [SerializeField] private GameObject uiBossHp_prefab;
    [SerializeField] private GameObject destroyedBflPref;


    public void Start()
    {
        Transform canvasTransform = GameObject.Find("Canvas").transform;
        uiBossHp = Instantiate(uiBossHp_prefab, canvasTransform).GetComponent<UI_BossHP_Bar_Scr>();
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

    //TODO: включать коллайдеры только с началом 2 фазы (когда пушка выдвинется)

    private void Explode()
    {
        GameObject destroy = Instantiate(destroyedBflPref, transform.parent);
        destroy.transform.localPosition = new(-0.38f, -18.33f, 1.62f);
        destroy.transform.localRotation = Quaternion.Euler(0, 180, 0);
        Destroy(gameObject);
    }
}
