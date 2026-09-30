using Godot;
using System;

public partial class CharacterBody2d : CharacterBody2D
{
    [Export]
    public float Speed = 200f;

    private Vector2 startPosition;
    private Vector2 targetPosition;

    private bool movingToTarget = false;
    private bool movingBack = false;

    public override void _Ready()
    {
        startPosition = GlobalPosition;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Left &&
                mouseButton.Pressed &&
                !movingToTarget &&
                !movingBack)
            {
                targetPosition = GetGlobalMousePosition();
                movingToTarget = true;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 destination;

        if (movingToTarget)
            destination = targetPosition;
        else if (movingBack)
            destination = startPosition;
        else
            return;

        Vector2 direction = destination - GlobalPosition;

        if (direction.Length() < 5)
        {
            GlobalPosition = destination;
            Velocity = Vector2.Zero;

            if (movingToTarget)
            {
                movingToTarget = false;
                movingBack = true;
            }
            else
            {
                movingBack = false;
            }
        }
        else
        {
            Velocity = direction.Normalized() * Speed;
            MoveAndSlide();
        }
    }
}