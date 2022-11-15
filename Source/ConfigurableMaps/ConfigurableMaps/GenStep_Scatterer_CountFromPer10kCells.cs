using System;
using HarmonyLib;
using Verse;

namespace ConfigurableMaps;

[HarmonyPatch(typeof(GenStep_Scatterer), "CountFromPer10kCells", null)]
public static class GenStep_Scatterer_CountFromPer10kCells
{
    public static void Prefix(ref float countPer10kCells)
    {
        if (DefsUtil.LumpsApplied || !Environment.StackTrace.Contains("GenStep_ScatterLumpsMineable"))
        {
            return;
        }

        var num = countPer10kCells;
        countPer10kCells *= MapSettings.OreLevels.GetMultiplier();
        Log.Message($"[Configurable Maps] OreLevel = {countPer10kCells}. Was {num}");
        DefsUtil.LumpsApplied = true;
    }
}
