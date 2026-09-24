namespace Ach.Units.Player
{
    public sealed class InteractState : PlayerStateBase
    {
        public bool IsCompleted { get; private set; }
        public InteractState(PlayerContext ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            IsCompleted = false;
            Ctx.Interact.Closest?.Interact(Ctx.Interact);
            Ctx.Intent.ConsumeInteract();
        }

        public override void Tick(float deltaTime)
        {
            IsCompleted = true;
        }

        public override void Exit()
        {

            IsCompleted = false;
        }
    }
}