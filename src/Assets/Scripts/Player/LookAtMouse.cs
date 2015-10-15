using UnityEngine;

namespace Assets.Scripts.Player
{
    public class LookAtMouse : MonoBehaviour
    {
        void FixedUpdate()
        {
            var playerPlane = new Plane(Vector3.up, transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float hitdist;
            if (playerPlane.Raycast(ray, out hitdist))
            {
                var targetPoint = ray.GetPoint(hitdist);
                
                var newRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
                newRotation.x = 0;
                newRotation.z = 0;
                transform.rotation = newRotation;
            }
        }
    }
}