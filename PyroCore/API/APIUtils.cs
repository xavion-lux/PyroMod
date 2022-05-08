using System;
using UnityEngine;
using VRC.UI.Elements;

namespace PyroMod.API
{
    internal static class APIUtils
    {
        private static System.Random rnd = new System.Random();
        private static VRC.UI.Elements.QuickMenu QuickMenuInstance;
        private static MenuStateController MenuStateControllerInstance;

        // Object References
        private static GameObject CategoryHeaderReference;
        private static GameObject CategoryBodyReference;
        private static GameObject MenuPageReference;
        private static GameObject TabButtonReference;
        private static GameObject MainButtonReference;
        private static GameObject SliderReference;

        // Sprites
        private static Sprite OnIconReference;
        private static Sprite OffIconReference;

        public static VRC.UI.Elements.QuickMenu GetQuickMenuInstance()
        {
            if (QuickMenuInstance == null)
                QuickMenuInstance = Resources.FindObjectsOfTypeAll<VRC.UI.Elements.QuickMenu>()[0];
            return QuickMenuInstance;
        }

        public static MenuStateController GetMenuStateControllerInstance()
        {
            if (MenuStateControllerInstance == null)
            {
                MenuStateControllerInstance = GetQuickMenuInstance().GetComponent<MenuStateController>();
            }
            return MenuStateControllerInstance;
        }

        public static GameObject GetCategoryHeader()
        {
            if (CategoryHeaderReference == null)
            {
                CategoryHeaderReference = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/QM_Foldout_Comfort").gameObject;
            }
            return CategoryHeaderReference;
        }

        public static GameObject GetCategoryBody()
        {
            if (CategoryBodyReference == null)
            {
                CategoryBodyReference = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_Comfort").gameObject;
            }
            return CategoryBodyReference;
        }

        public static GameObject GetMenuPage()
        {
            if (MenuPageReference == null)
            {
                MenuPageReference = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard").gameObject;
            }
            return MenuPageReference;
        }

        public static GameObject GetTabButton()
        {
            if (TabButtonReference == null)
            {
                TabButtonReference = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings").gameObject;
            }
            return TabButtonReference;
        }

        public static GameObject MainButton()
        {
            if (MainButtonReference == null)
            {
                var buttons = GetQuickMenuInstance().GetComponentsInChildren<UnityEngine.UI.Button>(true);
                for (int i = 0; i < buttons.Count; i++)
                {
                    if (buttons[i].name == "Button_Screenshot")
                    {
                        MainButtonReference = buttons[i].gameObject;
                    }
                }
            }
            return MainButtonReference;
        }

        public static GameObject GetSliderTemplate()
        {
            if (SliderReference == null)
            {
                SliderReference = GameObject.Find("UserInterface").transform.Find("MenuContent/Screens/Settings/AudioDevicePanel/VolumeSlider").gameObject;
            }
            return SliderReference;
        }

        public static Sprite GetOnIconSprite()
        {
            if (OnIconReference == null)
            {
                OnIconReference = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Notifications/Panel_NoNotifications_Message/Icon").GetComponent<UnityEngine.UI.Image>().sprite;
            }
            return OnIconReference;
        }

        public static Sprite GetOffIconSprite()
        {
            if (OffIconReference == null)
            {
                OffIconReference = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UI_Elements_Row_1/Button_ToggleQMInfo/Icon_Off").GetComponent<UnityEngine.UI.Image>().sprite;
            }
            return OffIconReference;
        }

        public static int RandomNumbers()
        {
            return rnd.Next(100000, 999999);
        }

        public static void DestroyChildren(this Transform transform)
        {
            transform.DestroyChildren(null);
        }

        public static void DestroyChildren(this Transform transform, Func<Transform, bool> exclude)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                if (exclude == null || exclude(transform.GetChild(i)))
                {
                    UnityEngine.Object.DestroyImmediate(transform.GetChild(i).gameObject);
                }
            }
        }
    }
}
