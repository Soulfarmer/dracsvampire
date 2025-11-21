using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace DracsVampireClass
{
    public class VampireClass : PlayerClass
    {
        public override string Code => "vampire";
        public override List<string> GetClassBehaviors  => new List<string>() { "vampireheal" };

        public float SunDamagePerSecond = 0.1f;
        public float NightRegenPerSecond = 0.5f;
        public int SunlightThreshold = 14;
        
        public bool InSunlight(ICoreAPI api, Entity entity)
        {
            BlockPos pos = entity.Pos.AsBlockPos;

            bool isDay = api.World.Calendar.GetDayLightStrength(pos) > 0.5f;
            int sunlight = api.World.BlockAccessor.GetLightLevel(pos, EnumLightLevelType.Sunbrightness);

            return isDay && sunlight >= SunlightThreshold;
        }

        public override void OnGameTick(ICoreAPI api, IPlayer player, float dt)
        {
            var ent = player.Entity;
            if (ent == null || !ent.Alive) return;

            if (InSunlight(api, ent))
            {
                ent.ReceiveDamage(new DamageSource()
                {
                    Source = EnumDamageSource.Internal,
                    Type = EnumDamageType.Heat
                }, SunDamagePerSecond);
            }
            else
            {
                // Heal a bit at night
                ent.ReceiveDamage(new DamageSource()
                {
                    Source = EnumDamageSource.Player,
                    Type = EnumDamageType.Heal
                }, -NightRegenPerSecond);
            }
        }
    }
}