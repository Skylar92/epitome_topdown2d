using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D contactFilter;

    private BoxCollider2D _collider2D;
    private Collider2D[] _hits = new Collider2D[10];

    protected virtual void Start()
    {
        _collider2D = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        _collider2D.OverlapCollider(contactFilter, _hits);
        for (var index = 0; index < _hits.Length; index++)
        {
            var hit = _hits[index];
            if (ReferenceEquals(hit, null)) continue;
            
            OnCollide(hit);
            _hits[index] = null;
        }
    }

    protected virtual void OnCollide(Collider2D hit) {}
}
