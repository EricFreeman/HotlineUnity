using UnityEngine;

namespace Assets.Scripts.Misc
{
    public class RemoveOffscreen : MonoBehaviour
    {
        void Update()
        {
            if (!GetComponent<Renderer>().isVisible && Time.fixedTime > 1)
            {
                DestroyImmediate(gameObject);
            }
        }
    }
}