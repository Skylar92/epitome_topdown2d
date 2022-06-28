using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace helper
{
    public delegate void CollidableEvent(Collider2D collider);

    public class Collidable2DService
    {
        private readonly Collider2D _collider;
        private readonly ContactFilter2D _filter;
        private readonly Collider2D[] _hits = new Collider2D[10];

        public event CollidableEvent OnPlayerCollideEvent;

        public Collidable2DService(Collider2D collider, ContactFilter2D filter)
        {
            _collider = collider;
            _filter = filter;
        }

        public void CheckCollider()
        {
            _collider.OverlapCollider(_filter, _hits);
            for (var index = 0; index < _hits.Length; index++)
            {
                var hit = _hits[index];
                if (ReferenceEquals(hit, null)) continue;

                if (hit.CompareTag("Player"))
                    OnPlayerCollideEvent?.Invoke(hit);

                _hits[index] = null;
            }
        }
    }
}