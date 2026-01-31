// Copyright (c) 2023 NicoIer and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.
#if UNITY_2021_3_OR_NEWER

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace UnityToolkit
{
    [RequireComponent(typeof(Collider2D))]
    public class Trigger2DEventEmitter : MonoBehaviour
    {
        public event Action<Collider2D> TriggerEnter = delegate { };
        public event Action<Collider2D> TriggerExit = delegate { };
        public event Action<Collider2D> TriggerStay = delegate { };

        public event Action<Collider2D> TriggerEnter_Fully = delegate { };
        public event Action<Collider2D> TriggerExit_Fully = delegate { };
        public new Collider2D collider2D { get; private set; }
        public LayerMask layerMask;
        HashSet<Collider2D> colliders_FullyEnter = new();

        private void Awake()
        {
            collider2D = GetComponent<Collider2D>();
            colliders_FullyEnter.Clear();
            Assert.IsNotNull(collider2D);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            {
                TriggerEnter(other);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            {
                TriggerExit(other);
                FullyExit(other);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            {
                TriggerStay(other);

                bool isFullyInside = FullyInsideByBounds(other, collider2D);
                if (isFullyInside) FullyEnter(other);
            }
        }

        void FullyEnter(Collider2D other)
        {
            if (!colliders_FullyEnter.Contains(other))
            {
                colliders_FullyEnter.Add(other);
                TriggerEnter_Fully(other);
            }
        }

        void FullyExit(Collider2D other)
        {
            if (colliders_FullyEnter.Contains(other))
            {
                colliders_FullyEnter.Remove(other);
                TriggerExit_Fully(other);
            }
        }

        static bool FullyInsideByBounds(Collider2D other, Collider2D box)
        {
            Bounds b = box.bounds;
            Bounds o = other.bounds;

            Vector3 p1 = new Vector3(o.min.x, o.min.y, b.center.z);
            Vector3 p2 = new Vector3(o.min.x, o.max.y, b.center.z);
            Vector3 p3 = new Vector3(o.max.x, o.min.y, b.center.z);
            Vector3 p4 = new Vector3(o.max.x, o.max.y, b.center.z);

            return b.Contains(p1) && b.Contains(p2) && b.Contains(p3) && b.Contains(p4);
        }
    }
}
#endif
