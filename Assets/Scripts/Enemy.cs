using System;
using System.Collections;
using System.Collections.Generic;
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

    protected override void Start()
    {
        base.Start();
        _playerTransform = GameManager.Instance.player.transform;
        _startPosition = transform.position;
        _hitBox = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
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
        _hitBox.OverlapCollider(filter, _hits);
        for (var index = 0; index < _hits.Length; index++)
        {
            var hit = _hits[index];
            if (hit == null) continue;

            if (hit.CompareTag("Player"))
            {
                _collidingWithPlayer = true;
            }

            _hits[index] = null;
        }
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