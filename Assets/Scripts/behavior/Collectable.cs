using UnityEngine;

public class Collectable : Collidable
{
    private bool _collected;

    protected override void Start()
    {
        base.Start();
        ColliderListener.OnPlayerCollideEvent += OnTryCollect;
    }

    private void OnTryCollect(Collider2D hit)
    {
        if (!_collected)
        {
            OnCollect();
        }
    }

    protected virtual void OnCollect()
    {
        _collected = true;
    }
}