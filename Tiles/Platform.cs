using ReLogic.Utilities;

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
        Item heldItem = Main.LocalPlayer.HeldItem;
        Config config = Config.Instance;
        int length = Math.Min(config.PlatformLength,heldItem.stack)-1;
        if (!Keybinds.HotkeyPlacePlatform.Current || length == 0)
            return;
        
        for (int n = 0; n < length; n++)
        {
            // If we have no items, immediately return
            if(heldItem.stack == 0) {
                heldItem.TurnToAir();
                return;
            }
            // 0 - no vertical place of platforms
            // -1 - place platforms upwards
            // 1 - place platforms downwards
            if (config.Vertical == 0) x+=Main.LocalPlayer.direction;
            else y-=config.Vertical;

            // Place the tile in the world

            if (isWall)
            {
                WorldGen.PlaceWall(
                    i: x,
                    j: y,
                    type: item.createWall);
                // Wall could not be placed
                if (Main.tile[x,y].WallType != item.createWall)
                    continue;
            } else {
                WorldGen.PlaceTile(
                    i: x,
                    j: y,
                    Type: item.createTile,
                    style: item.placeStyle);
                // Tile could not be placed
                if (Main.tile[x,y].TileType != item.createTile)
                    continue;
            } 

            // Tell other players about this tile
            NetMessage.SendTileSquare(
                whoAmi: Main.LocalPlayer.whoAmI,
                tileX: x,
                tileY: y,
                centeredSquareSize: 1);


            // Tile was placed so removed platform item from player inventory
            heldItem.stack--;
        }
    }
}
