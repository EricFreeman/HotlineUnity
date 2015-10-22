using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        private GameObject _player;

        void Start()
        {
            _player = GameObject.Find("Player");
        }

        void Update()
        {
            GetComponent<NavMeshAgent>().destination = _player.transform.position;
        }
    }
}