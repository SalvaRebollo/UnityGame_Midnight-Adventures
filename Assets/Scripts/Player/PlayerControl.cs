using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{

    public int velocidad;
    public int fuerzaSalto;
    public int coinAmount = 0;
    public int coinAmountToWin;
    public int health = 3;
    public Animator animacion;
    public int timeLevel;
    private float timeStart;
    private int timeExpended;
    

    private GameDataControl gameData;
    private Rigidbody2D fisica;
    private bool vulnerable;
    
    public Canvas canvas;
    private HUDControl hud;
    private SpriteRenderer sprite;
    private bool movRight;
    private bool movLeft;
    private bool movJump;
    public bool movAttack;
    public bool canUsePortals;
    private BoxCollider2D colliderAtaque;
    

    [Header("SaltoPared")]
    public LayerMask groundLayer;
    public float tiempoSaltoPared = 0.2f;
    private float tiempoSalto;
    public float sliceSpeed = -0.3f;
    public float distanceOfClimbWallRaycast = 1f;
    public int fuerzaSaltoAlDeslizarEnPared;
    private bool isSlice = false;
    private RaycastHit2D compruebaTocaPared;
    private bool miraDerecha;
    public   bool isAttack;
    public int score;

    public AudioClip jumpSfx;
    public AudioClip hurtSfx;
    public AudioClip coinSfx;

    private AudioSource audioSource;

    void Start()
    {
        timeStart = Time.time;
        vulnerable = true;
        canUsePortals = true;
        fisica = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animacion = GetComponent<Animator>();
        colliderAtaque = GetComponent<BoxCollider2D>();
        hud = canvas.GetComponent<HUDControl>();
        gameData = GameObject.Find("GameData").GetComponent<GameDataControl>();
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        float entradaX;

        if (movLeft)
            entradaX = -1;
        else if (movRight)
            entradaX = 1;
        else
            entradaX = Input.GetAxis("Horizontal");
        fisica.velocity = new Vector2(entradaX * velocidad, fisica.velocity.y);
        //Debug.Log("EntradaX: " + entradaX);
        //Debug.Log("fisica.velocity.y: " + fisica.velocity.y);
        // Si va a la derecha transform.localScale = new Vector3(1, 1, 1), si va a la izquierda transform.localScale = new Vector3(-1, 1, 1)
        if (entradaX != 0)
        {
            // Flip sprite and components (colliders)
            if (entradaX > 0)
            {
                if (transform.localScale.x < 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                miraDerecha = true;
            }
            else
            {
                if (transform.localScale.x > 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                miraDerecha = false;
            }
        }

        //Direccion dcha
        if (miraDerecha)
        {
            compruebaTocaPared = Physics2D.Raycast(transform.position, new Vector2(distanceOfClimbWallRaycast, 0), distanceOfClimbWallRaycast, groundLayer);
            Debug.DrawRay(transform.position, new Vector2(distanceOfClimbWallRaycast, 0), Color.green);
        }
        //Izda
        else
        {
            compruebaTocaPared = Physics2D.Raycast(transform.position, new Vector2(-distanceOfClimbWallRaycast, 0), distanceOfClimbWallRaycast, groundLayer);
            Debug.DrawRay(transform.position, new Vector2(-distanceOfClimbWallRaycast, 0), Color.green);
        }

        //Si esta tocando pared , no toca suelo y no esta moviendose
        //y mantiene el deslizarse solo un pequeño tiempo
        if (compruebaTocaPared && !IsPlayerOnGround() && entradaX != 0)
        {
            isSlice = true;
            tiempoSalto = Time.time * tiempoSaltoPared;
        }
        else if (tiempoSalto < Time.time)
        {
            isSlice = false;
        }

        //Si esta deslizando decrementa la velocidad de caida
        //a a una misma velocidad mínima
        if (isSlice)
        {
            fisica.velocity = new Vector2(fisica.velocity.x, Mathf.Clamp(fisica.velocity.y, sliceSpeed, float.MaxValue));

        }








        AnimatePlayer();
    }

    private void Update()
    {
        // Salto
        if (IsPlayerOnGround() && (Input.GetKeyDown(KeyCode.Space) || movJump))
        {
            audioSource.PlayOneShot(jumpSfx);
            movJump = false;
            fisica.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }

        //WallJump
        if (isSlice && (Input.GetKeyDown(KeyCode.Space) || movJump))
        {
            audioSource.PlayOneShot(jumpSfx);
            movJump = false;
            float angle = 45;
            Quaternion rotation;
            // (NO FUNCIONA EL SALTO, SE DESLIZA POR LA PARED EN LUGAR DE SALTAR AL LADO OPUESTO)Hacer que salte en diagonal
            if (miraDerecha) 
                rotation = Quaternion.Euler(0, 0, angle);
            else 
                rotation = Quaternion.Euler(0, 0, -angle);
             
            fisica.AddForce(rotation * Vector2.up * fuerzaSaltoAlDeslizarEnPared, ForceMode2D.Impulse);
        }

        UpdateTimeSpentAndCoins();
    }

    private void UpdateTimeSpentAndCoins()
    {
        // Actualiza tiempo empleado
        timeExpended  = (int)(Time.time - timeStart);
        hud.setTimeTxt(timeLevel - timeExpended );
        hud.setCoinsTxt(coinAmountToWin - coinAmount);
        // Comprueba si hemos consumido el tiempo del nivel
        if (timeLevel - timeExpended < 0) GameOver();
    }

    public void MakeDamage(int damage)
    {
        
        if (vulnerable)
        {
            audioSource.PlayOneShot(hurtSfx);
            health -= damage;
            hud.setHealthTxt(health);
            if (health <= 0) GameOver();
            vulnerable = false;
            sprite.color = Color.red;
            Invoke("MakeVulnerable", 1.0f);

        }
    }

    private void MakeVulnerable()
    {
        vulnerable = true;
        sprite.color = Color.white;
    }

   

    private void AnimatePlayer()
    {
        if (isSlice) animacion.Play("StickWallPlayerAnimation");
        else if (isAttack) animacion.Play("AttackPlayerAnimation");
        else if (!IsPlayerOnGround()) animacion.Play("JumpPlayerAnimation");
        //
        else if ((fisica.velocity.x > 1 || fisica.velocity.x < -1) && fisica.velocity.y == 0) animacion.Play("RunPlayerAnimation");
        //
        else if ((fisica.velocity.x < 1 || fisica.velocity.x > -1) && fisica.velocity.y == 0 && !isAttack) animacion.Play("IdlePlayerAnimation");
        
        

        //if (isPlayerTouchWallOnAir) animacion.Play("StickWallPlayerAnimation");
    }

    private bool IsPlayerOnGround()
    {
        RaycastHit2D toca = Physics2D.Raycast(transform.position + new Vector3(0, -2f, 0), Vector2.down, 0.2f);
        return toca.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MoviblePlatform"))
        {
            Debug.Log("Jugador entra plataforma");
            transform.parent = collision.transform;
        }

        //if (collision.gameObject.CompareTag("Ground"))
        //{
        //    Debug.Log("Jugador toca suelo");
        //    isPlayerOnGround = true;
        //}

        //if (collision.gameObject.CompareTag("Wall") && !isPlayerOnGround)
        //{
        //    Debug.Log("Jugador toca pared");
        //    isPlayerTouchWallOnAir = true;
        //}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MoviblePlatform"))
        {
            Debug.Log("Jugador sale plataforma");
            transform.parent = null;
        }
        //if (collision.gameObject.CompareTag("Ground"))
        //{
        //    Debug.Log("Jugador en el aire");
        //    isPlayerOnGround = false;
        //}

        //if (collision.gameObject.CompareTag("Wall"))
        //{
        //    Debug.Log("Jugador NO toca pared");
        //    isPlayerTouchWallOnAir = false;
        //}
    }


































    public void IncreaseCoinAmount(int amount)
    {
        audioSource.PlayOneShot(coinSfx);
        coinAmount += amount;
        CheckCointAmount();
    }

    private void CheckCointAmount()
    {
        Debug.Log("Monedas obtenidas: " + coinAmount);
        if (coinAmount == coinAmountToWin)
        {
            WinTheGame();
        }

    }
    private void WinTheGame()
    {
        score = (health * 100) + (timeLevel - timeExpended );
        Debug.Log("HAS GANADO!!! Puntuacion: " + score);
        SetGameData();
        SceneManager.LoadScene("LevelResults");
    }

    private void SetGameData()
    {
        gameData.Score = score;
        gameData.IsWinner = true;
        gameData.TimeExpended = timeExpended;
        SetNextLevelName();
    }

    private void SetNextLevelName()
    {
        string currentLevelName = SceneManager.GetActiveScene().name;
        int currentLevelNumber = Convert.ToInt32(currentLevelName.Substring(currentLevelName.Length - 1));
        Debug.Log("Current Level Number: " + currentLevelNumber);

        if (currentLevelNumber == 3)
        {
            gameData.NextLevelName = "GameOver";
            
        } else if (currentLevelNumber != 0)
        {
            currentLevelNumber++;
            gameData.NextLevelName = "Level" + currentLevelNumber;
        }
    }

    private void GameOver()
    {
        gameData.IsWinner = false;
        SceneManager.LoadScene("GameOver");
    }

    public void ButtonRight()
    {
        movRight = true;
        Debug.Log("ButtonRight");
    }
    public void ButtonNoRight()
    {
        movRight = false;
        Debug.Log("ButtonNoRight");
    }
    public void ButtonLeft()
    {
        movLeft = true;
        Debug.Log("ButtonLeft");
    }
    public void ButtonNoLeft()
    {
        movLeft = false;
        Debug.Log("ButtonNoLeft");
    }
    public void ButtonJump()
    {
        movJump = true;
        Debug.Log("ButtonJump");
    }
    public void ButtonNoJump()
    {
        movJump = false;
        Debug.Log("ButtonNoJump");
    }

    public void ButtonAttack()
    {
        movAttack = true;
        Debug.Log("ButtonAttack");
    }
    public void ButtonNoAttack()
    {
        movAttack = false;
        Debug.Log("ButtonNoAttack");
    }
}
