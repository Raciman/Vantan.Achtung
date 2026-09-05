
namespace Ach.Units.Player
{
    public interface ILocomotionView
    {
        bool IsDead { get; }
        bool IsSprinting { get; }
    }

    public interface IStanceView
    {
        bool IsAiming { get; }
        bool IsReloading { get; }
    }

}
