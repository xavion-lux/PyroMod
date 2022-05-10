using MelonLoader;
using static PyroMod.Main;

[assembly: MelonInfo(typeof(Example_Module.Main), "Example Module", "1.0.0")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace Example_Module
{
    public class Main : MelonMod
    {
        public static PyroModule module;

        public override void OnApplicationStart()
        {
            module = RegisterModule("Example Module", "1.0.0", "WTFBlaze");
            module.AddHook_PlayerJoined(nameof(PlayerJoined));
        }

        public static void PlayerJoined(VRC.Player player)
        {
            module.Logger.Log($"{player.field_Private_APIUser_0.displayName} has joined!");
        }
    }
}
