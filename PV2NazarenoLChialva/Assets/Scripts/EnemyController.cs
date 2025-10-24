using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Configuración General")]
    public float experienciaOtorgada = 10f;
    public Transform player; 
    public float detectRadius = 5.0f;
    public float speed = 2.0f;
    public float attackRadius = 1.0f;
    public float attackInterval = 2.0f;
    public float puntosDeDanio = 2.0f;

    [Header("Física y Suelo")]
    public LayerMask capaSuelo;
    public float longitudRaycast = 0.79f;

    [Header("Pooling")]
    public string poolTag = "Goblin";
    

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clipAtaque;
    public AudioClip clipDanio;

    
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
    private float escalaBaseX; 
    private float escalaBaseY; 

    private Jugador jugadorScript;
    private float vidaMaxima; 

    [Header("Vida del Enemigo")]
    [SerializeField] private float vida = 10f;

    void Awake()
    {
       
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        vidaMaxima = vida; 

        
        escalaBaseX = transform.localScale.x;
        escalaBaseY = transform.localScale.y;

      
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        
        if (player == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null) player = playerGO.transform;
        }
        if (player != null && jugadorScript == null)
        {
            jugadorScript = player.GetComponent<Jugador>();
        }

        this.enabled = true;
        vida = vidaMaxima;
        tiempoSiguienteAtaque = 0f;
        estaAtacando = false;
        recibiendoDanio = false;
        EnMovimiento = false;

        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<Collider2D>().isTrigger = false;
        }

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (animator != null)
        {
            animator.SetBool("Muerto", false);
            animator.SetBool("Atacado", false);
            animator.SetBool("EnMovimiento", false);
            animator.SetBool("IsFalling", false);
            animator.Play("Idle");
        }
    }

    void Update()
    {
       
        if (player == null || jugadorScript == null || !jugadorScript.EstasVivo())
        {
            EnMovimiento = false;
            movement = Vector2.zero;
            if (animator != null) animator.SetBool("EnMovimiento", false);
            return; 
        }

        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
        enSuelo = hit.collider != null;
        estaCayendo = !enSuelo && rb.linearVelocity.y < -0.1f;
        animator.SetBool("IsFalling", estaCayendo);

        
        if (estaCayendo || estaAtacando || recibiendoDanio)
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
                transform.localScale = new Vector3(escalaBaseX, escalaBaseY, 1f);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-escalaBaseX, escalaBaseY, 1f);
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
                
                PlayerController pc = player.GetComponent<PlayerController>();
                if (pc != null) pc.RecibirDanio(puntosDeDanio);

                animator.SetTrigger("Ataque");
                if (audioSource != null && clipAtaque != null) audioSource.PlayOneShot(clipAtaque);

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
        yield return new WaitForSeconds(duracionAtaque * 0.9f);
        estaAtacando = false;
    }

    public void RecibirDanio(float puntos)
    {
        if (!recibiendoDanio)
        {
            if (audioSource != null && clipDanio != null) audioSource.PlayOneShot(clipDanio);

            vida -= puntos;
            Debug.Log($"Vida del {gameObject.name}: {vida}");

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
        yield return new WaitForSeconds(duracionDanio * 0.9f);
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
        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = false;

        this.enabled = false; 

        StartCoroutine(EsperarMuerteYDevolverAPool());
    }

    private IEnumerator EsperarMuerteYDevolverAPool()
    {
        yield return new WaitForSeconds(1.0f);

        if (ObjectPooler.Instance != null)
        {
            
            ObjectPooler.Instance.ReturnObjectToPool(poolTag, gameObject);
        }
    }

    void FixedUpdate()
    {
        if (!this.enabled) return;

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