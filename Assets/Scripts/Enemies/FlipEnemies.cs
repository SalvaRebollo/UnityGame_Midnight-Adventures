using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipEnemies : MonoBehaviour
{
    private SpriteRenderer sprite;
    private float posicionXAnterior;
    private float posicionYAnterior;
    private PolygonCollider2D polygonCollider;

    void Start()
    {
        posicionXAnterior = transform.parent.position.x;
        posicionYAnterior = transform.parent.position.y;
        sprite = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((posicionXAnterior < transform.position.x) || (posicionYAnterior < transform.position.y)) {
            if (transform.localScale.x > 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //polygonCollider.offset = 
        } else
        {
            if (transform.localScale.x < 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //polygonCollider.transform.localScale = new Vector3(1, 1, 1);
        }
        
        //sprite.flipX = (posicionXAnterior < transform.position.x) || (posicionYAnterior < transform.position.y);
        posicionXAnterior = transform.position.x;
        posicionYAnterior = transform.position.y;
    }
}


