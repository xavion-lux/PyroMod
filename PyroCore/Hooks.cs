using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using static VRC.SDKBase.VRC_EventHandler;

namespace PyroMod
{
    public class Hooks
    {
        public class Hook
        {
            public MethodInfo TargetMethod { get; set; }
            public HarmonyMethod PrefixMethod { get; set; }
            public HarmonyMethod PostfixMethod { get; set; }
            public HarmonyLib.Harmony Instance { get; set; }

            public Hook(MethodInfo targetMethod, HarmonyMethod Before = null, HarmonyMethod After = null)
            {
                if (targetMethod == null || Before == null && After == null) return;
                Instance = new HarmonyLib.Harmony($"Patch:{targetMethod.DeclaringType.FullName}.{targetMethod.Name}");
                TargetMethod = targetMethod;
                PrefixMethod = Before;
                PostfixMethod = After;
                Initialize();
            }

            private void Initialize()
            {
                try
                {
                    _instance.Patch(TargetMethod, PrefixMethod, PostfixMethod);
                }
                catch (Exception ex)
                {
                    PyroLogs.Failure($"Failed to hook {TargetMethod?.Name}! | Error Message: {ex.Message}");
                }
            }
        }

        public class Patch
        {
            public PyroModule Module { get; set; }
            public MethodInfo TargetMethod { get; set; }
            public HarmonyMethod PrefixMethod { get; set; }
            public HarmonyMethod PostfixMethod { get; set; }
            public HarmonyLib.Harmony Instance { get; set; }

            public Patch(PyroModule module, MethodInfo targetMethod, HarmonyMethod Before = null, HarmonyMethod After = null)
            {
                if (targetMethod == null || Before == null && After == null) return;
                Module = module;
                Instance = module.HarmonyInstance;
                TargetMethod = targetMethod;
                PrefixMethod = Before;
                PostfixMethod = After;
                module.Patches.Add(this);
            }

            internal void ProcessPatch()
            {
                try
                {
                    _instance.Patch(TargetMethod, PrefixMethod, PostfixMethod);
                    Module.Logger.Success($"Patched {TargetMethod.Name}!");
                }
                catch (Exception ex)
                {
                    Module.Logger.Failure($"Failed to Register Patch {TargetMethod?.Name}! | Error Message: {ex.Message}");
                }
            }
        }

        private static readonly HarmonyLib.Harmony _instance = new HarmonyLib.Harmony("PyroMod");
        internal static event Action<VRC.Player> OnPlayerJoined;
        internal static event Action<VRC.Player> OnPlayerLeft;
        internal static event Action<VRC.Player, VrcEvent, VrcBroadcastType> OnRPCReceived;
        internal static event Action OnQMInitialized;
        internal static event Action<VRCPlayer> OnLocalPlayerLoaded;
        internal static event Action OnLeftRoom;

        internal static void Initialize()
        {
            new Hook(typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Void_Player_0)), GetPatch(nameof(PlayerJoined)), null);
            new Hook(typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Void_Player_1)), GetPatch(nameof(PlayerLeft)), null);
            new Hook(AccessTools.Method(typeof(VRC_EventDispatcherRFC), nameof(VRC_EventDispatcherRFC.Method_Public_Boolean_Player_VrcEvent_VrcBroadcastType_0)), GetPatch(nameof(RPCReceived)));
            new Hook(typeof(NetworkManager).GetMethod(nameof(NetworkManager.OnLeftRoom)), GetPatch(nameof(LeftRoom)));
        }

        private static HarmonyMethod GetPatch(string name)
        {
            PyroLogs.Success($"Succesfully hooked {name}!");
            return new HarmonyMethod(typeof(Hooks).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        }

        private static void PlayerJoined(VRC.Player __0)
        {
            OnPlayerJoined?.Invoke(__0);
        }

        private static void PlayerLeft(VRC.Player __0)
        {
            OnPlayerLeft?.Invoke(__0);
        }

        private static void RPCReceived(ref VRC.Player __0, ref VrcEvent __1, ref VrcBroadcastType __2)
        {
            OnRPCReceived?.Invoke(__0, __1, __2);
        }

        private static void LeftRoom()
        {
            OnLeftRoom?.Invoke();
        }

        internal static void OnQuickMenuInit()
        {
            OnQMInitialized.Invoke();
        }

        internal static void LocalPlayerLoaded(VRCPlayer vrcPlayer)
        {
            OnLocalPlayerLoaded?.Invoke(vrcPlayer);
        }
    }
}
