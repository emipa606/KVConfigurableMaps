using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ConfigurableMaps;

[HarmonyPatch(typeof(Page_CreateWorldParams), "DoWindowContents")]
public static class Patch_Page_CreateWorldParams_DoWindowContents
{
    private static void Postfix(Rect rect)
    {
        var additionalHeight = 0f;
        if (ModLister.GetActiveModWithIdentifier("thereallemon.randomgoodwill") != null ||
            ModLister.GetActiveModWithIdentifier("thereallemon.factioncontrol") != null)
        {
            additionalHeight = 40f;
        }

        var y = rect.y + rect.height - 78f - additionalHeight;
        Text.Font = GameFont.Small;
        string text = "ConfigurableMaps".Translate();
        if (Widgets.ButtonText(new Rect(0f, y, 150f, 32f), text) && !Find.WindowStack.TryRemove(typeof(SettingsWindow)))
        {
            Find.WindowStack.Add(new SettingsWindow());
        }
    }
}