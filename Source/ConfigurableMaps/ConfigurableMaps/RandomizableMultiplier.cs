using Verse;

namespace ConfigurableMaps;

public class RandomizableMultiplier : ARandomizableMultiplier
{
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref Multiplier, "multiplier", 1f);
    }
}
