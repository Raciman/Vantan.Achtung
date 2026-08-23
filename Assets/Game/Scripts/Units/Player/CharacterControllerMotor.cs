using UnityEngine;

namespace Units.Player
{
    public class CharacterControllerMotor : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        
        [SerializeField] private CharacterController controller;

        private Vector3 _moveIntent;
        
        public void SetMoveIntent(Vector3 dir) => _moveIntent = Vector3.ClampMagnitude(dir, 1f);

        public void Tick(float deltaTime)
        {
            var motion = _moveIntent * speed * deltaTime;
            controller.Move(motion);
        }
    }
}

