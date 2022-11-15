using System.Collections.Generic;
using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace ConfigurableMaps;

[HarmonyPatch(typeof(World), "NaturalRockTypesIn", null)]
public static class World_NaturalRockTypesIn
{
    [HarmonyPriority(800)]
    public static bool Prefix(int tile, ref IEnumerable<ThingDef> __result)
    {
        Rand.PushState();
        Rand.Seed = tile;
        try
        {
            if (WorldSettings.StoneMin <= 0)
            {
                WorldSettings.StoneMin = 1;
            }

            if (WorldSettings.StoneMax < WorldSettings.StoneMin)
            {
                WorldSettings.StoneMax = WorldSettings.StoneMin;
            }

            var wanted = Rand.RangeInclusive(WorldSettings.StoneMin, WorldSettings.StoneMax);
            __result = WorldGenerator_Generate.GetRandomStone(wanted);
        }
        finally
        {
            Rand.PopState();
        }

        return false;
    }
}
