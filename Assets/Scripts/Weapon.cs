using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : Collidable
{
    public int damagePoint = 1;
    public float pushForce = 2.0f;
    public int weaponLevel = 1;
    public int criticalChance = 0;
    
    private SpriteRenderer _weaponSprite;

    private float _cooldown = 0.5f;
    private float _lastSwing;

    private Animator _animator;
    private static readonly int SwingAnimator = Animator.StringToHash("Swing");

    private void Awake()
    {
        _weaponSprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();

        var instanceWeapon = GameManager.Instance.weaponList[weaponLevel - 1];
        UpdateWeapon(instanceWeapon);

        ColliderListener.OnEnemyCollideEvent += OnWeaponHitEnemy;
    }

    public void UpdateWeapon(WeaponMeta weaponMeta)
    {
        damagePoint = weaponMeta.damagePoint;
        criticalChance = weaponMeta.criticalChance;
        _weaponSprite.sprite = weaponMeta.sprite;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!Input.GetKey(KeyCode.Space) || !(Time.time - _lastSwing > _cooldown)) return;
        
        _lastSwing = Time.time;
        Swing();
    }

    private void Swing()
    {
        _animator.SetTrigger(SwingAnimator);
    }

    private void OnWeaponHitEnemy(Collider2D hit)
    {
        var isCriticalDamage = Random.Range(0, 100) < criticalChance;
        var damage = new Damage()
        {
            DamageAmount = isCriticalDamage ? damagePoint * 2 : damagePoint,
            Origin = transform.position,
            PushForce = pushForce,
            IsCriticalDamage = isCriticalDamage
        };
            
        hit.SendMessage("TakeDamage", damage);
    }
}