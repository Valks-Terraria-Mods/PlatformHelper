using ReLogic.Utilities;
using Terraria.ObjectData;

namespace PlatformHelper.Tiles;

public class Walls : GlobalWall
{
    public override void PlaceInWorld(int i, int j, int type, Item item)
    {
        Utils.Place(i, j, item, isWall: true);
    }
}

public class Tiles : GlobalTile
{
    public override void PlaceInWorld(int i, int j, int type, Item item)
    {
        Utils.Place(i, j, item, isWall: false);
    }
}

public static class Utils
{
    public static void Place(int x, int y, Item item, bool isWall)
    {
        if (!Keybinds.HotkeyPlacePlatform.Current) return;

        // Only extend placement for actual placeable items.
        if (isWall)
        {
            if (item.createWall < 0) return;
        }
        else
        {
            if (item.createTile < 0) return;

            // Multi-tile objects must be placed normally to avoid deleting or
            // duplicating parts of furniture when the extra placement runs.
            TileObjectData tileData = TileObjectData.GetTileData(item.createTile, item.placeStyle);
            if (tileData != null && (tileData.Width > 1 || tileData.Height > 1)) return;
        }

        Item heldItem = Main.LocalPlayer.HeldItem;
        Config config = Config.Instance;

        // Check length, subtract one since the game already placed one.
        int length = Math.Min(config.PlatformLength, heldItem.stack) - 1;
        if (length <= 0) return;

        for (int n = 0; n < length; n++)
        {
            if (heldItem.stack == 0)
            {
                heldItem.TurnToAir();
                return;
            }

            // 0 - no vertical placement; -1 - upwards; 1 - downwards.
            if (config.Vertical == 0) x += Main.LocalPlayer.direction;
            else y -= config.Vertical;

            if (isWall)
            {
                WorldGen.PlaceWall(i: x, j: y, type: item.createWall);
                if (Main.tile[x, y].WallType != item.createWall) continue;
            }
            else
            {
                WorldGen.PlaceTile(i: x, j: y, Type: item.createTile, style: item.placeStyle);
                if (Main.tile[x, y].TileType != item.createTile) continue;
            }

            heldItem.stack--;

            NetMessage.SendTileSquare(
                whoAmi: Main.LocalPlayer.whoAmI,
                tileX: x,
                tileY: y,
                centeredSquareSize: 1);
        }
    }
}
