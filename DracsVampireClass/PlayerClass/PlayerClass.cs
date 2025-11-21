using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace DracsVampireClass;

public abstract class PlayerClass
{
    public abstract string Code { get; }
    public abstract List<string> GetClassBehaviors { get; }

// Called every second
    public virtual void OnGameTick(ICoreAPI api, IPlayer player, float dt) { }

    // Called when the player logs in
    public virtual void OnLogin(ICoreAPI api, IPlayer player) { }
    
    public virtual void OnInteractEntity(ICoreAPI api, IPlayer player, Entity entity, EnumInteractMode mode) { }
    
}