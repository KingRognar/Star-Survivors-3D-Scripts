using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHP_Bar_Scr : MonoBehaviour
{
    [SerializeField] private Image HPBar;

    private void OnEnable()
    {
        UpdateHPBar(1,1);
    }

    public void UpdateHPBar(int curHp, int maxHp)
    {
        HPBar.fillAmount = (float)curHp / maxHp;
    }
}
