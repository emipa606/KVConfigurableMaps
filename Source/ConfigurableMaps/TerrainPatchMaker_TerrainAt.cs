using HarmonyLib;
using RimWorld;
using Verse;

namespace ConfigurableMaps;

[HarmonyPatch(typeof(TerrainPatchMaker), "TerrainAt")]
public static class TerrainPatchMaker_TerrainAt
{
    public static bool ignoreMap;

    public static void Prefix(Map map)
    {
        if (map.Biome?.generatesNaturally == false)
        {
            ignoreMap = true;
        }
    }

    public static void Postfix()
    {
        ignoreMap = false;
    }
}