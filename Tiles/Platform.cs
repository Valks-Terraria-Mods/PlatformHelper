using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlatformHelper.Tiles
{
    public class Platform : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, Item item) {
            if (PlatformHelper.PlatformHotKey.Current) {
                if (item.createTile == TileID.Platforms)
                {
                    int amount = Math.Min(Main.LocalPlayer.HeldItem.stack, 5);
                    Item heldItem = Main.LocalPlayer.HeldItem;
                    if (Main.LocalPlayer.direction == 1)
                    {
                        heldItem.stack -= (amount - 1);
                        for (int n = 1; n < amount; n++)
                        {
                            WorldGen.PlaceTile(i + n, j, item.createTile, false, false, -1, item.placeStyle);
                        }

                    }
                    else
                    {
                        heldItem.stack -= (amount - 1);
                        for (int n = 1; n < amount; n++)
                        {
                            WorldGen.PlaceTile(i - n, j, item.createTile, false, false, -1, item.placeStyle);
                        }
                    }
                }
            }
        }
    }
}
