# PyroMod
This mod makes it easier to create VRChat mods without having to stress about having all the appropriate materials. This mod is meant to be super user friendly and help make it easier for new mod developers get started making their own mods!

# Read Me
If you need assistance getting started with PyroMod feel free to join my discord to receive support from either me or others in the community!
https://discord.gg/BlazesClient

If you find a bug or want to suggest a feature to be added to PyroMod you can open an issue and let us know OR fork the repo and make those changes yourself and make a pull request!

To find a list of all available PyroModules join the discord linked above.

## How To Use

- Follow the basic project example from [MelonLoader's Official Wiki](https://melonwiki.xyz/#/modders/quickstart?id=basic-mod-setup)
- Add PyroMod as a reference to your project

Inside your Module's OnApplicationStart Method Register your Module with PyroMod
```cs
// [] optional parameters
var Module = PyroMod.Main.RegisterModule(ModuleName, ModuleVersion, ModuleAuthor, [ModuleColor], [ModuleDownloadURL]);
```

There is an example project attached to the repo

## Functions
| Method  | Description | Return Type |
| ------------- | ------------- | ------------- |
| RegisterModule  | Registers your Module with PyroMod  | PyroModule |
| CreateCategory | Creates a category for your module inside the main Pyro Modules menu  | QMCategory |
| CreateMenu | Creates a custom menu for your module | QMNestedMenu |
| CreateButton | Creates a single action button | QMSingleButton |
| CreateToggle | Creates a double action button toggle button | QMToggleButton |
| CreateSlider | Creates a slider inside your desired menu / category | QMSlider |
| AddHook_PlayerJoined | Calls your provided method whenever a player joins the room | N/A |
| AddHook_PlayerLeft | Calls your provided method whenever a player leaves the room | N/A |
| AddHook_RPCReceived | Calls your provided method whenever an rpc is sent in the room | N/A |
| AddHook_QMInitialized | Calls your provided method when the VRChat QuickMenu is Initialized | N/A |
| AddHook_LocalPlayerLoaded | Calls your provided method when the local player (the person using pyro) loads into a room | N/A |
| AddHook_LeftRoom | Calls your provided method when the local player (the person using pyro) leaves the room | N/A |

## Planned Features
- ~~Hooks for OnPlayerJoined, OnPlayerLeft, OnRPCReceived, etc~~ *[Completed]*
- ~~Easier Harmony Patching~~
- Wings Button API
- Added Icon to Tab Button
---
#### Disclaimer: Modification of the VRChat client goes against VRChat's <a href="https://hello.vrchat.com/legal">ToS</a> and could result in your account being banned. Use mods at your own risks.
#### This mod is meant to be used to better VRChat's modding community and to be a helpful learning tool for up and coming developers who need that boost to get start.
