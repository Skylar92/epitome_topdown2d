using System;
using System.Collections;
using System.Collections.Generic;
using helper;
using UnityEngine;

public class Enemy : Mover
{
    // Experience
    public int xpValue = 1;
    public ContactFilter2D filter;

    // Logic
    public float triggerLength = 1;
    public float chaseLength = 5;

    private bool _chasing;
    private bool _collidingWithPlayer;
    private Transform _playerTransform;
    private Vector3 _startPosition;

    // Hitbox
    private BoxCollider2D _hitBox;
    private Collider2D[] _hits = new Collider2D[10];

    private Collidable2DService _collidable2DService;

    protected override void Start()
    {
        base.Start();
        _playerTransform = GameManager.Instance.player.transform;
        _startPosition = transform.position;
        _hitBox = GetHitBox();
        _collidable2DService = new Collidable2DService(_hitBox, filter);
        _collidable2DService.OnPlayerCollideEvent += OnCollidingWithPlayer;
    }

    protected virtual BoxCollider2D GetHitBox()
    {
        return transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    protected virtual void FixedUpdate()
    {
        // Is the player in the range
        if (Vector3.Distance(_playerTransform.position, _startPosition) < chaseLength)
        {
            if (Vector3.Distance(_playerTransform.position, _startPosition) < triggerLength)
            {
                _chasing = true;
            }


            if (_chasing)
            {
                if (!_collidingWithPlayer)
                {
                    UpdateMotor((_playerTransform.position - transform.position).normalized);
                }
            }
            else
            {
                UpdateMotor(_startPosition - transform.position);
            }
        }
        else
        {
            UpdateMotor(_startPosition - transform.position);
            _chasing = false;
        }

        // Check for overlaps
        _collidingWithPlayer = false;
        _collidable2DService.CheckCollider();
    }

    private void OnCollidingWithPlayer(Collider2D player)
    {
        _collidingWithPlayer = true;
    }

    protected override void CalculateDirection(float x)
    {
        base.CalculateDirection(-x);
    }

    protected override void Death()
    {
        base.Death();
        Destroy(gameObject);
        GameManager.Instance.GrandXp(xpValue);
        GameManager.Instance.ShowText($"+{xpValue} XP", 15, Color.magenta, gameObject.transform.position,
            Vector3.up * 75, 1.5f);
    }
}