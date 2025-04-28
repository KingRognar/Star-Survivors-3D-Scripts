using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon SO", menuName = "Scriptable Objects/New Weapon SO", order = 2)]
public class Weapon_SO : ScriptableObject
{
    public GameObject weaponPrefab;
    public bool spawnOnParent;
    public UpgradeTree_SO upTree;
}
