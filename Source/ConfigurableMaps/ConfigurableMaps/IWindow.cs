using UnityEngine;

namespace ConfigurableMaps;

public interface IWindow
{
    string Name { get; }
}

public interface IWindow<T> : IWindow
{
    void DoWindowContents(Rect inRect, T t);

    T GetFieldValues();
}
