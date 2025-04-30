using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upgades;

public class UpgradeSystem_Scr : MonoBehaviour
{
    public static UpgradeSystem_Scr instance;

    [SerializeField] private int currentExp = 0;
    [SerializeField] private int expForLvl = 10;
    private float multiplierForNextLvl = 1.4f;
    private int currentLvl = 1;

    [SerializeField] private GameObject expShardPrefab;

    [SerializeField] private GameObject levelUpMenu;
    [SerializeField] private UI_UpgradeMenu_Scr newLevelUpMenu;

    [SerializeField] private UI_EXP_Bar_Scr expBarUI;
    [SerializeField] private List<UI_LvlUp_UpgradeOption_Scr> UIlvlUpOptions;

    private List<UpgradeTree_SO> upgradeTrees = new (); //TODO: добавляем, когда получаем новое оружие/ выбираем новый апгрейд

    public Dictionary<string, UpgradeStatus[]> upgradeStatusTracker = new Dictionary<string, UpgradeStatus[]> ();
    //TODO: добавлять новую пару при получении нового оружия
    //TODO: при выборе нового улучшения обновлять словарь
    //TODO: обновить поиск доступных улучшении при открытии окна повышения уровня


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
    /// Метод начисляющий очки опыта
    /// </summary>
    /// <param name="expAmount">Количество добовляемых очков опыта</param>
    public void AwardEXP(int expAmount)
    {
        currentExp += expAmount;
        expBarUI.UpdateEXPBar(currentExp, expForLvl);

        if (currentExp >= expForLvl) // TODO: поставить луп и всякое такое, чтобы при избытке опыта можно получить несколько уровней последовательно
            LevelUp();
    }
    /// <summary>
    /// Метод, выполняемый при повышении уровня
    /// </summary>
    private void LevelUp()
    {
        currentExp -= expForLvl;
        expForLvl = (int)(multiplierForNextLvl*expForLvl);
        currentLvl++;
        OpenNewLvlUpMenu();
    }
    public void InstantiateExpShards(Vector3 position, int expAmount)
    {
        int expLeft = expAmount;
        while (expLeft / 50 > 1)
        {
            //TODO: big exp shards
            expLeft -= 50;
        }
        while (expLeft / 5 > 1)
        {
            //TODO: medium exp shards
            expLeft -= 5;
        }
        while (expLeft > 0)
        {
            Vector3 randAddPos = new(UnityEngine.Random.Range(-1, 1), 0, UnityEngine.Random.Range(-1, 1));
            Instantiate(expShardPrefab, position + randAddPos.normalized, Quaternion.Euler(new Vector3(
                UnityEngine.Random.Range(-360, 360),
                UnityEngine.Random.Range(-360, 360),
                UnityEngine.Random.Range(-360, 360))));
            expLeft--;
        }
    }


