# Suppression (Continued) Mod - GitHub Copilot Instructions

## Mod Overview and Purpose

The "Suppression (Continued)" mod enhances the combat mechanics of RimWorld by introducing a suppression system. This system is designed to create a more strategic and less micromanagement-intensive combat experience by simulating the effects of heavy fire on pawns. The primary goals are to slow down the pace of battles, enhance tactical depth, and provide meaningful incentives for spread-out defensive positioning.

## Key Features and Systems

- **Suppression Mechanics**: Introduces a new hediff, 'Suppressed', which affects pawns when they are under heavy fire. Suppressed pawns become less effective in combat as they move slower, aim worse, and aim slower.

- **Suppression Calculation**: The suppression effect is calculated based on the proximity and impact of bullet hits. The system is balanced around the concept that 2-3 bursts from an LMG are sufficient to suppress an individual pawn, encouraging the tactical use of automatic fire and high-damage weapons.

- **Mood Impact**: The mod includes an option where a pawn's mood influences their chance of suppression, adding psychological factors into battle dynamics.

- **Ranged Weapon Focus**: An optional setting to only apply suppression effects to pawns using ranged weapons.

- **Multilingual Support**: Includes translations in French, Chinese, and Russian.

## Coding Patterns and Conventions

- **Modular Class Design**: The project uses a clean separation of concerns through static classes that handle specific aspects of pawn behavior and rendering.

- **Harmony Patching**: Utilizes the Harmony library for non-invasive patches to existing RimWorld methods, allowing for smooth integration and compatibility with other mods.

- **Clear Method Naming**: Internal classes and methods follow clear, descriptive naming conventions that reflect their purpose, aiding in maintainability and readability.

- **Framework**: Developed using .NET Framework version 4.7.2 and 4.8.

## XML Integration

- XML files are used primarily for localization, enabling support for multiple languages. Ensure all text assets are properly formatted and added to the XML files in the `Languages` directory.

## Harmony Patching

- Harmony patches are used extensively to introduce changes to game mechanics without modifying the core RimWorld assemblies. This ensures compatibility and eases updates. Copilot can assist by generating patches for new methods when expanded functionality is needed.

## Suggestions for Copilot

1. **Code Documentation**: When writing or extending methods, Copilot can assist in generating XML documentation comments that improve code readability and facilitate maintenance.

2. **XML Localization**: Use Copilot to streamline the addition of new language support, ensuring consistent formatting and integration.

3. **Harmony Patch Creation**: Utilize Copilot to scaffold new Harmony patches, including transpilers or postfixes, to expand or tweak the suppression mechanics.

4. **Testing and Debugging**: Generate unit tests for critical features, especially those involving calculations for suppression effects to ensure accuracy and balance.

5. **Feature Enhancement**: If new gameplay mechanics are added, Copilot can help draft initial class structures and method signatures based on existing patterns.

By following these suggestions and leveraging Copilot effectively, contributors can enhance and expand the "Suppression (Continued)" mod while maintaining high code quality.
