namespace ConfigurableMaps;

public class RandomizableFieldValue<T> : FieldValue<T>
{
    public GetValue<bool> GetRandomizableValue;
    public WindowUtil.OnChange<bool> OnRandomizableChange;

    public RandomizableFieldValue(string label, WindowUtil.OnChange<T> onChange, GetValue<T> getValue, T min, T max,
        T d, WindowUtil.OnChange<bool> onRandomizableChange, GetValue<bool> getRandomizableValue)
        : base(label, onChange, getValue, min, max, d)
    {
        OnRandomizableChange = onRandomizableChange;
        GetRandomizableValue = getRandomizableValue;
    }
}
