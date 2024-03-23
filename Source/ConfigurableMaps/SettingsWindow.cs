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

    public override Vector2 InitialSize => new Vector2(900f, 740f);

    public override void PostClose()
    {
        base.PostClose();
        mod.WriteSettings();
    }

    public override void DoWindowContents(Rect canvas)
    {
        mod.DoSettingsWindowContents(new Rect(0f, 0f, canvas.width, canvas.height - CloseButSize.y));
    }
}