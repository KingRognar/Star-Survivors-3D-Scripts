using DG.Tweening;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        if (isBarraging)
            Barrage();
    }

    public void StartBarrage(int rocketsToShoot)
    {
        if (isBarraging) return;

        barrageNum = rocketsToShoot;
        isBarraging = true;
        DOTween.To(() => tempT, x => tempT = x, rocketsToShoot, rocketsToShoot * launchDelay);
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
