using Godot;
using Godot.Collections;
using System;

public partial class Player : Node
{

	private Units units;

    public override void _Ready()
	{

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void onSelectionControllerSelectedUnits(Array<Units> units)
	{
		GD.Print(units);
	}
}
