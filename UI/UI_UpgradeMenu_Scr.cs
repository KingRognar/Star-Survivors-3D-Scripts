using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_UpgradeMenu_Scr : MonoBehaviour
{
    private UIDocument uiDoc;

    private List<Button> treeButtons;

    private void Awake()
    {
        uiDoc = GetComponent<UIDocument>();

        treeButtons = uiDoc.rootVisualElement.Q("WeaponUpgradeTree").Query<Button>().Where(elem => elem.name != "WeaponRoot").ToList();
        //Debug.Log(treeButtons.Count);
        foreach (Button button in treeButtons )
        {
            button.RegisterCallback<ClickEvent>(ev => UpgradeTreeButtonClickEvent(button));
        }
    }
    private void OnDisable()
    {
        foreach (Button button in treeButtons )
        {
            button.UnregisterCallback<ClickEvent>(ev => UpgradeTreeButtonClickEvent(button));
        }
    }

    private void UpgradeTreeButtonClickEvent(Button btn)
    {
        foreach (Button button in treeButtons)
        {
            button.style.backgroundColor = Color.grey;
        }

        btn.style.backgroundColor = Color.white;
    }
}
