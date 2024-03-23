namespace ConfigurableMaps;

public class FieldValue<T>
{
    public readonly T Default;

    public readonly GetValue<T> GetValue;

    public readonly string Label;

    public readonly T Max;

    public readonly T Min;
    public readonly WindowUtil.OnChange<T> OnChange;
    public string Buffer;

    public FieldValue(string label, WindowUtil.OnChange<T> onChange, GetValue<T> getValue, T min, T max, T d)
    {
        Label = label;
        OnChange = onChange;
        GetValue = getValue;
        Min = min;
        Max = max;
        Default = d;
        UpdateBuffer();
    }

    public void UpdateBuffer()
    {
        var val = GetValue();
        if (val is float)
        {
            object obj = val;
            Buffer = ((float)obj).ToString("0.00");
        }
        else
        {
            Buffer = val.ToString();
        }
    }
}