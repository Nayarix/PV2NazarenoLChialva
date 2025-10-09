using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float experienciaOtorgada = 10f;

    public Transform player;
    public float detectRadius = 5.0f;
    public float speed = 2.0f;
    public float attackRadius = 1.0f;
    public float attackInterval = 2.0f;
    public float puntosDeDanio = 2.0f;

    public LayerMask capaSuelo;
    public float longitudRaycast = 0.79f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool EnMovimiento;
    private Animator animator;
    private bool recibiendoDanio;
    private bool enRangoDeAtaque;
    private float tiempoSiguienteAtaque;
    private bool estaAtacando;
    private bool enSuelo;
    private bool estaCayendo;

    private Jugador jugadorScript;
    private const string POOL_TAG = "Goblin";


    [Header("Vida del Enemigo")]
    [SerializeField] private float vida = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        tiempoSiguienteAtaque = 0f;
        estaAtacando = false;
        enSuelo = false;
        estaCayendo = false;

        if (player != null)
        {
            jugadorScript = player.GetComponent<Jugador>();
        }

    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
        enSuelo = hit.collider != null;
        estaCayendo = !enSuelo && rb.linearVelocity.y < -0.1f;

        animator.SetBool("IsFalling", estaCayendo);

        if (estaCayendo)
        {
            animator.SetBool("EnMovimiento", EnMovimiento);
            return;
        }

        if (jugadorScript == null || !jugadorScript.EstasVivo())
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
        ProgressionManager.Instance.AddExperience(experienciaOtorgada);

        animator.SetBool("Muerto", true);

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = false;
        }

        this.enabled = false;

        StartCoroutine(EsperarMuerteYDevolverAPool());
    }

    private IEnumerator EsperarMuerteYDevolverAPool()
    {
        yield return new WaitForSeconds(1.0f); 

        if (ObjectPooler.Instance != null)
        {
            ObjectPooler.Instance.ReturnObjectToPool(POOL_TAG, gameObject);
        }
    }

    void FixedUpdate()
    {
        if (!this.enabled)
        {
            return;
        }

        if (!recibiendoDanio && !estaAtacando)
        {
            rb.linearVelocity = new Vector2(movement.x * speed, rb.linearVelocity.y);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}