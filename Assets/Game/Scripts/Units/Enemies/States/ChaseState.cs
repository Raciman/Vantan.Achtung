namespace Ach.Units.Enemies
{
    public sealed class ChaseState : EnemyStateBase
    {
        public ChaseState(EnemyContext ctx) : base(ctx)
        {
            
        }

        public override void Enter()
        {

        }

        public override void Tick(float deltaTime)
        {
            Ctx.Agent.SetDestination(Ctx.Target.Transform.position);
        }

        public override void Exit()
        {
            Ctx.Agent.ResetPath();
        }
    }
}

