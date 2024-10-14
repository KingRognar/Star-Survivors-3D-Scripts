using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HP_Bar_Scr : MonoBehaviour
{
    [SerializeField] private Image HPBar;
    [SerializeField] private TMP_Text HPText;

    private void Start()
    {
        UpdateHPBar();
    }

    public void UpdateHPBar()
    {
        HPBar.fillAmount = (float)Player_Stats_Scr.Ship.curHp / Player_Stats_Scr.Ship.maxHp;
        HPText.text = Player_Stats_Scr.Ship.curHp.ToString();
    }
}
