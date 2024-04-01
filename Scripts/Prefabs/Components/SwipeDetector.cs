using Godot;

namespace PowerSokoBan.Scripts.Prefabs.Components;

[GlobalClass]
public partial class SwipeDetector : Node
{
    public delegate void SwipedEventHandler(Vector2 direction);
    public delegate void SwipedCanceledEventHandler(Vector2 startPos);
    
    [Export]
    private float _maxDiagonalSlope = 100.0f;

    private Timer _timer;
    private Vector2 _startPos;
    
    public event SwipedEventHandler Swiped;
    public event SwipedCanceledEventHandler SwipedCanceled;
    
    public override void _Ready()
    {
        base._Ready();
        _timer = new Timer();
        AddChild(_timer);
        _timer.WaitTime = 1.0f;
        _timer.OneShot = true;
        _timer.Timeout += _on_Timer_timeout;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenTouch screenTouch == false) return;

        if (screenTouch.Pressed)
        {
            StartDetection(screenTouch.Position);
        } else if (!_timer.IsStopped())
        {
            EndDetection(screenTouch.Position);
        }
    }

    private void StartDetection(Vector2 position)
    {
        _startPos = position;
        _timer.Start();
    }

    private void EndDetection(Vector2 position)
    {
        _timer.Stop();
        var direction = (position - _startPos).Normalized();
        
        if (Mathf.Abs(direction.X) + Mathf.Abs(direction.Y) >= _maxDiagonalSlope) return;
        if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
        {
            Swiped(new Vector2(-Mathf.Sign(direction.X), (float)0.0));
        }
        else
        {
            Swiped(new Vector2((float)0.0, -Mathf.Sign(direction.Y)));
        }
    }

    private void _on_Timer_timeout()
    {
        SwipedCanceled(_startPos);
    }
}
