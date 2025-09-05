using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float detectRadius = 5.0f;
    public float speed = 2.0f;
    public float attackRadius = 1.0f; 
    public float attackInterval = 1.0f; 
    public float puntosDeDanio = 5.0f; 

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool EnMovimiento;
    private Animator animator;
    private bool recibiendoDanio;
    private bool enRangoDeAtaque;
    private float tiempoSiguienteAtaque;

    [Header("Vida del Enemigo")]
    [SerializeField] private float vida = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        tiempoSiguienteAtaque = 0f;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        enRangoDeAtaque = distanceToPlayer <= attackRadius;

        if (distanceToPlayer < detectRadius && !enRangoDeAtaque)
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
        else if (enRangoDeAtaque)
        {
            movement = Vector2.zero;
            EnMovimiento = false;

            if (Time.time >= tiempoSiguienteAtaque)
            {
                
                player.GetComponent<PlayerController>().RecibirDanio(puntosDeDanio);

                Debug.Log("ENEMIGO ATACA AL JUGADOR");

                tiempoSiguienteAtaque = Time.time + attackInterval;
            }
        }
        else
        {
            movement = Vector2.zero;
            EnMovimiento = false;
        }

        animator.SetBool("EnMovimiento", EnMovimiento);
    }

    public void RecibirDanio(float puntos)
    {
        if (!recibiendoDanio)
        {
            vida -= puntos;
            Debug.Log("Vida del enemigo: " + vida);

            if (vida > 0)
            {
                animator.SetBool("RecibeDanio", true);
                StartCoroutine(DanioCorrutina());
            }
            else
            {
                // Lógica de muerte del enemigo
            }
        }
    }

    private IEnumerator DanioCorrutina()
    {
        recibiendoDanio = true;
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("RecibeDanio", false);
        recibiendoDanio = false;
    }

    void FixedUpdate()
    {
        if (!recibiendoDanio)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }
}