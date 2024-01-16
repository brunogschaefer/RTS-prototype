using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class SelectionController : Node3D
{

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
        
    }

    public void onInputSingleSelection(Vector2 position)
    {
        GD.Print(position);

    }

    // to be implemented
    //private void projectRayCastFromCamera()
    //{
    //    int rayLength = 300;
    //    rayOrigin = camera.ProjectRayOrigin(position);
    //    rayTarget = rayOrigin + camera.ProjectRayNormal(position) * rayLength;


    //    PhysicsDirectSpaceState3D space = GetWorld3D().DirectSpaceState;
    //    PhysicsRayQueryParameters3D rayQuery = new PhysicsRayQueryParameters3D();
    //    rayQuery.From = rayOrigin;
    //    rayQuery.To = rayTarget;
    //    Dictionary intersectedRay = space.IntersectRay(rayQuery);

    //    if (intersectedRay != null)
    //    {
    //        GD.Print(intersectedRay["collider"]);
    //        Vector3 rayCollisionPoint = Position - (Vector3)intersectedRay["collider"];
    //    }
    //}

    //private void getMouseCoordinates()
    //{
    //    Vector2 mousePos = GetViewport().GetMousePosition();
    //    int rayLength = 100;
    //    rayOrigin = camera.ProjectRayOrigin(mousePos);
    //    rayTarget = rayOrigin + camera.ProjectRayNormal(mousePos) * rayLength;
    //}
}
