using UnityEngine;

public class Collectable : Collidable
{
    private bool _collected;

    protected override void OnCollide(Collider2D hit)
    {
        if (hit.CompareTag("Player") && !_collected)
        {
            OnCollect();
        }
    }

    protected virtual void OnCollect()
    {
        _collected = true;
    }
}
