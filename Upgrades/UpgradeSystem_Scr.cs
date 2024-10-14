using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem_Scr : MonoBehaviour
{
    public static UpgradeSystem_Scr instance;

    [SerializeField] private int currentExp = 0;
    [SerializeField] private int expForLvl = 10;
    private float multiplierForNextLvl = 1.4f;
    private int currentLvl = 1;

    [SerializeField] private GameObject levelUpMenu;

    [SerializeField] private UI_EXP_Bar_Scr expBarUI;
    [SerializeField] private List<UI_LvlUp_UpgradeOption_Scr> UIlvlUpOptions;

    //public List<UpgradeOption_SO> upgradesList = new List<UpgradeOption_SO>();
    public List<GenericUpgrade_SO> upgradesList = new List<GenericUpgrade_SO>();

    //[SerializeField] private GenericUpgrade_SO genericUpgrade;

    

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        AddUpgradesToList();
    }
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            genericUpgrade.UpgradeAction();
    }*/


    /// <summary>
    /// ����� ����������� ���� �����
    /// </summary>
    /// <param name="expAmount">���������� ����������� ����� �����</param>
    public void AwardEXP(int expAmount)
    {
        currentExp += expAmount;
        expBarUI.UpdateEXPBar(currentExp, expForLvl);

        if (currentExp >= expForLvl) // TODO: ��������� ��� � ������ �����, ����� ��� ������� ����� ����� �������� ��������� ������� ���������������
            LevelUp();
    }
    /// <summary>
    /// �����, ����������� ��� ��������� ������
    /// </summary>
    private void LevelUp()
    {
        currentExp -= expForLvl;
        expForLvl = (int)(multiplierForNextLvl*expForLvl);
        currentLvl++;
        OpenLvlUpMenu();
    }

    /// <summary>
    /// �����, ����������� ���� ������ ���������
    /// </summary>
    private void OpenLvlUpMenu()
    {
        if (upgradesList.Count == 0)
        {
            CloseLvlUpMenu();
            return;
        }

        Time.timeScale = 0;
        levelUpMenu.SetActive(true);


        int cnt = 3;
        if (upgradesList.Count < 3)
            cnt = upgradesList.Count;

        try
        {
            List<GenericUpgrade_SO> selectUpgrades = SelectSample(upgradesList, cnt);
            int i = 0;
            while (i < cnt)
            {
                //int num = UnityEngine.Random.Range(0, upgradesList.Count);
                UIlvlUpOptions[i].gameObject.SetActive(true);
                UIlvlUpOptions[i].upgrade_SO = selectUpgrades[i];
                UIlvlUpOptions[i].UpdateVisuals();


                i++;
            }
            while (i < 3)
            {
                UIlvlUpOptions[i].gameObject.SetActive(false);
                i++;
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex, this);
        }
    }
    /// <summary>
    /// �����, ����������� ���� ������ ���������
    /// </summary>
    public void CloseLvlUpMenu()
    {
        Time.timeScale = 1;
        expBarUI.UpdateEXPBar(currentExp, expForLvl);
        expBarUI.UpadteLVLtext(currentLvl);
        levelUpMenu.SetActive(false);
    }

    private void AddUpgradesToList() // TODO: ��������� ����� ��� ���������� ScriptableObjects
    {
        /*upgradesList.Clear();
        upgradesList.Add(IncreasePlayerAttackSpeed);
        upgradesList.Add(IncreasePlayerSpread);
        upgradesList.Add(IncreasePlayerHP);
        upgradesList.Add(IncreasePlayerArmor);*/
    }
    private List<GenericUpgrade_SO> SelectSample(List<GenericUpgrade_SO> transformsList, int numberOfSamples)
    {
        List<GenericUpgrade_SO> samples = new List<GenericUpgrade_SO>();
        int itemsLeft = transformsList.Count;
        int i = 0;
        int samplesTaken = 0;

        while (samplesTaken < numberOfSamples && i < transformsList.Count)
        {
            int rnd = UnityEngine.Random.Range(1, itemsLeft - i + 1);
            if (rnd <= (numberOfSamples - samplesTaken))
            {
                samples.Add(transformsList[i]);
                samplesTaken++;
            }
            i++;
        }

        return samples;
    }
}
