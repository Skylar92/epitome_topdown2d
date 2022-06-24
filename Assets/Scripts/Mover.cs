using UnityEngine;

public abstract class Mover : Fighter
{
    public string[] LayerNames;
    public float ySpeed = .75f;
    public float xSpeed = 1.0f;

    private static readonly Vector3 ToRight = Vector3.one;
    private  static readonly Vector3 ToLeft = new(-1, 1, 1);
    
    protected BoxCollider2D BoxCollider2D;
    protected RaycastHit2D Hit;


    protected virtual void Start()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }
    
    protected virtual void UpdateMotor(Vector3 input)
    {
        // Calculate direction
        CalculateDirection(input.x);
        
        // Do movement
        CalculateMovement(input);
    }

    protected virtual void CalculateDirection(float x)
    {
        transform.localScale = x switch
        {
            // Swap sprite direction
            > 0 => ToRight,
            < 0 => ToLeft,
            _ => transform.localScale
        };
    }

    protected virtual void CalculateMovement(Vector3 movementDelta)
    {
        PushDirection = Vector3.Lerp(PushDirection, Vector3.zero, pushRecoverySpeed);
        movementDelta += PushDirection;

        Hit = Physics2D.BoxCast(transform.position,
            BoxCollider2D.size, 0, new Vector2(0, movementDelta.y), Mathf.Abs(movementDelta.y * Time.deltaTime), LayerMask.GetMask(LayerNames));

        if (!Hit)
        {
            transform.Translate(0, movementDelta.y * Time.deltaTime * ySpeed, 0);
        }
        
        Hit = Physics2D.BoxCast(transform.position,
            BoxCollider2D.size, 0, new Vector2(movementDelta.x, 0), Mathf.Abs(movementDelta.x * Time.deltaTime), LayerMask.GetMask(LayerNames));

        if (!Hit)
        {
            transform.Translate(movementDelta.x * Time.deltaTime * xSpeed, 0, 0);
        }
    }
}
