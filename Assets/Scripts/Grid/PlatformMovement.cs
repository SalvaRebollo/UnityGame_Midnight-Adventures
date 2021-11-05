using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public Transform limite1;
    public Transform limite2;
    public float velocidad;
    private Transform posicionSiguiente;

    void Start()
    {
        // Inicializamos cual va a ser el primer destino
        posicionSiguiente = limite1;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, posicionSiguiente.position, velocidad * Time.deltaTime);
        // Si la plataforma llega al limite1, cambio a la posicionSiguiente
        if (transform.position == limite1.position)
            posicionSiguiente = limite2;

        if (transform.position == limite2.position)
            posicionSiguiente = limite1;

    }
}
