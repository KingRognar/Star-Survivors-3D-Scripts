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
    //[SerializeField] private string stat1Name;
    [SerializeField] private float stat1ValueChange;
    [HideInInspector] public MonoBehaviour WeaponScript;
    [SerializeField] private string methodName;

    [Space(20)]
    public List<GenericUpgrade_SO> nextUpgrades;

    //public UnityEvent upgradeEvent = new UnityEvent();
    //private UnityAction newAction;

    public void UpgradeAction()
    {
        //newAction = new UnityAction(Weapon_SnakeDrone_Scr.instance.AddTailSegments);
        //upgradeEvent.AddListener(newAction);

        //Type playerType = Player_Stats_Scr.instance.GetType();
        //FieldInfo field = playerType.GetField(stat1Name, BindingFlags.NonPublic | BindingFlags.Instance);
        //Debug.Log(stat1Name + " value is " + field.GetValue(Player_Stats_Scr.instance));

        /*if (WeaponScript != null)
        {
            Debug.Log(WeaponScript.name + " is assigned");
        }
        else
            Debug.Log(WeaponScript.name + " is not assigned");*/

        Type gameObjectType = WeaponScript.GetType();
        MethodInfo method = gameObjectType.GetMethod(methodName);
        if (method != null)
        {
            method.Invoke(WeaponScript, new object[] { stat1ValueChange });
            Debug.Log(methodName + " is found and invoked");
        }
        else
            Debug.Log(methodName + " is not found");

        //upgradeEvent.Invoke();
    }
}
