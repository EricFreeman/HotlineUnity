using UnityEngine;

namespace Assets.Scripts.Misc
{
    public class TrackObject : MonoBehaviour
    {
        public GameObject Object;
        public Vector3 Offset;

        void Update ()
        {
            transform.position = Object.transform.position + Offset;
        }
    }
}