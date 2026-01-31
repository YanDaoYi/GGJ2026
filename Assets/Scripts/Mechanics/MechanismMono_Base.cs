using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
using UnityToolkit;

namespace Assets.Scripts.Mechanics
{
    public abstract class MechanismMono_Base : MonoBehaviour
    {
        [PropertyOrder(0)]
        [ShowInInspector, ReadOnly]
        public abstract EMechanismType MechanismType { get; }
        public Trigger2DEventEmitter EventEmitter { get; private set; }
        [PropertyOrder(1)]
        [ShowInInspector, ReadOnly]
        BoxCollider2D _collider;
        [PropertyOrder(2)]
        [ShowInInspector, ReadOnly]
        protected Transform viewTf;
        [PropertyOrder(3)]
        [ShowInInspector, ReadOnly]
        protected Transform logicTf;

        protected virtual void Awake()
        {
            EventEmitter = GetComponentInChildren<Trigger2DEventEmitter>();
            _collider = GetComponentInChildren<BoxCollider2D>();
            viewTf = transform.Find("View");
            logicTf = transform.Find("Logic");
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
            viewTf = null;
            logicTf = null;
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
        Destination,
        Token
    }
}
