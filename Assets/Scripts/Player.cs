using UnityEngine;

public sealed class Player : Mover
{
    public int level = 1;

    private SpriteRenderer _spriteRenderer;

    public override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdatePlayerSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    private void FixedUpdate()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        
        UpdateMotor(new Vector3(x * xSpeed, y * ySpeed, 0));
    }

    public void OnLevelUp()
    {
        level += 1;
        maxHitPoint += 2;
        hitPoint = maxHitPoint;
    }

    public void OnPlayerLoaded()
    {
        maxHitPoint = InitialMaxHitPoint + level * 2;
        hitPoint = maxHitPoint;
    }
}