using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

//[CreateAssetMenu(fileName = "Upgrade Damage", menuName = "ScriptableObjects/Upgrade Damage", order = 1)]
public class UpgradeDamage_SO : UpgradeOption_SO
{
    private float damageMultiplierIncrease;
    private float bulletScaleIncrease;
    public override void UpgradeAction()
    {
        damageMultiplierIncrease = value_1;
        bulletScaleIncrease = value_2;
        Player_Stats_Scr.Ship.damageMultiplier += damageMultiplierIncrease;
        Player_Stats_Scr.Machinegun.projectileScale += bulletScaleIncrease;
    }
}
