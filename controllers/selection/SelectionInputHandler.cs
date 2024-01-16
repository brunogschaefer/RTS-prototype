using Godot;


/*
 *  Class to controll selection box behavior. It will emit it's informations to SelectionControl
 */
public partial class SelectionInputHandler : Control
{
    [Signal]
    public delegate void singleSelectionEventHandler(Vector2 position);

    [Signal]
    public delegate void boxSelectionEventHandler(Vector2 startPosition, Vector2 endPosition);

    private Panel selectionBox;
    protected Vector2 originPosition;
    protected Vector2 mousePosition;
    protected Vector2 size;
    protected Vector2 customScale;
    private bool isSelecting;

    public override void _Ready()
    {
        selectionBox = GetNode<Panel>("Panel");
        selectionBox.Size = Vector2.Zero;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton)
        {
            if (@event.IsActionPressed("left_click"))
            {
                originPosition = GetGlobalMousePosition();
                selectionBox.Position = originPosition;
                isSelecting = true;
            }
            else if (@event.IsActionReleased("left_click"))
            {
                setNormalizedSelectionArea();

                if (selectionBox.Size.Length() < 3)
                {
                    EmitSignal(SignalName.singleSelection, mousePosition);
                } else
                {
                    EmitSignal(SignalName.boxSelection, originPosition, mousePosition);
                }

                selectionBox.Size = Vector2.Zero;
                isSelecting = false;
            }
            
        }
        if (@event is InputEventMouseMotion)
        {
            if (isSelecting)
            {
                mousePosition = GetGlobalMousePosition();
                if (mousePosition.X < originPosition.X)
                {
                    customScale.X = -1;
                }
                if (mousePosition.X > originPosition.X)
                {
                    customScale.X = 1;
                }
                if (mousePosition.Y < originPosition.Y)
                {
                    customScale.Y = -1;
                }
                if (mousePosition.Y > originPosition.Y)
                {
                    customScale.Y = 1;
                }

                selectionBox.Scale = customScale;
                selectionBox.Size = (mousePosition - originPosition) * customScale;
            }
        }
    }

    private void setNormalizedSelectionArea()
    {
        float temporaryValue;
        if (originPosition.X > mousePosition.X)
        {
            temporaryValue = originPosition.X;
            originPosition.X = mousePosition.X;
            mousePosition.X = temporaryValue;
        }
        if (originPosition.Y > mousePosition.Y)
        {
            temporaryValue = originPosition.Y;
            originPosition.Y = mousePosition.Y;
            mousePosition.Y = temporaryValue;
        }
    }
}
