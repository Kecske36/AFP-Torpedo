using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export]
    public float Speed = 800f;

    private Vector2 startPosition;
    private float startRotation;

    private Vector2 targetPosition;
    private bool moving = false;

    public override void _Ready()
    {
        startPosition = GlobalPosition;
        startRotation = Rotation;
        targetPosition = GlobalPosition;
    }

    public override void _Input(InputEvent @event)
    {
        if (moving)
            return;

        if (@event.IsActionPressed("mouse_left_click"))
        {
            targetPosition = GetGlobalMousePosition();
            moving = true;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!moving)
            return;

        Vector2 direction = targetPosition - GlobalPosition;

        if (direction.Length() > 10)
        {
            LookAt(targetPosition);

            Velocity = direction.Normalized() * Speed;
            MoveAndSlide();
        }
        else
        {
            Velocity = Vector2.Zero;

            GlobalPosition = startPosition;
            Rotation = startRotation;

            moving = false;
        }
    }
}