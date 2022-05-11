using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;
using static PyroMod.Main;

namespace PyroMod.API.QuickMenu
{
    public class QMCategory
    {
        private PyroModule Module;
        private GameObject HeaderObject;
        private GameObject BodyObject;
        private Transform ChildTransform;
        private TextMeshProUGUI HeaderLabel;
        private QMFoldout qmFoldout;
        private string LabelTxt;
        private bool CurrentState;
        private readonly bool DefaultState;

        public QMCategory(PyroModule module, string labelTxt, bool defaultState = true)
        {
            Module = module;
            LabelTxt = labelTxt;
            DefaultState = defaultState;
        }

        internal void Initialize()
        {
            var num = APIUtils.RandomNumbers();
            HeaderObject = Object.Instantiate(APIUtils.GetCategoryHeader(), MainPyroMenu.GetMenuObject().transform.Find("ScrollRect/Viewport/VerticalLayoutGroup"), false); ;
            HeaderObject.name = $"{Module.ModuleName}-QMCategory-Header-{num}";
            BodyObject = Object.Instantiate(APIUtils.GetCategoryBody(), MainPyroMenu.GetMenuObject().transform.Find("ScrollRect/Viewport/VerticalLayoutGroup"), false);
            BodyObject.name = $"{Module.ModuleName}-QMCategory-Body-{num}";
            HeaderLabel = HeaderObject.GetComponentInChildren<TextMeshProUGUI>();
            HeaderLabel.text = LabelTxt;

            qmFoldout = HeaderObject.GetComponent<QMFoldout>();
            qmFoldout.field_Private_String_0 = $"{Module.ModuleName.Replace(' ', '-')}.{LabelTxt.Replace(' ', '-')}";
            qmFoldout.field_Private_Action_1_Boolean_0 = new System.Action<bool>(val =>
            {
                BodyObject.gameObject.SetActive(val);
                CurrentState = val;
            });

            HeaderObject.transform.Find("Background_Button").GetComponent<Toggle>().isOn = DefaultState;
            BodyObject.transform.DestroyChildren();
            ChildTransform = BodyObject.transform;
        }

        public void SetState(bool newState)
        {
            qmFoldout.field_Private_Action_1_Boolean_0.Invoke(newState);
        }

        public PyroModule GetModule()
        {
            return Module;
        }

        public Transform GetPlacementTransform()
        {
            return ChildTransform;
        }

        public bool GetCurrentState()
        {
            return CurrentState;
        }
    }
}
