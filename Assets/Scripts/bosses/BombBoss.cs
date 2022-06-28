using helper;
using UnityEngine;

using Random = UnityEngine.Random;

public class BombBoss : Enemy
{
    public float attackInterval = 1.75f;
    public Bomb bomb;
    public Bomb superBomb;

    private const float MovementTime = 5.0f;
    private const float GameScale = 0.16f;

    private float _lastAttackTime;
    private float _lastRandomMovementTime;
    private Vector3 _newPosition;

    private bool _playerInAttackRadius;
    private Vector3 _lastPlayerTransform;
    private Vector3 _lastPlayerTransformToAttack;

    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _attackRadiusCollider;

    private Collidable2DService _collidable2DService;

    protected override void Start()
    {
        base.Start();
        _lastAttackTime = Time.time;
        _newPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _attackRadiusCollider = GetComponentsInChildren<CircleCollider2D>()[0];
        _collidable2DService = new Collidable2DService(_attackRadiusCollider, filter);
        _collidable2DService.OnPlayerCollideEvent += OnPlayerCollide;
    }

    private void OnPlayerCollide(Collider2D player)
    {
        _playerInAttackRadius = true;
        var playerPosition = player.transform.position;
        _lastPlayerTransform = playerPosition;
        if (CanAttack())
            _lastPlayerTransformToAttack = playerPosition;
    }

    protected override BoxCollider2D GetHitBox()
    {
        return GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _playerInAttackRadius = false;
        _collidable2DService.CheckCollider();
    }

    protected override void FixedUpdate()
    {
        CalculateRandomMovement();

        if (transform.position != _newPosition && !_playerInAttackRadius)
        {
            UpdateMotor(_newPosition);
        }

        if (_playerInAttackRadius)
        {
            _spriteRenderer.color = Color.red;
            CalculateDirection(_lastPlayerTransform.x);
            DoAttack();
        }
        else
        {
            _spriteRenderer.color = Color.white;
        }
    }

    private void DoAttack()
    {
        if (!CanAttack()) return;

        _lastAttackTime = Time.time;

        if (Random.Range(0f, 1f) > 0.8)
            DoSuperAttack();
        else
            DoBaseAttack();
    }

    private void DoBaseAttack()
    {
        DoAttack(bomb, _lastPlayerTransformToAttack);
    }

    private void DoSuperAttack()
    {
        for (var i = 0; i < 3; i++)
        {
            var insideUnitCircle = Random.insideUnitCircle * (GameScale * 4);
            var targetDest = new Vector3(_lastPlayerTransformToAttack.x + insideUnitCircle.x,
                _lastPlayerTransformToAttack.y + insideUnitCircle.y, 0);
            DoAttack(superBomb, targetDest);
        }
    }

    private void DoAttack(Bomb bombObject, Vector3 end)
    {
        var position = transform.position;
        var instantiate = Instantiate(bombObject, position, Quaternion.identity);
        instantiate.start = position;
        instantiate.end = end;
        instantiate.speed = 1.0f;
    }

    private bool CanAttack()
    {
        return Time.time - _lastAttackTime > attackInterval;
    }

    protected override void TakeDamage(Damage damage)
    {
        base.TakeDamage(damage);
        var pushDirection = PushDirection;
        pushDirection.x = _lastPlayerTransform.x;
        UpdateMotor(pushDirection);
    }


    private void CalculateRandomMovement()
    {
        if (!(Time.time - _lastRandomMovementTime > MovementTime) || _playerInAttackRadius) return;

        _lastRandomMovementTime = Time.time;

        var insideUnitCircle = Random.insideUnitCircle * GameScale;
        _newPosition = new Vector3(insideUnitCircle.x, insideUnitCircle.y, 0);
    }
}