using Verse;

namespace ConfigurableMaps;

public class RandomizableMultiplier0 : ARandomizableMultiplier
{
    public float Value
    {
        get => Multiplier;
        set => Multiplier = value;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref Multiplier, "multiplier");
    }
}