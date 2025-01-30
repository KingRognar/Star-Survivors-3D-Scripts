using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boss_1_RocketLauncher_Scr : MonoBehaviour
{
    [SerializeField] private float launchDelay;
    private int barrageNum = 5;
    private bool isBarraging = false;
    private float tempT = 0; private int i = 0;

    private Transform[] barrels = new Transform[5];
    [SerializeField] private GameObject rocketPrefab;

    void Start()
    {
        barrels[0] = transform.GetChild(0);
        barrels[1] = transform.GetChild(1);
        barrels[2] = transform.GetChild(2);
        barrels[3] = transform.GetChild(3);
        barrels[4] = transform.GetChild(4);
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
            barrageSequence.AppendCallback(() => { Instantiate(rocketPrefab, barrels[i].position, barrels[i].rotation); });
            barrageSequence.AppendInterval(launchDelay);
        }
        barrageSequence.AppendCallback(() => { isBarraging = false; });
        return barrageSequence.Duration();
    }
    private void Barrage()
    {
        if (tempT <= i) return;
        if (barrageNum == i)
        {
            isBarraging = false;
            tempT = 0;
            i = 0;
        }

        Instantiate(rocketPrefab, barrels[i].position, barrels[i].rotation);
        i++;
    }
}
