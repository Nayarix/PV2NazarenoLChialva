using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5.0f;
    public Animator animator;
    public float FuezaDeSalto = 10.0f;
    public float longitudRaycast = 0.1f;
    public LayerMask capaSuelo;
    public float gravedadSalto = 3.0f;
    private bool EnSuelo;

    // Nuevo: para controlar el estado de daño
    public float tiempoInvencibilidad = 0.5f;
    private bool recibiendoDanio = false;

    private Rigidbody2D rb;

    private float escalaX = 0.08692736f;
    private float escalaY = 0.0876511f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float velocidadX = Input.GetAxis("Horizontal");
        transform.position += new Vector3(velocidadX * velocidad * Time.deltaTime, 0, 0);
        animator.SetFloat("movement", Mathf.Abs(velocidadX));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
        EnSuelo = hit.collider != null;

        if (EnSuelo && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0f, FuezaDeSalto), ForceMode2D.Impulse);
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

    // Nuevo: Lógica para recibir daño
    public void RecibirDanio(float puntos)
    {
        if (!recibiendoDanio)
        {
            // Llama a la función del script Jugador para modificar la vida
            GetComponent<Jugador>().ModificarVida(-puntos);

            // Activa el booleano del Animator para la animación de daño
            animator.SetBool("RecibeDanio", true);

            // Inicia la Coroutine de invencibilidad y reseteo del Animator
            StartCoroutine(DanioCorrutina());
        }
    }

    private IEnumerator DanioCorrutina()
    {
        recibiendoDanio = true;

        // Espera un tiempo para la animación de daño
        yield return new WaitForSeconds(tiempoInvencibilidad);

        // Desactiva la animación y el estado de invencibilidad
        animator.SetBool("RecibeDanio", false);
        recibiendoDanio = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}