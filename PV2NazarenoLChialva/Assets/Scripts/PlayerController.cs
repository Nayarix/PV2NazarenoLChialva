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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jugador = GetComponent<Jugador>(); 
    }

    void Update()
    {
        
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

        if (Input.GetKeyDown(KeyCode.Z) && !estaAtacando && EnSuelo)
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
            GetComponent<Jugador>().ModificarVida(-puntos);

            if (!jugador.EstasVivo())
            {
                animator.SetBool("Muerto", true);
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}