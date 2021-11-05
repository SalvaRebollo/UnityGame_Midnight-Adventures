using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public float efectoParallax;

    private Transform camara;
    private Vector3 ultimaPosicionCamara;
    public bool animateY;

    private void Start()
    {
        camara = Camera.main.transform;
        ultimaPosicionCamara = camara.position;
    }

    private void LateUpdate()
    {
        Vector3 movimientoFondo = camara.position - ultimaPosicionCamara;

        if (animateY)
            transform.position += new Vector3(movimientoFondo.x * efectoParallax, movimientoFondo.y, 0);
        else
            transform.position += new Vector3(movimientoFondo.x * efectoParallax, 0, 0);

        ultimaPosicionCamara = camara.position;
    }

}
