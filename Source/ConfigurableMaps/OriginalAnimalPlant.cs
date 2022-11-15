using RimWorld;

public struct OriginalAnimalPlant
{
    private const float MAX_ANIMAL = 40f;

    private const float MAX_PLANT = 100f;

    public BiomeDef Def;

    public readonly float Animal;

    public readonly float Plant;

    public OriginalAnimalPlant(BiomeDef def)
    {
        Def = def;
        Animal = def.animalDensity;
        Plant = def.plantDensity;
    }

    public void ApplyMultipliers(float animal, float plant)
    {
        Def.animalDensity = Animal * animal;
        switch (Def.animalDensity)
        {
            case < 0f:
                Def.animalDensity = 0f;
                break;
            case > 40f:
                Def.animalDensity = 40f;
                break;
        }

        Def.plantDensity = Plant * plant;
        switch (Def.plantDensity)
        {
            case < 0f:
                Def.plantDensity = 0f;
                break;
            case > 100f:
                Def.plantDensity = 100f;
                break;
        }
    }
}