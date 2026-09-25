namespace Ach.Units.Player
{
    public class DisabledState : PlayerStateBase
    {
        public DisabledState(PlayerContext ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}