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
    private bool[] turretsDestroyed = new bool[3] { false, false, false };
    private bool headIsTurnedBack = false;
    private bool movedAtAppearence = false; private Vector3 startPos, endPos;
    private bool movedToNewPos = false; private float moveT = 0; private Vector3 phase1Pos; [SerializeField] private Vector3 phase2Pos;
    private float bflShootInterval = 10f; private float nextBflShoot = -1; private float bflShootTime;
    private float headTurningSpeed = 25f;
    // start pos 0, -17.5, 9

    //TODO: BFL 

    //TODO: Animations

    //TODO: Visualize damage delt

    //TODO: Rocket launchers

    //TODO: Boss HP Bar(s)

    //TODO: прибраться

    //TODO: постепенно замедлить БГ до 18 (багает чёт)
    //TODO: включать пушки только после передвижения при появлении
    //TODO: структурировать переменные, мб namespaces?



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
        bodyTransforms.leftTurret.GetComponent<Boss_1_Turret_Scr>().isShooting = true;
        bodyTransforms.rightTurret.GetComponent<Boss_1_Turret_Scr>().isShooting = true;
        bodyTransforms.middleTurret.GetComponent<Boss_1_Spread_Scr>().isShooting = true;
    }

    private void Phase2Behaviour()
    {
        if (!movedToNewPos)
            MoveToNewPos();
        else
            Phase2Attack();

        if (!headIsTurnedBack)
        {
            BFLTurnBack();
            return;
        }

        BFLPlayerTracking();

    }
    private void BFLTurnBack()
    {
        Quaternion newRotation = Quaternion.RotateTowards(bodyTransforms.head.rotation, Quaternion.LookRotation(Vector3.left, Vector3.up), 25 * Time.deltaTime);
        if (bodyTransforms.head.rotation == newRotation)
        {
            GetComponent<Animator>().SetBool("BFL is extending", true);
            headIsTurnedBack = true;
            return;
        }
        bodyTransforms.head.rotation = newRotation;
    }
    private void BFLPlayerTracking()
    {
        Vector3 adjustedPlayerPos = Player_Stats_Scr.instance.transform.position - bodyTransforms.head.position; //TODO: handle destroyed player
        adjustedPlayerPos.y = 0;
        adjustedPlayerPos = Vector3.Cross(Vector3.up, adjustedPlayerPos);
        bodyTransforms.head.rotation = Quaternion.RotateTowards(bodyTransforms.head.rotation, Quaternion.LookRotation(adjustedPlayerPos, Vector3.up), headTurningSpeed * Time.deltaTime);
        //bodyTransforms.head.rotation = Quaternion.LookRotation(adjustedPlayerPos, Vector3.up);
    }
    private void MoveToNewPos()
    {
        transform.position = Vector3.Lerp(phase1Pos, phase2Pos, moveT);
        moveT = Mathf.MoveTowards(moveT, 1, Time.deltaTime * 0.1f);
        if (moveT == 1)
            movedToNewPos = true;
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
        phase1Pos = transform.position;
        bflShootTime = vfxTransforms.bflShotVfx.GetComponent<VisualEffect>().GetFloat("Time");
        nextBflShoot = Time.time + bflShootInterval;
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
