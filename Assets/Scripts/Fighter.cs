using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // Public fields
    public int hitPoint = 10;
    public int maxHitPoint = 10;
    public float pushRecoverySpeed = 0.2f;
    
    // Immunity
    protected float ImmuneTime = 1.0f;
    protected float LastImmuneTime;
    
    // Push
    protected Vector3 PushDirection;
    
    protected virtual void TakeDamage(Damage damage)
    {
        if (!(Time.time - LastImmuneTime > ImmuneTime)) return;
        
        LastImmuneTime = Time.time;
        hitPoint -= damage.DamageAmount;
        PushDirection = (transform.position - damage.Origin).normalized * damage.PushForce;

        var fontSize = damage.IsCriticalDamage ? 25 : 15;
        var message = damage.IsCriticalDamage ? "Critical!" : damage.DamageAmount.ToString();
        
        GameManager.Instance.ShowText(message, fontSize, Color.red, gameObject.transform.position, Vector3.up * 100, 0.5f);
            
        if (hitPoint <= 0)
            Death();
    }

    protected virtual void Death()
    {
        hitPoint = 0;
    }
}
