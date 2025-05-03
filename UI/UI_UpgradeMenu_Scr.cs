using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;
using Upgades;

public class UI_UpgradeMenu_Scr : MonoBehaviour
{
    //TODO: обновить систему добавления новых апгрейдов в UpgradeSystem_Scr
    //TODO: отвязать старую систему
    //TODO: кнопки
    //TODO: визуальная индикация уже открытых апгрейдов
    //TODO: смена текста при выборе из предлагаемых апгрейдов
    //TODO: смена текста при просмотре дерева




    private UIDocument uiDoc;
    private Button treeWeaponBtn;
    private List<Button> treeUpgrBtns;
    private List<Button> offeredUpgrBtns;
    private Button selectButton, rerollButton, forgetButton;
    private Label upgrName, upgrDesc;

    private List<(GenericUpgrade_SO upgr, UpgradeTree_SO tree)> offeredUpgrades;

    private Dictionary<UpgradeStatus, Color> upStatus_ColorDict = new Dictionary<UpgradeStatus, Color>();
    private int selectedTreeIndex = 0;

    private bool rerollUsed = false;
    private bool forgetUsed = false;

    private void Awake()
    {
        //Initialize();
    }
    private void OnEnable()
    {
        //Initialize();
    }
    private void OnDisable()
    {

    }
    private void OnDestroy()
    {
        foreach (Button button in treeUpgrBtns)
        {
            button.UnregisterCallback<ClickEvent>(ev => UpgradeTreeButtonClickEvent(button));
        }
        selectButton.UnregisterCallback<ClickEvent>(SelectButtonClickEvent);
        rerollButton.UnregisterCallback<ClickEvent>(RerollButtonClickEvent);
        forgetButton.UnregisterCallback<ClickEvent>(ForgetButtonClickEvent);
    }


    private void Initialize()
    {
        uiDoc = GetComponent<UIDocument>();

        treeWeaponBtn = uiDoc.rootVisualElement.Q("WeaponUpgradeTree").Query<Button>().Where(elem => elem.name == "WeaponRoot");
        treeUpgrBtns = uiDoc.rootVisualElement.Q("WeaponUpgradeTree").Query<Button>().Where(elem => elem.name != "WeaponRoot").ToList();
        selectButton = uiDoc.rootVisualElement.Q("Buttons").Q("SelectButton") as Button;
        rerollButton = uiDoc.rootVisualElement.Q("Buttons").Q("RerollButton") as Button;
        forgetButton = uiDoc.rootVisualElement.Q("Buttons").Q("ForgetButton") as Button;
        offeredUpgrBtns = uiDoc.rootVisualElement.Q("UpgradeSelection").Query<Button>().ToList(); // TODO: мб надо будет что-то добавить

        upgrName = uiDoc.rootVisualElement.Q("UpgradeDescription").Q("Name") as Label;
        upgrDesc = uiDoc.rootVisualElement.Q("UpgradeDescription").Q("Description") as Label;

        foreach (Button button in offeredUpgrBtns)
        {
            button.RegisterCallback<ClickEvent>(ev => OfferedUpgradeButtonClickEvent(button));
        }
        foreach (Button button in treeUpgrBtns)
        {
            button.RegisterCallback<ClickEvent>(ev => UpgradeTreeButtonClickEvent(button));
        }
        selectButton.RegisterCallback<ClickEvent>(SelectButtonClickEvent);
        rerollButton.RegisterCallback<ClickEvent>(RerollButtonClickEvent);
        forgetButton.RegisterCallback<ClickEvent>(ForgetButtonClickEvent);

        InitializeDictionary();

        rerollUsed = false;
        forgetUsed = false;
}
    private void InitializeDictionary()
    {
        upStatus_ColorDict.Clear();
        upStatus_ColorDict.Add(UpgradeStatus.locked, Color.gray);
        upStatus_ColorDict.Add(UpgradeStatus.available, Color.white);
        upStatus_ColorDict.Add(UpgradeStatus.unlocked, Color.yellow);
        upStatus_ColorDict.Add(UpgradeStatus.forgotten, Color.red);
    }

