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
    public static void Place(int i, int j, Item item, bool isWall)
    {
        if (!Keybinds.HotkeyPlacePlatform.Current)
            return;

        Config config = Config.Instance;
        Item heldItem = Main.LocalPlayer.HeldItem;

        for (int n = 0; n < config.PlatformLength; n++)
        {
            int x = i;
            int y = j;

            // Vertical
            // 0 - no vertical place of platforms
            // -1 - place platforms upwards
            // 1 - place platforms downwards
            if (config.Vertical == 0)
                // Get the tile based on what direction the player is facing
                x = Main.LocalPlayer.direction == 1 ? i + n : i - n;
            else if (config.Vertical == -1)
                y = j - n;
            else if (config.Vertical == 1)
                y = j + n;

            // Place the tile in the world
            bool successfullyPlaced = true;

            if (isWall)
            {
                WorldGen.PlaceWall(
                    i: x,
                    j: y,
                    type: item.createWall);
            }
            else
            {
                successfullyPlaced = WorldGen.PlaceTile(
                    i: x,
                    j: y,
                    Type: item.createTile,
                    style: item.placeStyle);
            } 

            // Tell other players about this tile
            NetMessage.SendTileSquare(
                whoAmi: Main.LocalPlayer.whoAmI,
                tileX: x,
                tileY: y,
                centeredSquareSize: 1);

            // Tile could not be placed
            if (!successfullyPlaced)
                continue;

            // Tile was placed so removed platform item from player inventory
            heldItem.stack--;

            if (heldItem.stack == 0)
            {
                heldItem.TurnToAir();

                // No more platform item to place so stop placing platforms
                break;
            }
        }
    }
}
