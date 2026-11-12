# GitHub Copilot Instructions for [KV] Configurable Maps (Continued)

## Mod Overview and Purpose
The [KV] Configurable Maps (Continued) mod provides configuration options to tailor the way new maps generate in RimWorld. These options are accessible through the "Mod Settings" menu under "Options." The mod allows users to customize various aspects of map generation, including things, terrain, and world map settings.

## Key Features and Systems
### Things Settings
- **Ruins and Ancient Shrines**: Configure the number of ruins and ancient shrines on new maps.
- **Building Placement**: Ensure that buildings in ruins are significantly more common than unattached wall segments and are shaped more like rooms than hallways.
- **Material Selection**: Decide whether ruins should be constructed from map-available stone types or random materials.
- **Biodensity**: Adjust the density of plant and animal life on new maps relative to base biome values.

### Terrain Settings
- **Ore Generation**: Control the amount of ore generation to make the game more or less challenging.
- **Fake Ores**: Disable fake ores like compacted steel, plasteel, and machinery.
- **Geysers**: Adjust the number of geysers for geothermal power.
- **Stone Chunks**: Control the quantity of stone chunks scattered on new maps.
- **Mountain Level**: Adjust the level of mountainous terrain.
- **Soil Fertility**: Modify the soil fertility to balance rich and regular soil.

### World Map Settings
- **Stone Types**: Configure the minimum and maximum number of stone types that can appear in a tile on the world map.
- **Stone Commonality**: Adjust the relative commonality of various stone types.

## Coding Patterns and Conventions
### General Conventions
- Follow the RimWorld modding conventions for naming and structuring code.
- Use descriptive and meaningful variable and method names.
- Implement mod settings through the RimWorld Mod Settings framework.
- Ensure compatibility with other mods by avoiding conflicts in namespace and method names.

### Harmony Patching
- Use Pardeike's Harmony Patch Library for patching RimWorld methods.
- Prefixes and postfixes are commonly used to modify method behavior.
- Ensure that patches are well-documented and maintain compatibility with future RimWorld updates.

### XML Integration
- Use XML files to define mod metadata, settings, and other configurations.
- Ensure XML files are well-structured and follow RimWorld's XML conventions.
- Use XML to define new settings and integrate them with the mod settings UI.

## XML Integration
The mod uses the following XML files:
- **About.xml**: Contains metadata about the mod, including the author and description.
- **ModSync.xml**: Defines the synchronization settings for the mod.
- **Version.xml**: Specifies the version of the mod and its compatibility with RimWorld.

## Harmony Patching
The mod uses Harmony patches to modify the following methods:
- `BaseGenUtility.RandomCheapWallStuff`
- `GenStep_RockChunks.GrowLowRockFormationFrom`
- `GenStep_Scatterer.CountFromPer10kCells`

### Example Patch
csharp
using HarmonyLib;
using RimWorld;

namespace ConfigurableMaps
{
    [HarmonyPatch(typeof(BaseGenUtility), "RandomCheapWallStuff")]
    public static class BaseGenUtility_RandomCheapWallStuff
    {
        [HarmonyPostfix]
        public static void Postfix(ref ThingDef __result)
        {
            // Custom logic to modify the wall stuff
        }
    }
}


## Suggestions for Copilot
- **Documentation**: Ensure that all patches and methods are well-documented with XML comments.
- **Testing**: Implement comprehensive testing for new features and patches.
- **Compatibility**: Test the mod with other popular mods to ensure compatibility.
- **Feedback**: Encourage users to provide feedback on new features and improvements.
- **Translation**: Consider adding more language translations to support a broader user base.

By following these guidelines and using Copilot effectively, you can contribute to the continued development and improvement of the [KV] Configurable Maps (Continued) mod.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

