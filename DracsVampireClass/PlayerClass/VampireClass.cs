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

        private ModConfigData _configData;

        public VampireClass(ModConfigData config)
        {
            _configData = config;
        }
        
        public bool InSunlight(ICoreAPI api, Entity entity)
        {
            BlockPos pos = entity.Pos.AsBlockPos;

            bool isDay = api.World.Calendar.GetDayLightStrength(pos) > 0.5f;
            int sunlight = api.World.BlockAccessor.GetLightLevel(pos, EnumLightLevelType.Sunbrightness);

            return isDay && sunlight >= _configData.SunlightThreshold;
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
                }, _configData.SunDamagePerSecond);
            }
            else
            {
                // Heal a bit at night
                ent.ReceiveDamage(new DamageSource()
                {
                    Source = EnumDamageSource.Internal,
                    Type = EnumDamageType.Heal
                }, -_configData.NightRegenPerSecond);
            }
        }
    }
}