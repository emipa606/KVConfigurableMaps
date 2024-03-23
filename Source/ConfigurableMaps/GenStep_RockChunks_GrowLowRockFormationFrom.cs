using HarmonyLib;
using RimWorld;
using Verse;

namespace ConfigurableMaps;

[HarmonyPatch(typeof(GenStep_RockChunks), "GrowLowRockFormationFrom", null)]
public static class GenStep_RockChunks_GrowLowRockFormationFrom
{
    public static ChunkLevelEnum ChunkLevel = ChunkLevelEnum.Normal;

    public static bool Prefix()
    {
        switch (ChunkLevel)
        {
            case ChunkLevelEnum.None:
                return false;
            case ChunkLevelEnum.Low:
                return Rand.Value < 0.5f;
            default:
                return true;
        }
    }
}