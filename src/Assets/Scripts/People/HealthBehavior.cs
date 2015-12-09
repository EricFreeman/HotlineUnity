using Assets.Scripts.Messages;
using Assets.Scripts.Weapons;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts.People
{
    public class HealthBehavior : MonoBehaviour
    {
        public int Health = 5;

        void Update()
        {
            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter(Collider col)
        {
            var bullet = col.GetComponent<Bullet>();

            if (bullet != null)
            {
                Health -= bullet.Damage;
                EventAggregator.SendMessage(new SpawnBloodMessage
                {
                    Position = bullet.transform.position,
                    Amount = Random.Range(bullet.Damage, bullet.Damage * 3),
                    Direction = bullet.transform.rotation.y,
                    Force = bullet.Speed
                });
            }
        }
    }
}