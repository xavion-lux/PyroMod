using System;
using UnityEngine;
using UnityEngine.UI;
using static PyroMod.Main;

namespace PyroMod.API.QuickMenu
{
    public class QMSlider
    {
        private PyroModule Module;
        private GameObject slider;
        public GameObject label;
        private Slider sliderComp;
        private Text text;
        private Transform PlacementTransform;
        private float MinValue;
        private float MaxValue;
        private float CurrentValue;
        private float PosX;
        private float PosY;
        private string SliderLabel;
        private readonly QMCategory Category;
        private readonly QMNestedMenu Menu;
        private readonly Action<float> SliderAction;

        public QMSlider(QMCategory category, string labelTxt, float minValue, float maxValue, float currentValue, Action<float> sliderAction)
        {
            Module = category.GetModule();
            Category = category;
            SliderLabel = labelTxt;
            MinValue = minValue;
            MaxValue = maxValue;
            CurrentValue = currentValue;
            SliderAction = sliderAction;
            Module.ModuleSliders.Add(this);
        }

        public QMSlider(QMNestedMenu menu,float posX, float posY, string labelTxt, float minValue, float maxValue, float currentValue, Action<float> sliderAction)
        {
            Module = menu.GetModule();
            Menu = menu;
            PosX = posX;
            PosY = posY;
            SliderLabel = labelTxt;
            MinValue = minValue;
            MaxValue = maxValue;
            CurrentValue = currentValue;
            SliderAction = sliderAction;
            Module.ModuleSliders.Add(this);
        }

        internal void Initialize()
        {
            if (Category != null)
                PlacementTransform = Category.GetPlacementTransform();
            else
                PlacementTransform = Menu.GetMenuObject().transform;
            slider = UnityEngine.Object.Instantiate(APIUtils.GetSliderTemplate(), PlacementTransform, false);
            slider.transform.localScale = new Vector3(1, 1, 1);
            slider.name = $"{Module.ModuleName}-Slider-{APIUtils.RandomNumbers()}";

            label = UnityEngine.Object.Instantiate(GameObject.Find("UserInterface/MenuContent/Screens/Settings/AudioDevicePanel/LevelText"), slider.transform);
            label.name = "QMSlider-Label";
            label.transform.localScale = new Vector3(1, 1, 1);
            label.GetComponent<RectTransform>().sizeDelta = new Vector2(360, 50);
            label.GetComponent<RectTransform>().anchoredPosition = new Vector2(10.4f, 55);
            sliderComp = slider.GetComponent<Slider>();
            sliderComp.wholeNumbers = true;
            sliderComp.onValueChanged = new Slider.SliderEvent();
            sliderComp.onValueChanged.AddListener(SliderAction);
            sliderComp.onValueChanged.AddListener(new Action<float>(delegate (float f)
            {
                slider.transform.Find("Fill Area/Label").GetComponent<Text>().text = $"{sliderComp.value}/{MaxValue}";
            }));

            text = label.GetComponent<Text>();
            text.resizeTextForBestFit = false;

            SetLocation(new Vector2(PosX, PosY));
            SetLabelText(SliderLabel);
            SetValue(MinValue, MaxValue, CurrentValue);
        }

        public void SetLocation(Vector2 location)
        {
            slider.GetComponent<RectTransform>().anchoredPosition = location;
        }

        public void SetLabelText(string label)
        {
            text.text = label;
        }

        public void SetLabelColor(Color color)
        {
            text.color = color;
        }

        public void SetValue(float min, float max, float current)
        {
            sliderComp.minValue = min;
            sliderComp.maxValue = max;
            sliderComp.value = current;
        }

        public GameObject GetGameObject()
        {
            return slider;
        }
    }
}
