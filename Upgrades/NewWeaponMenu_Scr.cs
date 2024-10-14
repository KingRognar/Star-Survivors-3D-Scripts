using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWeaponMenu_Scr : MonoBehaviour
{
    public static NewWeaponMenu_Scr instance;

    [SerializeField] private GameObject newWeaponMenu;
    [SerializeField] private List<UI_NewWeaponOption_Scr> UINewWeaponOptions;

    public List<Weapon_SO> weapon_SOs;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void OpenNewWeaponMenu()
    {
        if (weapon_SOs.Count == 0)
            CloseNewWeaponMenu();

        Time.timeScale = 0;
        newWeaponMenu.SetActive(true);


        int cnt = 3;
        if (weapon_SOs.Count < 3)
            cnt = weapon_SOs.Count;

        int i = 0;
        while (i < 3)
        {
            int num = Random.Range(0, weapon_SOs.Count);
            UINewWeaponOptions[i].weapon_SO = weapon_SOs[num];
            UINewWeaponOptions[i].UpdateVisuals();
            if (i + 1 > cnt)
                UINewWeaponOptions[i].gameObject.SetActive(false);

            i++;
        }
    }
    public void CloseNewWeaponMenu()
    {
        Time.timeScale = 1;
        newWeaponMenu.SetActive(false);
    }
}
