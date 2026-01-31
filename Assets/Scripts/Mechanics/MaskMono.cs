using UnityEngine;

namespace Assets.Scripts.Mechanics
{
    public class MaskMono : MonoBehaviour
    {
        void Awake()
        {
            SwitchCtrl.Singleton.RegisterMask(this);
        }
        void OnDestroy()
        {
            SwitchCtrl.Singleton.UnregisterMask(this);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector3 lossyScale = transform.lossyScale;
            Gizmos.DrawWireCube(transform.position, lossyScale);
        }
    }
}
