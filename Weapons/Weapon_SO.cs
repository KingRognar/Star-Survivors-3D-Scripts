using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon SO", menuName = "Scriptable Objects/New Weapon SO", order = 2)]
public class Weapon_SO : ScriptableObject
{
    public string weaponName;
    public string weaponDescription;

    public GameObject weaponPrefab;
    public bool spawnOnParent;
    public List<UpgradeOption_SO> weaponStartingUpgrades; 
}
