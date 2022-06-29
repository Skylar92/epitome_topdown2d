using UnityEngine;

public sealed class Player : Mover
{
    public int level = 1;

    private SpriteRenderer _spriteRenderer;
    private Weapon _weapon;

    public override void Awake()
    {
        base.Awake();
        _weapon = GetComponentInChildren<Weapon>();
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
        
        GameManager.Instance.ShowText("Level UP!", 36, new Color(0.47f, 1f, 0.4f), gameObject.transform.position, Vector3.up * 100, 1f);
    }

    public void OnPlayerLoaded()
    {
        // Reset if player have been killed
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        _weapon.Show();
        // a bit god help
        isAlive = true;
        
        maxHitPoint = InitialMaxHitPoint + level * 2;
        hitPoint = maxHitPoint;
    }

    protected override void Death()
    {
        base.Death();
        isAlive = false;
        // just pretend to be dead for epic
        gameObject.transform.rotation = new Quaternion(0f, 0f, 45f, 0f);
        _weapon.Hide();
        
        GameManager.Instance.OnPlayerDead();
    }
}