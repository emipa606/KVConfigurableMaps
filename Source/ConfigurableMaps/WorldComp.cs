using RimWorld.Planet;
using Verse;

namespace ConfigurableMaps;

internal class WorldComp(World world) : WorldComponent(world)
{
    public static float AnimalMultiplier = -1f;

    public static float PlantMultiplier = -1f;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref AnimalMultiplier, "animalMultiplier", -1f);
        Scribe_Values.Look(ref PlantMultiplier, "plantMultiplier", -1f);
        Settings.Reload();
    }

    public override void FinalizeInit(bool fromLoad)
    {
        base.FinalizeInit(fromLoad);
        CurrentSettings.ApplySettings(AnimalMultiplier, PlantMultiplier);
    }
}