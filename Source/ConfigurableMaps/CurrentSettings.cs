using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ConfigurableMaps;

public class CurrentSettings : IWindow<List<FieldValue<float>>>
{
    public static readonly List<OriginalAnimalPlant> Biomes = [];

    public string Name => "CM.CurrentSettings".Translate();

    public void DoWindowContents(Rect inRect, List<FieldValue<float>> fvs)
    {
        var y = inRect.y;
        foreach (var fv in fvs)
        {
            WindowUtil.DrawInputWithSlider(inRect.x, ref y, fv);
        }
    }

    public List<FieldValue<float>> GetFieldValues()
    {
        return
        [
            new FieldValue<float>("CM.AnimalDensity".Translate(), delegate(float v) { WorldComp.AnimalMultiplier = v; },
                () => WorldComp.AnimalMultiplier, 0f, 6f, 1f),

            new FieldValue<float>("CM.PlantDensity".Translate(), delegate(float v) { WorldComp.PlantMultiplier = v; },
                () => WorldComp.PlantMultiplier, 0f, 6f, 1f)
        ];
    }

    public static void ApplySettings(List<FieldValue<float>> fvs)
    {
        if (fvs != null)
        {
            ApplySettings(fvs[0].GetValue(), fvs[1].GetValue());
        }
        else
        {
            Log.Message("[Configurable Maps] No values to apply");
        }
    }

    public static void ApplySettings(float animalMultiplier, float plantMultiplier)
    {
        if (animalMultiplier <= 0f)
        {
            animalMultiplier = MapSettings.AnimalDensity.GetMultiplier();
            Log.Message($"[Configurable Maps] No map comp animal, now using {animalMultiplier}");
        }

        if (plantMultiplier <= 0f)
        {
            plantMultiplier = MapSettings.PlantDensity.GetMultiplier();
            Log.Message($"[Configurable Maps] No map comp plant, now using {plantMultiplier}");
        }

        foreach (var biome in Biomes)
        {
            biome.ApplyMultipliers(animalMultiplier, plantMultiplier);
        }
    }
}