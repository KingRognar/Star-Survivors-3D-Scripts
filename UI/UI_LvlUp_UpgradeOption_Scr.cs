using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_LvlUp_UpgradeOption_Scr : MonoBehaviour, IPointerClickHandler
{
    //public UpgradeOption_SO upgradeOptionSO;
    public GenericUpgrade_SO upgrade_SO;

    [SerializeField] private TMP_Text upgradeNameText;
    [SerializeField] private TMP_Text upgradeDescriptionText;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
        //Debug.Log("You chose " + upgradeOptionSO.upgradeName);

        //upgradeOptionSO.UpgradeAction();
        //UpgradeSystem_Scr.instance.upgradesList.AddRange(upgradeOptionSO.nextUpgrades);
        //UpgradeSystem_Scr.instance.upgradesList.Remove(upgradeOptionSO);

        upgrade_SO.UpgradeAction();
        /*if (upgrade_SO.nextUpgrades.Count != 0)
        {
            foreach (GenericUpgrade_SO upgrade in upgrade_SO.nextUpgrades)
            {
                upgrade.WeaponScript = upgrade_SO.WeaponScript; //BUG: когда выбрал machinegun damage N2 при 2 опциях всё полетело
            }
            //UpgradeSystem_Scr.instance.upgradeTrees.AddRange(upgrade_SO.nextUpgrades);
        }*/
        //UpgradeSystem_Scr.instance.upgradeTrees.Remove(upgrade_SO);

        UpgradeSystem_Scr.instance.CloseLvlUpMenu();
    }

    public void UpdateVisuals()
    {
        upgradeDescriptionText.text = upgrade_SO.upgradeDescription;
        upgradeNameText.text = upgrade_SO.upgradeName;
    }
}
