using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

//[CreateAssetMenu(fileName = "Upgrade CircleBots Number", menuName = "Scriptable Objects/Upgrade CircleBots Number", order = 1)]
public class UpgradeCircleBotsNumber_SO : UpgradeOption_SO
{
    int numberOfBotsToAdd;

    public override void UpgradeAction()
    {
        numberOfBotsToAdd = (int)value_1;
        Player_Stats_Scr.instance.GetComponentInChildren<Weapon_CircleBots_Scr>().AddBots(numberOfBotsToAdd);
    }
}
