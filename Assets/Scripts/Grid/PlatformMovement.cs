using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public Transform limite1;
    public Transform limite2;
    public float velocidad;
    private Transform posicionSiguiente;
    private bool playerOnPlatform = false;

    public string playerTag = "Player"; // Tag del objeto del jugador
    public float distanceThreshold = 1f; // Distancia máxima a la que se considera que un objeto está a la izquierda o a la derecha del jugador

    private GameObject playerObject; // Referencia al objeto del jugador

    void Start()
    {
        // Obtener el objeto del jugador por el tag
        playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject == null)
        {
            Debug.LogError("No se pudo encontrar un objeto con el tag '" + playerTag + "'.");
        }

        // Inicializamos cual va a ser el primer destino
        posicionSiguiente = limite1;
    }

    void Update()
    {

        
        if (playerOnPlatform)
        {
            transform.position = Vector3.MoveTowards(transform.position, posicionSiguiente.position, velocidad * Time.deltaTime);
            // Si la plataforma llega al limite1, cambio la posicionSiguiente a que llege a limite2
            if (transform.position == limite1.position)
                posicionSiguiente = limite2;

            if (transform.position == limite2.position)
                posicionSiguiente = limite1;
        } else
        {
            CheckPosition();
        }
        

    }

    private void CheckPosition()
    {
        if (playerObject == null) return;

        // Obtener la posición relativa del objeto con respecto al jugador
        Vector3 relativeDirection = playerObject.transform.InverseTransformDirection(transform.position - playerObject.transform.position);
        Debug.Log("relativePosition.x: " + relativeDirection.x);
        // Determinar si el objeto está a la izquierda o a la derecha del jugador
        if (relativeDirection.x > distanceThreshold)
        {
            Debug.Log("El objeto está a la derecha del jugador.");
            // Agregar aquí cualquier acción que desees ejecutar cuando el objeto está a la derecha del jugador.
            posicionSiguiente = limite1;
            transform.position = Vector3.MoveTowards(transform.position, posicionSiguiente.position, velocidad * Time.deltaTime);
        }
        else if (relativeDirection.x < -distanceThreshold)
        {
            Debug.Log("El objeto está a la izquierda del jugador.");
            // Agregar aquí cualquier acción que desees ejecutar cuando el objeto está a la izquierda del jugador.
            posicionSiguiente = limite2;
            transform.position = Vector3.MoveTowards(transform.position, posicionSiguiente.position, velocidad * Time.deltaTime);
        }



  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("El jugador ha subido a la plataforma.");
            // Agrega aquí cualquier acción que desees ejecutar cuando el jugador sube a la plataforma.
            playerOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("El jugador ha salido de la plataforma.");
            // Agrega aquí cualquier acción que desees ejecutar cuando el jugador sale de la plataforma.
            playerOnPlatform = false;
        }
    }
}
