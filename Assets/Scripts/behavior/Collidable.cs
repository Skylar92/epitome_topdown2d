using helper;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    private Collider2D _collider2D;

    protected Collidable2DService ColliderListener;

    protected virtual void Start()
    {
        _collider2D = GetComponent<BoxCollider2D>();
        ColliderListener = new Collidable2DService(_collider2D, contactFilter);
    }

    protected virtual void Update()
    {
        ColliderListener.CheckCollider();
    }
}