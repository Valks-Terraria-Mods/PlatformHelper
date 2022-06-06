global using System;
global using Terraria;
global using Terraria.ID;
global using Terraria.ModLoader;
global using Microsoft.Xna.Framework.Input;

namespace PlatformHelper;

public class PlatformHelper : Mod
{
    public static ModKeybind HotkeyPlacePlatform { get; private set; }

    public override void Load()
    {
        HotkeyPlacePlatform = KeybindLoader.RegisterKeybind(this, "Place Platform", Keys.F);
    }

    public override void Unload()
    {
        HotkeyPlacePlatform = null;
    }
}