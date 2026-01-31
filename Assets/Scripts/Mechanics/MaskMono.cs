using UnityEngine;
using UnityEngine.Assertions;
using UnityToolkit;

namespace Assets.Scripts.Mechanics
{
    public class MaskMono : MonoBehaviour
    {
        public Trigger2DEventEmitter EventEmitter { get; private set; }

        void Awake()
        {
            EventEmitter = GetComponent<Trigger2DEventEmitter>();
            Assert.IsNotNull(EventEmitter);
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
