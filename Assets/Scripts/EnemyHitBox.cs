using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : Collidable
{
    public int damagePoint = 1;
    public float pushForce = 5;

    protected override void OnCollide(Collider2D hit)
    {
        if (hit.CompareTag("Player"))
        {
            var damage = new Damage()
            {
                DamageAmount = this.damagePoint,
                Origin = transform.position,
                PushForce = pushForce
            };

            hit.SendMessage("TakeDamage", damage);
        }
    }
}