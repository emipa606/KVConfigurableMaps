using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace ConfigurableMaps;

public class MapSettings : IExposable, IWindow<MSFieldValues>
{
    public static ChunkLevelEnum ChunkLevel = ChunkLevelEnum.Normal;

    public static RandomizableMultiplier0 Fertility;

    public static RandomizableMultiplier0 Water;

    public static RandomizableMultiplier0 Mountain;

    public static RandomizableMultiplier Geysers;

    public static RandomizableMultiplier OreLevels;

    public static List<RandomizableMultiplier> Mineables;

    public static bool AreWallsMadeFromLocal;

    public static RandomizableMultiplier AnimalDensity;

    public static RandomizableMultiplier PlantDensity;

    public static RandomizableMultiplier Ruins;

    public static RandomizableMultiplier Shrines;

    public static RandomizableMultiplier AncientPipelineSection;

    public static RandomizableMultiplier AncientJunkClusters;

    private static bool initMineables;

    private float lastYTerrain;

    private float lastYThings;

    private Vector2 terrainScroll = Vector2.zero;

    private Vector2 thingsScroll = Vector2.zero;

    public void ExposeData()
    {
        Initialize();
        Scribe_Values.Look(ref ChunkLevel, "ChunkLevel", ChunkLevelEnum.Normal);
        Scribe_Values.Look(ref AreWallsMadeFromLocal, "AreWallsMadeFromLocal");
        Scribe_Deep.Look(ref Fertility, "Fertility");
        Scribe_Deep.Look(ref Water, "Water");
        Scribe_Deep.Look(ref Geysers, "Geysers");
        Scribe_Deep.Look(ref Mountain, "Mountain");
        Scribe_Deep.Look(ref OreLevels, "OreLevels");
        Scribe_Collections.Look(ref Mineables, "Mineables", LookMode.Deep);
        Scribe_Deep.Look(ref AnimalDensity, "AnimalDensity");
        Scribe_Deep.Look(ref PlantDensity, "PlantDensity");
        Scribe_Deep.Look(ref Ruins, "Ruins");
        Scribe_Deep.Look(ref Shrines, "Shrines");
        Scribe_Deep.Look(ref AncientPipelineSection, "AncientPipelineSection");
        Scribe_Deep.Look(ref AncientJunkClusters, "AncientJunkClusters");
    }

    public string Name => "CM.MapSettings".Translate();

    public void DoWindowContents(Rect rect, MSFieldValues fv)
    {
        var num = rect.width * 0.5f;
        var num2 = num - 10f;
        var width = num2 - 16f;
        var num3 = rect.y + 5f;
        var num4 = num3;
        Widgets.Label(new Rect(0f, num4, width, 28f), "CM.TerrainType".Translate());
        num4 += 30f;
        WindowUtil.DrawEnumSelection(0f, ref num4, "CM.ChunksLevel", ChunkLevel, GetChunkLevelLabel,
            delegate(ChunkLevelEnum e) { ChunkLevel = e; });
        Widgets.BeginScrollView(new Rect(rect.x, num4, num2, rect.height - num4), ref terrainScroll,
            new Rect(0f, 0f, width, lastYTerrain));
        lastYTerrain = 0f;
        for (var i = 0; i < fv.TerrainFieldValues.Count; i++)
        {
            if (i < 4)
            {
                WindowUtil.DrawInputWithSlider(0f, ref lastYTerrain, fv.TerrainFieldValues[i],
                    "PowerConsumptionLow".Translate().CapitalizeFirst(),
                    "PowerConsumptionHigh".Translate().CapitalizeFirst());
            }
            else
            {
                WindowUtil.DrawInputRandomizableWithSlider(0f, ref lastYTerrain,
                    fv.TerrainFieldValues[i] as RandomizableFieldValue<float>);
            }

            lastYTerrain += 5f;
        }

        Widgets.EndScrollView();
        num4 = num3;
        Widgets.Label(new Rect(num, num4, num2, 28f), "CM.ThingType".Translate());
        num4 += 30f;
        WindowUtil.DrawBoolInput(num, ref num4, "CM.WallsMadeFromLocal".Translate(), AreWallsMadeFromLocal,
            delegate(bool v) { AreWallsMadeFromLocal = v; });
        num4 += 5f;
        Widgets.Label(new Rect(num, num4, num2, 28f), "CM.Multipliers".Translate());
        num4 += 30f;
        Widgets.BeginScrollView(new Rect(num, num4, num2, rect.height - num4), ref thingsScroll,
            new Rect(0f, 0f, num2 - 16f, lastYThings));
        lastYThings = 0f;
        var thingsFieldValues = fv.ThingsFieldValues;
        foreach (var fv2 in thingsFieldValues)
        {
            WindowUtil.DrawInputRandomizableWithSlider(0f, ref lastYThings, fv2);
        }

        Widgets.EndScrollView();
    }

