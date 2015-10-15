using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float MovementSpeed;

        void Update ()
        {
            var delta = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * Time.deltaTime * MovementSpeed;

            transform.position += delta;
        }
    }
}