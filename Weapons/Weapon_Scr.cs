using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Scr : MonoBehaviour
{
    public List<GenericUpgrade_SO> listOfStartingUpgrades;

    protected virtual void Start()
    {
        foreach (GenericUpgrade_SO upgrade in listOfStartingUpgrades)
        {
            upgrade.WeaponScript = this;
        }
        UpgradeSystem_Scr.instance.upgradesList.AddRange(listOfStartingUpgrades);
    }
}
