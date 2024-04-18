using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using RimWorld;
using Verse;

namespace ConfigurableMaps;

internal static class DefsUtil
{
    private static bool applied;

    private static readonly List<Pair<BiomeDef, BiomeOriginalValues>> BiomeDefs =
        [];

    private static readonly List<Pair<GenStep_Scatterer, ScattererValues>> Scatterers =
        [];

    private static readonly List<Pair<ThingDef, float>> Mineability = [];

    private static Pair<GenStep_PreciousLump, FloatRange> PreciousLump;

    public static bool LumpsApplied { get; set; }

    public static void Update()
    {
        if (applied)
        {
            return;
        }

        applied = true;
        LumpsApplied = false;
        MapSettings.Initialize();
        var num = WorldComp.AnimalMultiplier < 0f
            ? MapSettings.AnimalDensity.GetMultiplier()
            : WorldComp.AnimalMultiplier;
        var num2 = WorldComp.PlantMultiplier < 0f
            ? MapSettings.PlantDensity.GetMultiplier()
            : WorldComp.PlantMultiplier;
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("[Configurable Maps] Using Map Settings:");
        BiomeDefs.Clear();
        foreach (var allDef in DefDatabase<BiomeDef>.AllDefs.Where(def => def.generatesNaturally))
        {
            try
            {
                BiomeDefs.Add(new Pair<BiomeDef, BiomeOriginalValues>(allDef,
                    new BiomeOriginalValues(allDef.animalDensity, allDef.plantDensity)));
                allDef.animalDensity *= num;
                allDef.plantDensity *= num2;
            }
            catch
            {
                Log.Error($"[Configurable Maps] failed to update biome {allDef.defName}");
            }
        }

        Scatterers.Clear();
        UpdateGenStepScatterer("ScatterRuinsSimple", MapSettings.Ruins, stringBuilder);
        UpdateGenStepScatterer("ScatterShrines", MapSettings.Shrines, stringBuilder);
        UpdateGenStepScatterer("SteamGeysers", MapSettings.Geysers, stringBuilder);
        UpdateGenStepScatterer("AncientPipelineSection", MapSettings.AncientPipelineSection, stringBuilder);
        UpdateGenStepScatterer("AncientJunkClusters", MapSettings.AncientJunkClusters, stringBuilder);
        ((GenStep_ScatterLumpsMineable)DefDatabase<GenStepDef>.GetNamed("PreciousLump").genStep).count = 100;
        foreach (var mineable in MapSettings.Mineables)
        {
            UpdateMineable(mineable, stringBuilder);
        }

        Log.Message(stringBuilder.ToString());
    }

    private static void UpdateMineable(RandomizableMultiplier rm, StringBuilder sb)
    {
        try
        {
            if (rm.ThingDef != null)
            {
                Mineability.Add(new Pair<ThingDef, float>(rm.ThingDef,
                    rm.ThingDef.building.mineableScatterCommonality));
                rm.ThingDef.building.mineableScatterCommonality = rm.GetMultiplier();
                sb.AppendLine(
                    $"- {rm.ThingDefName}.mineableScatterCommonality = {rm.ThingDef.building.mineableScatterCommonality}");
            }
            else
            {
                Log.Warning($"$[Configurable Maps] unable to patch {rm.ThingDefName}.");
            }
        }
        catch
        {
            Log.Error($"[Configurable Maps] failed to find and patch {rm.ThingDefName}");
        }
    }

    private static FieldInfo GetTotalValueRange()
    {
        return typeof(SitePartDef).GetField("totalValueRange", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    private static void UpdateGenStepScatterer(string genStepDefName, RandomizableMultiplier rm, StringBuilder sb)
    {
        try
        {
            var multiplier = rm.GetMultiplier();
            if (DefDatabase<GenStepDef>.GetNamed(genStepDefName, false)?.genStep is GenStep_Scatterer genStep_Scatterer)
            {
                Scatterers.Add(new Pair<GenStep_Scatterer, ScattererValues>(genStep_Scatterer,
                    new ScattererValues(genStep_Scatterer.countPer10kCellsRange)));
                genStep_Scatterer.countPer10kCellsRange.min *= multiplier;
                genStep_Scatterer.countPer10kCellsRange.max *= multiplier;
                sb.AppendLine($"- {genStepDefName}.countPer10kCellsRange = {genStep_Scatterer.countPer10kCellsRange}");
            }
            else
            {
                Log.Warning($"[Configurable Maps] unable to patch {genStepDefName}");
            }
        }
        catch
        {
            Log.Error($"[Configurable Maps] failed to update scatterer {genStepDefName}");
        }
    }

    public static void Restore()
    {
        if (!applied)
        {
            return;
        }

        foreach (var biomeDef in BiomeDefs)
        {
            biomeDef.First.animalDensity = biomeDef.Second.AnimalDensity;
            biomeDef.First.plantDensity = biomeDef.Second.PlantDensity;
        }

        BiomeDefs.Clear();
        foreach (var scatterer in Scatterers)
        {
            scatterer.First.countPer10kCellsRange.min = scatterer.Second.CountPer10kCellsRange.min;
            scatterer.First.countPer10kCellsRange.max = scatterer.Second.CountPer10kCellsRange.max;
        }

        Scatterers.Clear();
        foreach (var item in Mineability)
        {
            item.First.building.mineableScatterCommonality = item.Second;
        }

        Mineability.Clear();
        if (PreciousLump.First != null)
        {
            PreciousLump.First.totalValueRange.min = PreciousLump.Second.min;
            PreciousLump.First.totalValueRange.max = PreciousLump.Second.max;
        }

        applied = false;
        LumpsApplied = true;
    }

    private struct BiomeOriginalValues(float animalDensity, float plantDensity)
    {
        public readonly float AnimalDensity = animalDensity;

        public readonly float PlantDensity = plantDensity;
    }

    private struct ScattererValues(FloatRange fr)
    {
        public readonly FloatRange CountPer10kCellsRange = new FloatRange(fr.min, fr.max);
    }
}