    private List<GenericUpgrade_SO> GetPossibleUpgrades()
    {
        List<GenericUpgrade_SO> list = new List<GenericUpgrade_SO>();
        return list;
    }
    private List<(GenericUpgrade_SO,UpgradeTree_SO)> NewGetPossibleUpgrades()
    {
        List<(GenericUpgrade_SO, UpgradeTree_SO)> possibleList = new();
        
        foreach (UpgradeTree_SO tree in upgradeTrees)
        {
            for (int i = 0; i < tree.upgrades.Length; i++)
            {
                if (upgradeStatusTracker[tree.name][i] != UpgradeStatus.available)
                    continue;

                possibleList.Add((tree.upgrades[i],tree));
            }
        }

        return possibleList;
    }
    /// <summary>
    /// Метод, открывающий меню выбора улучшений
    /// </summary>
    private void OpenLvlUpMenu() //TODO: переписать под новый UI
    {
        if (upgradeTrees.Count == 0)
        {
            CloseLvlUpMenu();
            return;
        }

        Time.timeScale = 0;
        levelUpMenu.SetActive(true);


        int cnt = 3;
        if (upgradeTrees.Count < 3)
            cnt = upgradeTrees.Count;

        List<GenericUpgrade_SO> possibleUpgrades = GetPossibleUpgrades(); //TODO: нужно ещё как-то получать UpgradeTree_SO

        try
        {
            List<GenericUpgrade_SO> selectUpgrades = SelectSample(possibleUpgrades, cnt);
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
    /// Метод, закрывающий меню выбора улучшений
    /// </summary>
    public void CloseLvlUpMenu()
    {
        Time.timeScale = 1;
        expBarUI.UpdateEXPBar(currentExp, expForLvl);
        expBarUI.UpadteLVLtext(currentLvl);
        levelUpMenu.SetActive(false);
    }
    private void OpenNewLvlUpMenu() 
    {
        //TODO:

        List<(GenericUpgrade_SO upgr, UpgradeTree_SO tree)> possibleUpgrades = NewGetPossibleUpgrades();

        if (possibleUpgrades.Count == 0)
        {
            CloseLvlUpMenu();
            return;
        }

        Time.timeScale = 0;
        newLevelUpMenu.gameObject.SetActive(true);

        int cnt = 3;
        if (possibleUpgrades.Count < 3)
            cnt = possibleUpgrades.Count;

        List<(GenericUpgrade_SO upgr, UpgradeTree_SO tree)> selectedUpgrades = NewSelectSample(possibleUpgrades, cnt);
        newLevelUpMenu.UpdateUpgradeLists(selectedUpgrades);

        //newLevelUpMenu.UpdateUpgradeLists()
    }
    public void CloseNewLvlUpMenu()
    {
        //TODO: 
    }

    private void AddUpgradesToList() // TODO: придумать метод для добавления ScriptableObjects
    {
        /*upgradesList.Clear();
        upgradesList.Add(IncreasePlayerAttackSpeed);
        upgradesList.Add(IncreasePlayerSpread);
        upgradesList.Add(IncreasePlayerHP);
        upgradesList.Add(IncreasePlayerArmor);*/
    }
    private List<GenericUpgrade_SO> SelectSample(List<GenericUpgrade_SO> upgradesList, int numberOfSamples)
    {
        List<GenericUpgrade_SO> samples = new ();
        int itemsLeft = upgradesList.Count;
        int i = 0;
        int samplesTaken = 0;

        while (samplesTaken < numberOfSamples && i < upgradesList.Count)
        {
            int rnd = UnityEngine.Random.Range(1, itemsLeft - i + 1);
            if (rnd <= (numberOfSamples - samplesTaken))
            {
                samples.Add(upgradesList[i]);
                samplesTaken++;
            }
            i++;
        }

        return samples;
    }
    private List<(GenericUpgrade_SO, UpgradeTree_SO)> NewSelectSample(List<(GenericUpgrade_SO, UpgradeTree_SO)> possibleList, int numberOfSamples)
    {
        List<(GenericUpgrade_SO, UpgradeTree_SO)> samples = new();
        int itemsLeft = possibleList.Count;
        int i = 0;
        int samplesTaken = 0;

        while (samplesTaken < numberOfSamples && i < possibleList.Count)
        {
            int rnd = UnityEngine.Random.Range(1, itemsLeft - i + 1);
            if (rnd <= (numberOfSamples - samplesTaken))
            {
                samples.Add(possibleList[i]);
                samplesTaken++;
            }
            i++;
        }

        return samples;
    }

    public void AddUpgradeTree(UpgradeTree_SO upgradeTree)
    {
        upgradeTrees.Add(upgradeTree);
        upgradeStatusTracker.Add(
            upgradeTree.name,
            new UpgradeStatus[10] {
                UpgradeStatus.available, UpgradeStatus.locked, UpgradeStatus.locked, UpgradeStatus.locked, UpgradeStatus.locked,
                UpgradeStatus.available, UpgradeStatus.locked, UpgradeStatus.locked, UpgradeStatus.locked, UpgradeStatus.locked});
    }
}
namespace Upgades
{
    public enum UpgradeStatus
    {
        locked,
        available,
        unlocked,
        forgotten
    }
}
