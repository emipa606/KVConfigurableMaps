namespace ConfigurableMaps;

public class FieldValue<T>
{
    public string Buffer;

    public T Default;

    public GetValue<T> GetValue;

    public string Label;

    public T Max;

    public T Min;
    public WindowUtil.OnChange<T> OnChange;

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
