using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    private bool isAttacking;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRangeX;
    public float attackRangeY;
    public int damage;
    public Animator playerAnim;
    public Animator camAnim;

    public AudioClip attackSfx;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwAttack <= 0)
        {
            // then you can attack
            if (!GetComponent<PlayerControl>().isAttack && (Input.GetKeyDown(KeyCode.RightShift) || GetComponent<PlayerControl>().movAttack))
            {
                audioSource.PlayOneShot(attackSfx);
                GetComponent<PlayerControl>().isAttack = true;
                camAnim.SetTrigger("shake");
                timeBtwAttack = startTimeBtwAttack;
                Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, whatIsEnemies);
              
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    if (enemiesToDamage[i].GetComponent<EnemyControl>())
                    {
                        enemiesToDamage[i].GetComponent<EnemyControl>().TakeDamage(damage);
                    }

                }
                Invoke("IsNotAttacking", 0.5f);

            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
            
        }
        
    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY));
    }
    //if (Input.GetMouseButtonDown(0) && !isAttacking)
    //{
    //    isAttacking = true;
    //    //if (isAttacking) animacion.Play("AttackPlayerAnimation");
    //    Invoke("IsNotAttacking", 1f);
    //}
    private void IsNotAttacking()
    {
        GetComponent<PlayerControl>().isAttack = false;
    }
}
