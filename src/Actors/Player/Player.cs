using Godot;
using System;

public class Player : Actor, IActor
{
    protected StateMachine StateMachine;

    public override void _Ready()
    {
        this.StateMachine = new StateMachine();
        this.StateMachine.AddState((State)GetNode("States/Idle"));
        this.StateMachine.AddState((State)GetNode("States/Run"));
        this.StateMachine.AddState((State)GetNode("States/Jump"));
        this.StateMachine.AddState((State)GetNode("States/Fall"));
        this.StateMachine.ChangeState("Idle");
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        // this handle all the states
        this.StateMachine.Handle(this, delta);
    }

    public Vector2 GetSnapPosition(Vector2 direction)
    {
        return direction.y == 0 ? Vector2.Down * 60 : Vector2.Zero;
    }

    public Vector2 GetDirection()
    {
        return new Vector2(
            Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
            IsOnFloor() && Input.IsActionJustPressed("move_up") ? -Input.GetActionStrength("move_up") : 0
        );
    }

    public Vector2 CalculateVelocity(
        Vector2 linearVelocity,
        Vector2 direction,
        Vector2 speed
    ) {
        Vector2 moveVelocity = linearVelocity;
        moveVelocity.x = speed.x * direction.x;
        if (direction.y != 0) {
            moveVelocity.y = speed.y * direction.y;
        }
        return moveVelocity;
    }

    private void _OnScreenExited()
    {
        GD.Print("You are Dead!");
        QueueFree();
    }
}
