using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Scriptable Objects/New Upgrade", order = 1)]
public class GenericUpgrade_SO : ScriptableObject
{
    public string upgradeName;
    public string upgradeDescription;
    [HideInInspector] public MonoBehaviour WeaponScript;
    [SerializeField] private string methodName;
    public Sprite icon;

    public void UpgradeAction()
    {
        Type gameObjectType = WeaponScript.GetType();
        MethodInfo method = gameObjectType.GetMethod(methodName);
        if (method != null)
        {
            method.Invoke(WeaponScript, new object[] {});
            Debug.Log(methodName + " is found and invoked");
        }
        else
            Debug.Log(methodName + " is not found");
    }
}
