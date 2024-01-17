using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class SelectionController : Node3D
{
    [Signal]
    public delegate void selectedUnitsEventHandler(Array<Units> unitsSelected);

    [Export]
    public Camera3D camera;

    private Array<Units> unitsSelected;

    private Vector3 rayOrigin;
    private Vector3 rayTarget;


    public override void _Ready()
    {
        unitsSelected = new Array<Units>();
    }

    public void onInputBoxSelection(Vector2 position, Vector2 selectionArea)
    {
        Rect2 box = new Rect2(position, selectionArea - position);
        foreach (Units unit in GetTree().GetNodesInGroup("units"))
        {
            if (box.HasPoint(camera.UnprojectPosition(unit.GlobalTransform.Origin)))
            {
                unitsSelected.Add(unit);
                unit.select();
            }
            else
            {
                unitsSelected.Remove(unit);
                unit.unselect();
            }
        }
        EmitSignal(SignalName.selectedUnits, unitsSelected);
    }

    public void onInputSingleSelection(Vector2 position)
    {
        Variant variant = projectRayCastFromCamera(position);
        CollisionObject3D unitRigitBody = (CollisionObject3D) variant.AsGodotObject();

        Node3D unit = unitRigitBody.GetParent<Node3D>();
        if (unit.Name.Equals("Units"))
        {
            GD.Print("Sim");
        } else
        {

            GD.Print("Não");
        }


        GD.Print(unitRigitBody);

    }

    private Variant projectRayCastFromCamera(Vector2 position)
    {
        int rayLength = 1000;
        rayOrigin = camera.ProjectRayOrigin(position);
        rayTarget = rayOrigin + camera.ProjectRayNormal(position) * rayLength;


        PhysicsDirectSpaceState3D space = GetWorld3D().DirectSpaceState;
        PhysicsRayQueryParameters3D rayQuery = new PhysicsRayQueryParameters3D();
        rayQuery.From = rayOrigin;
        rayQuery.To = rayTarget;
        Dictionary intersectedRay = space.IntersectRay(rayQuery);
        return intersectedRay["collider"];
    }
}
