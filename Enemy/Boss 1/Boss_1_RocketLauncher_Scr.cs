using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boss_1_RocketLauncher_Scr : MonoBehaviour, IDamageable
{
    public float maxHp = 500f, curHp;
    [SerializeField] private float launchDelay;
    private bool isBarraging = false;

    [SerializeField] private Transform[] barrels = new Transform[5];
    [SerializeField] private GameObject rocketPrefab;

    void Start()
    {
        barrels[0] = transform.GetChild(0);
        barrels[1] = transform.GetChild(1);
        barrels[2] = transform.GetChild(2);
        barrels[3] = transform.GetChild(3);
        barrels[4] = transform.GetChild(4);
        curHp = maxHp;
    }

    public float StartBarrage(int rocketsToShoot, float initialDelay = 0f)
    {
        if (isBarraging) return launchDelay;

        //barrageNum = rocketsToShoot;
        isBarraging = true;
        //DOTween.To(() => tempT, x => tempT = x, rocketsToShoot, rocketsToShoot * launchDelay);

        Sequence barrageSequence = DOTween.Sequence();
        barrageSequence.AppendInterval(initialDelay);
        for (int i = 0; i < rocketsToShoot; i++)
        {
            int curBarrel = i;
            barrageSequence.AppendCallback(() => InstantiateRocket(curBarrel));
            barrageSequence.AppendInterval(launchDelay);
        }
        barrageSequence.AppendCallback(() => { isBarraging = false; });
        return barrageSequence.Duration();
    }
    private void InstantiateRocket(int barrelNum)
    {
        if (barrels[barrelNum] == null) return;
        Instantiate(rocketPrefab, barrels[barrelNum].position, barrels[barrelNum].rotation);
    }

    public void TakeDamage(int damage, Vector3 dmgTakenFromPos)
    {
        if (curHp <= 0)
            return;

        curHp -= damage;

        GetComponent<Enemy_Flash_Scr>().StartFlash();
        Collider collider = GetComponent<Collider>();
        Vector3 collisionPoint = collider.ClosestPoint(dmgTakenFromPos);
        DebriesMaker_Scr.instance.HitFromPosAndDir(collisionPoint, dmgTakenFromPos - collisionPoint);
        //uiBossHp.UpdateHPBar(curHp, maxHp);     //TODO: instantiate HP bar by yourself

        if (curHp <= 0)
            Explode();
    }
    private void Explode()
    {
        DebriesMaker_Scr.instance.ExplodeOnPos(transform.position);
        Destroy(gameObject);
    }

}