    public MSFieldValues GetFieldValues()
    {
        Initialize();
        var list = new List<FieldValue<float>>(6 + Mineables.Count)
        {
            new RandomizableMultiplierFieldValue("CM.FertilityLevel".Translate(), Fertility),
            new RandomizableMultiplierFieldValue("CM.WaterLevel".Translate(), Water),
            new RandomizableMultiplierFieldValue("CM.MountainLevel".Translate(), Mountain),
            new RandomizableMultiplierFieldValue("CM.Geysers".Translate(), Geysers),
            new RandomizableMultiplierFieldValue("CM.OreLevels".Translate(), OreLevels)
        };
        foreach (var mineable in Mineables)
        {
            if (mineable.ThingDef != null)
            {
                list.Add(new RandomizableMultiplierFieldValue(mineable.ThingDef.label, mineable));
            }
        }

        var mSFieldValues = new MSFieldValues
        {
            TerrainFieldValues = list,
            ThingsFieldValues = new RandomizableFieldValue<float>[]
            {
                new RandomizableMultiplierFieldValue("CM.AnimalDensity".Translate(), AnimalDensity),
                new RandomizableMultiplierFieldValue("CM.PlantDensity".Translate(), PlantDensity),
                new RandomizableMultiplierFieldValue("CM.Ruins".Translate(), Ruins),
                new RandomizableMultiplierFieldValue("CM.Shrines".Translate(), Shrines),
                new RandomizableMultiplierFieldValue("CM.AncientPipelineSection".Translate(), AncientPipelineSection),
                new RandomizableMultiplierFieldValue("CM.AncientJunkClusters".Translate(), AncientJunkClusters)
            }
        };
        return mSFieldValues;
    }

