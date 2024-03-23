namespace ConfigurableMaps;

public class RandomizableMultiplierFieldValue(string label, ARandomizableMultiplier rm) : RandomizableFieldValue<float>(
    label, rm.SetMultiplier, rm.GetMultiplier, rm.RandomMin, rm.RandomMax, rm.DefaultValue, rm.SetIsRandom,
    rm.GetIsRandom);