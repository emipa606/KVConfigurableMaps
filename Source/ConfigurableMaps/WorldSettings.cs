using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ConfigurableMaps;

public class WorldSettings : IExposable, IWindow<WSFieldValues>
{
    private const int DEFAULT_MIN_STONE = 2;

    private const int DEFAULT_MAX_STONE = 3;

    public static int StoneMin = 2;

    public static int StoneMax = 3;

    public static bool CommonalityRandom;

    private static bool initCommonalities;

    public static List<StoneCommonality> Commonalities = [];

    private float lastY;

    private Vector2 scroll = Vector2.zero;

    public void ExposeData()
    {
        if (StoneMin < 1)
        {
            StoneMin = 1;
        }

        if (StoneMax < StoneMin)
        {
            StoneMax = StoneMin;
        }

        Scribe_Values.Look(ref StoneMin, "StoneMin", 2);
        Scribe_Values.Look(ref StoneMax, "StoneMax", 3);
        Scribe_Values.Look(ref CommonalityRandom, "CommonalityRandom");
        Scribe_Collections.Look(ref Commonalities, "Commonalities");
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            Init();
        }
    }

    public string Name => "CM.WorldSettings".Translate();

    public void DoWindowContents(Rect rect, WSFieldValues fv)
    {
        Init();
        var y = rect.y;
        Widgets.Label(new Rect(rect.x, y, rect.width, 28f), "CM.StoneTypes".Translate());
        y += 30f;
        var stoneMinMaxBuffers = fv.StoneMinMaxBuffers;
        foreach (var fv2 in stoneMinMaxBuffers)
        {
            WindowUtil.DrawIntInput(rect.x, ref y, fv2);
        }

        if (StoneMin < 1)
        {
            StoneMin = 1;
        }

        if (StoneMax < StoneMin)
        {
            StoneMax = StoneMin;
        }

        y += 20f;
        Widgets.Label(new Rect(rect.x, y, rect.width, 28f), "CM.StoneCommonality".Translate());
        y += 30f;
        Widgets.Label(new Rect(rect.x, y, 100f, 28f), "Randomize".Translate());
        Widgets.Checkbox(new Vector2(rect.x + 110f, y - 2f), ref CommonalityRandom);
        y += 30f;
        if (CommonalityRandom)
        {
            return;
        }

        Widgets.BeginScrollView(new Rect(rect.x, y, rect.width, rect.height - y), ref scroll,
            new Rect(0f, 0f, rect.width - rect.x - 16f, lastY));
        lastY = 0f;
        var commonalityBuffers = fv.CommonalityBuffers;
        foreach (var fv3 in commonalityBuffers)
        {
            WindowUtil.DrawInputWithSlider(rect.x, ref lastY, fv3);
        }

        Widgets.EndScrollView();
    }

    public WSFieldValues GetFieldValues()
    {
        Init();
        var array = new FieldValue<float>[Commonalities.Count];
        for (var i = 0; i < Commonalities.Count; i++)
        {
            var c = Commonalities[i];
            array[i] = new FieldValue<float>(GetTranslation(c.StoneDefName, $"{c.StoneDefName}.label"),
                delegate(float v) { c.Commonality = v; }, () => c.Commonality, 0f, 100f, 50f);
        }

        var wSFieldValues = new WSFieldValues
        {
            StoneMinMaxBuffers =
            [
                new FieldValue<int>("min".Translate(), delegate(int v) { StoneMin = v; }, () => StoneMin, 0, 10, 2),
                new FieldValue<int>("max".Translate(), delegate(int v) { StoneMax = v; }, () => StoneMax, 0, 10, 3)
            ],
            CommonalityBuffers = array
        };
        return wSFieldValues;
    }

    public static void Init()
    {
        Commonalities ??= [];

        if (Commonalities.Count == 0 || !initCommonalities)
        {
            var hashSet = new HashSet<string>();
            var sortedDictionary = new SortedDictionary<string, StoneCommonality>();
            foreach (var allDef in DefDatabase<ThingDef>.AllDefs)
            {
                if (!allDef.IsNonResourceNaturalRock)
                {
                    continue;
                }

                initCommonalities = true;
                hashSet.Add(allDef.defName);
            }

            if (initCommonalities)
            {
                foreach (var commonality in Commonalities)
                {
                    if (hashSet.Contains(commonality.StoneDefName))
                    {
                        sortedDictionary[commonality.StoneDefName] = commonality;
                    }
                }

                foreach (var item in hashSet)
                {
                    if (sortedDictionary.ContainsKey(item))
                    {
                        continue;
                    }

                    var named = DefDatabase<ThingDef>.GetNamed(item, false);
                    if (named != null)
                    {
                        sortedDictionary[item] = new StoneCommonality
                        {
                            StoneDef = named,
                            StoneDefName = named.defName,
                            Commonality = 50f
                        };
                    }
                }

                Commonalities.Clear();
                foreach (var value in sortedDictionary.Values)
                {
                    Commonalities.Add(value);
                }
            }
        }

        if (!initCommonalities)
        {
            return;
        }

        var commonalities = Commonalities;
        if (commonalities is not { Count: > 0 } || Commonalities[0].StoneDef != null)
        {
            return;
        }

        foreach (var commonality2 in Commonalities)
        {
            commonality2.StoneDef = DefDatabase<ThingDef>.GetNamed(commonality2.StoneDefName, false);
        }

        Commonalities.RemoveAll(c => c.StoneDef == null);
    }

    private string GetTranslation(string eng, string toTranslate)
    {
        if (toTranslate.TryTranslate(out var result))
        {
            return result;
        }

        return eng;
    }
}