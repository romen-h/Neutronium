# 🚧 Nothing to see here. 🚧

Neutronium will be a modding API for Oxygen Not Included.

## What is going on right now?
- Developing an early patching system to be able to patch any game code (runs before first scene is loaded)
- Developing the bootstrapper/installer that gets [Unity Doorstop](https://github.com/NeighTools/UnityDoorstop) and the NeutroniumCore libray installed to the game folder.
- Automatic code-gen for a mergelib that will allow normal mods to use the API and not crash if the API isn't installed.

## What will be here someday?
- Ensure some popular shared libs are up-to-date and loaded early enough to beat the versions included with old mods (i.e. unmerged PLib)
- Elements API
- Techs API
