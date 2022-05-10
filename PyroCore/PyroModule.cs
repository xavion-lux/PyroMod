using PyroMod.API.QuickMenu;
using System;
using System.Collections.Generic;
using System.Reflection;
using VRC;
using VRC.SDKBase;

namespace PyroMod
{
    public class PyroModule
    {
        internal string ModuleName { get; set; }
        internal string ModuleVersion { get; set; }
        internal string ModuleAuthor { get; set; }
        internal string ModuleDownloadURL { get; set; }
        internal QMCategory Category { get; set; }
        internal List<QMNestedMenu> ModuleNestedMenus { get; set; } = new List<QMNestedMenu>();
        internal List<QMSingleButton> ModuleSingleButtons { get; set; } = new List<QMSingleButton>();
        internal List<QMToggleButton> ModuleToggleButtons { get; set; } = new List<QMToggleButton>();
        internal List<QMSlider> ModuleSliders { get; set; } = new List<QMSlider>();
        public PyroLogs.Instance Logger { get; set; }

        internal void InitializeModuleUI()
        {
            try
            {
                if (Category != null) Category.Initialize();
            }
            catch (Exception ex)
            {
                PyroLogs.Error($"Error Creating Module [{ModuleName}]'s Category! | Error Message: " + ex.Message);
            }

            try
            {
                for (int i = 0; i < ModuleNestedMenus.Count; i++)
                {
                    ModuleNestedMenus[i].Initialize();
                }
            }
            catch (Exception ex)
            {
                PyroLogs.Error($"Error Creating Module [{ModuleName}] Nested Menus! | Error Message: " + ex.Message);
            }

            try
            {
                for (int i = 0; i < ModuleSingleButtons.Count; i++)
                {
                    ModuleSingleButtons[i].Initialize();
                }
            }
            catch (Exception ex)
            {
                PyroLogs.Error($"Error Creating Module [{ModuleName}] Single Buttons! | Error Message: " + ex.Message);
            }

            try
            {
                for (int i = 0; i < ModuleToggleButtons.Count; i++)
                {
                    ModuleToggleButtons[i].Initialize();
                }
            }
            catch (Exception ex)
            {
                PyroLogs.Error($"Error Creating Module [{ModuleName}] Toggle Buttons! | Error Message: " + ex.Message);
            }

            try
            {
                for (int i = 0; i < ModuleSliders.Count; i++)
                {
                    ModuleSliders[i].Initialize();
                }
            }
            catch (Exception ex)
            {
                PyroLogs.Error($"Error Creating Module [{ModuleName}] Sliders! | Error Message: " + ex.Message);
            }
        }

        #region Create UI Methods

        public QMCategory CreateCategory(string categoryLabel)
        {
            if (Category != null)
            {
                Logger.Error("You cannot create more than one category!");
                return null;
            }
            Category = new QMCategory(this, categoryLabel);
            if (Main._qmIsInitialized) Category.Initialize();
            return Category;
        }

        public QMNestedMenu CreateMenu(QMCategory category, string btnText, string btnToolTip, string menuLabel)
        {
            var tmp = new QMNestedMenu(category, btnText, btnToolTip, menuLabel);
            if (Main._qmIsInitialized) tmp.Initialize();
            return tmp;
        }

        public QMNestedMenu CreateMenu(QMNestedMenu menu, float posX, float posY, string btnText, string btnToolTip, string menuLabel)
        {
            var tmp = new QMNestedMenu(menu, posX, posY, btnText, btnToolTip, menuLabel);
            if (Main._qmIsInitialized) tmp.Initialize();
            return tmp;
        }

        public QMSingleButton CreateButton(QMCategory category, string btnText, Action btnAction, string btnToolTip)
        {
            var tmp = new QMSingleButton(category, btnText, btnAction, btnToolTip);
            if (Main._qmIsInitialized) tmp.Initialize();
            return tmp;
        }

        public QMSingleButton CreateButton(QMNestedMenu menu, float posX, float posY, string btnText, Action btnAction, string btnToolTip)
        {
            var tmp = new QMSingleButton(menu, btnText, posX, posY, btnAction, btnToolTip);
            if (Main._qmIsInitialized) tmp.Initialize();
            return tmp;
        }

        public QMToggleButton CreateToggle(QMCategory category, string btnText, Action btnOnAction, Action btnOffAction, string btnToolTip, bool defaultState = false)
        {
            var tmp = new QMToggleButton(category, btnText, btnOnAction, btnOffAction, btnToolTip);
            if (Main._qmIsInitialized) tmp.Initialize();
            return tmp;
        }

