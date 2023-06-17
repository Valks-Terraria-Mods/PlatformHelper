namespace PlatformHelper.Tiles;

public class Platform : GlobalTile
{
    public override void PlaceInWorld(int i, int j, int type, Item item)
    {
        if (!PlatformHelper.HotkeyPlacePlatform.Current)
            return;

        var amount = Math.Min(Main.LocalPlayer.HeldItem.stack, 5);
        var heldItem = Main.LocalPlayer.HeldItem;

        bool directionRight = Main.LocalPlayer.direction == 1;

        heldItem.stack -= (amount - 1);
        for (int n = 1; n < amount; n++)
        {
            int tilePositionX = directionRight ? i + n : i - n;

            WorldGen.PlaceTile(tilePositionX, j, item.createTile, style: item.placeStyle);
            NetMessage.SendTileSquare(Main.LocalPlayer.whoAmI, tilePositionX, j, 1);
        }
    }
}
