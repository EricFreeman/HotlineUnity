using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyBehavior : MonoBehaviour
    {
        private void OnTriggerEnter(Collider col)
        {
            if (col.name.StartsWith("Bullet"))
            {
                Destroy(gameObject);
            }
        }
    }
}