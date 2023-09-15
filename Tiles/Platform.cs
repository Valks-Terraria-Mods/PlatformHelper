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
            // Get the tile based on what direction the player is facing
            int tilePositionX = 
                Main.LocalPlayer.direction == 1 ? i + n : i - n;

            // Place the tile in the world
            bool successfullyPlacedTile = 
                WorldGen.PlaceTile(
                    i: tilePositionX, 
                    j: j, 
                    Type: item.createTile, 
                    style: item.placeStyle);

            // Tell other players about this tile
            NetMessage.SendTileSquare(
                whoAmi: Main.LocalPlayer.whoAmI, 
                tileX: tilePositionX, 
                tileY: j, 
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
