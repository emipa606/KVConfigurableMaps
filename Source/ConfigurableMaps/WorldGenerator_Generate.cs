using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace ConfigurableMaps;

[HarmonyPatch(typeof(WorldGenerator), "GenerateWorld")]
public class WorldGenerator_Generate
{
    private static List<WeightedStoneType> weighted;

    [HarmonyPriority(800)]
    public static void Prefix()
    {
        try
        {
            DefsUtil.Update();
        }
        catch
        {
            Log.Error("[Configurable Maps] failed to apply map settings.");
        }
    }

    [HarmonyPriority(800)]
    public static void Postfix()
    {
        weighted?.Clear();
        weighted = null;
    }

    public static List<ThingDef> GetRandomStone(int wanted)
    {
        Init();
        List<ThingDef> list;
        if (weighted.Count < wanted)
        {
            Log.ErrorOnce("[Configurable Maps] Not enough weighted stone types > 0, using them all.",
                "[Configurable Maps] Not enough weighted stone types > 0, using them all.".GetHashCode());
            list = new List<ThingDef>(weighted.Count);
            var list2 = DefDatabase<ThingDef>.AllDefs.Where(d => d.IsNonResourceNaturalRock).ToList();
            for (var i = 0; i < wanted && i < list2.Count; i++)
            {
                list.Add(list2[i]);
            }

            return list;
        }

        var list3 = new List<WeightedStoneType>(weighted.Count);
        foreach (var item in weighted)
        {
            if (item.Weight > 0f)
            {
                list3.Add(item);
            }
        }

        if (list3.Count == wanted)
        {
            list = new List<ThingDef>(wanted);
            foreach (var item2 in list3)
            {
                if (item2.Weight > 0f)
                {
                    list.Add(item2.Def);
                }
            }
        }

        if (list3.Count < wanted)
        {
            Log.ErrorOnce("[Configurable Maps] Not enough weighted stone types > 0, getting first found.",
                "[Configurable Maps] Not enough weighted stone types > 0, getting first found.".GetHashCode());
            list = new List<ThingDef>(wanted);
            var list4 = DefDatabase<ThingDef>.AllDefs.Where(d => d.IsNonResourceNaturalRock).ToList();
            for (var j = 0; j < wanted && j < list4.Count; j++)
            {
                list.Add(list4[j]);
            }

            return list;
        }

        list = new List<ThingDef>(wanted);
        while (list.Count < wanted)
        {
            var num = 0f;
            foreach (var item3 in list3)
            {
                num += item3.Weight;
            }

            num = Rand.RangeInclusive((int)(num * 13f), (int)(num * 29f));
            while (num > 0f)
            {
                for (var k = 0; k < list3.Count; k++)
                {
                    num -= list3[k].Weight;
                    if (!(num <= 0f))
                    {
                        continue;
                    }

                    list.Add(list3[k].Def);
                    list3.RemoveAt(k);
                    break;
                }
            }
        }

        return list;
    }

    private static void Init()
    {
        if (weighted is { Count: > 0 })
        {
            return;
        }

        WorldSettings.Init();
        weighted = new List<WeightedStoneType>(WorldSettings.Commonalities.Count);
        foreach (var commonality in WorldSettings.Commonalities)
        {
            if (WorldSettings.CommonalityRandom)
            {
                var num = Rand.RangeInclusive(0, 10);
                weighted.Add(new WeightedStoneType(commonality.StoneDef, num));
            }
            else
            {
                weighted.Add(new WeightedStoneType(commonality.StoneDef, commonality.Commonality));
            }
        }
    }

    private struct WeightedStoneType(ThingDef def, float weight)
    {
        public readonly ThingDef Def = def;

        public readonly float Weight = weight;
    }
}