    public static void Initialize()
    {
        if (Fertility == null)
        {
            Fertility = new RandomizableMultiplier0();
        }

        Fertility.DefaultValue = 0f;
        Fertility.RandomMin = -3f;
        Fertility.RandomMax = 3f;
        if (Water == null)
        {
            Water = new RandomizableMultiplier0();
        }

        Water.DefaultValue = 0f;
        Water.RandomMin = -0.75f;
        Water.RandomMax = 0.75f;
        var mineables = MineableStuff.GetMineables();
        if (Mineables == null && mineables is { Count: > 0 })
        {
            Mineables = new List<RandomizableMultiplier>(mineables.Count);
            foreach (var item in mineables)
            {
                Mineables.Add(new RandomizableMultiplier
                {
                    ThingDef = item,
                    ThingDefName = item.defName,
                    DefaultValue = item.building.mineableScatterCommonality
                });
            }
        }
        else if (!initMineables && mineables.Count > 0)
        {
            initMineables = true;
            if (Mineables != null)
            {
                for (var num = Mineables.Count - 1; num >= 0; num--)
                {
                    var randomizableMultiplier = Mineables[num];
                    if (randomizableMultiplier.ThingDef != null || randomizableMultiplier.ThingDefName == "")
                    {
                        continue;
                    }

                    randomizableMultiplier.ThingDef =
                        DefDatabase<ThingDef>.GetNamed(randomizableMultiplier.ThingDefName, false);
                    if (randomizableMultiplier.ThingDef != null)
                    {
                        continue;
                    }

                    Mineables.RemoveAt(num);
                    Log.Message(
                        $"[Configurable Maps] Unable to load mineable thing def {randomizableMultiplier.ThingDefName}");
                }

                foreach (var item2 in mineables)
                {
                    var isMinable = false;
                    foreach (var mineable in Mineables)
                    {
                        isMinable = mineable.ThingDefName == item2.defName;
                        if (!isMinable)
                        {
                            continue;
                        }

                        mineable.ThingDef = item2;
                        mineable.DefaultValue = item2.building.mineableScatterCommonality;
                        break;
                    }

                    if (!isMinable)
                    {
                        Mineables.Add(new RandomizableMultiplier
                        {
                            ThingDef = item2,
                            ThingDefName = item2.defName
                        });
                    }
                }
            }
        }

        if (Geysers == null)
        {
            Geysers = new RandomizableMultiplier();
        }

        Geysers.DefaultValue = 1f;
        if (Mountain == null)
        {
            Mountain = new RandomizableMultiplier0();
        }

        Mountain.Max = 1.4f;
        Mountain.DefaultValue = 0f;
        Mountain.RandomMin = -0.15f;
        Mountain.RandomMax = 1.4f;
        if (AnimalDensity == null)
        {
            AnimalDensity = new RandomizableMultiplier();
        }

        AnimalDensity.RandomMax = 6f;
        if (PlantDensity == null)
        {
            PlantDensity = new RandomizableMultiplier();
        }

        PlantDensity.RandomMax = 6f;
        if (Ruins == null)
        {
            Ruins = new RandomizableMultiplier();
        }

        Ruins.RandomMax = 50f;
        if (Shrines == null)
        {
            Shrines = new RandomizableMultiplier();
        }

        Shrines.RandomMax = 50f;
        if (AncientPipelineSection == null)
        {
            AncientPipelineSection = new RandomizableMultiplier();
        }

        AncientPipelineSection.RandomMax = 50f;
        if (AncientJunkClusters == null)
        {
            AncientJunkClusters = new RandomizableMultiplier();
        }

        AncientJunkClusters.RandomMax = 50f;
        if (OreLevels == null)
        {
            OreLevels = new RandomizableMultiplier();
        }

        OreLevels.DefaultValue = 1f;
        OreLevels.Min = 0f;
        OreLevels.RandomMin = 0f;
        OreLevels.RandomMax = 6f;
        var num2 = 0;
        if (Mineables != null)
        {
            foreach (var mineable2 in Mineables)
            {
                if (mineable2.GetMultiplier() == 1f)
                {
                    num2++;
                }
            }

            if (num2 > 3)
            {
                Log.Message("[Configurable Maps] Correcting default values for mineables to their default values.");
                foreach (var mineable3 in Mineables)
                {
                    mineable3.SetMultiplier(mineable3.DefaultValue);
                }
            }
        }

        num2 = 0;
        var fertility = Fertility;
        num2 += fertility != null && fertility.GetMultiplier() == 1f ? 1 : 0;
        var num4 = num2;
        var water = Water;
        num2 = num4 + (water != null && water.GetMultiplier() == 1f ? 1 : 0);
        var num5 = num2;
        var mountain = Mountain;
        num2 = num5 + (mountain != null && mountain.GetMultiplier() == 1f ? 1 : 0);
        if (num2 < 2)
        {
            return;
        }

        Fertility.SetMultiplier(Fertility.DefaultValue);
        Water.SetMultiplier(Water.DefaultValue);
        Mountain.SetMultiplier(Mountain.DefaultValue);
        Log.Message(
            "[Configurable Maps] Correcting default values for Fertility, Water, and Mountain to their default values.");
    }

    private bool CheckBox(float x, ref float y, string label, ref bool b)
    {
        var num = b;
        Widgets.Label(new Rect(x, y, 200f, 28f), label);
        Widgets.Checkbox(215f, y, ref b);
        y += 30f;
        if (num != b && b)
        {
            Messages.Message("CM.RequiresRestartIfAMapWasCreated".Translate(), MessageTypeDefOf.CautionInput);
        }

        return num != b;
    }

    private string GetChunkLevelLabel(ChunkLevelEnum e)
    {
        return e switch
        {
            ChunkLevelEnum.None => "None".Translate(),
            ChunkLevelEnum.Low => "StoragePriorityLow".Translate().CapitalizeFirst(),
            ChunkLevelEnum.Normal => "StoragePriorityNormal".Translate().CapitalizeFirst(),
            _ => "CM.Random".Translate()
        };
    }
}