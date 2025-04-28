using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Tree", menuName = "Scriptable Objects/Upgrade Tree", order = 0)]
public class UpgradeTree_SO : ScriptableObject
{
    public Sprite Icon;
    public string upTreeName;
    public string description;
    public GenericUpgrade_SO[] upgrades = new GenericUpgrade_SO[10];
}
