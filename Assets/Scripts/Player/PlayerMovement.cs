using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement

    [HideInInspector]
    public float lastHorizontalVector;

    [HideInInspector]
    public float lastVerticalVector;

    [HideInInspector]
    public Vector2 moveDir;

    [HideInInspector]
    public Vector2 lastMoveVector;

    //references
    Rigidbody2D rb;
    
    PlayerStats player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        lastMoveVector = new Vector2(1, 0f); //Need to do this so that when the game starts and the player does not move, the projectile will have no momentum
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
        
    }

    void FixedUpdate()
    {
        Move();
    }

    void InputManager()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMoveVector = new Vector2(lastHorizontalVector, 0f); //last moved x
        }
        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
            lastMoveVector = new Vector2(0f, lastHorizontalVector); // last moved y
        }

        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMoveVector = new Vector2(lastHorizontalVector, lastVerticalVector); // while moving diagonally
        }
    }
    void Move()
    {
        rb.linearVelocity = new Vector2(moveDir.x * player.CurrentMoveSpeed, moveDir.y * player.CurrentMoveSpeed);
    }
}
