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
using UnityEngine;

[assembly: MelonInfo(typeof(PyroMod.Main), PyroBuildInfo.Name, PyroBuildInfo.Version, PyroBuildInfo.Author, PyroBuildInfo.RepoUrl)]
[assembly: MelonGame(PyroBuildInfo.GameAndDeveloper, PyroBuildInfo.GameAndDeveloper)]
[assembly: MelonColor(PyroBuildInfo.ModColor)]

namespace PyroMod
{
    public class PyroBuildInfo
    {
        public const string Name = "PyroMod";
        public const string Version = "1.0.3";
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
                            PyroLogs.Warning($"Your are running an outdated version of PyroMod! Latest Version: {(string)result["Version"]} | Your Version: {PyroBuildInfo.Version}. You can download the latest version from the official repo. https://github.com/WTFBlaze/PyroMod/releases");
                    }
                }
            }
            MelonCoroutines.Start(WaitForQM());
            Hooks.Initialize();
            for (int i = 0; i < PyroModules.Count; i++)
            {
                PyroModules[i].InitializePatches();
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex = -1)
                MelonCoroutines.Start(WaitForPlayer());
        }

        private IEnumerator WaitForQM()
        {
            while (UnityEngine.Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>()?.transform.Find("Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings").gameObject == null) yield return null;
            QuickMenuInitialized();
            Hooks.OnQuickMenuInit();
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

        private IEnumerator WaitForPlayer()
        {
            while (VRCPlayer.field_Internal_Static_VRCPlayer_0 == null) yield return null;
            Hooks.LocalPlayerLoaded(VRCPlayer.field_Internal_Static_VRCPlayer_0);
            yield break;
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
                HarmonyInstance = new HarmonyLib.Harmony("PyroPatching-" + moduleName),
            };
            PyroModules.Add(tmp);
            PyroLogs.Log($"Loaded Module {moduleName} v{moduleVersion} by {moduleAuthor}{(!string.IsNullOrEmpty(moduleDownloadUrl) ? $" (Download Link: {moduleDownloadUrl})" : string.Empty)}", ConsoleColor.Green);
            return tmp;
        }
    }
}
