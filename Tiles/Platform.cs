namespace PlatformHelper.Tiles;

public class Platform : GlobalTile
{
    public override void PlaceInWorld(int i, int j, int type, Item item)
    {
        if (!PlatformHelper.HotkeyPlacePlatform.Current)
            return;

        var amount = Math.Min(Main.LocalPlayer.HeldItem.stack, 5);
        var heldItem = Main.LocalPlayer.HeldItem;

        if (Main.LocalPlayer.direction == 1)
        {
            heldItem.stack -= (amount - 1);
            for (int n = 1; n < amount; n++)
            {
                WorldGen.PlaceTile(i + n, j, item.createTile, style: item.placeStyle);
                NetMessage.SendTileSquare(Main.LocalPlayer.whoAmI, i + n, j, 1);
            }
        }
        else
        {
            heldItem.stack -= (amount - 1);
            for (int n = 1; n < amount; n++)
            {
                WorldGen.PlaceTile(i - n, j, item.createTile, style: item.placeStyle);
                NetMessage.SendTileSquare(Main.LocalPlayer.whoAmI, i - n, j, 1);
            }
        }
    }
}
