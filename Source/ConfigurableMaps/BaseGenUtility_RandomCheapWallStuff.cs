using HarmonyLib;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace ConfigurableMaps;

[HarmonyPatch(typeof(BaseGenUtility), "RandomCheapWallStuff", typeof(TechLevel), typeof(bool))]
public static class BaseGenUtility_RandomCheapWallStuff
{
    public static void Postfix(ref ThingDef __result)
    {
        if (__result == ThingDefOf.WoodLog || __result.defName.ToLower().Contains("steel") && Rand.Int % 3 > 0)
        {
            return;
        }

        try
        {
            var map = BaseGen.globalSettings.map;
            var thingDef = Find.World.NaturalRockTypesIn(map.Tile).RandomElement();
            if (thingDef == null)
            {
                return;
            }

            var named = DefDatabase<ThingDef>.GetNamed($"Blocks{thingDef.defName}", false);
            if (named != null)
            {
                __result = named;
            }
            else
            {
                Log.Warning($"Configurable Maps: Failed to find Block for rock type {thingDef.defName}");
            }
        }
        catch
        {
            // ignored
        }
    }
}