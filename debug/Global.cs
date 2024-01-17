using Godot;
using System;

public partial class Global : Node
{
    private static Global _instance;
    public static Global Instance => _instance;
    public DebugPanel debug;

    public override void _EnterTree()
    {
        if (_instance != null)
        {
            this.QueueFree(); // The Singletone is already loaded, kill this instance
        }
        _instance = this;
    }
}
