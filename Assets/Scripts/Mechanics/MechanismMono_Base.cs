using UnityEngine;
using UnityEngine.Assertions;
using UnityToolkit;

namespace Assets.Scripts.Mechanics
{
    public abstract class MechanismMono_Base : MonoBehaviour
    {
        public abstract EMechanismType MechanismType { get; }
        public Trigger2DEventEmitter EventEmitter { get; private set; }
        BoxCollider2D _collider;

        protected virtual void Awake()
        {
            EventEmitter = GetComponentInChildren<Trigger2DEventEmitter>();
            _collider = GetComponentInChildren<BoxCollider2D>();
            Assert.IsNotNull(EventEmitter);
            EventEmitter.TriggerEnter += TriggerEnterHandle;
            EventEmitter.TriggerExit += TriggerExitHandle;
        }
        void OnDestroy()
        {
            EventEmitter.TriggerEnter -= TriggerEnterHandle;
            EventEmitter.TriggerExit -= TriggerExitHandle;
            EventEmitter = null;
            _collider = null;
        }

        protected virtual void TriggerEnterHandle(Collider2D other)
        {
            // To be overridden in derived classes
        }

        protected virtual void TriggerExitHandle(Collider2D other)
        {
            // To be overridden in derived classes
        }

        private void OnValidate()
        {
            _collider = GetComponentInChildren<BoxCollider2D>();
        }
    }

    public enum EMechanismType
    {
        Destination
    }
}
