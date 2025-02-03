using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Xsl;
using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;

public class Boss_1_Scr : MonoBehaviour
{
    private Phase phase = Phase.phase_1;
    [SerializeField] private BodyTransforms bodyTransforms;
    [SerializeField] private VFXTransforms vfxTransforms;

    #region Phase 1 Variables
    private bool[] turretsDestroyed = new bool[3] { false, false, false };
    #endregion
    #region Phase 2 variables
    private bool phase2InitMovementDone = false; [SerializeField] private Vector3 phase2Pos;
    private float attackInterval = 8f; private float bflShootTime; private bool readyToAttack = true;
    private float headTurningSpeed = 25f;
    [SerializeField] private Boss_1_RocketLauncher_Scr leftRocketLauncher;
    [SerializeField] private Boss_1_RocketLauncher_Scr rightRocketLauncher;
    private float curTotalHp, maxTotalHp;
    private bool isExploding = false;
    #endregion
    [SerializeField] private List<GameObject> destroyedVfxList = new List<GameObject>();
    private bool isMovingAway = false;

    // start pos 0, -17.5, 9
    //TODO: BFL 
    //TODO: Visualize damage delt
    //TODO: Rocket launchers
    //TODO: Boss HP Bar(s)
    //TODO: прибраться
    //TODO: add IDamageable and HP bars to rocketLaunchers


    private void Start()
    {
        //transform.position = new(0, -17.5f, 9);

        maxTotalHp = GetMaxTotalHp();
        curTotalHp = maxTotalHp;
        transform.position = new(0, -100, 90);
        bodyTransforms.head.rotation = Quaternion.Euler(0, 90, 0);
        BackgroundManager_Scr.AddToSpawnedElements(gameObject);
        MoveAtAppearence();
    }
    private void Update() //TODO: организовать покрасивее
    {
        if (phase != Phase.phase_2)
            return;

        if (isMovingAway)
        {
            transform.position += BackgroundManager_Scr.instance.backgroundSpeed * Time.deltaTime * new Vector3(0, 0, -1);
            return;
        }

        Phase2Behaviour();
    }


    private float GetMaxTotalHp()
    {
        float totalHp = 0;
        totalHp += bodyTransforms.cannon.GetComponent<Boss_1_BflCannon_Scr>().maxHp;
        totalHp += leftRocketLauncher.maxHp * 2;
        return totalHp;
    }
    private float GetCurTotalHp()
    {
        float totalHp = 0;
        if (bodyTransforms.cannon != null)
            totalHp += bodyTransforms.cannon.GetComponent<Boss_1_BflCannon_Scr>().curHp;
        if (leftRocketLauncher != null)
            totalHp += leftRocketLauncher.curHp;
        if (rightRocketLauncher != null)
            totalHp += rightRocketLauncher.curHp;
        return totalHp;
    }
    private void MoveAtAppearence()
    {
        Vector3 endPos = new(0, -17.5f, 9f);
        transform.DOMove(endPos, 6f).SetEase(Ease.InOutQuad).onComplete = () => { TurretsStartAttack(); };
        DOTween.To(() => BackgroundManager_Scr.instance.backgroundSpeed, x => BackgroundManager_Scr.instance.backgroundSpeed = x, 18, 6f);
    }
    public void TurretsStartAttack()
    {
        if (bodyTransforms.leftTurret != null)
            bodyTransforms.leftTurret.GetComponent<Boss_1_Turret_Scr>().isShooting = true;
        if (bodyTransforms.rightTurret != null)
            bodyTransforms.rightTurret.GetComponent<Boss_1_Turret_Scr>().isShooting = true;
        if (bodyTransforms.middleTurret != null)
            bodyTransforms.middleTurret.GetComponent<Boss_1_Spread_Scr>().isShooting = true;
    }    

