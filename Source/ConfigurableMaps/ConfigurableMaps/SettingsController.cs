using UnityEngine;
using Verse;

namespace ConfigurableMaps;

public class SettingsController : Mod
{
    public SettingsController(ModContentPack content)
        : base(content)
    {
        GetSettings<Settings>();
    }

    public override string SettingsCategory()
    {
        return "ConfigurableMaps".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        GetSettings<Settings>().DoWindowContents(inRect);
    }
}
