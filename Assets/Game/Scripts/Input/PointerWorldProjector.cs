using UnityEngine;

namespace Ach.Input
{
    public class PointerWorldProjector
    {
        private readonly Camera _camera;

        public PointerWorldProjector(Camera camera)
        {
            _camera = camera;
        }

        public bool TryProject(Vector2 screenPos, out Vector3 worldPoint, float groundY = 0f)
        {
            Ray ray = _camera.ScreenPointToRay(screenPos);
            var ground = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));
            if (ground.Raycast(ray, out float dist))
            {
                worldPoint = ray.GetPoint(dist);
                return true;
            }

            worldPoint = default;
            return false;
        }
    }

}
