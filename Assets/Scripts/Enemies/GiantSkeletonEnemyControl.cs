using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSkeletonEnemyControl : MonoBehaviour
{
    //Listener/Evento cuando un objeto entra y permanece en el area de colision del enemigo
    private void OnCollisionStay2D(Collision2D collision)
    {
        //Compara el objeto que entra en el area de colision con el tag del player
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerControl>().MakeDamage(1);
        }
    }
}
