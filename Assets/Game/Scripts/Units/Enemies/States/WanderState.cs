using UnityEngine;
using UnityEngine.AI;

namespace Ach.Units.Enemies
{
    public class WanderState : EnemyStateBase
    {
        public bool IsCompleted{ get; private set; }
        
        public WanderState(EnemyContext ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            IsCompleted = false;
            
            var randomPoint2D = Random.insideUnitCircle;
            var randomPoint3D = new Vector3(randomPoint2D.x, 0f, randomPoint2D.y);
            Vector3 randomPoint = Ctx.HomePosition + randomPoint3D * Ctx.Config.WanderRadius;
            Vector3 destination = NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, Ctx.Config.WanderRadius,
                    NavMesh.AllAreas) ? hit.position : Ctx.HomePosition;

            Ctx.Agent.SetDestination(destination);

        }

        public override void Tick(float deltaTime)
        {
            if(!Ctx.Agent.pathPending && Ctx.Agent.remainingDistance <= Ctx.Agent.stoppingDistance)
                IsCompleted = true;
        }

        public override void Exit()
        {
            IsCompleted = false;
        }
    }
}

