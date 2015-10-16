using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class Bullet : MonoBehaviour
    {
        public float Speed;
        public float Damage = 10;

        void Update()
        {
            transform.position += (transform.forward.normalized * Speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider col)
        {
            Destroy(gameObject);
        }
    }
}