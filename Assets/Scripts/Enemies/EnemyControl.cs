using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public float velocidad;
    public Vector3 posicionFin;
    public int health = 3;

    private Vector3 posicionInicio;
    private SpriteRenderer sprite;
    private bool moviendoAFin;
    private int aleatorio;
    private bool invencible;
    private float cooldownTime;
    public float startDazedTime;
    public GameObject bloodEffect;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        //Inicializala propiedad de inicio del enemigo
        posicionInicio = transform.position;
        //Posiciona al enemigo de forma aleatoria entre limites de inicio y fin
        //transform.position = new Vector3(Random.Range(transform.position.x, posicionFin.x),
        //                                Random.Range(transform.position.y, posicionFin.y),
        //                                0);

        //De manera aleatoria elegimos una direccion
        //Si es mayor de 0,5 dará true y si es menor o igual false
        moviendoAFin = (Random.value > 0.5f);
    }

    private void Update()
    {
        if (cooldownTime <= 0)
        {
            if ((health <= 1 && gameObject.CompareTag("Skeleton")) ||
                (health <= 3 && gameObject.CompareTag("GiantSkeleton")) ||
                (health <= 1 && gameObject.CompareTag("GhostHalo")) ||
                (health <= 1 && gameObject.CompareTag("Ghost")))
            { 
                velocidad = 5;
                return;
            }
            velocidad = 3;
        } else
        {
            velocidad = 0;
            cooldownTime -= Time.deltaTime; 
        }

        if (health == 0)
            Destroy(gameObject);
        
    }

    private void FixedUpdate()
    {
        MoverEnemigo();
    }

    private void MoverEnemigo()
    {
        //Calcula la posicion destino en funcion a la direccion
        Vector3 posicionDestino = (moviendoAFin) ? posicionFin : posicionInicio;
        //Mueve hacia el destino
        transform.position = Vector3.MoveTowards(transform.position,
                posicionDestino, velocidad * Time.deltaTime);
        //Cambia la direccion cuando toque los limites
        if (transform.position == posicionFin)
        {
            moviendoAFin = false;
        }

        if (transform.position == posicionInicio)
        {
            moviendoAFin = true;
        }
    }

    //Listener/Evento cuando un objeto entra y permanece en el area de colision del enemigo
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Compara el objeto que entra en el area de colision con el tag del player
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerControl>().MakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        cooldownTime = startDazedTime;
        //play hurt sound
        //instantiate blood effect
        //bloodEffect.Play();
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        //Destroy(bloodEffect);
        
        health -= damage;
        Debug.Log("DAMAGE TAKEN !");
        sprite.color = Color.red;
        Invoke("ChangeSpriteToDefaultColor", 1.0f);
    }
    
    private void ChangeSpriteToDefaultColor()
    {
        sprite.color = Color.white;
    }

}
