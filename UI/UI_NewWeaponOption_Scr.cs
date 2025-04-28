using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_NewWeaponOption_Scr : MonoBehaviour, IPointerClickHandler
{
    public Weapon_SO weapon_SO;

    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private TMP_Text weaponDescriptionText;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (weapon_SO.spawnOnParent)
        {
            Instantiate(weapon_SO.weaponPrefab, Player_Stats_Scr.instance.transform.position, Quaternion.identity, Player_Stats_Scr.instance.transform);
        } else
        {
            Instantiate(weapon_SO.weaponPrefab, Vector3.zero, Quaternion.identity);
        }


        NewWeaponMenu_Scr.instance.weapon_SOs.Remove(weapon_SO);

        NewWeaponMenu_Scr.instance.CloseNewWeaponMenu();
    }

    public void UpdateVisuals()
    {
        weaponDescriptionText.text = weapon_SO.upTree.description;
        weaponNameText.text = weapon_SO.upTree.upTreeName;
    }
}