    public void UpdateUpgradeLists(List<(GenericUpgrade_SO upgr, UpgradeTree_SO tree)> possibleUpgrades)
    {
        Initialize();
        offeredUpgrades = possibleUpgrades;
        InitialVusualsUpdate();
    }

    #region Update Visuals
    private void InitialVusualsUpdate()
    {
        UpdateSelectedButtonsVisuals();
        UpdateSelectedTree(selectedTreeIndex);
    }
    private void UpdateSelectedButtonsVisuals()
    {
        for (int i = 0; i < offeredUpgrBtns.Count; i++)
        {
            if (i < offeredUpgrades.Count)
            {
                offeredUpgrBtns[i].style.display = DisplayStyle.Flex;
                //selectedUpgradesButtons[i].Children().ToList()[0].style.backgroundImage = new StyleBackground(upgradesForChoise[i].tree.icon);
                //selectedUpgradesButtons[i].Children().ToList()[1].style.backgroundImage = new StyleBackground(upgradesForChoise[i].upgr.icon);
                offeredUpgrBtns[i][0].style.backgroundImage = new StyleBackground(offeredUpgrades[i].tree.icon);
                offeredUpgrBtns[i][1].style.backgroundImage = new StyleBackground(offeredUpgrades[i].upgr.icon);
            } else
            {
                offeredUpgrBtns[i].style.display = DisplayStyle.None;
            }
        }
    }
    private void UpdateSelectedTree(int index)
    {
        //treeWeaponBtn.Children().ToList()[0].style.backgroundImage = 
        treeWeaponBtn[0].style.backgroundImage = new StyleBackground(offeredUpgrades[index].tree.icon);
        for (int i = 0; i < treeUpgrBtns.Count; i++)
        {
            treeUpgrBtns[i][0].style.backgroundImage = new StyleBackground(offeredUpgrades[index].tree.upgrades[i].icon);
            //TODO: borders according to upgradeStatus
        }
    }
    private void UpdateText(int treeIndex, int upgrIndex)
    {
        upgrName.text = offeredUpgrades[treeIndex].tree.upgrades[upgrIndex].upgradeName;
        upgrDesc.text = offeredUpgrades[treeIndex].tree.upgrades[upgrIndex].upgradeDescription;
    }
    private void UpdateTreeBorders()
    {
        UpgradeStatus[] upgradeStatuses = UpgradeSystem_Scr.instance.upgradeStatusTracker[offeredUpgrades[selectedTreeIndex].tree.name];
        for (int i = 0; i < treeUpgrBtns.Count; i++)
        {
            treeUpgrBtns[i].style.backgroundColor = upStatus_ColorDict[upgradeStatuses[i]];
        }
    }
    #endregion

    //TODO: прочекать как этот UI работает с Disable\Enable

    #region Button Events
    private void UpgradeTreeButtonClickEvent(Button btn)
    {
/*        foreach (Button button in treeUpgrBtns)
        {
            button.style.backgroundColor = Color.grey;
        }

        btn.style.backgroundColor = Color.white;*/

        int i = 0;
        while (treeUpgrBtns[i] != btn) { i++; }
        UpdateText(selectedTreeIndex, i);
    }
    private void SelectButtonClickEvent(ClickEvent evt) //TODO: исключать апгрейд из пула \ обновлять доступные крч
    {
        offeredUpgrades[selectedTreeIndex].upgr.UpgradeAction();
        UpgradeSystem_Scr.instance.CloseNewLvlUpMenu();
    }
    private void RerollButtonClickEvent(ClickEvent evt)
    {
        //TODO:
    }
    private void ForgetButtonClickEvent(ClickEvent evt)
    {
        //TODO:
    }
    private void OfferedUpgradeButtonClickEvent(Button btn)
    {
        int i = 0;
        while (offeredUpgrBtns[i] != btn) { i++; }
        selectedTreeIndex = i;
        UpdateSelectedTree(selectedTreeIndex);
        UpdateTreeBorders();
    }
    #endregion
}
