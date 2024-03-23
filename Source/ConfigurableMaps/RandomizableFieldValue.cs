namespace ConfigurableMaps;

public class RandomizableFieldValue<T>(
    string label,
    WindowUtil.OnChange<T> onChange,
    GetValue<T> getValue,
    T min,
    T max,
    T d,
    WindowUtil.OnChange<bool> onRandomizableChange,
    GetValue<bool> getRandomizableValue)
    : FieldValue<T>(label, onChange, getValue, min, max, d)
{
    public readonly GetValue<bool> GetRandomizableValue = getRandomizableValue;
    public readonly WindowUtil.OnChange<bool> OnRandomizableChange = onRandomizableChange;
}