using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class UpgradeOption_SO : ScriptableObject
{
    protected UpgradeSystem_Scr upgradeSystem;
    protected Player_Stats_Scr playerStats;

    public float value_1;
    public float value_2;
    public float value_3;
    public bool bool_1;

    public string upgradeName;
    public string upgradeDesription;

    public List<UpgradeOption_SO> nextUpgrades = new List<UpgradeOption_SO>();

    private void Start()
    {
        upgradeSystem = UpgradeSystem_Scr.instance;
        playerStats = Player_Stats_Scr.instance;
    }

    public virtual void UpgradeAction() { }

}
