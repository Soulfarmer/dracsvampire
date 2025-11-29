using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DracsVampireClass;

/// <summary>
/// Small EntityBehavior added to entities so we can detect when a player interacted with them.
/// Runs on the server side.
/// </summary>
public class VampireHealBehavior : EntityBehavior
{
    // Amount to heal the interacting player by
    float totalHeal;
    private Entity _ent;
    public VampireHealBehavior(Entity entity) : base(entity)
    { 
        _ent = entity;
    }
   
    public override string PropertyName()
    {
        return "HealOnInteractBehavior";
    }

    // Called manually from the server-side ModSystem
    public void ApplySaturation(Entity target, EntityPlayer actor)
    {
        // Only works if target is alive 
        if (!target.Alive)
            return;
        
        if (actor is EntityPlayer epl)
        {
            /* Healing must be done through EntityBehaviorHealth for the "normal" way doesnt work.*/
            var hBehavior = actor.GetBehavior<EntityBehaviorHealth>();
            hBehavior.ApplyDoTEffect(EnumDamageSource.Internal, EnumDamageType.Heal, 1, totalHeal, TimeSpan.FromSeconds(1),1,0);
        }

        // Damage the target entity
        target.ReceiveDamage(new DamageSource()
        {
            Source = EnumDamageSource.Internal,
            Type = EnumDamageType.PiercingAttack
        }, this.totalHeal);
    }
    
    //TODO: Add a particle effect when the player is interacting with a creature
    public override void OnInteract(EntityAgent byEntity, ItemSlot itemslot, Vec3d hit, EnumInteractMode mode, ref EnumHandling handled)
    {
        // AdvancedParticleProperties props = new AdvancedParticleProperties()
        // {
        //     // Amount & lifetime
        //     Quantity = 20,
        //     LifeLength = 0.6f,
        //     MinSize = 0.15f,
        //     MaxSize = 0.35f,
        //     GravityEffect = -0.05f,    // rises upward like mist
        //     Opacity = 0.8f,
        //
        //     // Colors (deep red + darker core)
        //     Color = new Vec3f(0.8f, 0f, 0.05f),      // bright blood red
        //     ColorVar = new Vec3f(0.2f, 0f, 0.05f),   // small variation so it looks organic
        //
        //     // Initial position: around center of the entity
        //     MinPos = entity.Pos.XYZ.AddCopy(
        //         0,
        //         entity.SelectionBox.Y2 * 0.5f,
        //         0
        //     ),
        //     AddPos = new Vec3f(0.4f, 0.2f, 0.4f),
        //
        //     // Movement: slow drifting upward + light sideways motion
        //     MinVelocity = new Vec3f(-0.05f, 0.05f, -0.05f),
        //     AddVelocity = new Vec3f(0.1f, 0.15f, 0.1f),
        //
        //     // Random rotation / fade
        //     WindAffected = true,
        //     ShouldDieInAir = false
        // };
        //
        // entity.World.SpawnParticles(props);
    }
}
