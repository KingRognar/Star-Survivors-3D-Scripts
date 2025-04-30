using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Scr : MonoBehaviour
{
    public UpgradeTree_SO upTree_so;

    protected virtual void Start() //TODO: ���������� �� ��� ���� � Weapon_SO, ����� ����� ���������� ������ ��������� ������     ..  ��� ���?
    {
        foreach (GenericUpgrade_SO upgrade in upTree_so.upgrades)
        {
            upgrade.WeaponScript = this;
        }
        UpgradeSystem_Scr.instance.AddUpgradeTree(upTree_so);
    }
}
