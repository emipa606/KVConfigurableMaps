using UnityEngine;
using Verse;

namespace ConfigurableMaps;

internal class SettingsWindow : Window
{
    private readonly Mod mod;

    public SettingsWindow()
    {
        mod = LoadedModManager.GetMod(typeof(SettingsController));
        doCloseButton = true;
        doCloseX = true;
        forcePause = true;
        absorbInputAroundWindow = true;
    }

    public override Vector2 InitialSize => new(900f, 740f);

    public override void PostClose()
    {
        base.PostClose();
        mod.WriteSettings();
    }

    public override void DoWindowContents(Rect inRect)
    {
        mod.DoSettingsWindowContents(new Rect(0f, 0f, inRect.width, inRect.height - CloseButSize.y));
    }
}