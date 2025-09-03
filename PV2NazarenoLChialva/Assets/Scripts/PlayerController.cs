using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5.0f;
    public Animator animator;

    // Valores de la escala de mi personaje en el juego
    private float escalaX = 0.08692736f;
    private float escalaY = 0.0876511f;

    void Update()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");

     
        transform.position += new Vector3(inputHorizontal * velocidad * Time.deltaTime, 0, 0);

        
        animator.SetFloat("movement", Mathf.Abs(inputHorizontal));

        
        if (inputHorizontal > 0)
        {
            transform.localScale = new Vector3(escalaX, escalaY, 1f);
        }
        else if (inputHorizontal < 0)
        {
            transform.localScale = new Vector3(-escalaX, escalaY, 1f);
        }
    }
}