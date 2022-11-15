using RimWorld.Planet;
using Verse;

namespace ConfigurableMaps;

internal class WorldComp : WorldComponent
{
    public static float AnimalMultiplier = -1f;

    public static float PlantMultiplier = -1f;

    public WorldComp(World world)
        : base(world)
    {
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref AnimalMultiplier, "animalMultiplier", -1f);
        Scribe_Values.Look(ref PlantMultiplier, "plantMultiplier", -1f);
        Settings.Reload();
    }

    public override void FinalizeInit()
    {
        base.FinalizeInit();
        CurrentSettings.ApplySettings(AnimalMultiplier, PlantMultiplier);
    }
}
