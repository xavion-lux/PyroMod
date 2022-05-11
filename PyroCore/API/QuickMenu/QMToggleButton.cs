using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PyroMod.API.QuickMenu
{
    public class QMToggleButton : QMButtonBase
    {
        private readonly PyroModule Module;
        private readonly QMCategory Category;
        private readonly QMNestedMenu Menu;
        private readonly string BtnText;
        private Transform PlacementTransform;
        private readonly float BtnXLocation;
        private readonly float BtnYLocation;
        private readonly string BtnToolTip;
        private TextMeshProUGUI btnTextComp;
        private Action BtnOnAction;
        private Action BtnOffAction;
        private readonly bool DefaultState;
        private Button btnComp;
        private Image btnImageComp;
        private bool currentState;

        public QMToggleButton(QMCategory category, string btnText, Action onAction, Action offAction, string btnToolTip, bool defaultState = false)
        {
            Module = category.GetModule();
            Category = category;
            BtnText = btnText;
            BtnOnAction = onAction;
            BtnOffAction = offAction;
            BtnToolTip = btnToolTip;
            DefaultState = defaultState;
            Module.ModuleToggleButtons.Add(this);
        }

        public QMToggleButton(QMNestedMenu menu, float posX, float posY, string btnText, Action onAction, Action offAction, string btnToolTip, bool defaultState = false)
        {
            Module = menu.GetModule();
            Menu = menu;
            BtnXLocation = posX;
            BtnYLocation = posY;
            BtnText = btnText;
            BtnOnAction = onAction;
            BtnOffAction = offAction;
            BtnToolTip = btnToolTip;
            DefaultState = defaultState;
            Module.ModuleToggleButtons.Add(this);
        }

        internal void Initialize()
        {
            if (Category != null)
                PlacementTransform = Category.GetPlacementTransform();
            else
                PlacementTransform = Menu.GetMenuObject().transform;
            button = UnityEngine.Object.Instantiate(APIUtils.MainButton(), PlacementTransform, true);
            button.name = $"{Module.ModuleName}-ToggleButton-{APIUtils.RandomNumbers()}";
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 176);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(-68, 796);
            btnTextComp = button.GetComponentInChildren<TextMeshProUGUI>(true);
            btnComp = button.GetComponentInChildren<Button>(true);
            btnComp.onClick = new Button.ButtonClickedEvent();
            btnComp.onClick.AddListener(new Action(HandleClick));
            btnImageComp = button.transform.Find("Icon").GetComponentInChildren<Image>(true);

            initShift[0] = 0;
            initShift[1] = 0;
            SetLocation(BtnXLocation, BtnYLocation);
            SetButtonText(BtnText);
            SetButtonActions(BtnOnAction, BtnOffAction);
            SetToolTip(BtnToolTip);
            SetActive(true);

            currentState = DefaultState;
            var tmpIcon = currentState ? APIUtils.GetOnIconSprite() : APIUtils.GetOffIconSprite();
            btnImageComp.sprite = tmpIcon;
            btnImageComp.overrideSprite = tmpIcon;
        }

        private void HandleClick()
        {
            currentState = !currentState;
            var stateIcon = currentState ? APIUtils.GetOnIconSprite() : APIUtils.GetOffIconSprite();
            btnImageComp.sprite = stateIcon;
            btnImageComp.overrideSprite = stateIcon;
            if (currentState)
            {
                BtnOnAction.Invoke();
            }
            else
            {
                BtnOffAction.Invoke();
            }
        }

        public void SetButtonText(string buttonText)
        {
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = buttonText;
        }

        public void SetButtonActions(Action onAction, Action offAction)
        {
            BtnOnAction = onAction;
            BtnOffAction = offAction;
        }

        public void SetToggleState(bool newState, bool shouldInvoke = false)
        {
            try
            {
                var newIcon = newState ? APIUtils.GetOnIconSprite() : APIUtils.GetOffIconSprite();
                btnImageComp.sprite = newIcon;
                btnImageComp.overrideSprite = newIcon;
                currentState = newState;

                if (shouldInvoke)
                {
                    if (newState)
                    {
                        BtnOnAction.Invoke();
                    }
                    else
                    {
                        BtnOffAction.Invoke();
                    }
                }
            }
            catch {}
        }

        public void ClickMe()
        {
            HandleClick();
        }

        public bool GetCurrentState()
        {
            return currentState;
        }
    }
}
