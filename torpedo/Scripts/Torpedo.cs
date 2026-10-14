using Godot;
using System;
using Torpedo.Scripts;

public partial class Player : CharacterBody2D
{
    [Export] public float Speed = 800f;
    [Export] public TorpedoRacs torpedoRacs;
    [Export] public Hajo_lehelyez hajo_Lehelyez;
    [Export] public JatekKezelo JatekVezerlo;


    private Vector2 startPosition;
    private float startRotation;
    private Vector2 targetPosition;
    private bool moving = false;

    // Kezdő értékek megadása
    public override void _Ready()
    {
        startPosition = GlobalPosition;
        startRotation = Rotation;
        targetPosition = GlobalPosition;
    }

    // Egér kattintás ellenőrzése
    public override void _Input(InputEvent @event)
    {
        if (JatekVezerlo == null || JatekVezerlo.JatekVege ||
            torpedoRacs == null || hajo_Lehelyez == null ||
            !torpedoRacs.isEnemyBoard || hajo_Lehelyez.isEnemyBoard ||
            !hajo_Lehelyez.MindenHajoLerakva || moving)
            return;

        if (@event.IsActionPressed("mouse_left_click") &&
            torpedoRacs.GetGlobalRect().HasPoint(GetGlobalMousePosition()))
        {
            targetPosition = GetGlobalMousePosition();
            moving = true;
        }
    }

    // Torpedó kilővése
    public override void _PhysicsProcess(double delta)
    {
        if (JatekVezerlo == null || JatekVezerlo.JatekVege ||
            torpedoRacs == null || hajo_Lehelyez == null ||
            !torpedoRacs.isEnemyBoard || hajo_Lehelyez.isEnemyBoard ||
            !hajo_Lehelyez.MindenHajoLerakva)
        {
            Velocity = Vector2.Zero;
            moving = false;
            return;
        }

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