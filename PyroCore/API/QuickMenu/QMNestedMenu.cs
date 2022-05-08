using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;
using VRC.UI.Elements.Menus;
using static PyroMod.Main;

namespace PyroMod.API.QuickMenu
{
    public class QMNestedMenu
    {
        private PyroModule Module;
        //private string btnQMLoc;
        private GameObject MenuObject;
        private TextMeshProUGUI MenuTitleText;
        private UIPage MenuPage;
        private GameObject BackButton;
        private QMSingleButton MainButton;
        private string MenuName;
        private string BtnText;
        private string BtnToolTip;
        private string MenuLabel;
        private float BtnPosX;
        private float BtnPosY;
        private QMCategory Category;
        private QMNestedMenu Menu;

        public QMNestedMenu(QMCategory category, string btnText, string btnTooltip, string menuLabel)
        {
            Module = category.GetModule();
            Category = category;
            BtnText = btnText;
            BtnToolTip = btnTooltip;
            MenuLabel = menuLabel;
            Module.ModuleNestedMenus.Add(this);
        }

        public QMNestedMenu(QMNestedMenu menu, float posX, float posY, string btnText, string toolTipText, string menuTitle)
        {
            Module = menu.GetModule();
            Menu = menu;
            BtnPosX = posX;
            BtnPosY = posY;
            BtnText = btnText;
            BtnToolTip = toolTipText;
            MenuLabel = menuTitle;
            Module.ModuleNestedMenus.Add(this);
        }

        internal void Initialize()
        {
            MenuName = $"{Module.ModuleName}-Menu-{APIUtils.RandomNumbers()}";
            MenuObject = UnityEngine.Object.Instantiate(APIUtils.GetMenuPage(), APIUtils.GetMenuPage().transform.parent);
            MenuObject.name = MenuName;
            MenuObject.SetActive(false);
            UnityEngine.Object.DestroyImmediate(MenuObject.GetComponent<LaunchPadQMMenu>());
            MenuPage = MenuObject.AddComponent<UIPage>();
            MenuPage.field_Public_String_0 = MenuName;
            MenuPage.field_Private_Boolean_1 = true;
            MenuPage.field_Protected_MenuStateController_0 = APIUtils.GetQuickMenuInstance().prop_MenuStateController_0;
            MenuPage.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
            MenuPage.field_Private_List_1_UIPage_0.Add(MenuPage);
            APIUtils.GetQuickMenuInstance().prop_MenuStateController_0.field_Private_Dictionary_2_String_UIPage_0.Add(MenuName, MenuPage);
            MenuObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").DestroyChildren();
            MenuTitleText = MenuObject.GetComponentInChildren<TextMeshProUGUI>(true);
            MenuTitleText.text = MenuLabel;
            BackButton = MenuObject.transform.GetChild(0).Find("LeftItemContainer/Button_Back").gameObject;
            BackButton.SetActive(true);
            BackButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            BackButton.GetComponentInChildren<Button>().onClick.AddListener(new Action(() =>
            {
                MenuPage.Method_Protected_Virtual_New_Void_0();
            }));
            MenuObject.transform.GetChild(0).Find("RightItemContainer/Button_QM_Expand").gameObject.SetActive(false);
            // Creates the main button based on if the category is null or not. One of them is always null.
            if (Category != null) 
                MainButton = new QMSingleButton(Category, BtnText, OpenMe, BtnToolTip);
            else
                MainButton = new QMSingleButton(Menu, BtnText, BtnPosX, BtnPosY, OpenMe, BtnToolTip);

            for (int i = 0; i < MenuObject.transform.childCount; i++)
            {
                if (MenuObject.transform.GetChild(i).name != "Header_H1" && MenuObject.transform.GetChild(i).name != "ScrollRect")
                {
                    UnityEngine.Object.Destroy(MenuObject.transform.GetChild(i).gameObject);
                }
            }
            MenuObject.transform.Find("ScrollRect").GetComponent<ScrollRect>().enabled = false;
        }

        public void OpenMe()
        {
            APIUtils.GetQuickMenuInstance().prop_MenuStateController_0.Method_Public_Void_String_UIContext_Boolean_0(MenuPage.field_Public_String_0);
        }

        public void CloseMe()
        {
            MenuPage.Method_Public_Virtual_New_Void_0();
        }

        public string GetMenuName()
        {
            return MenuName;
        }

        public PyroModule GetModule()
        {
            return Module;
        }

        public GameObject GetMenuObject()
        {
            return MenuObject;
        }

        public QMSingleButton GetMainButton()
        {
            return MainButton;
        }

        public GameObject GetBackButton()
        {
            return BackButton;
        }
    }
}
