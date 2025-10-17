using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5.0f;
    public Animator animator;
    public float FuezaDeSalto = 30.0f;
    public float longitudRaycast = 0.79f;
    public LayerMask capaSuelo;
    public float gravedadSalto = 3.0f;
    private bool EnSuelo;
    private bool estaAtacando;

    public float tiempoInvencibilidad = 0.5f;
    private bool recibiendoDanio = false;

    private Rigidbody2D rb;
    private Jugador jugador;

    private float escalaX = 0.08692736f;
    private float escalaY = 0.0876511f;

    [Header("Audio")]
    public AudioSource audioSource; 
    public AudioClip clipAtaque;    
    public AudioClip clipDanio;     

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jugador = GetComponent<Jugador>();
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

    }

    void Update()
    {
        if (!jugador.EstasVivo())
        {
            if (animator.GetBool("Muerto"))
            {
                animator.SetBool("Muerto", false);
                animator.Play("Idle");
            }
            return;
        }

        if (!jugador.EstasVivo())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float velocidadX = Input.GetAxis("Horizontal");
        transform.position += new Vector3(velocidadX * velocidad * Time.deltaTime, 0, 0);
        animator.SetFloat("movement", Mathf.Abs(velocidadX));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
        EnSuelo = hit.collider != null;

        if (EnSuelo && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0f, FuezaDeSalto), ForceMode2D.Impulse);
        }

        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Mouse0)) && !estaAtacando && EnSuelo)
        {
            IniciarAtaque();
        }

        animator.SetBool("EnSuelo", EnSuelo);

        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = gravedadSalto;
        }
        else
        {
            rb.gravityScale = 1f;
        }

        if (velocidadX > 0)
        {
            transform.localScale = new Vector3(escalaX, escalaY, 1f);
        }
        else if (velocidadX < 0)
        {
            transform.localScale = new Vector3(-escalaX, escalaY, 1f);
        }


    }

    private void IniciarAtaque()
    {
        estaAtacando = true;
        animator.SetBool("Atacando", estaAtacando);

        if (audioSource != null && clipAtaque != null)
        {
            audioSource.PlayOneShot(clipAtaque);
        }
    }

    public void DesactivarAtaque()
    {
        estaAtacando = false;
        animator.SetBool("Atacando", estaAtacando);


    }

    public void RecibirDanio(float puntos)
    {
        if (!recibiendoDanio)
        {
            if (audioSource != null && clipDanio != null)
            {
                audioSource.PlayOneShot(clipDanio);
            }

            GetComponent<Jugador>().ModificarVida(-puntos);

            if (!jugador.EstasVivo())
            {
                animator.SetBool("Muerto", true);
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
                GetComponent<Collider2D>().enabled = false;
                this.enabled = false;
                StartCoroutine(RespawnDespuesDeMuerte());
            }
            else
            {
                animator.SetBool("RecibeDanio", true);
                StartCoroutine(DanioCorrutina()); 
            }
        }
    }

    
    private IEnumerator DanioCorrutina()
    {
        recibiendoDanio = true;
        yield return new WaitForSeconds(tiempoInvencibilidad);
        animator.SetBool("RecibeDanio", false);
        recibiendoDanio = false;
    }

    private IEnumerator RespawnDespuesDeMuerte()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<Jugador>().Respawn();
    }

    public void ReactivarJugador()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
        this.enabled = true;
        animator.SetBool("RecibeDanio", false);
        animator.SetBool("Atacando", false);
        animator.SetFloat("movement", 0f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}