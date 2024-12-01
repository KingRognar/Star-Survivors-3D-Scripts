using System;
using UnityEngine;
using UnityEngine.VFX;

public class Boss_1_Scr : MonoBehaviour
{
    private Phase phase = Phase.phase_1;
    [SerializeField] private BodyTransforms bodyTransforms;
    [SerializeField] private VFXTransforms vfxTransforms;
    private bool[] turretsDestroyed = new bool[3] { false, false, false };
    private bool headIsTurnedBack = false;
    private bool movedToNewPos = false; private float moveT = 0; private Vector3 phase1Pos; [SerializeField] private Vector3 phase2Pos;
    private float bflShootInterval = 10f; private float nextBflShoot = -1; private float bflShootTime;
    // start pos 0, -17.5, 9

    //TODO: BFL 

    //TODO: Animations

    //TODO: Visualize damage delt

    //TODO: Rocket launchers

    //TODO: Boss HP Bar(s)

    //TODO: прибраться

    private void Start()
    {
        bodyTransforms.head.rotation = Quaternion.Euler(0, 90, 0);
    }
    private void Update()
    {
        if (phase != Phase.phase_2)
            return;

        Phase2Behaviour();

        if (Input.GetKeyDown(KeyCode.H))
            vfxTransforms.bflShotVfx.gameObject.SetActive(!vfxTransforms.bflShotVfx.gameObject.activeInHierarchy);
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
        bodyTransforms.head.rotation = Quaternion.RotateTowards(bodyTransforms.head.rotation, Quaternion.LookRotation(adjustedPlayerPos, Vector3.up), 25 * Time.deltaTime);
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
            nextBflShoot += bflShootInterval;
        }
        if (nextBflShoot - bflShootInterval + bflShootTime < Time.time)
        {
            vfxTransforms.bflShotVfx.gameObject.SetActive(false);
        }
    }

    public void TurretIsDestriyed(int turretID)
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
    }
    [Serializable] private class VFXTransforms
    {
        public Transform bflShotVfx;
    }
}
