using MelonLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PyroMod;
using PyroMod.API.QuickMenu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;

[assembly: MelonInfo(typeof(PyroMod.Main), PyroBuildInfo.Name, PyroBuildInfo.Version, PyroBuildInfo.Author, PyroBuildInfo.RepoUrl)]
[assembly: MelonGame(PyroBuildInfo.GameAndDeveloper, PyroBuildInfo.GameAndDeveloper)]
[assembly: MelonColor(PyroBuildInfo.ModColor)]

namespace PyroMod
{
    public class PyroBuildInfo
    {
        public const string Name = "PyroMod";
        public const string Version = "1.0.1";
        public const string Author = "WTFBlaze";
        public const string RepoUrl = "https://github.com/WTFBlaze/PyroMod";
        public const string GameAndDeveloper = "VRChat";
        public const ConsoleColor ModColor = ConsoleColor.DarkRed;
    }

    public class Main : MelonMod
    {
        internal static List<PyroModule> PyroModules = new List<PyroModule>();
        internal static QMTabMenu MainPyroMenu;
        internal static bool _qmIsInitialized;

        public override void OnApplicationStart()
        {
            if (!Directory.Exists("UserData\\PyroMod"))
            {
                Directory.CreateDirectory("UserData\\PyroMod");
            }
            if (!Directory.Exists("UserData\\PyroMod\\Logs"))
            {
                Directory.CreateDirectory("UserData\\PyroMod\\Logs");
            }
            PyroLogs.Initialize();
            using (WebClient webClient = new WebClient())
            {
                var data = webClient.DownloadString("https://cdn.wtfblaze.com/mods/Mods.json");
                var result = JsonConvert.DeserializeObject<JToken>(data);

                foreach (var item in result)
                {
                    if ((string)item["Name"] == "PyroMod")
                    {
                        if ((string)item["Version"] == PyroBuildInfo.Version)
                            PyroLogs.Log("PyroMod is up to date!");
                        else
                            PyroLogs.Warning($"Your are running an outdated version of PyroMod! Latest Version: {(string)result["tag_name"]} | Your Version: {PyroBuildInfo.Version}. You can download the latest version from the official repo. https://github.com/WTFBlaze/PyroMod/releases");
                    }
                }
            }
            MelonCoroutines.Start(WaitForQM());
            Hooks.Initialize();
        }

        private IEnumerator WaitForQM()
        {
            while (UnityEngine.Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>()?.transform.Find("Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings").gameObject == null) yield return null;
            QuickMenuInitialized();
            yield break;
        }

        private void QuickMenuInitialized()
        {
            PyroLogs.Log("Initializing Module UI's...");
            MainPyroMenu = new QMTabMenu("Pyro Mod Created by WTFBlaze!", "PyroMod Modules");
            for(int i = 0; i < PyroModules.Count; i++)
            {
                PyroModules[i].InitializeModuleUI();
            }
            _qmIsInitialized = true;
        } 

        public static PyroModule RegisterModule(string moduleName, string moduleVersion, string moduleAuthor, ConsoleColor? moduleColor = ConsoleColor.DarkGray, string moduleDownloadUrl = null)
        {
            // Check for duplicate named Modules because I am evil and nobody can name identically named modules lol
            if (PyroModules.Exists(x => x.ModuleName.ToLower() == moduleName.ToLower()))
            {
                PyroLogs.Error($"Failed to load module [{moduleName} by {moduleAuthor}] because a module by that name from another author already exists!");
                return null;
            }

            PyroModule tmp = new PyroModule()
            {
                ModuleName = moduleName,
                ModuleAuthor = moduleAuthor,
                ModuleDownloadURL = moduleDownloadUrl,
                ModuleVersion = moduleVersion,
                Logger = new PyroLogs.Instance(moduleName, (ConsoleColor)moduleColor),
            };
            PyroModules.Add(tmp);
            PyroLogs.Log($"Loaded Module {moduleName} v{moduleVersion} by {moduleAuthor}{(!string.IsNullOrEmpty(moduleDownloadUrl) ? $" (Download Link: {moduleDownloadUrl})" : string.Empty)}", ConsoleColor.Green);
            return tmp;
        }

        public class PyroModule
        {
            internal string ModuleName { get; set; }
            internal string ModuleVersion { get; set; }
            internal string ModuleAuthor { get; set; }
            internal string ModuleDownloadURL { get; set; }
            internal List<QMCategory> ModuleCategories { get; set; } = new List<QMCategory>();
            internal List<QMNestedMenu> ModuleNestedMenus { get; set; } = new List<QMNestedMenu>();
            internal List<QMSingleButton> ModuleSingleButtons { get; set; } = new List<QMSingleButton>();
            internal List<QMToggleButton> ModuleToggleButtons { get; set; } = new List<QMToggleButton>();
            internal List<QMSlider> ModuleSliders { get; set; } = new List<QMSlider>();
            public PyroLogs.Instance Logger { get; set; }

