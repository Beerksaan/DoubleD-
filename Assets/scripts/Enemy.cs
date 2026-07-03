using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] points;

    private int currentPoint;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (points.Length == 0)
        {
            enabled = false;
            return;
        }
    }

    void Update()
    {
        Transform target = points[currentPoint];

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime);

        spriteRenderer.flipX = target.position.x < transform.position.x;

        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            currentPoint = (currentPoint + 1) % points.Length;
        }
    }
}