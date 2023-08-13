namespace PlatformHelper.Tiles;

public class Platform : GlobalTile
{
    public override void PlaceInWorld(int i, int j, int type, Item item)
    {
        if (!RegisterTheKeybind.HotkeyPlacePlatform.Current)
            return;

        var heldItem = Main.LocalPlayer.HeldItem;

        for (int n = 0; n < 4; n++)
        {
            int tilePositionX = Main.LocalPlayer.direction == 1 ? i + n : i - n;

            bool successfullyPlacedTile = WorldGen.PlaceTile(tilePositionX, j, item.createTile, style: item.placeStyle);
            NetMessage.SendTileSquare(Main.LocalPlayer.whoAmI, tilePositionX, j, 1);

            if (!successfullyPlacedTile) continue;

            heldItem.stack--;
            if (heldItem.stack == 0)
            {
                heldItem.TurnToAir();
                break;
            }
        }
    }
}
