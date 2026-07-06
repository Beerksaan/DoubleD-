using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] points;

    private int currentPoint = 0;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (points.Length < 2)
        {
            enabled = false;
            return;
        }

        rb.position = points[0].position;
        currentPoint = 1;
    }

    void FixedUpdate()
    {
        Vector2 newPos = Vector2.MoveTowards(
            rb.position,
            points[currentPoint].position,
            speed * Time.fixedDeltaTime);

        rb.MovePosition(newPos);

        spriteRenderer.flipX = newPos.x > points[currentPoint].position.x;

        if (Vector2.Distance(rb.position, points[currentPoint].position) < 0.1f)
        {
            currentPoint = (currentPoint + 1) % points.Length;
        }
    }
}