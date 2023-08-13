global using System;
global using Terraria;
global using Terraria.ID;
global using Terraria.ModLoader;
global using Microsoft.Xna.Framework.Input;

using Terraria.ModLoader.Config;

namespace PlatformHelper;

public class RegisterTheKeybind : ModSystem
{
    public static ModKeybind HotkeyPlacePlatform { get; private set; }

    public override void Load()
    {
        HotkeyPlacePlatform = KeybindLoader.RegisterKeybind(Mod, "Place Platform", "F");
    }

    public override void Unload()
    {
        HotkeyPlacePlatform = null;
    }
}
