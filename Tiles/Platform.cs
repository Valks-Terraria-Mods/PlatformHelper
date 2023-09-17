namespace PlatformHelper.Tiles;

public class Platform : GlobalTile
{
    public override void PlaceInWorld(int i, int j, int type, Item item)
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
            bool successfullyPlacedTile = 
                WorldGen.PlaceTile(
                    i: x, 
                    j: y, 
                    Type: item.createTile, 
                    style: item.placeStyle);

            // Tell other players about this tile
            NetMessage.SendTileSquare(
                whoAmi: Main.LocalPlayer.whoAmI, 
                tileX: x, 
                tileY: y, 
                centeredSquareSize: 1);

            // Tile could not be placed
            if (!successfullyPlacedTile) 
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
