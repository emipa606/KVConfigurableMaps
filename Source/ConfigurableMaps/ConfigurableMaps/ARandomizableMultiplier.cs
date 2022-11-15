using Verse;

namespace ConfigurableMaps;

public abstract class ARandomizableMultiplier : IExposable
{
    public float DefaultValue;

    protected bool IsRandom;

    public float Max = 100000f;

    public float Min;

    protected float Multiplier;

    public float RandomMax;

    public float RandomMin;
    public ThingDef ThingDef;

    public string ThingDefName;

    public ARandomizableMultiplier()
    {
        DefaultValue = 1f;
        Multiplier = 1f;
        RandomMin = 0f;
        RandomMax = 6f;
    }

    public virtual void ExposeData()
    {
        Scribe_Values.Look(ref ThingDefName, "defName", "");
        Scribe_Values.Look(ref IsRandom, "isRandom");
    }

    public void SetMultiplier(float v)
    {
        if (Multiplier > Max)
        {
            Multiplier = Max;
        }
        else if (Multiplier < Min)
        {
            Multiplier = Min;
        }
        else
        {
            Multiplier = v;
        }
    }

    public bool GetIsRandom()
    {
        return IsRandom;
    }

    public void SetIsRandom(bool b)
    {
        IsRandom = b;
    }

    public float GetMultiplier()
    {
        if (IsRandom)
        {
            return Rand.RangeInclusive((int)(RandomMin * 1000f), (int)(RandomMax * 1000f)) * 0.001f;
        }

        return Multiplier;
    }
}
