using Terraria.ModLoader;

namespace PlatformHelper
{
	class PlatformHelper : Mod
	{
        public static ModHotKey PlatformHotKey;

        public PlatformHelper()
		{
		}

        public override void Load()
        {
            PlatformHotKey = RegisterHotKey("Platform Helper", "F");
        }

        public override void Unload()
        {
            PlatformHotKey = null;
        }
    }
}
