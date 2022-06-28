using System.Collections;
using UnityEngine;

public class Bomb : Collidable
{
    public Vector3 start;
    public Vector3 end;

    public float speed = 1.0f;
    public float timeAliveAfterArrive = 2;

    // Time when the movement started.
    private float _startTime;
    private float _journeyLength;

    protected override void Start()
    {
        base.Start();
        _startTime = Time.time;
        _journeyLength = Vector3.Distance(start, end);
    }

    protected override void Update()
    {
        base.Update();

        // If bomb already came, destroy it
        if (transform.position == end)
            StartCoroutine(DestroyWithDelay());

        // Distance moved equals elapsed time times speed..
        var distCovered = (Time.time - _startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        var fractionOfJourney = distCovered / _journeyLength;

        transform.position = Vector3.Lerp(start, end, fractionOfJourney);
    }

    protected virtual IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(timeAliveAfterArrive);
        Destroy(gameObject);
    }

    protected override void OnCollide(Collider2D hit)
    {
        if (!hit.CompareTag("Player")) return;

        var damage = new Damage()
        {
            DamageAmount = 1,
            Origin = transform.position,
            PushForce = 0
        };

        hit.SendMessage("TakeDamage", damage);
        Destroy(gameObject);
    }
}