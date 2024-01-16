using Godot;

public partial class Units : Node3D
{

	private bool isSelected = false;

	public override void _Ready()
	{

        GetNode<MeshInstance3D>("%SelectionCircle").Hide();
    }

	public override void _Process(double delta)
	{
	}

	public void select() {
		isSelected = true;
		GetNode<MeshInstance3D>("%SelectionCircle").Show();
	}

	public void unselect() {
		isSelected = false;
        GetNode<MeshInstance3D>("%SelectionCircle").Hide();
    }
}