    private void Phase2Behaviour()
    {
        if (!phase2InitMovementDone || isExploding)
            return;

        Phase2Attack();
        BFLPlayerTracking();
        curTotalHp = GetCurTotalHp();
        if (curTotalHp <= 0)
            ExplosionSequence();
    }
    private void BFLPlayerTracking()
    {
        Vector3 playerPos;
        if (Player_Stats_Scr.instance != null)
            playerPos = Player_Stats_Scr.instance.transform.position;
        else
            playerPos = new(0, 0, -10f);
        Vector3 adjustedPlayerPos = playerPos - bodyTransforms.head.position; //TODO: handle destroyed player
        adjustedPlayerPos.y = 0;
        adjustedPlayerPos = Vector3.Cross(Vector3.up, adjustedPlayerPos);
        bodyTransforms.head.rotation = Quaternion.RotateTowards(bodyTransforms.head.rotation, Quaternion.LookRotation(adjustedPlayerPos, Vector3.up), headTurningSpeed * Time.deltaTime);
        //bodyTransforms.head.rotation = Quaternion.LookRotation(adjustedPlayerPos, Vector3.up);
    }
    private void Phase2Attack()
    {
        if (!readyToAttack)
            return;

        readyToAttack = false;
        Sequence attackSequence = DOTween.Sequence();
        if (vfxTransforms.bflShotVfx != null)
        {
            attackSequence.AppendCallback(() => {
                vfxTransforms.bflShotVfx.gameObject.SetActive(true);
                headTurningSpeed = 15f;
            });
            attackSequence.AppendInterval(bflShootTime);
            attackSequence.AppendCallback(() => {
                vfxTransforms.bflShotVfx.gameObject.SetActive(false);
                headTurningSpeed = 25f;
            });
        }

        int additionalRockets = (int)(maxTotalHp - curTotalHp) / (int)(maxTotalHp / 3);
        float rocketBarrageTime = 0f;
        if (leftRocketLauncher != null)
        {
            attackSequence.AppendCallback(() => { rocketBarrageTime = leftRocketLauncher.StartBarrage(3 + additionalRockets); });
        }
        if (rightRocketLauncher != null)
        {
            float secondTime = 0f;
            attackSequence.AppendCallback(() => { secondTime = rightRocketLauncher.StartBarrage(3 + additionalRockets, 1f); });
            if (secondTime > rocketBarrageTime)
                rocketBarrageTime = secondTime;
        }
        attackSequence.AppendInterval(rocketBarrageTime + attackInterval);

        attackSequence.AppendCallback(() => { readyToAttack = true; });
    }

    public void TurretIsDestroyed(int turretID)
    {
        turretsDestroyed[turretID] = true;
        CheckTurretsState();
    }
    private void CheckTurretsState()
    {
        foreach (bool turretIsDestroyed in turretsDestroyed)
        {
            if (!turretIsDestroyed)
                return;
        }
        InitializePhase2();
    }
    private void InitializePhase2()
    {
        phase = Phase.phase_2;

        bflShootTime = vfxTransforms.bflShotVfx.GetComponent<VisualEffect>().GetFloat("Time");

        float animTime = 6f;
        Sequence animSequence = DOTween.Sequence();
        animSequence.Append(bodyTransforms.head.DORotate(Quaternion.LookRotation(Vector3.left, Vector3.up).eulerAngles, animTime).SetEase(Ease.InOutQuad));
        animSequence.Join(transform.DOMove(phase2Pos, animTime).SetEase(Ease.InOutQuad));
        animSequence.InsertCallback(animTime / 2, ExtendBfl);
        animSequence.AppendCallback(() => { phase2InitMovementDone = true; });
    }
    private void ExtendBfl()
    {
        GetComponent<Animator>().SetBool("BFL is extending", true);
    }

    private void ExplosionSequence() //TODO: взрывать все выпущенные ракеты
    {
        isExploding = true;
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() => {
            _ = DebriesMaker_Scr.instance.BigObjectExplosions(bodyTransforms.head.GetChild(1).GetComponent<Collider>(),
            bodyTransforms.head, 40, 40, 50);
        });
        sequence.AppendInterval(2.5f);
        sequence.AppendCallback(() => { DebriesMaker_Scr.instance.ScreenFlash(); });
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() => { 
            Destroy(bodyTransforms.head.gameObject);
            gameObject.GetComponent<Animator>().enabled = false;
            foreach (GameObject gm in destroyedVfxList)
                gm.SetActive(true);
            isMovingAway = true;
        });
    }

    private enum Phase
    {
        phase_1,
        phase_2,
    }
    [Serializable] private class BodyTransforms
    {
        public Transform head;
        public Transform cannon;
        public Transform leftTurret;
        public Transform rightTurret;
        public Transform middleTurret;
    }
    [Serializable] private class VFXTransforms
    {
        public Transform bflShotVfx;
    }
}
