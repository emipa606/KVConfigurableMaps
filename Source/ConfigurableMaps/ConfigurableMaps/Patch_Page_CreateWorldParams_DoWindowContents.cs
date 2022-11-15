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
        var y = rect.y + rect.height - 78f;
        Text.Font = GameFont.Small;
        string text = "ConfigurableMaps".Translate();
        if (Widgets.ButtonText(new Rect(0f, y, 150f, 32f), text) && !Find.WindowStack.TryRemove(typeof(SettingsWindow)))
        {
            Find.WindowStack.Add(new SettingsWindow());
        }
    }
}
