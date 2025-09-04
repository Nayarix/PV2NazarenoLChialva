using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float detectRadius = 5.0f;
    public float speed = 2.0f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool EnMovimiento;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            if (direction.x > 0)
            {
                transform.localScale = new Vector3(0.08692736f, 0.0876511f, 1f);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-0.08692736f, 0.0876511f, 1f);
            }

            movement = new Vector2(direction.x, 0);
            EnMovimiento = true;
        }
        else
        {
            movement = Vector2.zero;
            EnMovimiento = false;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        animator.SetBool("EnMovimiento", EnMovimiento);
    }
}