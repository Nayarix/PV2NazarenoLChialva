using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    
    [Header("Configuracion")]
    [SerializeField] float velocidad = 5f;

    
    private float moverHorizontal;
    private Vector2 direccion;

    
    private Rigidbody2D miRigidbody2D;

    
    private void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
    }

    
    private void Update()
    {
        moverHorizontal = Input.GetAxis("Horizontal");
        direccion = new Vector2(moverHorizontal, 0f);
    }
    private void FixedUpdate()
    {
        miRigidbody2D.AddForce(direccion * velocidad);
    }
}