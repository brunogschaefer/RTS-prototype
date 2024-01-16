using Godot;

public partial class CameraControl : Node3D
{
	/*
	 * Camera Movement WASD
	 */
	[ExportCategory("Camera Movement")]


    [ExportGroup("Movement")]
    [Export(PropertyHint.Range, "0,100,1")]
    public float cameraMoveSpeed = 20.0f;

    [Export(PropertyHint.Range, "0,1.0,0.1")]
    public float movementLerp = 0.1f;


    /*
	* Camera Movement Pan
	*/
    [ExportSubgroup("Movement Pan")]
	[Export(PropertyHint.Range, "0,32,4")]
	public int cameraAutomaticPanMargin = 16;
    [Export(PropertyHint.Range, "0,20,0.5")]
    public float cameraAutomaticPanSpeed = 12;

    /*
	* Camera Rotation
	*/
    [ExportGroup("Rotation")]
	[Export(PropertyHint.Range, "0,10,0.1")]
	public float cameraRotationSpeed = 5f;
    [Export(PropertyHint.Range, "0,0.3,0.01")]
    public float rotationLerp = 0.1f;

    /*
	* Camera Zoom
	*/
    [ExportGroup("Zoom")]
	[Export(PropertyHint.Range, "0,100,1")]
	public double cameraZoomSpeed = 1.0f;
	[Export(PropertyHint.Range, "0,100,1")]
	public double cameraMinimumZoom = 4.0f;
    [Export(PropertyHint.Range, "0,100,1")]
    public double cameraMaximumZoom = 16.0;
    [Export(PropertyHint.Range, "0,10.0,0.1")]
    public float zoomLerp = 8.0f;

    private double zoom;
    private double targetZoom;

    /*
	* Camera Structure
	*/
    private Node3D cameraSocket;
	private Camera3D camera;

    /*
	* Camera Validations
	*/
    private bool canProcess = true;
	private bool canMove = true;
	private bool canZoom = true;
	private bool canPan = true;
	private bool canRotate = true;

    /*
	* Cached Variables
	*/
	private Vector3 cachedMovement;
	private Vector3 cachedRotation;
	private Vector3 cachedZoom;
	private Vector3 cachedPan;

    public override void _Ready()
	{
        cameraSocket = GetNode<Node3D>("%camera_socket");
		camera = cameraSocket.GetNode<Camera3D>("camera");
		camera.Position = new Vector3(camera.Position.X, camera.Position.Y, (float) cameraMinimumZoom);
		targetZoom = cameraMinimumZoom;
    }

	public override void _Process(double delta)
	{

        if (!canProcess) 
			return;

		if (Input.IsActionPressed("escape"))
			GetTree().Quit();

		cameraBaseMove((float) delta);
        cameraBaseRotation((float) delta);
        cameraZoomUpdate((float) delta);
		//cameraAutomaticPan(delta); to-do

    }

    public override void _UnhandledInput(InputEvent @event)
    {
		// Camera Zoom
        if (@event.IsAction("zoom_in"))
		{
            targetZoom = Mathf.Clamp(targetZoom - cameraZoomSpeed, cameraMinimumZoom, cameraMaximumZoom);
        }
		else if (@event.IsAction("zoom_out"))
		{
            targetZoom = Mathf.Clamp(targetZoom + cameraZoomSpeed, cameraMinimumZoom, cameraMaximumZoom);
		}
    }


    /*
	* Camera Movement WASD OK
	*/
    private void cameraBaseMove(float delta)
	{
		if (!canMove)
			return;

		Vector3 input = Vector3.Zero;

        input.X = Input.GetAxis("camera_left", "camera_right");
		input.Z = Input.GetAxis("camera_forward", "camera_backward");
		input = input.Rotated(Vector3.Up, Rotation.Y).Normalized();

        cachedMovement += input * cameraMoveSpeed * delta;

        Position = Position.Lerp(cachedMovement, movementLerp);
    }

    /*
	* Camera Zoom OK
	*/
    private void cameraZoomUpdate(float delta)
	{
		if (!canZoom)
			return;

        zoom = Mathf.Lerp(zoom, targetZoom, zoomLerp * delta);
		if (zoom != targetZoom)
		{
			cachedZoom.Z = (float) zoom;
            camera.Position = cachedZoom;
		}
    }

    
    /*
	* Camera Rotation OK
	*/
    private void cameraBaseRotation(float delta)
	{
		if (!canRotate) 
			return;

        Vector3 input = Vector3.Zero;

        input.Y = Input.GetAxis("camera_rotate_left", "camera_rotate_right");

		cachedRotation += input * cameraRotationSpeed * delta;

        Rotation = Rotation.Lerp(cachedRotation, rotationLerp);

    }

    /*
	* Camera Pan
	*/
    private void cameraAutomaticPan(double delta)
    {
        if (!canPan)
            return;

        cachedPan.X = -1;
        cachedPan.Y = -1;
        Viewport currentViewport = GetViewport();
        Rect2 viewportVisibleRectangle = currentViewport.GetVisibleRect();
        Vector2 viewportSize = viewportVisibleRectangle.Size;
        Vector2 currentMousePoisiton = currentViewport.GetMousePosition();

        double zoomFactor = camera.Position.Z * 0.1;


        // X pan
        if (currentMousePoisiton.X < cameraAutomaticPanMargin
            || currentMousePoisiton.X > viewportSize.X - cameraAutomaticPanMargin)
        {
            if (currentMousePoisiton.X > viewportSize.X / 2)
            {
                cachedPan.X += cameraAutomaticPanSpeed * (float)zoomFactor * (float)delta;
            }

        }

        // Y pan
        if (currentMousePoisiton.Y < cameraAutomaticPanMargin
            || currentMousePoisiton.Y > viewportSize.Y - cameraAutomaticPanMargin)
        {
            if (currentMousePoisiton.Y > viewportSize.Y / 2)
            {
                cachedPan.Z += cameraAutomaticPanSpeed * (float)zoomFactor * (float)delta;
            }

        }
        GD.Print(cachedPan);
        Position = Position.Lerp(cachedPan, movementLerp);
    }

}
