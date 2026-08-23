using Units.Player;
using UnityEngine;

public sealed class FreeState : PlayerStateBase
{
    private PlayerContext _ctx;
    private PlayerIntentProvider _intent;
    private CharacterControllerMotor _motor;
    public FreeState(PlayerContext ctx, CharacterControllerMotor motor, PlayerIntentProvider intent) : base(ctx)
    {
        _ctx = ctx;
        _intent = intent;
        _motor = motor;
    }
    
    public override void Enter()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        _motor.SetMoveIntent(_intent.MoveDirection);

    }

    public override void Exit()
    {
        
    }
}
