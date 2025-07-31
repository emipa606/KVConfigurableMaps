# GitHub Copilot Instructions for RimWorld Mod Project: Configurable Maps

## Mod Overview and Purpose
"Configurable Maps" is a RimWorld modification designed to enhance the map generation process by providing players with the ability to customize specific parameters within their game worlds. The mod aims to offer greater control over map features such as elevation, fertility, rock formations, and resource distribution, thus enriching the diversity and gameplay experience.

## Key Features and Systems
- **Customizable Map Settings**: Allow players to adjust elevation, fertility, and other environmental factors directly from the game interface.
- **Randomizable Multipliers**: Implement systems to apply randomness in configuration parameters, enhancing replayability.
- **Harmony Patches**: Utilize Harmony to integrate new functionality without disrupting the core game mechanics.
- **XML Configuration**: Facilitate XML integration for defining default settings that can be modified through user input.
- **Field Value Management**: Employ classes like `FieldValue<T>` and `RandomizableFieldValue<T>` to store and update configurable values.

## Coding Patterns and Conventions
- **Class Naming**: Use clear and descriptive class names that indicate their purpose, such as `RandomizableMultiplier` and `SettingsController`.
- **Method Naming**: Employ action-oriented names for methods reflecting their functionality, like `DoWindowContents`, `ApplySettings`, and `ExposeData`.
- **Inheritance and Interfaces**: Leverage abstract classes such as `ARandomizableMultiplier` and utilize interfaces like `IExposable` and `IWindow<T>` for consistent expose and window operations.
- **Access Modifiers**: Keep internal utilities with restricted access, like the `internal static class DefsUtil`, ensuring encapsulation of core logic.

## XML Integration
- Integrate XML configurations to define biomes, settings defaults, and other map-related parameters, facilitating easier updates and customizations.
- Structure XML files to represent default settings, which can be overridden in-game, ensuring smooth transitions between built-in and user-defined configurations.

## Harmony Patching
- **Patching Philosophy**: Utilize Harmony for method patching to introduce custom logic without modifying original game code. This promotes compatibility and reduces risk of conflicts with other mods.
- **Example Patch**: Implement patches like `Patch_Page_CreateWorldParams_DoWindowContents` to alter UI elements dynamically.
- **Safety Considerations**: Ensure all patches are non-destructive and reversible, mimicking original function signatures closely to maintain stability.

## Suggestions for Copilot
- **Boilerplate Code**: Generate common RimWorld modding structures, such as boilerplate for `IExposable` implementations or Harmony patch templates.
- **Method Stubs**: Assist in creating stubs for commonly used functions in map generation and mod settings.
- **Pattern Recognition**: Identify and suggest recurring patterns in XML parsing or field value management to promote uniformity across the codebase.
- **Refactoring Guidance**: Provide suggestions for extracting reusable components, improving code modularity and maintainability.

This instruction file consolidates the mod's intentions and structure into a comprehensive guide, ensuring collaborators can utilize GitHub Copilot effectively to enhance and extend the "Configurable Maps" mod's functionality in RimWorld.
