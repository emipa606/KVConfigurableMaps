using Verse;

namespace ConfigurableMaps;

public class StoneCommonality : IExposable
{
    public const float DEFAULT_COMMONALITY = 50f;

    public float Commonality;

    public ThingDef StoneDef;

    public string StoneDefName;

    public void ExposeData()
    {
        Scribe_Values.Look(ref Commonality, "commonality", 50f);
        Scribe_Values.Look(ref StoneDefName, "defName");
    }
}
