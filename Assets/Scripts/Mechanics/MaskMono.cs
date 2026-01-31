using UnityEngine;
using UnityEngine.Assertions;
using UnityToolkit;

namespace Assets.Scripts.Mechanics
{
    public class MaskMono : MonoBehaviour
    {
        public Trigger2DEventEmitter EventEmitter { get; private set; }
        BoxCollider2D _collider;

        void Awake()
        {
            EventEmitter = GetComponentInChildren<Trigger2DEventEmitter>();
            Assert.IsNotNull(EventEmitter);
            SwitchCtrl.Singleton.RegisterMask(this);
        }
        void OnDestroy()
        {
            SwitchCtrl.Singleton.UnregisterMask(this);
        }
        private void OnDrawGizmos()
        {
            if (_collider == null) return;
            Gizmos.color = Color.green;
            Vector3 lossyScale = _collider.transform.lossyScale;
            Gizmos.DrawWireCube(_collider.transform.position, lossyScale);
        }

        private void OnValidate()
        {
            _collider = GetComponentInChildren<BoxCollider2D>();
        }
    }
}
