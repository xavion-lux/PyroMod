using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Menus;

namespace PyroMod.API.QuickMenu
{
    internal class QMTabMenu
    {
        protected GameObject MenuObject;
        protected TextMeshProUGUI MenuTitleText;
        protected UIPage MenuPage;
        protected GameObject MainButton;
        protected GameObject BadgeObject;
        protected TextMeshProUGUI BadgeText;
        protected MenuTab MenuTabComp;
        protected string MenuName;

        internal QMTabMenu(string toolTipText, string menuTitle, Sprite img = null)
        {
            Initialize(toolTipText, menuTitle, img);
        }

        private void Initialize(string btnToolTipText, string menuTitle, Sprite img = null)
        {
            MenuName = $"PyroMod-Menu-{APIUtils.RandomNumbers()}";
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
            var list = APIUtils.GetQuickMenuInstance().prop_MenuStateController_0.field_Public_ArrayOf_UIPage_0.ToList();
            list.Add(MenuPage);
            APIUtils.GetQuickMenuInstance().prop_MenuStateController_0.field_Public_ArrayOf_UIPage_0 = list.ToArray();
            MenuObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").DestroyChildren();
            MenuTitleText = MenuObject.GetComponentInChildren<TextMeshProUGUI>(true);
            MenuTitleText.text = menuTitle;
            MenuObject.transform.GetChild(0).Find("RightItemContainer/Button_QM_Expand").gameObject.SetActive(false);
            for (int i = 0; i < MenuObject.transform.childCount; i++)
            {
                if (MenuObject.transform.GetChild(i).name != "Header_H1" && MenuObject.transform.GetChild(i).name != "ScrollRect")
                {
                    UnityEngine.Object.Destroy(MenuObject.transform.GetChild(i).gameObject);
                }
            }
            MenuObject.transform.Find("ScrollRect").GetComponent<ScrollRect>().enabled = false;
            MainButton = UnityEngine.Object.Instantiate(APIUtils.GetTabButton(), APIUtils.GetTabButton().transform.parent);
            MainButton.name = $"PyroMod-{APIUtils.RandomNumbers()}";
            MenuTabComp = MainButton.GetComponent<MenuTab>();
            MenuTabComp.field_Private_MenuStateController_0 = APIUtils.GetMenuStateControllerInstance();
            MenuTabComp.field_Public_String_0 = MenuName;
            MenuTabComp.GetComponent<StyleElement>().field_Private_Selectable_0 = MenuTabComp.GetComponent<Button>();
            BadgeObject = MainButton.transform.GetChild(0).gameObject;
            BadgeText = BadgeObject.GetComponentInChildren<TextMeshProUGUI>();
            MainButton.GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                MenuTabComp.GetComponent<StyleElement>().field_Private_Selectable_0 = MenuTabComp.GetComponent<Button>();
            }));
            SetToolTip(btnToolTipText);
            if (img != null)
            {
                SetImage(img);
            }
        }

        internal void SetImage(Sprite newImg)
        {
            MainButton.transform.Find("Icon").GetComponent<Image>().sprite = newImg;
            MainButton.transform.Find("Icon").GetComponent<Image>().overrideSprite = newImg;
            MainButton.transform.Find("Icon").GetComponent<Image>().color = Color.white;
            MainButton.transform.Find("Icon").GetComponent<Image>().m_Color = Color.white;
        }

        internal void SetToolTip(string newText)
        {
            MainButton.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().field_Public_String_0 = newText;
        }

        internal void SetIndex(int newPosition)
        {
            MainButton.transform.SetSiblingIndex(newPosition);
        }

        internal void SetActive(bool newState)
        {
            MainButton.SetActive(newState);
        }

        internal void SetBadge(bool showing = true, string text = "")
        {
            if (BadgeObject == null || BadgeText == null)
            {
                return;
            }
            BadgeObject.SetActive(showing);
            BadgeText.text = text;
        }

        internal string GetMenuName()
        {
            return MenuName;
        }

        internal GameObject GetMenuObject()
        {
            return MenuObject;
        }

        public GameObject GetMainButton()
        {
            return MainButton;
        }
    }
}