            internal void InitializeModuleUI()
            {
                try
                {
                    for (int i = 0; i < ModuleCategories.Count; i++)
                    {
                        ModuleCategories[i].Initialize();
                    }
                }
                catch (Exception ex)
                {
                    PyroLogs.Error($"Error Creating Module [{ModuleName}] Categories! | Error Message: " + ex.Message);
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
                var tmp = new QMCategory(this, categoryLabel);
                if (_qmIsInitialized) tmp.Initialize();
                return tmp;
            }

            public QMNestedMenu CreateMenu(QMCategory category, string btnText, string btnToolTip, string menuLabel)
            {
                var tmp = new QMNestedMenu(category, btnText, btnToolTip, menuLabel);
                if (_qmIsInitialized) tmp.Initialize();
                return tmp;
            }

            public QMNestedMenu CreateMenu(QMNestedMenu menu, float posX, float posY, string btnText, string btnToolTip, string menuLabel)
            {
                var tmp = new QMNestedMenu(menu, posX, posY, btnText, btnToolTip, menuLabel);
                if (_qmIsInitialized) tmp.Initialize();
                return tmp;
            }

            public QMSingleButton CreateButton(QMCategory category, string btnText, Action btnAction, string btnToolTip)
            {
                var tmp = new QMSingleButton(category, btnText, btnAction, btnToolTip);
                if (_qmIsInitialized) tmp.Initialize();
                return tmp;
            }

            public QMSingleButton CreateButton(QMNestedMenu menu, float posX, float posY, string btnText, Action btnAction, string btnToolTip)
            {
                var tmp = new QMSingleButton(menu, btnText, posX, posY, btnAction, btnToolTip);
                if (_qmIsInitialized) tmp.Initialize();
                return tmp;
            }

            public QMToggleButton CreateToggle(QMCategory category, string btnText, Action btnOnAction, Action btnOffAction, string btnToolTip, bool defaultState = false)
            {
                var tmp = new QMToggleButton(category, btnText, btnOnAction, btnOffAction, btnToolTip);
                if (_qmIsInitialized) tmp.Initialize();
                return tmp;
            }

            public QMToggleButton CreateToggle(QMNestedMenu category, float posX, float posY, string btnText, Action btnOnAction, Action btnOffAction, string btnToolTip, bool defaultState = false)
            {
                var tmp = new QMToggleButton(category, posX, posY, btnText, btnOnAction, btnOffAction, btnToolTip);
                if (_qmIsInitialized) tmp.Initialize();
                return tmp;
            }

            public QMSlider CreateSlider(QMCategory category, string labelTxt, float minValue, float maxValue, float currentValue, Action<float> onSliderChanged)
            {
                var tmp = new QMSlider(category, labelTxt, minValue, maxValue, currentValue, onSliderChanged);
                if (_qmIsInitialized) tmp.Initialize();
                return tmp;
            }

            public QMSlider CreateSlider(QMNestedMenu menu, float posX, float posY, string labelTxt, float minValue, float maxValue, float currentValue, Action<float> onSliderChanged)
            {
                var tmp = new QMSlider(menu, posX, posY, labelTxt, minValue, maxValue, currentValue, onSliderChanged);
                if (_qmIsInitialized) tmp.Initialize();
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
                if (methodParams[0].ParameterType != typeof(VRC.Player))
                {
                    Logger.Error("Failed to Hook PlayerJoined! | Error Message: first parameter is not vrc.player!");
                    return;
                }
                Hooks.OnPlayerJoined += new Action<VRC.Player>(player =>
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
                if (methodParams[0].ParameterType != typeof(VRC.Player))
                {
                    Logger.Error("Failed to Hook PlayerLeft! | Error Message: first parameter is not vrc.player!");
                    return;
                }
                Hooks.OnPlayerLeft += new Action<VRC.Player>(player =>
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
                if (methodParams[0].ParameterType != typeof(VRC.Player))
                {
                    Logger.Error("Failed to Hook RPCReceived! | Error Message: first parameter is not vrc.player!");
                    return;
                }
                if (methodParams[1].ParameterType != typeof(VRC.SDKBase.VRC_EventHandler.VrcEvent))
                {
                    Logger.Error("Failed to Hook RPCReceived! | Error Message: second parameter is not vrc.sdkbase.vrc_eventhandler.vrcevent!");
                    return;
                }
                if (methodParams[2].ParameterType != typeof(VRC.SDKBase.VRC_EventHandler.VrcBroadcastType))
                {
                    Logger.Error("Failed to Hook RPCReceived! | Error Message: third parameter is not vrc.sdkbase.vrc_eventhandler.vrcbroadcasttype!");
                    return;
                }
                Hooks.OnRPCReceived += new Action<VRC.Player, VRC.SDKBase.VRC_EventHandler.VrcEvent, VRC.SDKBase.VRC_EventHandler.VrcBroadcastType>((player, eventInfo, broadcastType) =>
                {
                    method.Invoke(null, new object[] { player, eventInfo, broadcastType });
                });
            }

            #endregion
        }
    }
}
