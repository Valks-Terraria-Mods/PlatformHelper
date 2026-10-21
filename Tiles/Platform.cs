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
    private static bool IsPlacing = false;
    public static void Place(int x, int y, Item item, bool isWall)
    {
        // Skip attempt if we're already trying in an attempt.
        //  Not skipping this will probably freeze the game as it tries to loop to the rest of the world
        
        if(IsPlacing) return;
        Player localPlayer = Main.LocalPlayer;
        Item heldItem = Main.LocalPlayer.HeldItem;
    	if(!Keybinds.HotkeyPlacePlatform.Current || heldItem != item) return;
        Config config = Config.Instance;
        // Check length, subtract one since the game already placed one
        int length = Math.Min(config.PlatformLength,heldItem.stack);
        if (length == 0) return;
        // Store the player's current target position
        int targetX = Player.tileTargetX;
        int targetY = Player.tileTargetY;
        
        bool releaseUseItem = localPlayer.releaseUseItem;
        bool controlUseItem = localPlayer.controlUseItem;
        
        // Prevent function re-running
        /* FIXME PLACING BLOCKS AT RANGE LIMIT CAUSES STACK DESYNC */
        IsPlacing = true;
        for (int n = 1; n < length; n++) 
        {
            // If stack is 0, immediately return
            if(heldItem.stack == 0) {
                heldItem.TurnToAir();
                break;
            }
            // 0 - no vertical place of platforms
            // -1 - place platforms upwards
            // 1 - place platforms downwards
            if (config.Vertical == 0) x+=Main.LocalPlayer.direction;
            else y-=config.Vertical;
            // Move player's target to x/y
            Player.tileTargetX = x;
            Player.tileTargetY = y;
            // Change animation time to 0 to skip place animation
            localPlayer.itemTime = 0;
            localPlayer.itemAnimation = 0;
                
            // If the tile is the same, skip
            if(isWall ? Main.tile[x, y].WallType == heldItem.createWall : Main.tile[x, y].TileType == heldItem.createTile) {
                continue;
            }
            // Place item
            Main.LocalPlayer.ItemCheck();
        }
        IsPlacing = false;
        Player.tileTargetX = targetX;
        Player.tileTargetY = targetY;
    }
}
