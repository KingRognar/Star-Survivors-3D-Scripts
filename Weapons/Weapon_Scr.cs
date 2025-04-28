using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Scr : MonoBehaviour
{
    public UpgradeTree_SO upTree_so;

    protected virtual void Start() //TODO: перекинуть всё это дело в Weapon_SO, когда будет нормальный способ получения оружия     ..  или нет?
    {
        foreach (GenericUpgrade_SO upgrade in upTree_so.upgrades)
        {
            upgrade.WeaponScript = this;
        }
        UpgradeSystem_Scr.instance.upgradeTrees.Add(upTree_so);
        UpgradeSystem_Scr.instance.upgradeUnlockTracker.Add(new bool[10]);
    }
}
