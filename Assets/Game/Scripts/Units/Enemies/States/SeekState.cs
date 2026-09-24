using UnityEngine;

namespace Ach.Units.Enemies
{
    public class SeekState : EnemyStateBase
    {
        public bool IsCompleted { get; private set; }
        public SeekState(EnemyContext ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            IsCompleted = false;
            Ctx.Agent.SetDestination(Ctx.Intent.LastSeenTargetPosition);
        }

        public override void Tick(float deltaTime)
        {
            if (!Ctx.Agent.pathPending && Ctx.Agent.remainingDistance <= Ctx.Agent.stoppingDistance)
                IsCompleted = true;
        }

        public override void Exit()
        {
            IsCompleted = false;
        }
    }
}