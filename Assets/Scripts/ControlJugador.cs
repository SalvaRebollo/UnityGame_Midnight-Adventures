using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlJugador : MonoBehaviour
{

    public int velocidad;
    public int fuerzaSalto;
    public int numVidas;
    public int tiempoNivel;

    public int puntuacion;

    public Canvas canvas;

    [Header("Sonidos")]
    public AudioClip saltoSfx;
    public AudioClip vidaSfx;
    public AudioClip recolectarSfx;

    [Header("SaltoPared")]
    public LayerMask capaPlataformaColisionable;
    public float tiempoSaltoPared = 0.2f;
    private float tiempoSalto;
    public float velocidadSaltoPared = -0.3f;
    public float distanciaSaltoPared = 0.5f;
    private bool estaDeslizando = false;
    private RaycastHit2D compruebaTocaPared;
    private bool miraDerecha;
    private float valorHorizontal;

    //Cayendo
    private float velocidadY;        
    private bool estaCayendo = false;  


    private Rigidbody2D fisica;
    private SpriteRenderer sprite;
    private Animator animacion;
    private bool vulnerable;
    private float tiempoInicio;
    private int tiempoEmpleado;
    //private ControlHUD hud;
    private AudioSource audiosource;

    //private ControlDatosJuego datosJuego;

    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        tiempoInicio = Time.time;
        
        vulnerable = true;
        fisica = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animacion = GetComponent<Animator>();
        //hud = canvas.GetComponent<ControlHUD>();
        miraDerecha = true;

        //datosJuego = GameObject.Find("DatosJuego").GetComponent<ControlDatosJuego>();

        velocidadY = fisica.velocity.y;

        //Actualiza HUD
        //hud.SetVidasTxt(numVidas);
        //hud.SetObjetosTxt(GameObject.FindGameObjectsWithTag("PowerUp").Length);
    }

    private void FixedUpdate()
    {
        //Movimiento de teclas de entrada
        float entradaX = Input.GetAxis("Horizontal");
        fisica.velocity = new Vector2(entradaX * velocidad, fisica.velocity.y);

        valorHorizontal = Input.GetAxis("Horizontal") * velocidad * Time.deltaTime;

        //Flip correcto
        if ((valorHorizontal < 0 && miraDerecha) || (valorHorizontal > 0 && !miraDerecha))
        {
            miraDerecha= !miraDerecha;
            sprite.flipX = !miraDerecha;
        }


        //Direccion dcha
        if (miraDerecha)
        {
            compruebaTocaPared = Physics2D.Raycast(transform.position, new Vector2(distanciaSaltoPared, 0), distanciaSaltoPared, capaPlataformaColisionable);
            Debug.DrawRay(transform.position, new Vector2(distanciaSaltoPared, 0), Color.green);
        }
        //Izda
        else
        {
            compruebaTocaPared = Physics2D.Raycast(transform.position, new Vector2(-distanciaSaltoPared, 0), distanciaSaltoPared, capaPlataformaColisionable);
            Debug.DrawRay(transform.position, new Vector2(-distanciaSaltoPared, 0), Color.green);

        }

        //Si esta tocando pared , no toca suelo y no esta moviendose
        //y mantiene el deslizarse solo un pequeño tiempo
        if (compruebaTocaPared && !TocarSuelo() && entradaX != 0 )
        {
            estaDeslizando = true;
            tiempoSalto = Time.time * tiempoSaltoPared;
        }else if (tiempoSalto < Time.time)
        { 
            estaDeslizando = false;
        }

        //Si esta deslizando decrementa la velocidad de caida
        //a a una misma velocidad mínima
        if (estaDeslizando)
        {
            fisica.velocity = new Vector2(fisica.velocity.x, Mathf.Clamp(fisica.velocity.y, velocidadSaltoPared, float.MaxValue));             
            
        }



    }

    private void Update()
    {
       
        if (velocidadY > 0)
        {
            estaCayendo= false;
        }
        
        if (velocidadY < 0)
        {
            estaCayendo = true;
        }

        velocidadY = fisica.velocity.y;

        
        //Salto
        if (TocarSuelo() && Input.GetKeyDown(KeyCode.Space) || estaDeslizando && Input.GetKeyDown(KeyCode.Space))
        {        
            fisica.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            //Reproduce el sonido del salto
            audiosource.PlayOneShot(saltoSfx);
        }

        animarJugador();

        if (GameObject.FindGameObjectsWithTag("PowerUp").Length == 0)
            GanarJuego();

        //Actualiza tiempo Empleado
        tiempoEmpleado = (int)(Time.time - tiempoInicio);
        //hud.SetTiempoTxt(tiempoNivel - tiempoEmpleado);// Txt Cuenta atras

        //comprueba si hemos consumido el tiempo del nivel
        if (tiempoNivel - tiempoEmpleado < 0) FinJuego();

    }

    private void GanarJuego()
    {        
        puntuacion = (numVidas * 100) + (tiempoNivel - tiempoEmpleado);
        Debug.Log("You WIN!!!" + puntuacion);
        //datosJuego.Ganado = true;
        //datosJuego.Puntuacion = puntuacion;
        SceneManager.LoadScene("FinNivel");
    }

    private void animarJugador()
    {

        //Jugador Saltando hacia arriba
        if (!TocarSuelo() && !estaCayendo && !estaDeslizando) animacion.Play("jugadorSaltando");
        //Jugador Saltando cayendo
        else if (!TocarSuelo() && estaCayendo || estaDeslizando) animacion.Play("jugadorCayendo");
        //Jugador Corriendo
        else if ((fisica.velocity.x > 1 || fisica.velocity.x < -1) && fisica.velocity.y == 0) animacion.Play("jugadorCorriendo");
        //Jugador Parado
        else if ((fisica.velocity.x < 1 || fisica.velocity.x > -1) && fisica.velocity.y == 0) animacion.Play("jugadorParado");
        
    }

    private bool TocarSuelo()
    {
        RaycastHit2D toca = Physics2D.Raycast(transform.position + new Vector3(0,-2f,0) , Vector2.down, 0.2f);  
        return toca.collider != null;
    }

    public void FinJuego()
    {
        //datosJuego.Ganado = false;
        //datosJuego.Puntuacion = puntuacion;
        SceneManager.LoadScene("FinNivel");
    }


    public void IncrementarPuntos(int cantidad)
    {
        puntuacion += cantidad;
    }

    public void QuitarVida()
    {
        if (vulnerable) 
        {
            audiosource.PlayOneShot(vidaSfx);
            vulnerable = false;
            numVidas--;
            //hud.SetVidasTxt(numVidas);//Actualiza HUD
            if (numVidas == 0) FinJuego();
            Invoke("HacerVulnerable", 1f);
            sprite.color = Color.red;
        }
    }

    private void HacerVulnerable()
    {
        vulnerable = true;
        sprite.color = Color.white;
    }

    public void CogerObjeto()
    {
        audiosource.PlayOneShot(recolectarSfx);
        Invoke("ActualizaHUDObjeto", 0.2f);        
    }

    private void ActualizaHUDObjeto()
    {
        //hud.SetObjetosTxt(GameObject.FindGameObjectsWithTag("PowerUp").Length);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag =="PlataformaMovil")
        {
            transform.parent = collision.gameObject.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlataformaMovil")
        {
            transform.parent = null;
        }
    }

}
