using System;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        public float FieldOfView = 120f;
        public float ViewDistance = 5.5f;
        public float MoveSpeed = 2f;
        public float TurnSpeed = 6f;

        public EnemyState State = EnemyState.Patrolling;
        private Vector3 _lastKnownLocation;

        private GameObject _player;

        void Start()
        {
            _player = GameObject.Find("Player");
        }

        void FixedUpdate()
        {
            switch (State)
            {
                case EnemyState.Idle:
                    if (CanSeePlayer()) State = EnemyState.Detect;
                    break;
                case EnemyState.Patrolling:
                    if (CanSeePlayer()) State = EnemyState.Detect;
                    break;
                case EnemyState.Searching:
                    SearchState();
                    break;
                case EnemyState.Detect:
                    AlertState();
                    break;
            }
        }

        #region Search State

        private bool searchDir;
        private int searchAmount;

        private void SearchState()
        {
            if (CanSeePlayer())
            {
                searchAmount = 0;
                State = EnemyState.Detect;
            }

            var stopAmount = Math.Abs(searchAmount - transform.rotation.eulerAngles.y);
            if (stopAmount < 3 || (stopAmount - 360 < 3 && stopAmount - 360 > 0))
            {
                searchDir = !searchDir;
                searchAmount = (int)transform.rotation.eulerAngles.y + (120 * (searchDir ? 1 : -1));
            }

            transform.Rotate(0, 1 * (searchDir ? 1 : -1), 0);
        }

        #endregion

        #region Alert State

        private void AlertState()
        {
            if (!CanSeePlayer())
            {
                GetComponent<NavMeshAgent>().destination = _lastKnownLocation;

                if (Vector3.Distance(transform.position, _lastKnownLocation) < .1f)
                {
                    State = EnemyState.Searching;
                }
            }
            else
            {
                _lastKnownLocation = _player.transform.position;
                GetComponent<NavMeshAgent>().destination = _player.transform.position;

                var targetRotation = Quaternion.LookRotation(_player.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);
            }
        }

        #endregion

        private bool CanSeePlayer()
        {
            var rayDirection = _player.transform.position - transform.position;

            if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, rayDirection, out hit) && Vector3.Distance(transform.position, _player.transform.position) < ViewDistance)
                    if (hit.transform.tag == "Player")
                        return true;
            }

            return false;
        }
    }
}