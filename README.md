# Neutronium
Neutronium is a collection of merge libraries for modding the game Oxygen Not Included.

## Libraries

### [Audio](./Audio)
Allows custom sounds to be loaded from file and played.
- Supports positional audio relative to the game camera.
- WIP

### [Post Processing](./PostProcessing)
API for screen-space effects.
- Day & Night LUT (Color Correction)

## Planned Libraries

### Diseases
- Adding new disease (germ) types
- Custom shader for disease overlay to support unique disease textures. (thanks Aki!)

### Elements
- Adding new elements through an API to avoid patch bloat
- Detecting elements added by other mods to enable optional features in your own mod
- Backup elements so they can exist even after a mod is disabled. (For loading old saves)
- Configurable conflict resolution so that multiple mods can share the same element if they both add it

### Personas
- Adding new duplicant personalities
- Adding personality traits (good/bad traits)
- Support for completely custom art

### Worlds
A library for adding entirely custom clusters and asteroids with compatibility for the YAML based worldgen mods

### Zones  
A library for adding new biome/zone types. (The background texture for a biome)
