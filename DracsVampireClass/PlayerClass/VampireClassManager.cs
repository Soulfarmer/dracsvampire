using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace DracsVampireClass;

public class VampireClassManager
{
        public static string ClassKey = "playerclass";

        // --------------------------------------------------
        // SET CLASS (SERVER ONLY)
        // --------------------------------------------------
        public static void SetPlayerClass(IServerPlayer player, string classCode)
        {
            if (player?.Entity == null) return;

            var attrs = player.Entity.WatchedAttributes;

            attrs.SetString(ClassKey, classCode);
            attrs.MarkPathDirty(ClassKey);       // <- Sync to client
        }

        // --------------------------------------------------
        // GET CLASS (BOTH CLIENT & SERVER)
        // --------------------------------------------------
        public static string GetPlayerClass(EntityPlayer entityPlayer)
        {
            if (entityPlayer == null) return null;

            return entityPlayer.WatchedAttributes.GetString(ClassKey, null);
        }

        // --------------------------------------------------
        // GET CLASS USING IPlayer (SERVER OR CLIENT)
        // Automatically picks the correct entity version.
        // --------------------------------------------------
        public static string GetPlayerClass(IPlayer player)
        {
            return GetPlayerClass(player?.Entity);
        }

        // --------------------------------------------------
        // CHECK CLASS (convenience)
        // --------------------------------------------------
        public static bool IsVampire(IPlayer player)
        {
            return GetPlayerClass(player) == "vampire";
        }

        public static bool IsVampire(EntityPlayer entityPlayer)
        {
            return GetPlayerClass(entityPlayer) == "vampire";
        }
    }
    