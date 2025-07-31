using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ConfigurableMaps;

public class Settings : ModSettings
{
    public static bool detectedImpassableMaps;

    public static bool detectedFertileFields;

    public static bool detectedCuprosStones;

    internal static ToShow toShow;

    private static List<FieldValue<float>> cgFieldValues;

    public static WorldSettings WorldSettings;

    public static MapSettings MapSettings;

    public static CurrentSettings CurrentSettings;

    private MSFieldValues msFieldValues;

    private WSFieldValues wsFieldValues;

    public static void OpenOnMapSettings()
    {
        toShow = ToShow.None;
    }

    public static float GetRandomMultiplier(int min = 0, int max = 40000)
    {
        return Rand.RangeInclusive(min, max) * 0.01f;
    }

    public void DoWindowContents(Rect rect)
    {
        WorldSettings ??= new WorldSettings();

        MapSettings ??= new MapSettings();

        CurrentSettings ??= new CurrentSettings();

        string text = toShow == ToShow.None ? "WorldChooseButton".Translate() :
            toShow != ToShow.World ? MapSettings.Name : WorldSettings.Name;
        if (Widgets.ButtonText(new Rect(rect.x, rect.y, 300f, 28f), text))
        {
            var list = new List<FloatMenuOption>(3)
            {
                new(WorldSettings.Name, delegate { toShow = ToShow.World; }),
                new(MapSettings.Name, delegate { toShow = ToShow.Map; })
            };
            if (Current.Game != null)
            {
                list.Add(new FloatMenuOption(CurrentSettings.Name, delegate { toShow = ToShow.CurrentGame; }));
            }

            Find.WindowStack.Add(new FloatMenu(list));
        }

        rect.y += 30f;
        switch (toShow)
        {
            case ToShow.World:
            {
                wsFieldValues ??= WorldSettings.GetFieldValues();

                WorldSettings.DoWindowContents(rect, wsFieldValues);
                break;
            }
            case ToShow.Map:
            {
                msFieldValues ??= MapSettings.GetFieldValues();

                MapSettings.DoWindowContents(rect, msFieldValues);
                break;
            }
            case ToShow.CurrentGame:
            {
                cgFieldValues ??= CurrentSettings.GetFieldValues();

                CurrentSettings.DoWindowContents(rect, cgFieldValues);
                break;
            }
        }
    }

    public static void Reload()
    {
        if (cgFieldValues == null)
        {
            return;
        }

        foreach (var cgFieldValue in cgFieldValues)
        {
            cgFieldValue.UpdateBuffer();
        }
    }

    public override void ExposeData()
    {
        WorldSettings ??= new WorldSettings();

        MapSettings ??= new MapSettings();

        base.ExposeData();
        Scribe_Deep.Look(ref WorldSettings, "WorldSettings");
        Scribe_Deep.Look(ref MapSettings, "MapSettings");
        if (Scribe.mode == LoadSaveMode.Saving)
        {
            var list = cgFieldValues;
            if (list is { Count: > 0 })
            {
                CurrentSettings.ApplySettings(cgFieldValues);
            }
        }

        DefsUtil.Restore();
    }

    internal enum ToShow
    {
        None,
        World,
        Map,
        CurrentGame
    }
}