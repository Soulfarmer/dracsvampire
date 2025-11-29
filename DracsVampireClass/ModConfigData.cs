namespace DracsVampireClass;

public class ModConfigData
{
    /// <summary>
    /// Damage the player takes when in sunlight. (When (isDay && LightLevel>SunlightThreshold))
    /// </summary>
    public float SunDamagePerSecond = 0.1f;
    /// <summary>
    /// How much the player heals during the night(when LightLevel < SunlightThreshold)
    /// </summary>
    public float NightRegenPerSecond = 0.5f;
    /// <summary>
    /// Threshold for the player to start taking damage. (LightLevel>SunlightThreshold)
    /// </summary>
    public int SunlightThreshold = 10;
}