        public QMToggleButton CreateToggle(QMNestedMenu category, float posX, float posY, string btnText, Action btnOnAction, Action btnOffAction, string btnToolTip, bool defaultState = false)
        {
            var tmp = new QMToggleButton(category, posX, posY, btnText, btnOnAction, btnOffAction, btnToolTip);
            if (Main._qmIsInitialized) tmp.Initialize();
            return tmp;
        }

        public QMSlider CreateSlider(QMCategory category, string labelTxt, float minValue, float maxValue, float currentValue, Action<float> onSliderChanged)
        {
            var tmp = new QMSlider(category, labelTxt, minValue, maxValue, currentValue, onSliderChanged);
            if (Main._qmIsInitialized) tmp.Initialize();
            return tmp;
        }

        public QMSlider CreateSlider(QMNestedMenu menu, float posX, float posY, string labelTxt, float minValue, float maxValue, float currentValue, Action<float> onSliderChanged)
        {
            var tmp = new QMSlider(menu, posX, posY, labelTxt, minValue, maxValue, currentValue, onSliderChanged);
            if (Main._qmIsInitialized) tmp.Initialize();
            return tmp;
        }

        #endregion

        #region Hook Methods

        public void AddHook_PlayerJoined(string methodName)
        {
            var callingClass = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().DeclaringType;
            MethodInfo method = callingClass.GetMethod(methodName);
            if (method == null)
            {
                Logger.Error("Failed to Hook PlayerJoined! | Error Message: method is null!");
                return;
            }
            var methodParams = method.GetParameters();
            if (methodParams.Length != 1)
            {
                if (methodParams.Length < 1)
                    Logger.Error("Failed to Hook PlayerJoined! | Error Message: too many parameters!");
                else
                    Logger.Error("Failed to Hook PlayerJoined! | Error Message: method missing parameters!");
                return;
            }
            if (methodParams[0].ParameterType != typeof(Player))
            {
                Logger.Error("Failed to Hook PlayerJoined! | Error Message: first parameter is not vrc.player!");
                return;
            }
            Hooks.OnPlayerJoined += new Action<Player>(player =>
            {
                method.Invoke(null, new object[] { player });
            });
        }

        public void AddHook_PlayerLeft(string methodName)
        {
            var callingClass = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().DeclaringType;
            MethodInfo method = callingClass.GetMethod(methodName);
            if (method == null)
            {
                Logger.Error("Failed to Hook PlayerLeft! | Error Message: method is null!");
                return;
            }
            var methodParams = method.GetParameters();
            if (methodParams.Length != 1)
            {
                if (methodParams.Length < 1)
                    Logger.Error("Failed to Hook PlayerLeft! | Error Message: too many parameters!");
                else
                    Logger.Error("Failed to Hook PlayerLeft! | Error Message: method missing parameters!");
                return;
            }
            if (methodParams[0].ParameterType != typeof(Player))
            {
                Logger.Error("Failed to Hook PlayerLeft! | Error Message: first parameter is not vrc.player!");
                return;
            }
            Hooks.OnPlayerLeft += new Action<Player>(player =>
            {
                method.Invoke(null, new object[] { player });
            });
        }

        public void AddHook_RPCReceived(string methodName)
        {
            var callingClass = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().DeclaringType;
            MethodInfo method = callingClass.GetMethod(methodName);
            if (method == null)
            {
                Logger.Error("Failed to Hook RPCReceived! | Error Message: method is null!");
                return;
            }
            var methodParams = method.GetParameters();
            if (methodParams.Length != 3)
            {
                if (methodParams.Length < 3)
                    Logger.Error("Failed to Hook RPCReceived! | Error Message: too many parameters!");
                else
                    Logger.Error("Failed to Hook RPCReceived! | Error Message: method missing parameters!");
                return;
            }
            if (methodParams[0].ParameterType != typeof(Player))
            {
                Logger.Error("Failed to Hook RPCReceived! | Error Message: first parameter is not vrc.player!");
                return;
            }
            if (methodParams[1].ParameterType != typeof(VRC_EventHandler.VrcEvent))
            {
                Logger.Error("Failed to Hook RPCReceived! | Error Message: second parameter is not vrc.sdkbase.vrc_eventhandler.vrcevent!");
                return;
            }
            if (methodParams[2].ParameterType != typeof(VRC_EventHandler.VrcBroadcastType))
            {
                Logger.Error("Failed to Hook RPCReceived! | Error Message: third parameter is not vrc.sdkbase.vrc_eventhandler.vrcbroadcasttype!");
                return;
            }
            Hooks.OnRPCReceived += new Action<Player, VRC_EventHandler.VrcEvent, VRC_EventHandler.VrcBroadcastType>((player, eventInfo, broadcastType) =>
            {
                method.Invoke(null, new object[] { player, eventInfo, broadcastType });
            });
        }

        #endregion

        #region Misc Methods

        public QMCategory GetCategory()
        {
            return Category;
        }

        #endregion
    }

}
