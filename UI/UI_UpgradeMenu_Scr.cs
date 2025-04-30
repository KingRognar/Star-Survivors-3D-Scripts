using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class UI_UpgradeMenu_Scr : MonoBehaviour
{
    //TODO: обновить систему добавления новых апгрейдов в UpgradeSystem_Scr
    //TODO: отвязать старую систему
    //TODO: кнопки
    //TODO: визуальная индикация уже открытых апгрейдов
    //TODO: смена текста при выборе из предлагаемых апгрейдов
    //TODO: смена текста при просмотре дерева




    private UIDocument uiDoc;
    private List<Button> treeButtons;
    private List<Button> selectedUpgradesButtons;
    private Button selectButton, rerollButton, forgetButton;


    private List<(GenericUpgrade_SO upgr, UpgradeTree_SO tree)> upgradesForChoise;

    private void Awake()
    {
        Initialize();
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {

    }
    private void OnDestroy()
    {
        foreach (Button button in treeButtons)
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

        treeButtons = uiDoc.rootVisualElement.Q("WeaponUpgradeTree").Query<Button>().Where(elem => elem.name != "WeaponRoot").ToList();
        selectButton = uiDoc.rootVisualElement.Q("Buttons").Q("SelectButton") as Button;
        rerollButton = uiDoc.rootVisualElement.Q("Buttons").Q("RerollButton") as Button;
        forgetButton = uiDoc.rootVisualElement.Q("Buttons").Q("ForgetButton") as Button;
        selectedUpgradesButtons = uiDoc.rootVisualElement.Q("UpgradeSelection").Query<Button>().ToList(); // TODO: мб надо будет что-то добавить


        foreach (Button button in treeButtons)
        {
            button.RegisterCallback<ClickEvent>(ev => UpgradeTreeButtonClickEvent(button));
        }
        selectButton.RegisterCallback<ClickEvent>(SelectButtonClickEvent);
        rerollButton.RegisterCallback<ClickEvent>(RerollButtonClickEvent);
        forgetButton.RegisterCallback<ClickEvent>(ForgetButtonClickEvent);


        AssignUpgradeIcons();
    }
    private void AssignUpgradeIcons()
    {
/*        for (int i = 0; i < treeButtons.Count; i++ )
        {
            Button btn = treeButtons[i];
            if (weaponSO.upTree.upgrades[i] != null && weaponSO.upTree.upgrades[i].icon != null)
                btn.Children().ToList()[0].style.backgroundImage = new StyleBackground(weaponSO.upTree.upgrades[i].icon);
        }*/
    }

    public void UpdateUpgradeLists(List<(GenericUpgrade_SO upgr, UpgradeTree_SO tree)> possibleUpgrades)
    {
        upgradesForChoise = possibleUpgrades;
        UpdateUpgradeVisuals();
    }

    private void UpdateUpgradeVisuals()
    {
        UpdateSelectedButtonsVisuals();
    }
    private void UpdateSelectedButtonsVisuals() //TODO: закончил здесь
    {
        for (int i = 0; i < selectedUpgradesButtons.Count; i++)
        {
            selectedUpgradesButtons[i].Children().ToList()[0].style.backgroundImage = new StyleBackground(upgradesForChoise[i].tree.icon);
            selectedUpgradesButtons[i].Children().ToList()[1].style.backgroundImage = new StyleBackground(upgradesForChoise[i].upgr.icon);

            //TODO: выключать VE если больше нет апгрейдов
        }
    }

    #region ButtonEvents
    private void UpgradeTreeButtonClickEvent(Button btn)
    {
        foreach (Button button in treeButtons)
        {
            button.style.backgroundColor = Color.grey;
        }

        btn.style.backgroundColor = Color.white;
    }
    private void SelectButtonClickEvent(ClickEvent evt)
    {
        //TODO:
    }
    private void RerollButtonClickEvent(ClickEvent evt)
    {
        //TODO:
    }
    private void ForgetButtonClickEvent(ClickEvent evt)
    {
        //TODO:
    }
    #endregion
}
