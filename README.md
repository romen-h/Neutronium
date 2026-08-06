# 🚧 Nothing to see here. 🚧

Neutronium will be a modding API for Oxygen Not Included.

## Features

### Neutronium Loader
A regular ONI mod loading from UserMod2 that installs [Unity Doorstop](https://github.com/NeighTools/UnityDoorstop) and **Neutronium Core**.

### Neutronium Core
- Runs as soon as the game launches via Unity Dootstop.
- Applies essential fixes to the game in code that runs before regular mods even load
  - Relocating the game's saves/mods folder to avoid OneDrive filesystem issues.
  - Disabling the Steam Workshop (but not Achievements!)
  - Fixing mod crash blame (To be implemented...)
- Loading plugins nested in mods and giving them opportunity to early patch the game too.
- Implementation of the **Neutronium API**

### Neutronium API
A library that promises to be forwards compatible so that mods built against it never stop working.

### Neutronium API MergeLib
A merge library that mocks the **Neutronium API** and allows mods to have soft dependency on **Neutronium Core**.
- This library is automatically generated to ensure perfect synchronization with the actual **Neutronium API** surface.

## What will be here someday?
- Ensure some popular shared libs are up-to-date and loaded early enough to beat the versions included with old mods (i.e. unmerged PLib)
- Elements API
- Techs API
