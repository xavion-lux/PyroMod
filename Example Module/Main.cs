using MelonLoader;
using PyroMod;
using System;
using static PyroMod.Main;

[assembly: MelonInfo(typeof(Example_Module.Main), "Example Module", "1.0.0")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace Example_Module
{
    public class Main : MelonMod
    {
        public class ModuleInfo
        {
            public const string Name = "Your Module Name Here";
            public const string Version = "1.0.0";
            public const string Author = "Your Name Here";
            public const ConsoleColor Color = ConsoleColor.Yellow;
            public const string DownloadURL = "https://github.com/YourUsername/YourRepoName";
        }

        // The core access point for your module and PyroMod!
        public static PyroModule module;

        public override void OnApplicationStart()
        {
            /* 
             * Required to tell PyroMod you are a valid Module!
             * 
             * Additional parameters you can add (in this order)
             * - ModuleInfo.Color
             * - ModuleInfo.DownloadURL
             */
            module = RegisterModule(ModuleInfo.Name, ModuleInfo.Version, ModuleInfo.Author);

            // Create your mod's category
            module.CreateCategory("Label Text");

            // Single Button
            module.CreateButton(module.GetCategory(), "UwU", delegate
            {
                /*
                 *  PyroLogs Example
                 *  
                 *  Available Log Types
                 *  Logger.Log();
                 *  Logger.Warning();
                 *  Logger.Error();
                 *  Logger.Success();
                 *  Logger.Failure();
                 */
                module.Logger.Log("UwU!");
            }, "The UwU Button");

            // Nested Menu Button
            var subMenu = module.CreateMenu(module.GetCategory(), "Sub Menu", "This is a nested button that opens a sub menu!", "Sub Menu");

            /*
             *  Placing UI Items inside a Sub Menu Example
             *
             *  PosX: 1, PosY: 0, will place the button at the very top left of the menu.
             *  If you want to place another button right next to it just change PosX to 2.
             *  
             *  Addition Parameters
             *  - (Boolean) Default State : the default state the button should be set to. 
             */
            module.CreateToggle(subMenu, 1, 0, "Toggle Example", delegate
            {
                module.Logger.Log("Toggle State: True!");
            }, delegate
            {
                module.Logger.Log("Toggle State: False!");
            }, "Toggle Button Example");

            // Slider
            module.CreateSlider(subMenu, 15, 25, "Slider lol", -300, -250, 0.35f, delegate (float newValue)
            {
                module.Logger.Log($"Slider Value: {newValue}");
            });
        }

        public static void PlayerJoined(VRC.Player player)
        {
            module.Logger.Log($"{player.field_Private_APIUser_0.displayName} has joined!");
        }
    }
}
