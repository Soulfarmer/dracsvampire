using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace DracsVampireClass;

// TODO: If the player feeds of a drifter/drifter variant during a Temporal storm it gets a temporary boost

public class DracsVampireClassModSystem : ModSystem
{
    ICoreServerAPI sapi;
    Dictionary<string, PlayerClass> classes = new Dictionary<string, PlayerClass>();
    private int attachScanIntervalMs = 10000;


    public override void Start(ICoreAPI api)
    {
        base.Start(api);
        api.RegisterEntityBehaviorClass("HealOnInteractBehavior",typeof(VampireHealBehavior));
    }
    public override void StartServerSide(ICoreServerAPI api)
    {
        base.StartServerSide(api);
        sapi = api;
        
        // Register all available classes
        classes["vampire"] = new VampireClass();

        api.Event.PlayerJoin += OnPlayerJoin;
        api.Event.OnPlayerInteractEntity += OnPlayerInteractEntity;
        
        api.World.RegisterGameTickListener(OnTick, 1000);
        
        
        api.ChatCommands.Create("setclass")
            .WithArgs(api.ChatCommands.Parsers.Word("classname"))
            .RequiresPlayer()
            .RequiresPrivilege(Privilege.chat)
            .HandleWith((args) =>
            {
                var player = args.Caller.Player as IServerPlayer;
                string classname = (string)args[0];

                var pcs = api.ModLoader.GetModSystem<DracsVampireClassModSystem>();
                pcs.SetPlayerClass(player, classname);

                return TextCommandResult.Success($"Your class is now {classname}");
            });
    }

    private void OnPlayerInteractEntity(Entity entity, IPlayer byPlayer, ItemSlot slot, Vec3d hitPosition, int mode, ref EnumHandling handling)
    {
        // Only trigger for right-click interact (mode 1)
        if (mode != 1) return;

        // We only care about EntityPlayer
        EntityPlayer epl = byPlayer?.Entity as EntityPlayer;
        if (epl == null) return;

        // Check if player is a vampire
        if (!VampireClassManager.IsVampire(byPlayer))
            return;

        // Player must have the behavior attached
        var beh = epl.GetBehavior<VampireHealBehavior>();
        if (beh == null) return;

        // Execute the behavior logic
        beh.ApplySaturation(entity, epl);
    }

    public override void StartClientSide(ICoreClientAPI capi)
    {
        capi.Event.PlayerJoin += player =>
        {
            if (VampireClassManager.IsVampire(player))
            {
                //player.Entity.AddBehavior(new VampireHealClientEffectsBehavior(player.Entity));
            }
        };
    }

    private void OnPlayerJoin(IServerPlayer player)
    {
        EntityPlayer epl = player.Entity;
        if (epl == null) return;
        
        if (VampireClassManager.IsVampire(player))
        {
            // Attach behavior if not already present
            if (!epl.HasBehavior("vampireheal"))
            {
                epl.AddBehavior(new VampireHealBehavior(epl));
            }
        }
    }

    /// <summary>
    /// Use OnTick to check for Sunlight/Night
    /// </summary>
    /// <param name="dt"></param>
    private void OnTick(float dt)
    {
        foreach (var player in sapi.World.AllOnlinePlayers)
        {
            string pclass = VampireClassManager.GetPlayerClass(player);

            if (VampireClassManager.IsVampire(player))
            {
                classes[pclass].OnGameTick(sapi, player, dt);
            }
        }
    }

    public string GetPlayerClass(IServerPlayer player)
    {
        var data = player.GetModData<string>("playerclass");
        if (data == null) return "none";
        return data;
    }

    public void SetPlayerClass(IServerPlayer player, string classCode)
    {
        player.SetModData("playerclass", classCode);
        VampireClassManager.SetPlayerClass(player,classCode);
        var epl = player.Entity;
        // Attach behavior if not already present
        if (!epl.HasBehavior("vampireheal"))
        {
            epl.AddBehavior(new VampireHealBehavior(epl));
        }
    }
}