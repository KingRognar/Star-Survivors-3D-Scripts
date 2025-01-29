using System;
using System.Threading.Tasks;
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
    private bool movedAtAppearence = false; private Vector3 startPos, endPos;
    #endregion
    #region Phase 2 variables
    private bool phase2InitMovementDone = false;
    private float moveT = 0; [SerializeField] private Vector3 phase2Pos;
    private float bflShootInterval = 10f; private float nextBflShoot = -1; private float bflShootTime;
    private float headTurningSpeed = 25f;
    [SerializeField] private Boss_1_RocketLauncher_Scr leftRocketLauncher;
    #endregion

    // start pos 0, -17.5, 9
    //TODO: BFL 
    //TODO: Animations
    //TODO: Visualize damage delt
    //TODO: Rocket launchers
    //TODO: Boss HP Bar(s)
    //TODO: прибраться
    //TODO: постепенно замедлить БГ до 18 (багает чёт)
    //TODO: переделать на tween'ы - 100%


    private void Start()
    {
        //transform.position = new(0, -17.5f, 9);
        transform.position = new(0, -100, 90);
        bodyTransforms.head.rotation = Quaternion.Euler(0, 90, 0);
        BackgroundManager_Scr.AddToSpawnedElements(gameObject);
    }
    private void Update()
    {
        if (phase != Phase.phase_2)
        {
            if (!movedAtAppearence) MoveAtAppearence();
            return;
        }


        Phase2Behaviour();

        if (Input.GetKeyDown(KeyCode.H))
            vfxTransforms.bflShotVfx.gameObject.SetActive(!vfxTransforms.bflShotVfx.gameObject.activeInHierarchy);
    }


    private void MoveAtAppearence()
    {
        DOTween.To(() => moveT, x => moveT = x, 1f, 6f);
        startPos = transform.position;
        endPos = new(0, -17.5f, 9);
        _ = MoveTask();
        movedAtAppearence = true;
    }
    private async Task MoveTask()
    {
        while (moveT != 1 && !destroyCancellationToken.IsCancellationRequested)
        {
            transform.position = Vector3.Lerp(startPos, endPos, moveT);
            BackgroundManager_Scr.instance.backgroundSpeed = Mathf.Lerp(45, 18, moveT);
            await Task.Yield();
        }
        moveT = 0;
        TurretsStartAttack(); 
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
        if (!phase2InitMovementDone)
            return;

        Phase2Attack();
        BFLPlayerTracking();

    }
    private void BFLPlayerTracking()
    {
        Vector3 adjustedPlayerPos = Player_Stats_Scr.instance.transform.position - bodyTransforms.head.position; //TODO: handle destroyed player
        adjustedPlayerPos.y = 0;
        adjustedPlayerPos = Vector3.Cross(Vector3.up, adjustedPlayerPos);
        bodyTransforms.head.rotation = Quaternion.RotateTowards(bodyTransforms.head.rotation, Quaternion.LookRotation(adjustedPlayerPos, Vector3.up), headTurningSpeed * Time.deltaTime);
        //bodyTransforms.head.rotation = Quaternion.LookRotation(adjustedPlayerPos, Vector3.up);
    }
    private void Phase2Attack()
    {
        if (bodyTransforms.cannon == null)
            return;

        if (nextBflShoot <= Time.time)
        {
            vfxTransforms.bflShotVfx.gameObject.SetActive(true);
            headTurningSpeed = 15f;
            nextBflShoot += bflShootInterval;
        }
        if (nextBflShoot - bflShootInterval + bflShootTime < Time.time)
        {
            vfxTransforms.bflShotVfx.gameObject.SetActive(false);
            headTurningSpeed = 25f;
        }
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
        animSequence.AppendCallback(OnPhase2MovementEnd);

        moveT = 0;
    }
    void OnPhase2MovementEnd()
    {
        phase2InitMovementDone = true;
        leftRocketLauncher.StartBarrage(3);
    }
    void ExtendBfl()
    {
        GetComponent<Animator>().SetBool("BFL is extending", true);
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
