using MelonLoader;
using PyroMod;
using PyroMod.API.QuickMenu;
using static PyroMod.Main;

[assembly: MelonInfo(typeof(PyroFlight.Main), "PyroFlight", "1.0.0", "WTFBlaze")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonAdditionalDependencies("PyroMod")]

namespace PyroFlight
{
    public class Main : MelonMod
    {
        public static PyroModule module;
        public static PyroLogs.Instance logger;
        public static QMCategory menu;

        public override void OnApplicationStart()
        {
            module = RegisterModule("Pyro Flight", "1.0.0", "WTFBlaze", "https://github.com/WTFBlaze/PyroFlight");
            menu = module.CreateCategory("Pyro Flight");
            var subMenu = module.CreateMenu(menu, "Menu Test", "Menu test lol", "Menu Test");
            module.CreateButton(subMenu, 1, 0, "UwU", delegate
            {
                logger.Log("UwU Clicked!");
            }, "Write uwu to the console!");
        }
    }
}
