using System;
using UnityEngine;

public class Boss_1_Scr : MonoBehaviour
{
    private Phase phase = Phase.phase_1;
    [SerializeField] private BodyTransforms bodyTransforms;
    [SerializeField] private VFXTransforms vfxTransforms;
    private bool[] turretsDestroyed = new bool[3] { true, false, false};
    // start pos 0, -17.5, 9
    
    //TODO: Turrets on back

    //TODO: BFL 
    //TODO: Extend BFL at the begining of phase 2

    //TODO: Health bar

    //TODO: Animations

    //TODO: Visualize damage delt

    //TODO: Rocket launchers

    //TODO: Boss HP Bar(s)


    private void Start()
    {
        bodyTransforms.head.rotation = Quaternion.Euler(0, 90, 0);
    }
    private void Update()
    {
        if (phase == Phase.phase_1)
        {

        }
        if (phase == Phase.phase_2)
        {
            BFLPlayerTracking();
        }
        if (Input.GetKeyDown(KeyCode.H))
            vfxTransforms.bflShotVfx.gameObject.SetActive(!vfxTransforms.bflShotVfx.gameObject.activeInHierarchy);
    }


    private void BFLPlayerTracking()
    {
        Vector3 adjustedPlayerPos = Player_Stats_Scr.instance.transform.position - bodyTransforms.head.position; //TODO: handle destroyed player
        adjustedPlayerPos.y = 0;
        adjustedPlayerPos = Vector3.Cross(Vector3.up, adjustedPlayerPos);
        bodyTransforms.head.rotation = Quaternion.LookRotation(adjustedPlayerPos, Vector3.up);
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
        phase = Phase.phase_2;
    }

    private enum Phase
    {
        phase_1,
        phase_2,
    }
    [Serializable] private class BodyTransforms
    {
        public Transform head;
    }
    [Serializable] private class VFXTransforms
    {
        public Transform bflShotVfx;
    }
}
