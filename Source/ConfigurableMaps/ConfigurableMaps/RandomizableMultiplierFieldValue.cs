namespace ConfigurableMaps;

public class RandomizableMultiplierFieldValue : RandomizableFieldValue<float>
{
    public RandomizableMultiplierFieldValue(string label, ARandomizableMultiplier rm)
        : base(label, rm.SetMultiplier, rm.GetMultiplier, rm.RandomMin, rm.RandomMax, rm.DefaultValue, rm.SetIsRandom,
            rm.GetIsRandom)
    {
    }
}
