using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyMovement : MonoBehaviour {

        public GameObject Player;

        void Update () {
            GetComponent<NavMeshAgent>().destination = Player.transform.position;
        }
    }
}