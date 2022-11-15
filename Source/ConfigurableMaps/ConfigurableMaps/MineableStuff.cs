using System.Collections.Generic;
using Verse;

namespace ConfigurableMaps;

public static class MineableStuff
{
    private static readonly List<ThingDef> mineables = new List<ThingDef>();

    public static List<ThingDef> GetMineables()
    {
        if (mineables.Count >= 2)
        {
            return mineables;
        }

        mineables.Clear();
        foreach (var item in DefDatabase<ThingDef>.AllDefsListForReading)
        {
            if (item is not { mineable: true })
            {
                continue;
            }

            var building = item.building;
            if (building is { isResourceRock: true })
            {
                mineables.Add(item);
            }
        }

        return mineables;
    }
}
