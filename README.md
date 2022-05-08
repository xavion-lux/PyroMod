# PyroMod
Your new favorite VRChat Mod Core!

## Purpose
This mod makes it easier to create VRChat mods without having to stress about having all the appropriate materials. This mod is meant to be super user friendly and help make it easier for new mod developers get started making their own mods!

## How To Use

- Add PyroMod as a reference to your project

# Read Me
If you need assistance getting started with PyroMod feel free to join my discord to receive support from either me or others in the community!
https://discord.gg/BlazesClient

```cs
[assembly: MelonInfo(typeof(PyroFlight.Main), "PyroFlight", "1.0.0", "WTFBlaze")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonAdditionalDependencies("PyroMod")] // This is important to tell MelonLoader that PyroMod is an ABSOLUTELY MUST HAVE for your mod to run

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
```

## Functions
| Method  | Description | Return Type |
| ------------- | ------------- | ------------- |
| RegisterModule  | Registers your Module with PyroMod  | PyroModule |
| CreateCategory | Creates a category for your module inside the main Pyro Modules menu  | QMCategory |
| CreateMenu | Creates a custom menu for your module | QMNestedMenu |
| CreateButton | Creates a single action button | QMSingleButton |
| CreateToggle | Creates a double action button toggle button | QMToggleButton |
| CreateSlider | Creates a slider inside your desired menu / category | QMSlider
