using System;
using System.Collections.Generic;
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

[HarmonyPatch(typeof(TerrainThreshold), "TerrainAtValue", null)]
public static class TerrainThreshold_TerrainAtValue
{
    public static bool Prefix(List<TerrainThreshold> threshes, float val, ref TerrainDef __result)
    {
        if (TerrainPatchMaker_TerrainAt.ignoreMap)
        {
            return true;
        }

        var terrainDef = __result;
        __result = null;
        var multiplier = MapSettings.Fertility.GetMultiplier();
        if (threshes[0].min < -900f)
        {
            var num = 0.87f - multiplier;
            if (num < 0f)
            {
                num = 0f;
            }

            if (threshes[0].terrain.defName == "Soil")
            {
                if (val >= -999f && val < num)
                {
                    __result = TerrainDef.Named("Soil");
                }
                else if (val >= num && val < 999f)
                {
                    __result = TerrainDef.Named("SoilRich");
                }
            }
            else
            {
                if (threshes[0].terrain.defName != "Sand" || !(threshes[0].max < 0.5f))
                {
                    return true;
                }

                num += 0.03f;
                switch (val)
                {
                    case >= -999f and < 0.45f:
                        __result = TerrainDef.Named("Sand");
                        break;
                    case >= 0.45f when val < num:
                        __result = TerrainDef.Named("Soil");
                        break;
                    default:
                    {
                        if (val >= num && val < 999f)
                        {
                            __result = TerrainDef.Named("SoilRich");
                        }

                        break;
                    }
                }
            }

            if (__result != null)
            {
                return false;
            }

            __result = terrainDef;
            return true;
        }

        var num2 = MapSettings.Water.GetMultiplier() * -1f;
        if (Math.Abs(num2) < 0.01)
        {
            return true;
        }

        multiplier *= -2f;
        for (var i = 0; i < threshes.Count; i++)
        {
            if (threshes[0].terrain.defName == "SoilRich")
            {
                if (i == 0 && threshes[i].min + num2 <= val && threshes[i].max + num2 + multiplier > val)
                {
                    __result = threshes[i].terrain;
                }

                if (i == 1)
                {
                    if (threshes[i].min + num2 + multiplier <= val && threshes[i].max + num2 > val)
                    {
                        __result = threshes[i].terrain;
                    }
                }
                else if (threshes[i].min + num2 <= val && threshes[i].max + num2 > val)
                {
                    __result = threshes[i].terrain;
                }
            }
            else if (threshes[i].min + num2 <= val && threshes[i].max + num2 > val)
            {
                __result = threshes[i].terrain;
            }
        }

        return false;
    }
}