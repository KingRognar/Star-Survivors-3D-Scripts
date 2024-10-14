using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Upgrade Firerate", menuName = "ScriptableObjects/Upgrade Firerate", order = 2)]
public class UpgradeFirerate_SO : UpgradeOption_SO
{
    [SerializeField] private float firerateMultiplierDecrease = 0.1f;

    public override void UpgradeAction()
    {
        Player_Stats_Scr.Ship.firerateMultiplier -= firerateMultiplierDecrease;
    }
}
