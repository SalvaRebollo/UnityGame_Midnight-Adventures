using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    public int amount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            collision.GetComponent<PlayerControl>().IncreaseCoinAmount(amount);
            Destroy(gameObject);
        }
    }
}
