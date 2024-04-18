using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ConfigurableMaps;

[StaticConstructorOnStartup]
public class HarmonyPatches
{
    static HarmonyPatches()
    {
        new Harmony("com.configurablemaps.rimworld.mod").PatchAll(Assembly.GetExecutingAssembly());
        var hashSet = new HashSet<string>();
        foreach (var item in ModsConfig.ActiveModsInLoadOrder)
        {
            hashSet.Add(item.Name);
        }

        Settings.detectedFertileFields = hashSet.Contains("[RF] Fertile Fields");
        Settings.detectedCuprosStones = hashSet.Contains("Cupro's Stones");
        Settings.detectedImpassableMaps = hashSet.Contains("[KV] Impassable Map Maker");
        foreach (var allDef in DefDatabase<BiomeDef>.AllDefs.Where(def => def.generatesNaturally))
        {
            CurrentSettings.Biomes.Add(new OriginalAnimalPlant(allDef));
        }
    }
}