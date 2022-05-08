using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static PyroMod.Main;

namespace PyroMod.API.QuickMenu
{
    public class QMSingleButton : QMButtonBase
    {
        private readonly PyroModule Module;
        private readonly string BtnText;
        private Transform PlacementTransform;
        private readonly float BtnXLocation;
        private readonly float BtnYLocation;
        private readonly string BtnToolTip;
        private readonly Action BtnAction;
        private readonly QMCategory Category;
        private readonly QMNestedMenu Menu;

        public QMSingleButton(QMCategory category, string btnText, Action btnAction, string btnTooltip)
        {
            Module = category.GetModule();
            Category = category;
            BtnText = btnText;
            BtnAction = btnAction;
            BtnToolTip = btnTooltip;
            Module.ModuleSingleButtons.Add(this);
        }

        public QMSingleButton(QMNestedMenu menu, string btnText, float btnXPos, float btnYPos, Action btnAction, string btnToolTip)
        {
            Module = menu.GetModule();
            Menu = menu;
            BtnText = btnText;
            BtnXLocation = btnXPos;
            BtnYLocation = btnYPos;
            Module.ModuleSingleButtons.Add(this);
        }

        internal void Initialize()
        {
            if (Category != null)
                PlacementTransform = Category.GetPlacementTransform();
            else
                PlacementTransform = Menu.GetMenuObject().transform;
            button = UnityEngine.Object.Instantiate(APIUtils.MainButton(), PlacementTransform, true);
            button.name = $"{Module.ModuleName}-SingleButton-{APIUtils.RandomNumbers()}";
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().fontSize = 30;
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 176);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(-68, 796);
            button.transform.Find("Icon").GetComponentInChildren<Image>().gameObject.SetActive(false);
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().rectTransform.anchoredPosition += new Vector2(0, 50);

            initShift[0] = 0;
            initShift[1] = 0;
            SetLocation(BtnXLocation, BtnYLocation);

            SetButtonText(BtnText);
            SetToolTip(BtnToolTip);
            SetAction(BtnAction);
        }

        public void SetBackgroundImage(Sprite newImg)
        {
            button.transform.Find("Background").GetComponent<Image>().sprite = newImg;
            button.transform.Find("Background").GetComponent<Image>().overrideSprite = newImg;
            RefreshButton();
        }

        public void SetButtonText(string buttonText)
        {
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = buttonText;
        }

        public void SetAction(Action buttonAction)
        {
            button.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            if (buttonAction != null)
                button.GetComponent<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>(buttonAction));
        }

        public void SetInteractable(bool newState)
        {
            button.GetComponent<Button>().interactable = newState;
            RefreshButton();
        }

        public void ClickMe()
        {
            button.GetComponent<Button>().onClick.Invoke();
        }

        public Image GetBackgroundImage()
        {
            return button.transform.Find("Background").GetComponent<Image>();
        }

        private void RefreshButton()
        {
            button.SetActive(false);
            button.SetActive(true);
        }
    }
}
