using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ConfigurableMaps;

[HarmonyPatch(typeof(MapGenerator), "GenerateMap")]
public static class MapGenerator_Generate
{
    private static RandomizableMultiplier0 buMtnValue;

    private static RandomizableMultiplier0 buWaterValue;

    [HarmonyPriority(800)]
    public static void Prefix(MapParent parent, MapGeneratorDef mapGenerator,
        IEnumerable<GenStepWithParams> extraGenStepDefs)
    {
        try
        {
            buMtnValue = null;
            buWaterValue = null;
            if (shouldOverrideSettings(parent, mapGenerator, extraGenStepDefs))
            {
                buMtnValue = MapSettings.Mountain;
                buWaterValue = MapSettings.Water;
                if (buMtnValue.GetMultiplier() > 0.7f)
                {
                    Log.Warning($"[Configurable Maps] For quest maps, capping mountain density to {0.7f}");
                    MapSettings.Mountain = new RandomizableMultiplier0();
                    MapSettings.Mountain.SetIsRandom(false);
                    MapSettings.Mountain.SetMultiplier(0.7f);
                }

                if (buWaterValue.GetMultiplier() > 0.7f)
                {
                    Log.Warning($"[Configurable Maps] For quest maps, capping water density to {0.7f}");
                    MapSettings.Water = new RandomizableMultiplier0();
                    MapSettings.Water.SetIsRandom(false);
                    MapSettings.Water.SetMultiplier(0.7f);
                }
            }
        }
        catch
        {
            Log.Error("[Configurable Maps] Issue while creating map, not all settings applied.");
        }

        try
        {
            DefsUtil.Update();
        }
        catch
        {
            Log.Error("[Configurable Maps] failed to apply map settings.");
        }

        try
        {
            GenStep_RockChunks_GrowLowRockFormationFrom.ChunkLevel = MapSettings.ChunkLevel;
            if (MapSettings.ChunkLevel == ChunkLevelEnum.Random)
            {
                GenStep_RockChunks_GrowLowRockFormationFrom.ChunkLevel =
                    (ChunkLevelEnum)Rand.RangeInclusive(0, Enum.GetNames(typeof(ChunkLevelEnum)).Length - 1);
            }
        }
        catch
        {
            Log.Error("[Configurable Maps] Failed to grow custom rock chunks.");
        }
    }

    private static bool shouldOverrideSettings(MapParent parent, MapGeneratorDef mapGenerator,
        IEnumerable<GenStepWithParams> extraGenStepDefs)
    {
        foreach (var item in Current.Game.questManager.QuestsListForReading)
        {
            if (item.State != QuestState.Ongoing)
            {
                continue;
            }

            foreach (var questLookTarget in item.QuestLookTargets)
            {
                if (questLookTarget.Tile == parent.Tile)
                {
                    return true;
                }
            }
        }

        if (mapGenerator.genSteps.Any(genStep => genStep.genStep.def.defName.Contains("Archonexus")))
        {
            return true;
        }

        return extraGenStepDefs != null && extraGenStepDefs.Any(genStep => genStep.def.defName.Contains("Archonexus"));
    }

    [HarmonyPriority(800)]
    public static void Postfix()
    {
        if (buMtnValue != null)
        {
            MapSettings.Mountain = buMtnValue;
            buMtnValue = null;
        }

        if (buWaterValue != null)
        {
            MapSettings.Water = buWaterValue;
            buWaterValue = null;
        }

        DefsUtil.Restore();
    }
}