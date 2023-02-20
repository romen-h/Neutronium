# LUT API

## About

The LUT API allows for multiple mods to register color correction lookup tables.  
Each LUT entry can have a priority value that allows the LUT patch to resolve conflicts and chose a LUT with the highest priority every time color correction is recomputed.

## Usage

```csharp

public class MyMod : UserMod2
{
	public override void OnLoad(Harmony harmony)
	{
		var lutAPI = LUT_API.Setup(harmony);

		Texture2D nightLUT = LoadNightLUT(); // Implement this yourself

		lutAPI.RegisterLUT(
			"MyMod.Night",	// A unique string for your LUT, if you collide with another mod it will throw an exception
			LUTSlot.Night,	// The slot that the LUT applies to (day or night)
			100,			// A priority integer, highest priority value wins conflict resolution
			nightLUT		// The texture for the LUT
		);
	}
}