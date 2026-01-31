using UnityEngine;

namespace Assets.Scripts.Mechanics
{
    public class MaskMono : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector3 lossyScale = transform.lossyScale;
            Gizmos.DrawWireCube(transform.position, lossyScale);
        }
    }
}
