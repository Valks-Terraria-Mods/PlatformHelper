﻿using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace PlatformHelper;

public class Config : ModConfig
{
    public static Config Instance { get; private set; }

    public override ConfigScope Mode => ConfigScope.ServerSide;

    [DefaultValue(4)]
    [Range(2, 1000)]
    public int PlatformLength;

    [DefaultValue(0)]
    [Range(-1, 1)]
    public int Vertical;

    public override void OnLoaded()
    {
        Instance = this;
    }
}
