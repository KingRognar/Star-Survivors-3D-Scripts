using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class UI_EXP_Bar_Scr : MonoBehaviour
{
    [SerializeField] private Image expBar;
    [SerializeField] private TMP_Text lvlText;

    float nextFillAmount = 0;
    float t = 0;
    bool barIsFilling = false;

    private void Update()
    {
        if (!barIsFilling)
            return;

        t = Mathf.MoveTowards(t, 1, Time.deltaTime);
        expBar.fillAmount = Mathf.Lerp(expBar.fillAmount, nextFillAmount, t);
        if (t == 1)
            barIsFilling = false;
    }
    private void Start()
    {
        UpdateEXPBar(0,10);
        UpadteLVLtext(1);
    }

    public void UpdateEXPBar(int curEXP, int lvlEXP)
    {
        //expBar.fillAmount = (float)curEXP / lvlEXP;
        nextFillAmount = (float)curEXP / lvlEXP;
        barIsFilling = true;
        t = 0;
    }
    public void UpadteLVLtext(int lvl)
    {
        lvlText.text = lvl.ToString();
    }
}
