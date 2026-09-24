using Ach.Units.Enemies;

public sealed class IdleState : EnemyStateBase
{

    public bool IsCompleted { get; private set; }
    private float _elapsed;
    public IdleState(EnemyContext ctx) : base(ctx)
    {
    }

    public override void Enter()
    {
        Ctx.Agent.ResetPath();
        IsCompleted = false;
        _elapsed = 0f;
    }

    public override void Tick(float deltaTime)
    {
        _elapsed += deltaTime;
        if(_elapsed >= Ctx.Config.IdleDuration)
            IsCompleted = true;
    }

    public override void Exit()
    {
        IsCompleted = false;
        _elapsed = 0f;
    }
}
