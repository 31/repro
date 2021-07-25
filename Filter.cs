using Godot;
using GodotCsIsAssignableFrom;

public class Filter : Spatial
{
	public override void _Ready()
	{
		var r = Utilities.Children_StripVisuals(this);
		GD.Print("Result ", r);
	}
}
