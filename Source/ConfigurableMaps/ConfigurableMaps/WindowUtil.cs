using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ConfigurableMaps;

public static class WindowUtil
{
    public delegate string GetLabel<E>(E e) where E : Enum;

    public delegate void OnChange<T>(T t);

    public static void DrawFloatInput(float x, ref float y, FieldValue<float> fv)
    {
        fv.Buffer = DrawLabeledInput(x, y, fv.Label, fv.Buffer, out var nextX);
        if (float.TryParse(fv.Buffer, out var result))
        {
            fv.OnChange(result);
        }

        if (Widgets.ButtonText(new Rect(nextX, y, 100f, 28f), "CM.Default".Translate()))
        {
            fv.OnChange(fv.Default);
            fv.UpdateBuffer();
        }

        y += 40f;
    }

    public static void DrawRandomizableFloatInput(float x, ref float y, RandomizableFieldValue<float> fv)
    {
        if (fv.GetRandomizableValue())
        {
            DrawLabel(x, y, 150f, fv.Label);
        }
        else
        {
            fv.Buffer = DrawLabeledInput(x, y, fv.Label, fv.Buffer, out var nextX);
            if (float.TryParse(fv.Buffer, out var result))
            {
                fv.OnChange(result);
            }

            if (fv.Default != 0f && Widgets.ButtonText(new Rect(nextX, y, 100f, 28f), "CM.Default".Translate()))
            {
                fv.OnChange(fv.Default);
                fv.UpdateBuffer();
            }
        }

        y += 30f;
        Widgets.Label(new Rect(x, y, 100f, 28f), "Randomize".Translate());
        var checkOn = fv.GetRandomizableValue();
        Widgets.Checkbox(new Vector2(x + 110f, y - 2f), ref checkOn);
        fv.OnRandomizableChange(checkOn);
        y += 30f;
    }

    public static void DrawInputWithSlider(float x, ref float y, FieldValue<float> fv, string leftLabel = null,
        string rightLabel = null)
    {
        DrawFloatInput(x, ref y, fv);
        y += 10f;
        if (!float.TryParse(fv.Buffer, out var result))
        {
            result = -10000f;
        }

        if (leftLabel == null)
        {
            leftLabel = fv.Min.ToString("0.00");
        }

        if (rightLabel == null)
        {
            rightLabel = fv.Max.ToString("0.00");
        }

        var num = Widgets.HorizontalSlider_NewTemp(new Rect(x, y, 300f, 20f), result, fv.Min, fv.Max, false, null,
            leftLabel,
            rightLabel);
        if (result != num && Math.Abs(result - num) > 0.01)
        {
            fv.OnChange(num);
            fv.UpdateBuffer();
        }

        y += 40f;
    }

    public static void DrawInputRandomizableWithSlider(float x, ref float y, RandomizableFieldValue<float> fv)
    {
        DrawRandomizableFloatInput(x, ref y, fv);
        y += 10f;
        if (!float.TryParse(fv.Buffer, out var result))
        {
            result = 0f;
        }

        if (fv.GetRandomizableValue())
        {
            return;
        }

        var num = Widgets.HorizontalSlider_NewTemp(new Rect(x, y, 300f, 20f), result, fv.Min, fv.Max, false, null,
            fv.Min.ToString("0.0"), fv.Max.ToString("0.0"));
        if (result != num && Math.Abs(result - num) > 0.001)
        {
            fv.OnChange(num);
            fv.UpdateBuffer();
        }

        y += 40f;
    }

    public static void DrawIntInput(float x, ref float y, FieldValue<int> fv)
    {
        fv.Buffer = DrawLabeledInput(x, y, fv.Label, fv.Buffer, out var nextX);
        if (double.TryParse(fv.Buffer, out var result))
        {
            fv.OnChange((int)result);
        }

        if (fv.Default != 0 && Widgets.ButtonText(new Rect(nextX, y, 100f, 28f), "CM.Default".Translate()))
        {
            fv.OnChange(fv.Default);
            fv.UpdateBuffer();
        }

        y += 40f;
    }

    public static bool DrawBoolInput(float x, ref float y, string label, bool value, OnChange<bool> onChange,
        string tip = "", float buttonWidth = 100f)
    {
        DrawLabel(x, y, 240f, label);
        if (tip != "")
        {
            TooltipHandler.TipRegion(new Rect(x, y, 240f + buttonWidth, 32f), tip);
        }

        if (Widgets.ButtonText(new Rect(x + 240f, y, buttonWidth, 32f), value.ToString()))
        {
            value = !value;
            onChange(value);
        }

        y += 40f;
        return value;
    }

    public static string DrawLabeledInput(float x, float y, string label, string buffer, out float nextX)
    {
        DrawLabel(x, y, 150f, label);
        var result = Widgets.TextField(new Rect(x + 160f, y, 60f, 32f), buffer);
        nextX = 240f;
        return result;
    }

    public static void DrawLabel(float x, ref float y, float width, string label, float yInc = 32f)
    {
        DrawLabel(x, y, width, label);
        y += yInc;
    }

    public static void DrawLabel(float x, float y, float width, string label)
    {
        if (label == null)
        {
            return;
        }

        if (label.Length > width * 0.137)
        {
            Text.Font = GameFont.Tiny;
        }

        Widgets.Label(new Rect(x, y + 2f, width, 30f), label);
        Text.Font = GameFont.Small;
    }

    public static void DrawEnumSelection<E>(float x, ref float y, string label, E value, GetLabel<E> getLabel,
        OnChange<E> onChange) where E : Enum
    {
        Widgets.Label(new Rect(x, y, 150f, 28f), label.Translate());
        if (Widgets.ButtonText(new Rect(160f, y, 100f, 28f), getLabel(value)))
        {
            var enumValues = typeof(E).GetEnumValues();
            var list = new List<FloatMenuOption>(enumValues.Length);
            for (var i = 0; i < enumValues.Length; i++)
            {
                var e = (E)enumValues.GetValue(i);
                list.Add(new enumFloatMenu<E>(getLabel(e), onChange, e));
            }

            Find.WindowStack.Add(new FloatMenu(list));
        }

        y += 35f;
    }

    private class enumFloatMenu<E> : FloatMenuOption where E : Enum
    {
        private readonly E e;

        public enumFloatMenu(string label, OnChange<E> onChange, E e)
            : base(label, null)
        {
            var enumFloatMenu = this;
            this.e = e;
            action = delegate { onChange(enumFloatMenu.e); };
        }
    }
}