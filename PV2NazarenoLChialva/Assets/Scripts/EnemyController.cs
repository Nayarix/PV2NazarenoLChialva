using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float detectRadius = 5.0f;
    public float speed = 2.0f;
    public float attackRadius = 1.0f;
    public float attackInterval = 2.0f;
    public float puntosDeDanio = 2.0f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool EnMovimiento;
    private Animator animator;
    private bool recibiendoDanio;
    private bool enRangoDeAtaque;
    private float tiempoSiguienteAtaque;
    private bool estaAtacando;

    private Jugador jugadorScript;

    [Header("Vida del Enemigo")]
    [SerializeField] private float vida = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        tiempoSiguienteAtaque = 0f;
        estaAtacando = false;

        jugadorScript = player.GetComponent<Jugador>();
    }

    void Update()
    {
        

        if (!jugadorScript.EstasVivo())
        {
            EnMovimiento = false;
            movement = Vector2.zero;
            animator.SetBool("EnMovimiento", EnMovimiento);
            animator.SetBool("Atacado", false);
            return;
        }

        if (estaAtacando || recibiendoDanio)
        {
            movement = Vector2.zero;
            EnMovimiento = false;
            animator.SetBool("EnMovimiento", false);
            return;
        }

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
            animator.SetBool("Atacado", false);
        }
        else if (enRangoDeAtaque)
        {
            movement = Vector2.zero;
            EnMovimiento = false;
            animator.SetBool("Atacado", false);

            if (Time.time >= tiempoSiguienteAtaque)
            {
                player.GetComponent<PlayerController>().RecibirDanio(puntosDeDanio);
                animator.SetTrigger("Ataque");
                Debug.Log("ENEMIGO ATACA AL JUGADOR");
                tiempoSiguienteAtaque = Time.time + attackInterval;

                estaAtacando = true;
                StartCoroutine(EsperarFinAtaque());
            }
        }
        else
        {
            movement = Vector2.zero;
            EnMovimiento = false;
            animator.SetBool("Atacado", false);
        }

        animator.SetBool("EnMovimiento", EnMovimiento);
    }

    private IEnumerator EsperarFinAtaque()
    {
        yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float duracionAtaque = stateInfo.length;

        yield return new WaitForSeconds(duracionAtaque);

        estaAtacando = false;
    }

    public void RecibirDanio(float puntos)
    {
        if (!recibiendoDanio)
        {
            vida -= puntos;
            Debug.Log("Vida del enemigo: " + vida);

            if (vida > 0)
            {
                animator.SetBool("Atacado", true);
                recibiendoDanio = true;

                StartCoroutine(RecuperarseDeDanio());
            }
            else
            {
                Morir();
            }
        }
    }

    private IEnumerator RecuperarseDeDanio()
    {
        yield return null;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float duracionDanio = stateInfo.length;

        yield return new WaitForSeconds(duracionDanio);

        animator.SetBool("Atacado", false);
        recibiendoDanio = false;
    }

    private void Morir()
    {
        
        animator.SetBool("Muerto", true);

       
        rb.bodyType = RigidbodyType2D.Static; 
        GetComponent<Collider2D>().enabled = false; 
        this.enabled = false; 
    }

    void FixedUpdate()
    {
        if (!recibiendoDanio && !estaAtacando && !animator.GetBool("Atacado"))
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }

    public bool PuedeMoverse()
    {
        return !recibiendoDanio && !estaAtacando && !animator.GetBool("Atacado");
    }
}