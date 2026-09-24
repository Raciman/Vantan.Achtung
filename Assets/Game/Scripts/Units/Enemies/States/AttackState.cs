using UnityEngine;

namespace Ach.Units.Enemies
{
    public class AttackState : EnemyStateBase
    {
        public bool IsCompleted { get; private set; }
        public float LastAttackTime { get; private set; } 


        private bool _hitApplied;
        private float _elapsed;
        public AttackState(EnemyContext ctx) : base(ctx)
        {
            LastAttackTime = float.NegativeInfinity;
        }

        public override void Enter()
        {
            Ctx.Agent.updateRotation = false;
            _elapsed = 0f;
            _hitApplied = false;
            IsCompleted = false;
            Ctx.Animator.PlayAttack();
        }

        public override void Tick(float deltaTime)
        {
            _elapsed += deltaTime;


            if (_elapsed < Ctx.Config.AttackHitTime)
                RotateTowardsTarget(deltaTime);
            else if (!_hitApplied)
            {
                _hitApplied = true;
                TryApplyDamage();
            }

            
            if(_elapsed >= Ctx.Config.AttackDuration)
                IsCompleted = true;
                
        }

        private void RotateTowardsTarget(float deltaTime)
        {
            Vector3 dir = Ctx.Target.Transform.position - Ctx.Agent.transform.position;
            dir.y = 0f;
            
            var look = Quaternion.LookRotation(dir);
            var t = Ctx.Agent.transform;
            t.rotation = Quaternion.RotateTowards(t.rotation, look, Ctx.Agent.angularSpeed * deltaTime);
        }

        private void TryApplyDamage()
        {
            Vector3 toTarget = Ctx.Target.Transform.position - Ctx.Agent.transform.position;
            toTarget.y = 0f;
            if(toTarget.magnitude > Ctx.Config.AttackHitRange)
                return;
            if(Vector3.Angle(Ctx.Agent.transform.forward, toTarget) > Ctx.Config.AttackAngle * 0.5f)
                return;
            
            Ctx.Target.ApplyDamage(Ctx.Config.Damage);
        }

        public override void Exit()
        {
            Ctx.Agent.updateRotation = true;
            LastAttackTime = Time.time;
            _elapsed = 0f;
            IsCompleted = false;
        }
    }
}

